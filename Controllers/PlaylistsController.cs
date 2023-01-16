using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Helga_ProiectMPA.Data;
using Helga_ProiectMPA.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace Helga_ProiectMPA.Controllers
{
    public class PlaylistsController : Controller
    {
        private readonly LibraryContext _context;

        public PlaylistsController(LibraryContext context)
        {
            _context = context;
        }

        // GET: Playlists
        public async Task<IActionResult> Index(
        string sortOrder,
        string currentFilter,
        string searchString,
        int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            if (searchString != null)
            {
                pageNumber = 1;
            } 
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var playlists = from b in _context.Playlists
                        select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                playlists = playlists.Where(s => s.Title.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "title_desc":
                    playlists = playlists.OrderByDescending(b => b.Title);
                    break;
                case "Price":
                    playlists = playlists.OrderBy(b => b.Price);
                    break;
                case "price_desc":
                    playlists = playlists.OrderByDescending(b => b.Price);
                    break;
                default:
                    playlists = playlists.OrderBy(b => b.Title);
                    break;
            }
            int pageSize = 2;
            return View(await PaginatedList<Playlist>.CreateAsync(playlists.AsNoTracking(), pageNumber ??
           1, pageSize));
        }

        // GET: Playlists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Playlists == null)
            {
                return NotFound();
            }
            var playlist = await _context.Playlists
              .Include(s => s.Orderings)
              .ThenInclude(e => e.Buyer)
              .AsNoTracking()
              .FirstOrDefaultAsync(m => m.ID == id);


            if (playlist == null)
            {
                return NotFound();
            }

            return View(playlist);
        }

        // GET: Playlists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Playlists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Author,Price")] Playlist playlist)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(playlist);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex*/)
            {

                ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists ");
            }
            return View(playlist);
        }

        // GET: Playlists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Playlists == null)
            {
                return NotFound();
            }

            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist == null)
            {
                return NotFound();
            }
            return View(playlist);
        }

        // POST: Playlists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")] 
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var playlistToUpdate = await _context.Playlists.FirstOrDefaultAsync(s => s.ID == id);
            if (await TryUpdateModelAsync<Playlist>(playlistToUpdate,"",s => s.Author, s => s.Title, s => s.Price))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +"Try again, and if the problem persists");
                    
             
                }
               
            }
            return null;

        }

        // GET: Playlists/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }
            var playlist = await _context.Playlists
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == id);
            if (playlist == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                "Delete failed. Try again";
            }

            return View(playlist);
        }

        // POST: Playlists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Playlists.Remove(playlist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            catch (DbUpdateException /* ex */)
            {

                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }
    }
}
