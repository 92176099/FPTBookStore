using FPTBookStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using FPTBookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace FPTBookStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly FPTBookContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(FPTBookContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            dynamic model = new System.Dynamic.ExpandoObject();
            List<Book> books= await _context.Books.ToListAsync();
            List<Book> allGenres =  books.OrderByDescending(s => s.BookSells).Take(4).ToList();
            List<Book> fantasy = books.Where(s=>s.GenreId==8).Take(4).ToList();
            List<Book> romantic = books.Where(s => s.GenreId == 3).Take(4).ToList();
            List<Book> comic = books.Where(s => s.GenreId == 4).Take(4).ToList();
            List<Book> business = books.Where(s => s.GenreId == 12).Take(4).ToList();
            Book bestSell = books.OrderByDescending(s => s.BookSells).First();
            model.BestSell = bestSell;
            model.AllGenres = allGenres;
            model.Fantasy = fantasy;
            model.Romantic = romantic;
            model.Comic = comic;
            model.Business = business;
            return View(model);
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