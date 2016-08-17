using DataAccess.Entities;
using System.ComponentModel.DataAnnotations;

namespace TaskManagerMVC.ViewModels.Users
{
    public class UserDetailsVM : BaseDetailsVM<User>
    {
        public string Username { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Admin")]
        public bool IsAdmin { get; set; }
    }
}