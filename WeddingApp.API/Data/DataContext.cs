using WeddingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace WeddingApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}

        public DbSet<Value> Values { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Photo> Photos { get; set; }

        public DbSet<Place> Places { get; set; }

        public DbSet<PhotoForPlace> PhotosForPlaces { get; set; }
        
        public DbSet<Reservation> Reservations { get; set; }
    }
}