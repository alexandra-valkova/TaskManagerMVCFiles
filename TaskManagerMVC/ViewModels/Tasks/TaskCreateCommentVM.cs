using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManagerMVC.ViewModels.Tasks
{
    public class TaskCreateCommentVM
    {
        public int TaskID { get; set; }

        public int UserID { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        public DateTime CreateDate { get; set; }
    }
}