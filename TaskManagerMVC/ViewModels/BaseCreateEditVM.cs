using DataAccess.Entities;

namespace TaskManagerMVC.ViewModels
{
    public class BaseCreateEditVM<T> where T : BaseEntity
    {
        public int ID { get; set; }
    }
}