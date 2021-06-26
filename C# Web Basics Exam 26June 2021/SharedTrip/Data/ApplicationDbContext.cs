namespace SharedTrip.Data
{
    using Microsoft.EntityFrameworkCore;
    using SharedTrip.Data.Models;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
            
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Trip> Trips { get; set; }

        public DbSet<UserTrip> UserTrips { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(DatabaseConfiguration.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTrip>()
                .HasKey(x => new { x.TripId, x.UserId });

            modelBuilder.Entity<UserTrip>()
                .HasOne(x => x.User).WithMany(x => x.UserTrips).HasForeignKey(x => x.UserId);

            modelBuilder.Entity<UserTrip>()
                .HasOne(x => x.Trip).WithMany(x => x.UserTrips).HasForeignKey(x => x.TripId);
        }
    }
}
