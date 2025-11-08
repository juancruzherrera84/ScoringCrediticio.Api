using Microsoft.AspNetCore.Mvc;

namespace Scoring.Api.Controllers
{
    public class ClientesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
