using Microsoft.AspNetCore.Mvc;
using MVCore.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace MVCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MusicContext _context;

        public HomeController(ILogger<HomeController> logger, MusicContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var authors = _context.Authors.ToList();

            if (authors == null || !authors.Any())
            {
                _logger.LogWarning("Не найдено авторов в базе данных.");
                return View();
            }

            return View(authors);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
