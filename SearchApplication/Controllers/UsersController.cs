using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DCode.Data.DbContexts;
using SearchApplication.Models;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace SearchApplication.Controllers
{
    public class UsersController : Controller
    {
        public ActionResult GetFilteredUsers(FormCollection fm)
        {
            Entities db = new Entities();
            // IEnumerable<user> userList;
            //userList = db.users.ToList();
            SearchModelForUsers searchModel = new SearchModelForUsers();
            searchModel.Designation = fm["ddl_Desg"].ToString();
            searchModel.Status = fm["ddl_Status"].ToString();
            IEnumerable<user> result = db.users.ToList();

            if (searchModel != null)
            {
                if (!string.IsNullOrEmpty(searchModel.Designation))
                    result = result.Where(x => x.DESIGNATION == searchModel.Designation);
                if (!string.IsNullOrEmpty(searchModel.Status))
                    result = result.Where(x => x.STATUS.Contains(searchModel.Status));

            }
            TempData["FilterResults"] = result;

            return View(result);
        }

        [System.Web.Mvc.HttpGet]
        //[System.Web.Mvc.ActionName("GetData")]
        public ActionResult GetFilteredUsers()
        {
            Entities db = new Entities();
            IEnumerable<user> result = db.users.ToList();
            return View(result);
        }


        public ActionResult ExportToExcel()
        {
            var gv = new GridView();
            if (TempData["FilterResults"] is null)
            {
                Entities db = new Entities();
                IEnumerable<user> result = db.users.ToList();
                TempData["FilterResults"] = result;
            }
            
            gv.DataSource = (IEnumerable<user>)TempData["FilterResults"];
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
            
        }

    }
}