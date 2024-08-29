using Microsoft.EntityFrameworkCore;

namespace PhanTranMinhTam_TestLan2.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Music> Musics { get; set; }
        public DbSet<MusicGenre> MusicGenres { get; set; }
        public DbSet<Schedule> Schedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Music 
            modelBuilder.Entity<Music>()
                .HasKey(m => m.MusicId);
            modelBuilder.Entity<Music>()
                .HasOne(m => m.MusicGenre)
                .WithMany(g => g.Musics)
                .HasForeignKey(m => m.GenreId)
                .OnDelete(DeleteBehavior.Restrict);

            // MusicGenre
            modelBuilder.Entity<MusicGenre>()
                .HasKey(g => g.GenreId);

            // Schedule
            modelBuilder.Entity<Schedule>()
                .HasKey(s => s.ScheduleId);
            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Music)
                .WithMany(m => m.Schedules)
                .HasForeignKey(s => s.MusicId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ensure that the combination of DayOfWeek, StartDate, EndDate, StartTime, and EndTime is unique
            modelBuilder.Entity<Schedule>()
                .HasIndex(s => new { s.DayOfWeek, s.StartDate, s.EndDate, s.StartTime, s.EndTime })
                .IsUnique();
        }
    }
}
