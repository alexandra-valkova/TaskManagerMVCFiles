using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TaskManagerMVC.Models;

namespace TaskManagerMVC.ViewModels.Tasks
{
    public class TaskDetailsVM : BaseDetailsVM<Task>
    {
        public string Title { get; set; }

        public string Description { get; set; }

        [Display(Name = "Logged Hours")]
        public int WorkingHours { get; set; }

        public string Creator { get; set; }

        public string Responsible { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Last Edited Date")]
        public DateTime LastEditDate { get; set; }

        public StatusEnum Status { get; set; }

        public List<Logwork> Logworks { get; set; }

        public List<Comment> Comments { get; set; }

        public Pager CommentsPager { get; set; }

        public Pager LogworksPager { get; set; }
    }
}