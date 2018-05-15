using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Course_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Course_Project.Views.Profile
{
    public static class ManageNavPages
    {
        
        public static string Hidden {get;set;}

        public static string Disabled { get; set; }

        public static string userId { get; set; }

        public static string currentUserId { get; set; }

        public static string ActivePageKey => "ActivePage";

        public static string Index => "Index";

        public static string ChangePassword => "ChangePassword";

        

        public static string MyNews => "MyNews";

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);

        

        public static string MyNewsNavClass(ViewContext viewContext) => PageNavClass(viewContext, MyNews);
    
        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string;
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }

        public static void AddActivePage(this ViewDataDictionary viewData, string activePage) => viewData[ActivePageKey] = activePage;

}
}
