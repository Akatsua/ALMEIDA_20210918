using Gateway.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Infrastructure
{
    public class CategoryRepository
    {
        private readonly CategoryContext CategoryContext;

        public CategoryRepository(CategoryContext categoryContext)
        {
            CategoryContext = categoryContext ?? 
                throw new ArgumentNullException(nameof(categoryContext));
        }

        public async Task<IEnumerable<string>> GetCategoriesAsync()
        {
            return await CategoryContext
                .Categories
                .Select(category => category.CategoryName)
                .ToListAsync();
        }
    }
}
