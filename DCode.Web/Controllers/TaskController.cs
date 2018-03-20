using DCode.Models.RequestModels;
using DCode.Models.ResponseModels.Task;
using DCode.Services.Task;
using DCode.Web.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCode.Web.Controllers
{
    [AuthorizeDCode]
    public class TaskController : Controller
    {
        private ITask _taskService;
        public TaskController(ITask taskService)
        {
            _taskService = taskService;
        }
        // GET: Task
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test()
        {
            return Json("Test", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpsertTask(TaskRequest taskRequest)
        {
            try
            {
                var result = _taskService.UpsertTask(taskRequest);
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// This method is used to change the status of the task to close
        /// </summary>
        /// <param name="taskId">Task Id</param>
        /// <returns></returns>
        public ActionResult CloseTask(int taskId)
        {
            var result = _taskService.CloseTask(taskId);
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpGet]
        public ActionResult GetSkills()
        {
            return Json(_taskService.GetSkills(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTasks()
        {
            return Json(_taskService.GetTasks(), JsonRequestBehavior.AllowGet);
        }
    }
}