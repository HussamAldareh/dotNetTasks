
using Microsoft.EntityFrameworkCore;
using Task2_MVC_.Models;



namespace Task2_MVC_.Model
{
    public class AppDb :DbContext

    {
        public AppDb(DbContextOptions<AppDb> options)
            : base(options)
        {
        }


        public DbSet<Category> categories { get; set; }
        public DbSet<Products> products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Products>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
