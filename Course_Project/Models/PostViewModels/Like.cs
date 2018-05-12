using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course_Project.Models.PostViewModels
{
    public class Like
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public Comment ParentComment { get; set; }

        public string CommentId { get; set; } 

        public string UserId { get; set; }




    }
}
