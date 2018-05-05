using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Course_Project.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [RegularExpression(@"^[A-Za-zа-яА-Я]+$", ErrorMessage = "Имя может содержать только буквы")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Длина имени должна быть от 2 до 50 символов")]
        public string Firstname { get; set; }

        [RegularExpression(@"^[A-Za-zа-яА-Я]+$", ErrorMessage = "Фамилия может содержать только буквы")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Длина фамилии должна быть от 2 до 50 символов")]
        public string Lastname { get; set; }

        [RegularExpression(@"^(?:1(?:00?|\d)|[2-5]\d|[6-9]\d?)$", ErrorMessage = "Возраст от 6 до 100 лет")]     
        public int Age { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }
    }
}
