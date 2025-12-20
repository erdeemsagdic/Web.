using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SPORSALONUYONETIM.Models;

namespace SPORSALONUYONETIM.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Service> Services { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

          
            builder.Entity<Service>()
                .HasOne(s => s.Trainer)
                .WithMany()
                .HasForeignKey(s => s.TrainerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
