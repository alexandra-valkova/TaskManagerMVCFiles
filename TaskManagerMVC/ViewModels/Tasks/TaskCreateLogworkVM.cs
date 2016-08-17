using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManagerMVC.ViewModels.Tasks
{
    public class TaskCreateLogworkVM
    {
        public int TaskID { get; set; }

        public int UserID { get; set; }

        [Required]
        [Display(Name = "Logged Hours")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a valid integer!")]        
        public int WorkingHours { get; set; }

        public DateTime CreateDate { get; set; }
    }
}