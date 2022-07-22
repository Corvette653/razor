using razor.Models;
using razor.Services;

public class SessionMiddleware
{
    RequestDelegate _next;

    public SessionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext ctx)
    {
        Session? s;
        if (ctx.Request.Cookies["SessionCookie"] == null)
            s = SessionService.CreateSession();
        else
        {
            s = SessionService.Get(ctx.Request.Cookies["SessionCookie"]);
            if(s == null || (s.ExpireDate < DateTime.Now && s.UserId == -1))
                s = SessionService.RenewSession(s);
            else if(s.ExpireDate < DateTime.Now && s.UserId != -1)
            {
                s = SessionService.RenewSession(s);
                ctx.Response.Cookies.Append("SessionCookie", s.Code);
                ctx.Response.Redirect("/login");
            }
            else
                SessionService.ProlongSession(s);
        }
        ctx.Response.Cookies.Append("SessionCookie", s.Code);
        ctx.Items.Add("SessionCookie", s.Code);
        await _next(ctx);
    }
}