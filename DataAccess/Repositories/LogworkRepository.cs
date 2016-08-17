using System;
using System.Collections.Generic;
using System.IO;
using DataAccess.Entities;

namespace DataAccess.Repositories
{
    public class LogworkRepository : BaseRepository<Logwork>
    {
        public LogworkRepository(string filePath) : base(filePath)
        {
        }

        protected override Logwork GetEntity(StreamReader sr)
        {
            Logwork logwork = new Logwork();

            logwork.ID = int.Parse(sr.ReadLine());
            logwork.TaskID = int.Parse(sr.ReadLine());
            logwork.UserID = int.Parse(sr.ReadLine());
            logwork.WorkingHours = int.Parse(sr.ReadLine());
            logwork.CreateDate = DateTime.Parse(sr.ReadLine());

            return logwork;
        }

        protected override void SaveEntity(Logwork entity, StreamWriter sw)
        {
            sw.WriteLine(entity.ID);
            sw.WriteLine(entity.TaskID);
            sw.WriteLine(entity.UserID);
            sw.WriteLine(entity.WorkingHours);
            sw.WriteLine(entity.CreateDate);
        }

        public List<Logwork> GetAll(int taskID)
        {
            List<Logwork> logworks = new List<Logwork>();

            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            using (StreamReader sr = new StreamReader(fs))
            {
                while (!sr.EndOfStream)
                {
                    Logwork logwork = GetEntity(sr);

                    if (logwork.TaskID == taskID)
                    {
                        logworks.Add(logwork);
                    }
                }
            }

            return logworks;
        }
    }
}