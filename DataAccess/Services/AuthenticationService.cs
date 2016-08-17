using DataAccess.Entities;
using DataAccess.Repositories;

namespace DataAccess.Services
{
    public class AuthenticationService
    {
        public User LoggedUser { get; set; }

        public void AuthenticateUser(string username, string password)
        {
            UserRepository userRepo = RepoFactory.GetUserRepository();

            LoggedUser = userRepo.GetByUsernameAndPassword(username, password);
        }
    }
}