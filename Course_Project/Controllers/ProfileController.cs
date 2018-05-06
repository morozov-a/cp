using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Course_Project.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        public IActionResult SetPassword()
        {
            return View();
        }
        public IActionResult ExternalLogins()
        {
            return View();
        }
        public IActionResult MyNews()
        {
            return View();
        }
    }
}