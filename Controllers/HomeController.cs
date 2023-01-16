using Microsoft.EntityFrameworkCore;
using Helga_ProiectMPA.Data;
using Helga_ProiectMPA.Models.LibraryViewModels;
using Helga_ProiectMPA.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Helga_ProiectMPA.Controllers
{
    public class HomeController : Controller
    {
        private readonly LibraryContext _context;
        public HomeController(LibraryContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
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
        public async Task<ActionResult> Statistics()
        {
            IQueryable<OrderGroup> data =
            from order in _context.Orderings
            group order by order.OrderingDate into dateGroup
            select new OrderGroup()
            {
                OrderingDate = dateGroup.Key,
                PlaylistCount = dateGroup.Count()
            };
            return View(await data.AsNoTracking().ToListAsync());
        }
        public IActionResult Chat()
        {
            return View();
        }

    }
}