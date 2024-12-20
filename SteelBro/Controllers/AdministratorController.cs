using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SteelBro.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdministratorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Orders()
        {
            return View();
        }
        public IActionResult Finance()
        {
            return View();
        }
        public IActionResult Clients()
        {
            return View();
        }
        public IActionResult Workers()
        { 
            return View();
        }
        public IActionResult Components()
        {
            return View();
        }
    }
}
