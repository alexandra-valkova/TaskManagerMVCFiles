using System;
using System.IO;
using DataAccess.Entities;

namespace DataAccess.Repositories
{
    public class TaskRepository : BaseRepository<Task>
    {
        public TaskRepository(string filePath) : base(filePath)
        {
        }

        protected override Task GetEntity(StreamReader sr)
        {
            Task task = new Task();

            task.ID = int.Parse(sr.ReadLine());
            task.Title = sr.ReadLine();
            task.Description = sr.ReadLine();
            task.WorkingHours = int.Parse(sr.ReadLine());
            task.CreatorID = int.Parse(sr.ReadLine());
            task.ResponsibleID = int.Parse(sr.ReadLine());
            task.CreateDate = DateTime.Parse(sr.ReadLine());
            task.LastEditDate = DateTime.Parse(sr.ReadLine());
            task.Status = (StatusEnum)Enum.Parse(typeof(StatusEnum), sr.ReadLine());

            return task;
        }

        protected override void SaveEntity(Task entity, StreamWriter sw)
        {
            sw.WriteLine(entity.ID);
            sw.WriteLine(entity.Title);
            sw.WriteLine(entity.Description);
            sw.WriteLine(entity.WorkingHours);
            sw.WriteLine(entity.CreatorID);
            sw.WriteLine(entity.ResponsibleID);
            sw.WriteLine(entity.CreateDate);
            sw.WriteLine(entity.LastEditDate);
            sw.WriteLine(entity.Status);
        }
    }
}