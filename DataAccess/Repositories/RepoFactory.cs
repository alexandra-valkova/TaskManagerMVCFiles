using System.Configuration;

namespace DataAccess.Repositories
{
    public class RepoFactory
    {
        public static UserRepository GetUserRepository()
        {
            string path = ConfigurationManager.AppSettings["dataPath"];
            return new UserRepository(path + @"\users.txt");
        }

        public static TaskRepository GetTaskRepository()
        {
            string path = ConfigurationManager.AppSettings["dataPath"];
            return new TaskRepository(path + @"\tasks.txt");
        }

        public static LogworkRepository GetLogworkRepository()
        {
            string path = ConfigurationManager.AppSettings["dataPath"];
            return new LogworkRepository(path + @"\logworks.txt");
        }

        public static CommentRepository GetCommentRepository()
        {
            string path = ConfigurationManager.AppSettings["dataPath"];
            return new CommentRepository(path + @"\comments.txt");
        }
    }
}