using MedicalArchive.API.Domain.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

namespace MedicalArchive.API.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<DoctorAccess> DoctorAccesses { get; set; }
        public DbSet<DoctorAppointment> DoctorAppointments { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Referral> Referrals { get; set; }
        public DbSet<Vaccination> Vaccinations { get; set; }
        public DbSet<MedicalDocument> MedicalDocuments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User конфігурація
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Doctor конфігурація
            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // DoctorAccess конфігурація
            modelBuilder.Entity<DoctorAccess>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(d => d.User)
                    .WithMany(u => u.DoctorAccesses)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict); // Обмеження видалення

                entity.HasOne(d => d.Doctor)
                    .WithMany(u => u.DoctorAccesses)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.Restrict); // Обмеження видалення
            });

            // DoctorAppointment конфігурація
            modelBuilder.Entity<DoctorAppointment>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(d => d.User)
                    .WithMany(u => u.DoctorAppointments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade); // Каскадне видалення
            });

            // Prescription конфігурація
            modelBuilder.Entity<Prescription>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(p => p.User)
                    .WithMany(u => u.Prescriptions)
                    .HasForeignKey(p => p.UserId)
                    .OnDelete(DeleteBehavior.Cascade); // Каскадне видалення
            });

            // Referral конфігурація
            modelBuilder.Entity<Referral>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(r => r.User)
                    .WithMany(u => u.Referrals)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Cascade); // Каскадне видалення
            });

            // Vaccination конфігурація
            modelBuilder.Entity<Vaccination>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(v => v.User)
                    .WithMany(u => u.Vaccinations)
                    .HasForeignKey(v => v.UserId)
                    .OnDelete(DeleteBehavior.Cascade); // Каскадне видалення
            });

            // MedicalDocument конфігурація
            modelBuilder.Entity<MedicalDocument>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(md => md.User)
                    .WithMany(u => u.MedicalDocuments)
                    .HasForeignKey(md => md.UserId)
                    .OnDelete(DeleteBehavior.Cascade); // Каскадне видалення

                // Опціональні зв'язки
                entity.HasOne(md => md.DoctorAppointment)
                    .WithMany(da => da.Documents)
                    .HasForeignKey(md => md.DoctorAppointmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull); // Не видаляти документ при видаленні прийому

                entity.HasOne(md => md.Prescription)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(md => md.PrescriptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull); // Не видаляти документ при видаленні рецепту

                entity.HasOne(md => md.Referral)
                    .WithMany(r => r.Documents)
                    .HasForeignKey(md => md.ReferralId)
                    .OnDelete(DeleteBehavior.ClientSetNull); // Не видаляти документ при видаленні направлення

                entity.HasOne(md => md.Vaccination)
                    .WithMany(v => v.Documents)
                    .HasForeignKey(md => md.VaccinationId)
                    .OnDelete(DeleteBehavior.ClientSetNull); // Не видаляти документ при видаленні щеплення
            });
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var entity = (BaseEntity)entityEntry.Entity;

                if (entityEntry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                }
                else
                {
                    entity.UpdatedAt = DateTime.UtcNow;
                }
            }
        }
    }
}
