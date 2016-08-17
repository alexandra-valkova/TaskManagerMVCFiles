using DataAccess.Entities;

namespace TaskManagerMVC.ViewModels
{
    public class BaseDetailsVM<T> where T : BaseEntity
    {
        public int ID { get; set; }
    }
}