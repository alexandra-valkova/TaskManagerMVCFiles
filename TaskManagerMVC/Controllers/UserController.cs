using DataAccess.Entities;
using DataAccess.Repositories;
using System;
using System.Linq;
using TaskManagerMVC.ViewModels.Users;

namespace TaskManagerMVC.Controllers
{
    public class UserController : BaseController<User, UserIndexVM, UserDetailsVM, UserCreateEditVM>
    {
        public override BaseRepository<User> GetRepo()
        {
            return RepoFactory.GetUserRepository();
        }

        // Index
        public override UserIndexVM PopulateIndexModel(UserIndexVM model)
        {
            TryUpdateModel(model);

            BaseRepository<User> userRepo = GetRepo();

            //if (model.SearchString != null)
            //{
            //    model.Items = userRepo.GetAll(u => u.Username.Contains(model.SearchString)
            //                                    || u.FirstName.Contains(model.SearchString)
            //                                    || u.LastName.Contains(model.SearchString));
            //}

            //else
            //{
            //    model.Items = userRepo.GetAll();
            //}

            Predicate<User> filter = null;

            if (string.IsNullOrEmpty(model.SearchString))
            {
                model.Items = userRepo.GetAll();
            }

            else
            {
                string[] searchArray = model.SearchString.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                filter = u => (searchArray.Any(word => u.FirstName.Contains(word))
                            || searchArray.Any(word => u.LastName.Contains(word))
                            || searchArray.Any(word => u.Username.Contains(word)));

                model.Items = userRepo.GetAll(filter);
            }

            return model;
        }

        // Details
        public override UserDetailsVM PopulateDetailsModel(UserDetailsVM model)
        {
            BaseRepository<User> userRepo = GetRepo();
            User user = userRepo.GetByID(model.ID);

            model.Username = user.Username;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.IsAdmin = user.IsAdmin;

            return model;
        }

        // CreateEdit
        public override UserCreateEditVM PopulateCreateEditModel(UserCreateEditVM model)
        {
            if (model.ID > 0)
            {
                BaseRepository<User> userRepo = GetRepo();
                User user = userRepo.GetByID(model.ID);

                model.Username = user.Username;
                model.Password = user.Password;
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.IsAdmin = user.IsAdmin;
            }

            return model;
        }
    }
}