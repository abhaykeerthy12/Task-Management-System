using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToDo.DataAccess.InMemory;
using ToDo.Core.Models;

namespace ToDo.WebUi.Controllers
{
    public class TaskController : Controller
    {
        TaskRepository context;

        public TaskController()
        {
            context = new TaskRepository();
        }

        // the main index page with the list of tasks
        public ActionResult Index()
        {
            List<TaskModel> taskList= context.Collection().ToList();
            return View(taskList);
        }

        // now for the actual CRUD operation methods
        // keep in mind that, we need two methods for some operations with same name
        // the first one is the forms to fill in data, like create form, edit form etc
        // the second is the method which gets the data form the forms by HTTP-POST protocol

        // here is a brief description of what the methods do in common : 
        // first the check if the form has all validations is matched to a success, else redisplay the page with specific validation errors
        // if everything is ok, then they call the specific methods in TaskRepository (context)
        // and also call commit method to confirm, after that redirect the user to index page

        // create method 1 - the page with the form
        public ActionResult Create()
        {
            TaskModel task = new TaskModel();
            return View(task);
        }

        // create method 2 - the method which process the data from the form got by HTTP-POST method
        [HttpPost]
        public ActionResult Create(TaskModel task)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
    .Where(x => x.Value.Errors.Count > 0)
    .Select(x => new { x.Key, x.Value.Errors })
    .ToArray();
                return View(task);
            }
            else
            {
                context.Insert(task);
                context.Commit();

                return RedirectToAction("Index");
            }
        }

        // edit method 1 - the form
        public ActionResult Edit(string id)
        {
            TaskModel taskToEdit = context.Find(id);
            if(taskToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(taskToEdit);
            }
        }

        // edit method 2 - the logic
        [HttpPost]
        public ActionResult Edit(TaskModel task, string id){
            TaskModel taskToEdit = context.Find(id);
            if(taskToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid){
                    return View(task);
                }
                else
                {
                    taskToEdit.TaskString = task.TaskString;
                    context.Commit();

                    return RedirectToAction("Index");
                }
            }
        }

        // delete method 1
        public ActionResult Delete(string id)
        {
            TaskModel taskToDelete = context.Find(id);
            if (taskToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(taskToDelete);
            }
        }

        // delete method 2
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string id) {
            TaskModel taskToDelete = context.Find(id);
            if (taskToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(id);
                context.Commit();

                return RedirectToAction("Index");
            }
        }

    }
}