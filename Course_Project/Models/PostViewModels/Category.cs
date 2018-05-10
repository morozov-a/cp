using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Course_Project.Models.PostViewModels
{
    public class Category
    {
        [Key]
        public string CategoryName { get; set; }
    }
}
