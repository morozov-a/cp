using Course_Project.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course_Project.ViewComponents
{
    public class LatestPostsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public LatestPostsViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int howMany)
        {
            var lastPost = await _context.Posts
                                            .OrderByDescending(a => a.LastModified)
                                            .Include(a => a.Author)
                                            .Take(howMany)
                                            .ToListAsync();
            return View(lastPost);
        }
    }
}
