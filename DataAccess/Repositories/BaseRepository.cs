using System.Collections.Generic;
using System.IO;
using DataAccess.Entities;
using System;

namespace DataAccess.Repositories
{
    public abstract class BaseRepository<T> where T : BaseEntity
    {
        public string FilePath { get; set; }

        public BaseRepository(string filePath)
        {
            FilePath = filePath;
        }

        public virtual List<T> GetAll()
        {
            List<T> entities = new List<T>();

            using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate))
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

            using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate))
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
            using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate))
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

            using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate))
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

            using (FileStream fs = new FileStream(FilePath, FileMode.Append))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                SaveEntity(item, sw);
            }
        }

        private void Edit(T item)
        {
            string fileName = FilePath.Substring(FilePath.LastIndexOf(@"\") + 1);
            string fileFolder = FilePath.Substring(0, FilePath.LastIndexOf(@"\"));

            string tempFile = fileFolder + "temp." + fileName;

            using (FileStream ifs = new FileStream(FilePath, FileMode.OpenOrCreate))
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

            File.Delete(FilePath);
            File.Move(tempFile, FilePath);
        }

        public void Delete(T item)
        {
            string fileName = FilePath.Substring(FilePath.LastIndexOf(@"\") + 1);
            string fileFolder = FilePath.Substring(0, FilePath.LastIndexOf(@"\"));

            string tempFile = fileFolder + "temp." + fileName;

            using (FileStream ifs = new FileStream(FilePath, FileMode.OpenOrCreate))
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

            File.Delete(FilePath);
            File.Move(tempFile, FilePath);
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