using System;
using System.Collections.Generic;
using System.IO;
using DataAccess.Entities;

namespace DataAccess.Repositories
{
    public class CommentRepository : BaseRepository<Comment>
    {
        public CommentRepository(string filePath) : base(filePath)
        {
        }

        protected override Comment GetEntity(StreamReader sr)
        {
            Comment comment = new Comment();

            comment.ID = int.Parse(sr.ReadLine());
            comment.TaskID = int.Parse(sr.ReadLine());
            comment.UserID = int.Parse(sr.ReadLine());
            comment.Text = sr.ReadLine();
            comment.CreateDate = DateTime.Parse(sr.ReadLine());

            return comment;
        }

        protected override void SaveEntity(Comment entity, StreamWriter sw)
        {
            sw.WriteLine(entity.ID);
            sw.WriteLine(entity.TaskID);
            sw.WriteLine(entity.UserID);
            sw.WriteLine(entity.Text);
            sw.WriteLine(entity.CreateDate);
        }

        public List<Comment> GetAll(int taskID)
        {
            List<Comment> comments = new List<Comment>();

            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            using (StreamReader sr = new StreamReader(fs))
            {
                while (!sr.EndOfStream)
                {
                    Comment comment = GetEntity(sr);

                    if (comment.TaskID == taskID)
                    {
                        comments.Add(comment);
                    }
                }
            }

            return comments;
        }
    }
}