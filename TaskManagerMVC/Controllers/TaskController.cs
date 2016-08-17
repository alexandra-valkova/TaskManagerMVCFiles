using DataAccess.Entities;
using System;
using DataAccess.Repositories;
using TaskManagerMVC.Models;
using TaskManagerMVC.ViewModels.Tasks;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace TaskManagerMVC.Controllers
{
    public class TaskController : BaseController<Task, TaskIndexVM, TaskDetailsVM, TaskCreateEditVM>
    {
        public override BaseRepository<Task> GetRepo()
        {
            return RepoFactory.GetTaskRepository();
        }

        // Index
        public override TaskIndexVM PopulateIndexModel(TaskIndexVM model)
        {
            TryUpdateModel(model);

            BaseRepository<Task> taskRepo = GetRepo();

            model.Items = taskRepo.GetAll(t => t.CreatorID == AuthenticationManager.LoggedUser.ID
                                            || t.ResponsibleID == AuthenticationManager.LoggedUser.ID);

            //if (model.SearchString != null)
            //{
            //    model.Items = model.Items.Where(t => t.Title.Contains(model.SearchString)
            //                                      || t.Description.Contains(model.SearchString)).ToList();
            //}

            if (model.SearchString != null)
            {
                Predicate<Task> filter = null;

                string[] searchArray = model.SearchString.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                filter = t => (searchArray.Any(word => t.Title.Contains(word))
                            || searchArray.Any(word => t.Description.Contains(word)));

                model.Items = taskRepo.GetAll(filter);

                //if (model.SearchString != null)
                //{
                //    model.Items = model.Items.Where(t => (t.CreatorID == AuthenticationManager.LoggedUser.ID || t.ResponsibleID == AuthenticationManager.LoggedUser.ID) 
                //                                      && (t.Title.Contains(model.SearchString) || t.Description.Contains(model.SearchString))).ToList();
                //}
            }

            return model;
        }

        // Details
        public override TaskDetailsVM PopulateDetailsModel(TaskDetailsVM model)
        {
            TryUpdateModel(model);

            BaseRepository<Task> taskRepo = GetRepo();
            UserRepository userRepo = RepoFactory.GetUserRepository();
            LogworkRepository logworkRepo = RepoFactory.GetLogworkRepository();
            CommentRepository commentRepo = RepoFactory.GetCommentRepository();

            Task task = taskRepo.GetByID(model.ID);

            model.Title = task.Title;
            model.Description = task.Description;
            model.WorkingHours = task.WorkingHours;
            model.Creator = userRepo.GetByID(task.CreatorID).Username;
            model.Responsible = userRepo.GetByID(task.ResponsibleID).Username;
            model.CreateDate = task.CreateDate;
            model.LastEditDate = task.LastEditDate;
            model.Status = task.Status;
            model.Logworks = logworkRepo.GetAll(l => l.TaskID == task.ID);
            model.Comments = commentRepo.GetAll(c => c.TaskID == task.ID);

            model.LogworksPager = new Pager(model.Logworks.Count, model.LogworksPager == null ? 1 : model.LogworksPager.CurrentPage, "LogworksPager.", "Details", "Task", model.LogworksPager == null ? 3 : model.LogworksPager.PageSize);
            model.Logworks = model.Logworks.Skip((model.LogworksPager.CurrentPage - 1) * model.LogworksPager.PageSize).Take(model.LogworksPager.PageSize).ToList();

            model.CommentsPager = new Pager(model.Comments.Count, model.CommentsPager == null ? 1 : model.CommentsPager.CurrentPage, "CommentsPager.", "Details", "Task", model.CommentsPager == null ? 3 : model.CommentsPager.PageSize);
            model.Comments = model.Comments.Skip((model.CommentsPager.CurrentPage - 1) * model.CommentsPager.PageSize).Take(model.CommentsPager.PageSize).ToList();

            return model;
        }

        // CreateEdit
        public override TaskCreateEditVM PopulateCreateEditModel(TaskCreateEditVM model)
        {
            if (model.ID > 0) // Edit
            {
                BaseRepository<Task> taskRepo = GetRepo();
                Task task = taskRepo.GetByID(model.ID);

                model.Title = task.Title;
                model.Description = task.Description;
                model.WorkingHours = task.WorkingHours;
                model.CreatorID = task.CreatorID;
                model.ResponsibleID = task.ResponsibleID;
                model.CreateDate = task.CreateDate;
                model.LastEditDate = task.LastEditDate;
                model.Status = task.Status;
            }

            UserRepository userRepo = RepoFactory.GetUserRepository();
            List<User> usersAll = userRepo.GetAll();
            List<SelectListItem> users = new List<SelectListItem>();
            foreach (User user in usersAll)
            {
                users.Add(new SelectListItem { Text = user.Username, Value = user.ID.ToString(), Selected = model.ResponsibleID == user.ID });
            }
            model.UsersList = users;

            return model;
        }

        // On CreateEdit
        public override void OnCreateEdit(Task entity)
        {
            entity.LastEditDate = DateTime.Now;

            if (entity.ID == 0) // Create
            {
                entity.CreateDate = DateTime.Now;
                entity.CreatorID = AuthenticationManager.LoggedUser.ID;
            }
        }

        // Logwork
        public PartialViewResult CreateLogwork(int? id)
        {
            TaskCreateLogworkVM model = new TaskCreateLogworkVM()
            {
                TaskID = id.Value
            };

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult CreateLogwork(TaskCreateLogworkVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Logwork logwork = new Logwork()
            {
                TaskID = model.TaskID,
                UserID = AuthenticationManager.LoggedUser.ID,
                WorkingHours = model.WorkingHours,
                CreateDate = DateTime.Now
            };

            LogworkRepository logworkRepo = RepoFactory.GetLogworkRepository();
            logworkRepo.Save(logwork);

            return RedirectToAction("Details", new { id = model.TaskID });
        }

        // Comment
        public PartialViewResult CreateComment(int? id)
        {
            TaskCreateCommentVM model = new TaskCreateCommentVM()
            {
                TaskID = id.Value
            };

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult CreateComment(TaskCreateCommentVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Comment comment = new Comment()
            {
                TaskID = model.TaskID,
                UserID = AuthenticationManager.LoggedUser.ID,
                Text = model.Text,
                CreateDate = DateTime.Now
            };

            CommentRepository commentRepo = RepoFactory.GetCommentRepository();
            commentRepo.Save(comment);

            return RedirectToAction("Details", new { id = model.TaskID });
        }
    }
}