using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using ToDo.Core.Models;

namespace ToDo.DataAccess.InMemory
{
    public class TaskRepository
    {
        // create a memory cache object
        ObjectCache cache = MemoryCache.Default;

        // create a list of tasks with type as task model as the data type
        List<TaskModel> tasks;

        public TaskRepository()
        {
            // get a cache of model name as a list
            tasks = cache["tasks"] as List<TaskModel>;

            // check if the cache named tasks already exists, if not, create one
            if(tasks == null)
            {
                tasks = new List<TaskModel>();
            }
        }

        // now the basic tasks list in cache is created, but we to perform CRUD operations in the list
        // we create methods as a operations and confirm operations pattern
        // so when ever a operation is called or performed, we need to call the confirm operation method, Commit method

        // commit method
        public void Commit()
        {
            cache["tasks"] = tasks;
        }

        // now the basic CRUD operation methods
        // insert method
        public void Insert(TaskModel task)
        {
            tasks.Add(task);
        }

        // update method
        public void Update(TaskModel task)
        {
            TaskModel taskToUpdate = tasks.Find(t => t.Id == task.Id);
            if(taskToUpdate != null)
            {
                taskToUpdate = task;
            }
            else
            {
                throw new Exception("Task not found!");
            }
        }

        // delete method
        public void Delete(string id)
        {
            TaskModel taskToDelete = tasks.Find(t => t.Id == id);
            if(taskToDelete != null)
            {
                tasks.Remove(taskToDelete);
            }
            else
            {
                throw new Exception("Task not found!");
            }
        }

        // find a single task with matching id in the task cache list
        public TaskModel Find(string id)
        {
            TaskModel task = tasks.Find(t => t.Id == id);
            if (task != null)
            {
                return task;
            }
            else
            {
                throw new Exception("Task not found!");
            }
        }

        // get all tasks as a queriable list from the cache task list
        public IQueryable<TaskModel> Collection()
        {
            return tasks.AsQueryable();
        }

    }
}
