using DataAccess.Entities;
using System.Collections.Generic;
using TaskManagerMVC.Models;

namespace TaskManagerMVC.ViewModels
{
    public class BaseIndexVM<T> where T : BaseEntity
    {
        public List<T> Items { get; set; }

        public Pager Pager { get; set; }

        public string SearchString { get; set; }
    }
}