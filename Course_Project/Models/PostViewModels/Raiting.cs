using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course_Project.Models.PostViewModels
{
    public class Raiting
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public Post ParentPost { get; set; }

        public string PostId { get; set; }

        public string UserId { get; set; }

        public int Value { get; set; }
    }
}
