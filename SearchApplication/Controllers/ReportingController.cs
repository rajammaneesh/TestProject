using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data;
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
        
       
        // GET: Reporting
        public ActionResult TopRequestors()
        {
            string conString = System.Configuration.ConfigurationManager.ConnectionStrings["DcodeConnectionString"].ToString();
            MySqlConnection mySql = new MySqlConnection(conString);
            MySqlCommand cmd = new MySqlCommand("select FIRST_NAME, LAST_NAME, EMAIL_ID from users where id IN(select user_id from tasks group by USER_ID order by count(USER_ID) desc)", mySql);
            // select FIRST_NAME, LAST_NAME, EMAIL_ID from users where id IN(select user_id from tasks group by USER_ID order by count(USER_ID) desc);
            mySql.Open();
            MySqlDataReader dr = cmd.ExecuteReader();
            user u = new user();
            List<user> topRequestorsList = new List<user>();
            while (dr.Read())
            {
                u = new user();
                u.FIRST_NAME = dr["FIRST_NAME"].ToString();
                u.LAST_NAME = dr["LAST_NAME"].ToString();
                u.EMAIL_ID = dr["EMAIL_ID"].ToString();
                topRequestorsList.Add(u);
       
            }
            TempData["FilteredResults"] = topRequestorsList ;
            dr.Close();
            mySql.Close();
            return View(topRequestorsList);
        }
        public ActionResult TopContributors()
        {
            string conString = System.Configuration.ConfigurationManager.ConnectionStrings["DcodeConnectionString"].ToString();
            MySqlConnection mySql = new MySqlConnection(conString);
            MySqlCommand cmd = new MySqlCommand("select a.APPLICANT_ID,u.EMAIL_ID,count(u.EMAIL_ID) from taskapplicants as a, tasks as t, users as u where a.TASK_ID=t.ID and a.APPLICANT_ID=u.ID and a.status in ('Active','Assigned','Closed') group by u.EMAIL_ID order by count(u.EMAIL_ID) desc ", mySql);
            mySql.Open();
            MySqlDataReader dr = cmd.ExecuteReader();
           Contributor c = new Contributor();
           // taskapplicant t = new taskapplicant();                
            List<Contributor> topContributorsList = new List<Contributor>();          
            while (dr.Read())
            {
                topContributorsList = new List<Contributor>();
                c.APPLICANT_ID = Convert.ToInt32(dr[0]);
               c.EMAIL_ID = dr[1].ToString();
               c.Count = Convert.ToInt32(dr[2]);
                topContributorsList.Add(c);

            }
            TempData["FilteredResults"] = topContributorsList;
            dr.Close();
            mySql.Close();
            return View(topContributorsList);
        }
        public ActionResult TaskRequestorsPerOffering()
        {
            string conString = System.Configuration.ConfigurationManager.ConnectionStrings["DcodeConnectionString"].ToString();
            MySqlConnection mySql = new MySqlConnection(conString);
            MySqlCommand cmd = new MySqlCommand("select o.Code, count(o.code) as tasks_applied from tasks as t, users as u, offerings as o where t.USER_ID = u.id and u.OFFERING_ID = o.Id group by o.Code order by count(o.code) desc", mySql);

            mySql.Open();
            MySqlDataReader dr = cmd.ExecuteReader();
            SplitPerOffering so = new SplitPerOffering();
            List<SplitPerOffering> TaskRequestorsList = new List<SplitPerOffering>();
            while (dr.Read())
            {
                 so = new SplitPerOffering();
                so.Code = dr[0].ToString();
                so.TasksApplied = Convert.ToInt32(dr[1]);
                TaskRequestorsList.Add(so);

            }
            TempData["FilteredResults"] = TaskRequestorsList;
            dr.Close();
            mySql.Close();
            ViewBag.PageTitle = "Task Requestors Per Offering List";
            return View("~/Views/Reporting/SplitPerOffering.cshtml", TaskRequestorsList);
        }
        public ActionResult TaskApplicantsPerOffering()
        {
            string conString = System.Configuration.ConfigurationManager.ConnectionStrings["DcodeConnectionString"].ToString();
            MySqlConnection mySql = new MySqlConnection(conString);
            MySqlCommand cmd = new MySqlCommand("select o.Code,count(o.code) as tasks_applied from taskapplicants as a, users as u,offerings as o where a.APPLICANT_ID = u.id and u.OFFERING_ID=o.Id group by o.Code order by count(o.code) desc", mySql);
           
            mySql.Open();
            MySqlDataReader dr = cmd.ExecuteReader();
            SplitPerOffering so = new SplitPerOffering();
            List<SplitPerOffering> TaskApplicantsList = new List<SplitPerOffering>();
            while (dr.Read())
            {
                so = new SplitPerOffering();
                so.Code = dr[0].ToString();
                so.TasksApplied = Convert.ToInt32(dr[1]);
                TaskApplicantsList.Add(so);

            }
            TempData["FilteredResults"] = TaskApplicantsList;
            dr.Close();
            mySql.Close();
            ViewBag.PageTitle = "Task Applicants Per Offering List";
            return View("~/Views/Reporting/SplitPerOffering.cshtml",TaskApplicantsList);
        }
        public ActionResult ExportToExcel()
        {
            var gv = new GridView();
            //if (TempData["FilteredResults"] is null)
            //{
            //    Entities db = new Entities();
            //    IEnumerable<task> result = db.tasks.ToList();
            //    TempData["FilteredResults"] = result;
            //}
            gv.DataSource = (IEnumerable<object>)TempData["FilteredResults"];
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