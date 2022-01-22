using Microsoft.AspNetCore.Mvc;

namespace lab_6.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            return View();
        }
    }
}