using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SteelBro.Controllers
{
    [Authorize(Roles = "Financier")]
    public class FinancierController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Finance()
        {
            return View();
        }
    }
}
