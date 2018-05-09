using Course_Project.Data;
using Course_Project.Models;
using Course_Project.Models.PostViewModels;
using Course_Project.Services;
using Imgur.API;
using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using RotativaCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WkWrap.Core;

namespace Course_Project.Controllers
{
    
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public PostsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Posts.Include(a => a.Author);
            return View(/*await applicationDbContext.ToListAsync()*/);
        }

        [AllowAnonymous]
        public async Task<IActionResult> DisplaySingleCategory()
        {
            var applicationDbContext = _context.Posts.Include(a => a.Author);
            return View(await applicationDbContext.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title, Abstract,Content,Category,TagString")] PostViewModel post)
        {
            if (ModelState.IsValid)
            {
                char[] delimeterChars = { ' ', ',' };
                if (post.TagString != null)
                {
                    string[] words = post.TagString.Split(delimeterChars);
                    foreach (var word in words)
                    {
                        post.Tags.Add(new TagViewModel() { Name = word });
                    }
                }
                if (post.Picture == null)
                {
                    var img = await _context.Source.FindAsync("DefaultUser");
                    post.Picture = img.Picture;
                }
                post.LastModified = DateTime.UtcNow;
                post.ParentId = post.Id;
                post.Author = await _userManager.GetUserAsync(User);
                post.CreatedDate = DateTime.UtcNow;
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.Include(a => a.Author).SingleOrDefaultAsync(m => m.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(string postId, PostViewModel post)
        {
            var commentedPost = await _context.Posts.SingleOrDefaultAsync(m => m.Id == postId);
            _context.Comments.Add(new CommentViewModel()
            {
                Text = post.Comment,
                CreatedDate = DateTime.UtcNow,
                Author = await _userManager.GetUserAsync(User),
                PostId = commentedPost.ParentId,
            });
            _context.SaveChanges();
            return Redirect("Details/" + postId);
        }

        [HttpPost]
        public async Task<IActionResult> AddLike(string Id, string postId)
        {

            var comment = _context.Comments.Include(a => a.Author).Include(a => a.Liked).SingleOrDefault(m => m.Id == Id);
            foreach (var user in comment.Liked.Where(a => a.Id == _userManager.GetUserId(User)))
            {
                return RedirectToAction("Details/" + postId);
            }
            comment.Likes += 1;
            comment.Liked.Add(await _userManager.GetUserAsync(User));
            _context.SaveChanges();
            return RedirectToAction("Details/" + postId);
        }
    }
}