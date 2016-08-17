using System;

namespace DataAccess.Entities
{
    public class Logwork : BaseEntity
    {
        public int TaskID { get; set; }

        public int UserID { get; set; }

        public int WorkingHours { get; set; }

        public DateTime CreateDate { get; set; }
    }
}