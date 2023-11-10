using Microsoft.EntityFrameworkCore;

namespace WebApplication4.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<PersonModel> Persons { get; set; }
        public DbSet<TeamModel> Teams { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RegisteredUserModel> RegisteredUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonModel>()
                .HasOne(p => p.Role)
                .WithMany()
                .HasForeignKey(p => p.RoleId)
                .OnDelete(DeleteBehavior.Restrict); // Установите действие ON DELETE NO ACTION

            // Другие настройки моделей...

            base.OnModelCreating(modelBuilder);
        }
    }
}