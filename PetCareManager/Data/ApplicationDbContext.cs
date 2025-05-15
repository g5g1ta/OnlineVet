using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetCareManager.Models;

namespace PetCareManager.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<VetSchedule> VetSchedules { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        
            builder.Entity<Appointment>()
                .HasOne(a => a.Vet)
                .WithMany()
                .HasForeignKey(a => a.VetId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MedicalRecord>()
                .HasOne(m => m.Vet)
                .WithMany()
                .HasForeignKey(m => m.VetId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<VetSchedule>()
                .HasOne(vs => vs.Vet)
                .WithMany() 
                .HasForeignKey(vs => vs.VetId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Pet>()
                .HasOne(p => p.Owner)
                .WithMany()
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}