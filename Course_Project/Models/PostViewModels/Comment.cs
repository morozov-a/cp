using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Course_Project.Models.PostViewModels
{
    public class Comment
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public  ApplicationUser Author { get; set; } = new ApplicationUser();

        public string AuthorId { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public string PostId { get; set; }

        public Post Post { get; set; } = new Post();

        public List<Like> Likes { get; set; } = new List<Like>();

        [Required]
        public string Text { get; set; }


    }
}
