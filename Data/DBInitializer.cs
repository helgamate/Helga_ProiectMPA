using Microsoft.EntityFrameworkCore;
using Helga_ProiectMPA.Models;
using System.Runtime.ConstrainedExecution;

namespace Helga_ProiectMPA.Data
{
    public class DBInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new
           LibraryContext(serviceProvider.GetRequiredService
            <DbContextOptions<LibraryContext>>()))
            {
                if (context.Playlists.Any())
                {
                    return; // BD a fost creata anterior
                }
                
                var orderings = new Ordering[]
{
                    new Ordering{PlaylistID=1,BuyerID=1050,OrderingDate=DateTime.Parse("2021-02-25")},
                    new Ordering{PlaylistID=3,BuyerID=1045,OrderingDate=DateTime.Parse("2021-09-28")},
                    new Ordering{PlaylistID = 1,BuyerID=1045,OrderingDate=DateTime.Parse("2021-10-28")},
                    new Ordering{PlaylistID=2,BuyerID=1050,OrderingDate=DateTime.Parse("2021-09-28")},
                    new Ordering{PlaylistID=4,BuyerID=1050,OrderingDate=DateTime.Parse("2021-09-28")},
                    new Ordering{PlaylistID=6,BuyerID=1050,OrderingDate=DateTime.Parse("2021-10-28")},
};
                foreach (Ordering e in orderings)
                {
                    context.Orderings.Add(e);
                }
                context.SaveChanges();
                var publishers = new Publisher[]
                {

                    new Publisher{PublisherName="Musical",Adress="Str. Aviatorilor, nr. 40, Bucuresti"},
                    new Publisher{PublisherName="Lyrics",Adress="Str. Plopilor, nr. 35, Ploiesti"},
                    new Publisher{PublisherName="Notes",Adress="Str. Cascadelor, nr.22, Cluj-Napoca"},
                }
                foreach (Publisher p in publishers)
                {
                    context.Publishers.Add(p);
                }
                context.SaveChanges();
                var playlists = context.Playlists;
                var publishedplaylists = new PublishedPlaylist[]
                {
                    new PublishedPlaylist {PlaylistID = playlists.Single(c => c.Title == "Bad Blood" ).ID,
                        PublisherID = publishers.Single(i => i.PublisherName =="Musical").ID},
                    new PublishedPlaylist {PlaylistID = playlists.Single(c => c.Title == "Africa" ).ID,
                        PublisherID = publishers.Single(i => i.PublisherName =="Musical").ID},
                    new PublishedPlaylist {PlaylistID = playlists.Single(c => c.Title == "Umbrella" ).ID,
                        PublisherID = publishers.Single(i => i.PublisherName =="Lyrics").ID},
                    new PublishedPlaylist {PlaylistID = playlists.Single(c => c.Title == "Tik Tok" ).ID,
                        PublisherID = publishers.Single(i => i.PublisherName == "Lyrics").ID },
                    new PublishedPlaylist {PlaylistID = playlists.Single(c => c.Title == "Just Dance" ).ID,
                        PublisherID = publishers.Single(i => i.PublisherName == "Notes").ID},
                    new PublishedPlaylist {PlaylistID = playlists.Single(c => c.Title == "Toxic" ).ID,
                        PublisherID = publishers.Single(i => i.PublisherName == "Notes").ID},
                };
                foreach (PublishedPlaylist pb in publishedplaylists)
                {
                    context.PublishedPlaylists.Add(pb);
                }
                context.SaveChanges();
            }
        }
    }
}