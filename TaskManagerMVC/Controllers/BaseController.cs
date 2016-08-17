using DataAccess.Entities;
using DataAccess.Repositories;
using System.Linq;
using System.Web.Mvc;
using TaskManagerMVC.Models;
using TaskManagerMVC.ViewModels;

namespace TaskManagerMVC.Controllers
{
    public abstract class BaseController<T, I, D, C> : Controller
        where T : BaseEntity, new()
        where I : BaseIndexVM<T>, new()
        where D : BaseDetailsVM<T>, new()
        where C : BaseCreateEditVM<T>, new()
    {
        public abstract BaseRepository<T> GetRepo();

        public abstract I PopulateIndexModel(I model);

        public abstract D PopulateDetailsModel(D model);

        public abstract C PopulateCreateEditModel(C model); // GET

        public virtual void OnCreateEdit(T entity) { } // POST

        public ActionResult Index()
        {
            if (AuthenticationManager.LoggedUser == null)
            {
                return RedirectToAction("Login", "Home");
            }

            I model = new I();
            PopulateIndexModel(model);

            // Pagination
            model.Pager = new Pager(model.Items.Count, model.Pager == null ? 1 : model.Pager.CurrentPage, "Pager.", "Index", typeof(T).Name.ToString(), model.Pager == null ? 3 : model.Pager.PageSize);
            model.Items = model.Items.Skip((model.Pager.CurrentPage - 1) * model.Pager.PageSize).Take(model.Pager.PageSize).ToList();

            return View(model);
        }

        public ActionResult Details(int? id)
        {
            if (AuthenticationManager.LoggedUser == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (id == null)
            {
                return RedirectToAction("Index");
            }

            D model = new D()
            {
                ID = id.Value
            };

            PopulateDetailsModel(model);

            return View(model);
        }

        [HttpGet]
        public ActionResult CreateEdit(int? id)
        {
            if (AuthenticationManager.LoggedUser == null)
            {
                return RedirectToAction("Login", "Home");
            }

            C model = new C();

            if (id > 0) // Edit
            {
                model.ID = id.Value;
            }

            PopulateCreateEditModel(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult CreateEdit(T entity)
        {
            if (AuthenticationManager.LoggedUser == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(entity);
            }

            BaseRepository<T> entityRepo = GetRepo();

            OnCreateEdit(entity);

            entityRepo.Save(entity);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (AuthenticationManager.LoggedUser == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (id == null)
            {
                return RedirectToAction("Index");
            }

            BaseRepository<T> entityRepo = GetRepo();

            T entity = entityRepo.GetByID(id.Value);

            if (entity == null)
            {
                return HttpNotFound();
            }

            entityRepo.Delete(entity);

            return RedirectToAction("Index");
        }
    }
}