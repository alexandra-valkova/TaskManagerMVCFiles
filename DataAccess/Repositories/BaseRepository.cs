using System.Collections.Generic;
using System.IO;
using DataAccess.Entities;
using System;

namespace DataAccess.Repositories
{
    public abstract class BaseRepository<T> where T : BaseEntity
    {
        public string filePath { get; set; }

        public BaseRepository(string filePath)
        {
            this.filePath = filePath;
        }

        public virtual List<T> GetAll()
        {
            List<T> entities = new List<T>();

            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            using (StreamReader sr = new StreamReader(fs))
            {
                while (!sr.EndOfStream)
                {
                    T entity = GetEntity(sr);
                    entities.Add(entity);
                }
            }

            return entities;
        }

        public virtual List<T> GetAll(Predicate<T> filter)
        {
            List<T> entities = new List<T>();

            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            using (StreamReader sr = new StreamReader(fs))
            {
                while (!sr.EndOfStream)
                {
                    T entity = GetEntity(sr);
                    if (filter(entity))
                    {
                        entities.Add(entity);
                    }
                }
            }

            return entities;
        }

        public virtual T GetByID(int id)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            using (StreamReader sr = new StreamReader(fs))
            {
                while (!sr.EndOfStream)
                {
                    T entity = GetEntity(sr);

                    if (entity.ID == id)
                    {
                        return entity;
                    }
                }

                return null;
            }
        }

        private int GetNextID()
        {
            int id = 0;

            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            using (StreamReader sr = new StreamReader(fs))
            {
                while (!sr.EndOfStream)
                {
                    T entity = GetEntity(sr);

                    if (id < entity.ID)
                    {
                        id = entity.ID;
                    }
                }

                id++;
                return id;
            }
        }

        private void Add(T item)
        {
            item.ID = GetNextID();

            using (FileStream fs = new FileStream(filePath, FileMode.Append))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                SaveEntity(item, sw);
            }
        }

        private void Edit(T item)
        {
            string fileName = filePath.Substring(filePath.LastIndexOf(@"\") + 1);
            string fileFolder = filePath.Substring(0, filePath.LastIndexOf(@"\"));

            string tempFile = fileFolder + "temp." + fileName;

            using (FileStream ifs = new FileStream(filePath, FileMode.OpenOrCreate))
            using (StreamReader sr = new StreamReader(ifs))
            using (FileStream ofs = new FileStream(tempFile, FileMode.OpenOrCreate))
            using (StreamWriter sw = new StreamWriter(ofs))
            {
                while (!sr.EndOfStream)
                {
                    T entity = GetEntity(sr);

                    if (item.ID == entity.ID)
                    {
                        SaveEntity(item, sw);
                    }

                    else
                    {
                        SaveEntity(entity, sw);
                    }
                }
            }

            File.Delete(filePath);
            File.Move(tempFile, filePath);
        }

        public void Delete(T item)
        {
            string fileName = filePath.Substring(filePath.LastIndexOf(@"\") + 1);
            string fileFolder = filePath.Substring(0, filePath.LastIndexOf(@"\"));

            string tempFile = fileFolder + "temp." + fileName;

            using (FileStream ifs = new FileStream(filePath, FileMode.OpenOrCreate))
            using (StreamReader sr = new StreamReader(ifs))
            using (FileStream ofs = new FileStream(tempFile, FileMode.OpenOrCreate))
            using (StreamWriter sw = new StreamWriter(ofs))
            {
                while (!sr.EndOfStream)
                {
                    T entity = GetEntity(sr);

                    if (item.ID != entity.ID)
                    {
                        SaveEntity(entity, sw);
                    }
                }
            }

            File.Delete(filePath);
            File.Move(tempFile, filePath);
        }

        public void Save(T entity)
        {
            if (entity.ID == 0)
            {
                Add(entity);
            }

            else
            {
                Edit(entity);
            }
        }

        //Abstract Methods

        protected abstract void SaveEntity(T entity, StreamWriter sw);

        protected abstract T GetEntity(StreamReader sr);
    }
}