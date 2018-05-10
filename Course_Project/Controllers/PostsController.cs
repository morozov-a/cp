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
using System.Threading;
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
            return View(await applicationDbContext.ToListAsync());
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
        public async Task<IActionResult> Create([Bind("Title, Abstract,Content,Category,TagString")] Post post)
        {
            if (ModelState.IsValid)
            {
                char[] delimeterChars = { ' ', ',' };
                if (post.TagString != null)
                {
                    string[] words = post.TagString.Split(delimeterChars);
                    foreach (var word in words)
                    {
                        post.Tags.Add(new Tag() { Name = word });
                    }
                }
                if (post.Picture == null)
                {
                    var img = await _context.Source.FindAsync("DefaultUser");
                    post.Picture = img.Picture;
                }
                post.LastModified = DateTime.UtcNow;
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
        public async Task<IActionResult> AddComment(string postId, Post post)
        {
            var user = await _userManager.GetUserAsync(User);
            var commentedPost = await _context.Posts.SingleOrDefaultAsync(m => m.Id == postId);
            _context.Comments.Add(new Comment()
            {
                Text = post.Comment,
                CreatedDate = DateTime.UtcNow,
                Author = user,
                PostId = commentedPost.Id

            });
            _context.SaveChanges();
           
            return PartialView("_CommentsBody", commentedPost);
        }

        [HttpPost]
        public async Task<IActionResult> AddLike(string Id, string postId)
        {
            var commentedPost = await _context.Posts.SingleOrDefaultAsync(m => m.Id == postId);
            var comment = _context.Comments.Include(a => a.Author).Include(a => a.Liked).SingleOrDefault(m => m.Id == Id);
            foreach (var user in comment.Liked.Where(a => a.Id == _userManager.GetUserId(User)))
            {
                return PartialView("_CommentsBody", commentedPost);
            }
            comment.Likes += 1;
            comment.Liked.Add(await _userManager.GetUserAsync(User));
            _context.SaveChanges();
            return PartialView("_CommentsBody", commentedPost);
        }

        public async Task<IActionResult> UploadImageAsync(IList<IFormFile> files)
        {
            var id = HttpContext.Request.Query["id"].ToString();
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var client = new ImgurClient("556830a80ac5829", "9438948e5e7df4b5151a61b882626c499ef4925e");
                var endpoint = new ImageEndpoint(client);
                IImage image;
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                       
                        using (var fileStream = file.OpenReadStream())
                        using (var ms = new MemoryStream())
                        {
                            fileStream.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            string s = Convert.ToBase64String(fileBytes);
                            image = await endpoint.UploadImageBinaryAsync(fileBytes);
                        }
                        Debug.Write("Image uploaded. Image Url: " + image.Link);
                        var current = _context.Posts.SingleOrDefault(a => a.Id == id);
                        current.Picture = image.Link;
                        _context.SaveChanges();
                    }
                }
            }
            catch (ImgurException imgurEx)
            {
                Debug.Write("An error occurred uploading an image to Imgur.");
                Debug.Write(imgurEx.Message);

            }
            return null;
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Posts.SingleOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Post post)
        {

            var parentPost = await _context.Posts.SingleOrDefaultAsync(m => m.Id == post.Id);
            if (post.TagString != null)
            {
                char[] delimeterChars = { ' ', ',' };
                string[] words = post.TagString.Split(delimeterChars);
                foreach (var word in words)
                {
                    if (await _context.Tags.FindAsync(word) == null)
                        post.Tags.Add(new Tag() { Name = word });
                }
            }
            parentPost.Content = post.Content;
            parentPost.Category = post.Category;
            parentPost.Abstract = post.Abstract;
            parentPost.TagString = post.TagString;
            parentPost.Title = post.Title;
            parentPost.LastModified = DateTime.UtcNow;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Posts.SingleOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {

            var article = await _context.Posts.Include(a => a.Tags).Include(a => a.Comments).Include(a => a.Author).SingleOrDefaultAsync(m => m.Id == id);
            foreach (var comment in article.Comments)
            {
                _context.Remove(comment);
            }
            
                _context.Posts.Remove(article);
           
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}