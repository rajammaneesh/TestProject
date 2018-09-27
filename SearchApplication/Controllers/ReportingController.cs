using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DCode.Data.DbContexts;
using SearchApplication.Models;
using MySql.Data.MySqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SearchApplication.Controllers
{
    public class ReportingController : Controller
    {

        string conString = System.Configuration.ConfigurationManager.ConnectionStrings["DcodeConnectionString"].ToString();
        // GET: Reporting
        public ActionResult TopRequestors()
        {
            List<user> topRequestorsList = new List<user>();
            using (MySqlConnection mySql = new MySqlConnection(conString))
            {
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand("select FIRST_NAME, LAST_NAME, EMAIL_ID from users where id IN(select user_id from tasks group by USER_ID order by count(USER_ID) desc)", mySql))
                    {
                        mySql.Open();
                        MySqlDataReader dr = cmd.ExecuteReader();
                        user u = new user();
                        while (dr.Read())
                        {
                            u = new user();
                            u.FIRST_NAME = Convert.ToString(dr["FIRST_NAME"]);
                            u.LAST_NAME = Convert.ToString(dr["LAST_NAME"]);
                            u.EMAIL_ID = Convert.ToString(dr["EMAIL_ID"]);
                            topRequestorsList.Add(u);
                        }
                    }
                    TempData["FilteredResults"] = topRequestorsList;
                }
                finally
                {
                    if (mySql != null)
                    {
                        mySql.Close();
                    }
                }
            }


            return View(topRequestorsList);
        }
        public ActionResult TopContributors()
        {
            List<Contributor> topContributorsList = new List<Contributor>();
            using (MySqlConnection mySql = new MySqlConnection(conString))
            {
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand("select a.APPLICANT_ID,u.EMAIL_ID,count(u.EMAIL_ID) from taskapplicants as a, tasks as t, users as u where a.TASK_ID=t.ID and a.APPLICANT_ID=u.ID and a.status in ('Active','Assigned','Closed') group by u.EMAIL_ID order by count(u.EMAIL_ID) desc ", mySql))
                    {
                        mySql.Open();
                        MySqlDataReader dr = cmd.ExecuteReader();
                        Contributor c = new Contributor();
                        while (dr.Read())
                        {
                            topContributorsList = new List<Contributor>();
                            c.APPLICANT_ID = Convert.ToInt32(dr[0]);
                            c.EMAIL_ID = Convert.ToString(dr[1]);
                            c.Count = Convert.ToInt32(dr[2]);
                            topContributorsList.Add(c);

                        }
                    }
                    TempData["FilteredResults"] = topContributorsList;
                }

                finally
                {
                    if (mySql != null)
                    {
                        mySql.Close();
                    }
                }
            }
            return View(topContributorsList);
        }
        public ActionResult TaskRequestorsPerOffering()
        {
            List<SplitPerOffering> TaskRequestorsList = new List<SplitPerOffering>();
            using (MySqlConnection mySql = new MySqlConnection(conString))
            {
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand("select o.Code, count(o.code) as tasks_applied from tasks as t, users as u, offerings as o where t.USER_ID = u.id and u.OFFERING_ID = o.Id group by o.Code order by count(o.code) desc", mySql))
                    {
                        mySql.Open();
                        MySqlDataReader dr = cmd.ExecuteReader();
                        SplitPerOffering so = new SplitPerOffering();

                        while (dr.Read())
                        {
                            so = new SplitPerOffering();
                            so.Code = Convert.ToString(dr[0]);
                            so.TasksApplied = Convert.ToInt32(dr[1]);
                            TaskRequestorsList.Add(so);
                        }
                    }
                    TempData["FilteredResults"] = TaskRequestorsList;
                }
                finally
                {
                    if (mySql != null)
                    {
                        mySql.Close();
                    }
                }
            }
            ViewBag.PageTitle = "Task Requestors Per Offering List";
            return View("~/Views/Reporting/SplitPerOffering.cshtml", TaskRequestorsList);
        }
        public ActionResult TaskApplicantsPerOffering()
        {
            List<SplitPerOffering> TaskApplicantsList = new List<SplitPerOffering>();
            using (MySqlConnection mySql = new MySqlConnection(conString))
            {
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand("select o.Code,count(o.code) as tasks_applied from taskapplicants as a, users as u,offerings as o where a.APPLICANT_ID = u.id and u.OFFERING_ID=o.Id group by o.Code order by count(o.code) desc", mySql))
                    {
                        mySql.Open();
                        MySqlDataReader dr = cmd.ExecuteReader();
                        SplitPerOffering so = new SplitPerOffering();

                        while (dr.Read())
                        {
                            so = new SplitPerOffering();
                            so.Code = Convert.ToString(dr[0]);
                            so.TasksApplied = Convert.ToInt32(dr[1]);
                            TaskApplicantsList.Add(so);
                        }
                    }
                    TempData["FilteredResults"] = TaskApplicantsList;
                }

                finally
                {
                    if (mySql != null)
                    {
                        mySql.Close();
                    }
                }
                ViewBag.PageTitle = "Task Applicants Per Offering List";
                return View("~/Views/Reporting/SplitPerOffering.cshtml", TaskApplicantsList);
            }
        }


        //users section
        public ActionResult GetFilteredUsers(FormCollection fm)
        {
            Entities db = new Entities();
            SearchModelForUsers searchModel = new SearchModelForUsers();
            searchModel.Designation = Convert.ToString(fm["ddl_Desg"]);
            searchModel.Status = Convert.ToString(fm["ddl_Status"]);
            IEnumerable<user> result = db.users.ToList();

            if (searchModel != null)
            {
                if (!string.IsNullOrEmpty(searchModel.Designation))
                    result = result.Where(x => x.DESIGNATION == searchModel.Designation);
                if (!string.IsNullOrEmpty(searchModel.Status))
                    result = result.Where(x => x.STATUS.Contains(searchModel.Status));

            }
            TempData["FilteredResults"] = result;
            return View(result);
        }

        [HttpGet]
        public ActionResult GetFilteredUsers()
        {
            Entities db = new Entities();
            IEnumerable<user> result = db.users.ToList();
            TempData["FilteredResults"] = result;
            return View(result);
        }


        //tasks section
        public ActionResult GetFilteredTasks(FormCollection fm)
        {
            Entities db = new Entities();
            IEnumerable<task_type> taskTypesList = db.task_type.ToList();
            IEnumerable<offering> offeringsList = db.offerings.ToList();
            ViewBag.taskTypesList = taskTypesList;
            ViewBag.offeringsList = offeringsList;

            SearchModel searchModel = new SearchModel();
            if (Convert.ToString(fm["ddl_Task_Type"]) != "")
            {
                searchModel.Task_Type_id = Convert.ToInt32(fm["ddl_Task_Type"]);
            }
            if (Convert.ToString(fm["ddl_Offerings"]) != "")
            {
                searchModel.Offerings_id = Convert.ToInt32(fm["ddl_Offerings"]);
            }
            if (Convert.ToString(fm["ddl_Status"]) != "")
            {
                searchModel.Status = Convert.ToString(fm["ddl_Status"]);
            }


            IEnumerable<task> result = db.tasks.ToList();

            if (searchModel != null)
            {
                if (searchModel.Task_Type_id.HasValue)
                    result = result.Where(x => x.TASK_TYPE_ID == searchModel.Task_Type_id);
                if (searchModel.Offerings_id.HasValue)
                    result = result.Where(x => x.OFFERING_ID == searchModel.Offerings_id);
                if (!string.IsNullOrEmpty(searchModel.Status))
                    result = result.Where(x => x.STATUS.Contains(searchModel.Status));

            }
            TempData["FilteredResults"] = result;
            return View(result);
        }

        [HttpGet]
        public ActionResult GetFilteredTasks()
        {
            Entities db = new Entities();
            IEnumerable<task_type> taskTypesList = db.task_type.ToList();
            IEnumerable<offering> offeringsList = db.offerings.ToList();
            ViewBag.taskTypesList = taskTypesList;
            ViewBag.offeringsList = offeringsList;
            IEnumerable<task> result = db.tasks.ToList();
            TempData["FilteredResults"] = result;

            return View(result);
        }


        public ActionResult ExportToExcel()
        {
            var gv = new GridView();
            gv.DataSource = (IEnumerable<object>)TempData["FilteredResults"];
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=ReportingExcelWorkBook.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(Convert.ToString(objStringWriter));
            Response.Flush();
            Response.End();
            return View();
        }


    }
}