using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace razor.Pages;
using razor.Models;
using razor.Services;
public class AuthPageModel : PageModel
{
    public User CurrUser { get; set; } = new User();
    public bool MessageExists { get; set; } = false;
    public string Message { get; set; } = "";
    public virtual void OnGet()
    {
        Session? s = SessionService.Get((string)this.HttpContext.Items["SessionCookie"]);
        CurrUser = AuthentificationService.Get(s.UserId);
    }
    public void OnPostLogOut()
    {
        AuthentificationService.LogOutUser((string)this.HttpContext.Items["SessionCookie"]);
        this.HttpContext.Response.Redirect("/");
    }
}