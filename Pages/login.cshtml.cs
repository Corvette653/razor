using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using razor.Models;
using razor.Services;

namespace razor.Pages
{
    public class loginModel : AuthPageModel
    {
        [BindProperty]
        public User loginUser { get; set; } = new User();
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();
            if(AuthentificationService.LoginUser((string)this.HttpContext.Items["SessionCookie"], loginUser))
                return Redirect("/");
            return Page();
        }
    }
}