using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Gateway.Infrastructure.Database
{
    public class CategoryContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }

        public CategoryContext(DbContextOptions<CategoryContext> options) : 
            base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var categoryTable = modelBuilder.Entity<Category>().ToTable("category");

            // This can be generated from an enum for better safety of use
            categoryTable.HasData(new[]
            {
                new Category(1, "Exercice"),
                new Category(2, "Education"),
                new Category(3, "Recipe")
            });
        }
    }

    public class Category
    {
        public Category(int categoryId, string categoryName)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
