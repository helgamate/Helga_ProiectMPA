using Helga_ProiectMPA.Models;
using Microsoft.EntityFrameworkCore;

namespace Helga_ProiectMPA.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
        }
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<Ordering> Orderings { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<PublishedPlaylist> PublishedPlaylists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Buyer>().ToTable("Buyer");
            modelBuilder.Entity<Ordering>().ToTable("Ordering");
            modelBuilder.Entity<Playlist>().ToTable("Playlist");
            modelBuilder.Entity<Publisher>().ToTable("Publisher");
            modelBuilder.Entity<PublishedPlaylist>().ToTable("PublishedPlaylist");
            modelBuilder.Entity<PublishedPlaylist>()
            .HasKey(c => new { c.PlaylistID, c.PublisherID });//configureaza cheia primara compusa
        }
    }
}
