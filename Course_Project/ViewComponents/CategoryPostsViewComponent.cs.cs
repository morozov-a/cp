using Course_Project.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course_Project.ViewComponents
{
    public class CategoryPostsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public CategoryPostsViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string category, int howMany)
        {
            var lastPost = await _context.Posts
                                            .Where(a => a.Category == category)
                                            .Include(a => a.Author)
                                            .Take(howMany)
                                            .ToListAsync();

            return View(lastPost);
        }
    }
}
