using System;

namespace DataAccess.Entities
{
    public class Task : BaseEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int WorkingHours { get; set; }

        public int CreatorID { get; set; }

        public int ResponsibleID { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastEditDate { get; set; }

        public StatusEnum Status { get; set; }
    }

    public enum StatusEnum
    {
        Pending,
        Done
    }
}