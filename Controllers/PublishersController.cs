using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Helga_ProiectMPA.Data;
using Helga_ProiectMPA.Models;
using Helga_ProiectMPA.Models.LibraryViewModels;

namespace Helga_ProiectMPA.Controllers
{
    public class PublishersController : Controller
    {
        private readonly LibraryContext _context;

        public PublishersController(LibraryContext context)
        {
            _context = context;
        }

        // GET: Publishers
        public async Task<IActionResult> Index(int? id, int? playlistID)
        {
            var viewModel = new PublisherIndexData();
            viewModel.Publishers = await _context.Publishers
            .Include(i => i.PublishedPlaylists)
            .ThenInclude(i => i.Playlist)
            .ThenInclude(i => i.Orderings)
            .ThenInclude(i => i.Buyer)
            .AsNoTracking()
            .OrderBy(i => i.PublisherName)
            .ToListAsync();
            if (id != null)
            {
                ViewData["PublisherID"] = id.Value;
                Publisher publisher = viewModel.Publishers.Where(i => i.ID == id.Value).Single();
                viewModel.Playlists = publisher.PublishedPlaylists.Select(s => s.Playlist);
            }
            if (playlistID != null)
            {
                ViewData["PlaylistID"] = playlistID.Value;
                viewModel.Orderings = viewModel.Playlists.Where(x => x.ID == playlistID).Single().Orderings;
            }
            return View(viewModel);
        }

        // GET: Publishers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Publishers == null)
            {
                return NotFound();
            }

            var publisher = await _context.Publishers
                .FirstOrDefaultAsync(m => m.ID == id);
            if (publisher == null)
            {
                return NotFound();
            }

            return View(publisher);
        }

        // GET: Publishers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Publishers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,PublisherName,Adress")] Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                _context.Add(publisher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(publisher);
        }

        // GET: Publishers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var publisher = await _context.Publishers
            .Include(i => i.PublishedPlaylists).ThenInclude(i => i.Playlist)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == id);

            if (publisher == null)
            {
                return NotFound();
            }
            PopulatePublishedPlaylistData(publisher);
            return View(publisher);

        }
        private void PopulatePublishedPlaylistData(Publisher publisher)
        {
            var allPlaylists = _context.Playlists;
            var publisherPlaylists = new HashSet<int>(publisher.PublishedPlaylists.Select(c => c.PlaylistID));
            var viewModel = new List<PublishedPlaylistData>();
            foreach (var playlist in allPlaylists)
            {
                viewModel.Add(new PublishedPlaylistData
                {
                    PlaylistID = playlist.ID,
                    Title = playlist.Title,
                    IsPublished = publisherPlaylists.Contains(playlist.ID)
                });
            }
            ViewData["Playlists"] = viewModel;
        }

        // POST: Publishers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,PublisherName,Adress")] Publisher publisher)
        {
            if (id != publisher.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(publisher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PublisherExists(publisher.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(publisher);
        }

        // GET: Publishers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Publishers == null)
            {
                return NotFound();
            }

            var publisher = await _context.Publishers
                .FirstOrDefaultAsync(m => m.ID == id);
            if (publisher == null)
            {
                return NotFound();
            }

            return View(publisher);
        }

        // POST: Publishers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Publishers == null)
            {
                return Problem("Entity set 'LibraryContext.Publishers'  is null.");
            }
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher != null)
            {
                _context.Publishers.Remove(publisher);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PublisherExists(int id)
        {
          return _context.Publishers.Any(e => e.ID == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedPlaylists)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publisherToUpdate = await _context.Publishers
            .Include(i => i.PublishedPlaylists)
            .ThenInclude(i => i.Playlist)
            .FirstOrDefaultAsync(m => m.ID == id);

            if (await TryUpdateModelAsync<Publisher>(publisherToUpdate,"",i => i.PublisherName, i => i.Adress))
            {
                UpdatePublishedPlaylists(selectedPlaylists, publisherToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {

                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, ");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdatePublishedPlaylists(selectedPlaylists, publisherToUpdate);
            PopulatePublishedPlaylistData(publisherToUpdate);
            return View(publisherToUpdate);
        }
        private void UpdatePublishedPlaylists(string[] selectedPlaylists, Publisher publisherToUpdate)
        {
            if (selectedPlaylists == null)
            {
                publisherToUpdate.PublishedPlaylists = new List<PublishedPlaylist>();
                return;
            }
            var selectedPlaylistsHS = new HashSet<string>(selectedPlaylists);
            var publishedPlaylists = new HashSet<int>
            (publisherToUpdate.PublishedPlaylists.Select(c => c.Playlist.ID));
            foreach (var playlist in _context.Playlists)
            {
                if (selectedPlaylistsHS.Contains(playlist.ID.ToString()))
                {
                    if (!publishedPlaylists.Contains(playlist.ID))
                    {
                        publisherToUpdate.PublishedPlaylists.Add(new PublishedPlaylist{ PublisherID =publisherToUpdate.ID,PlaylistID = playlist.ID});
                    }
                }
                else
                {
                    if (publishedPlaylists.Contains(playlist.ID))
                    {
                        PublishedPlaylist playlistToRemove = publisherToUpdate.PublishedPlaylists.FirstOrDefault(i=> i.PlaylistID == playlist.ID);
                        _context.Remove(playlistToRemove);
                    }
                }
            }
        }

    }
}
