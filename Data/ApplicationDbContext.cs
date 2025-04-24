

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudyHub.Models;

namespace StudyHub.Data
{
    public class ApplicationDbContext: IdentityDbContext<CustomUser>
    {
        public DbSet<StudyRoom> StudyRooms { get; set; }
        public DbSet<Message> Messages { get; set; }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudyRoom>()
                .HasKey(r => r.RoomName); 

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Room)
                .WithMany(r => r.Messages)
                .HasForeignKey(m => m.RoomName)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
