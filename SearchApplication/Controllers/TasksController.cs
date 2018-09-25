using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using DCode.Data;
using DCode.Data.DbContexts;
using SearchApplication.Models;

namespace SearchApplication.Controllers
{
    public class TasksController : Controller
    {

        [HttpPost]
        //[System.Web.Mvc.ActionName("FilteredData")]
        public ActionResult GetFilteredTasks(FormCollection fm)
        {
            Entities db = new Entities();
            IEnumerable<task_type> taskTypesList = db.task_type.ToList();
            IEnumerable<offering> offeringsList = db.offerings.ToList();
            //IEnumerable<service_line> serviceLineList = db.service_line.ToList();
            ViewBag.taskTypesList = taskTypesList;
            ViewBag.offeringsList = offeringsList;
            //ViewBag.serviceLine = serviceLineList;

            SearchModel searchModel = new SearchModel();
            if (fm["ddl_Task_Type"].ToString() != "")
            {
                searchModel.Task_Type_id = Convert.ToInt32(fm["ddl_Task_Type"]);                
            }
            if (fm["ddl_Offerings"].ToString() != "" )
            {
                searchModel.Offerings_id = Convert.ToInt32(fm["ddl_Offerings"]);
            }
            //if (fm["ddl_ServiceLine"].ToString() != "")
            //{
            //    searchModel.Service_Line_id = Convert.ToInt32(fm["ddl_ServiceLine"]);
            //}
            if (fm["ddl_Status"].ToString() != "")
            {
                searchModel.Status = (fm["ddl_Status"]).ToString();
            }


            IEnumerable<task> result = db.tasks.ToList();

            if (searchModel != null)
            {
                if (searchModel.Task_Type_id.HasValue)
                    result = result.Where(x => x.TASK_TYPE_ID == searchModel.Task_Type_id);
                if (searchModel.Offerings_id.HasValue)
                   result = result.Where(x => x.OFFERING_ID == searchModel.Offerings_id);
                //if (searchModel.Service_Line_id.HasValue)
                //    result = result.Where(x => x.SERVICE_LINE_ID == searchModel.Service_Line_id);
                if (!string.IsNullOrEmpty(searchModel.Status))
                    result = result.Where(x => x.STATUS.Contains(searchModel.Status));

            }
            TempData["FilteredResults"] = result;

            return View(result);
        }

        [HttpGet]
        [Authorize]
        //[System.Web.Mvc.ActionName("GetData")]
        public ActionResult GetFilteredTasks()
        {
            Entities db = new Entities();
            IEnumerable<task_type> taskTypesList = db.task_type.ToList();
            IEnumerable<offering> offeringsList = db.offerings.ToList();
            //IEnumerable<service_line> serviceLineList = db.service_line.ToList();
            ViewBag.taskTypesList = taskTypesList;
            ViewBag.offeringsList = offeringsList;
            //ViewBag.serviceLine = serviceLineList;
            IEnumerable<task> result = db.tasks.ToList();
           
            return View(result);
        }


        public ActionResult ExportToExcel()
        {
            var gv = new GridView();
            if (TempData["FilteredResults"] is null)
            {
                Entities db = new Entities();
                IEnumerable<task> result = db.tasks.ToList();
                TempData["FilteredResults"] = result;
            }
            gv.DataSource = (IEnumerable<task>)TempData["FilteredResults"];
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=DemoExcel.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return View();
            // return View("UserViewController/GetFilteredUsers");
        }
    }
}