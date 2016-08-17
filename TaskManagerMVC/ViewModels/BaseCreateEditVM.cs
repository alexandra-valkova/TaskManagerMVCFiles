using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskManagerMVC.ViewModels
{
    public class BaseCreateEditVM<T> where T : BaseEntity
    {
        public int ID { get; set; }
    }
}