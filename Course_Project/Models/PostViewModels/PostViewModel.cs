using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Course_Project.Models.PostViewModels
{
    public class PostViewModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Picture { get; set; }


        public string Category { get; set; }

        public string TagString { get; set; }

        public string ParentId { get; set; }
        
        public List<TagViewModel> Tags { get; set; } = new List<TagViewModel>();

        [Required]
        public string Title { get; set; }

        public string Abstract { get; set; }

        [Required]
        public string Content { get; set; }

        public string Comment { get; set; }

        public List<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();

        public ApplicationUser Author { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModified { get; set; }
    }
}
