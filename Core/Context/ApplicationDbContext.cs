using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StockWebApi.Core.Entities;

namespace StockWebApi.Core.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Marchandise> Marchandises { get; set; }
        public DbSet<Operation> Operations { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Marchandise>()
                .HasOne(Marchandise => Marchandise.Category)
                .WithMany(Category => Category.Marchandises)
                .HasForeignKey(Marchandise => Marchandise.CategoryId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Operation>()
                .HasOne(Operation => Operation.Marchandise)
                .WithMany(Marchandise => Marchandise.Operations)
                .HasForeignKey(Operation => Operation.Reference);

            modelBuilder.Entity<Category>()
                .HasIndex(u => u.Name)
                .IsUnique();


        }
    }
}
