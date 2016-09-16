using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class TasksController : Controller
    {
        private TasksEntities db = new TasksEntities();

        // GET: Tasks
        public ActionResult Index()
        {
            var tasks = db.Tasks.Include(t => t.Task2).Include(t => t.TaskList);
            return View(tasks.ToList());
        }

        // GET: Tasks/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var task = db.Tasks.Find(id);
            }
            catch (HttpException e)
            {
                throw new HttpException(404, "Task not found.");
            }

            return View(db.Tasks.Find(id));
        }
        [HttpGet]
        public PartialViewResult ShowTaskByID(int id)
        {
            try
            {
                var tasks = db.Tasks.ToList();
            }
            catch (HttpException e)
            {
                throw new HttpException(404, "Task nout found.");
            }

            return PartialView("_ShowTask", db.Tasks.Find(id));
        }



        // GET: Tasks/Create

        public ActionResult Create()
        {
            ViewBag.TaskListId = new SelectList(db.TaskLists, "TaskListId", "Name");
            return View();
        }
        // POST: Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TaskId,TaskListId,TaskName,StartDate,FinishDate")] Task task)
        {
            if (ModelState.IsValid)
            {
                db.Tasks.Add(task);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TaskListId = new SelectList(db.Tasks, "TaskId", "TaskName", task.TaskListId);
            ViewBag.TaskListId = new SelectList(db.TaskLists, "TaskListId", "Name", task.TaskListId);
            return View(task);
        }

        // GET: Tasks/Edit/5
        public PartialViewResult EditTaskID(int id)
        {
            return PartialView("_EditTask", db.Tasks.Find(id));
        }

        [HttpPost, ActionName("EditTaskID")]
       public ActionResult Edit([Bind(Include = "TaskId,TaskListId,TaskName,StartDate,FinishDate")] Task editTask)
        {
            try
            {

                db.Entry(editTask).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            ViewBag.TaskListId = new SelectList(db.Tasks, "TaskId", "TaskName", task.TaskListId);
            ViewBag.TaskListId = new SelectList(db.TaskLists, "TaskListId", "Name", task.TaskListId);
            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "TaskId,TaskListId,TaskName,StartDate,FinishDate")] Task task)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(task).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.TaskListId = new SelectList(db.Tasks, "TaskId", "TaskName", task.TaskListId);
        //    ViewBag.TaskListId = new SelectList(db.TaskLists, "TaskListId", "Name", task.TaskListId);
        //    return View(task);
        //}
        public PartialViewResult TaskByID(int id)
        {
            return PartialView("_DeleteTask", db.Tasks.Find(id));
        }
        [HttpPost, ActionName("TaskByID")]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Tasks.Remove(db.Tasks.Find(id));
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // GET: Tasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
