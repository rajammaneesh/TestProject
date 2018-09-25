using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DCode.Web.Security;

namespace DCode.Web.Controllers
{
    [AuthorizeReportingTask]
    public class ReportingBaseController : Controller
    {
        // GET: ReportingBase
        public ActionResult Base()
        {
            return View();
        }
    }
}