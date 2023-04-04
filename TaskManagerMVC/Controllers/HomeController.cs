using System.Web.Mvc;
using TaskManagerMVC.Models;
using TaskManagerMVC.ViewModels.Home;

namespace TaskManagerMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginVM model)
        {
            AuthenticationManager.Login(model.Username, model.Password);

            if (AuthenticationManager.LoggedUser == null)
            {
                ModelState.AddModelError(string.Empty, "Wrong username or password!");
                return View(model);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            if (AuthenticationManager.LoggedUser != null)
            {
                AuthenticationManager.Logout();
            }

            return RedirectToAction("Login");
        }
    }
}