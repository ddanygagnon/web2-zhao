using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Zhao.Backend.Models;

namespace Zhao.Backend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger) => _logger = logger;

        public IActionResult Index() => View();

        public IActionResult PdfMenu()
            => File("~/pdf/ZhaoMenu.pdf", "application/pdf");

        public IActionResult PdfCarteVins()
            => File("~/pdf/ZhaoCarteDesVins.pdf", "application/pdf");

        public IActionResult LienVersGithub()
            => Redirect("https://github.com/ddanygagnon?tab=repositories");

        public IActionResult LienVersFb()
            => Redirect("https://www.facebook.com/danygagnoon/");

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel
            {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}