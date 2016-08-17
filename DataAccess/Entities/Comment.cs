using System;

namespace DataAccess.Entities
{
    public class Comment : BaseEntity
    {
        public int TaskID { get; set; }

        public int UserID { get; set; }

        public string Text { get; set; }

        public DateTime CreateDate { get; set; }
    }
}