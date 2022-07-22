using razor.Models;

namespace razor.Services;

public static class SessionService
{
    static List<Session> Sessions { get; } = new List<Session>();

    public static Session? Get(string code)
    {
        return Sessions.FirstOrDefault(s => s.Code == code, null);
    }

    public static string CreateSessionCode()
    {
        Random rnd = new Random();
        string? x;
        do
        {
            x = "";
            for (int i = 0; i < 13; i++)
                x += (char)rnd.Next(33, 122);
        } while (Get(x) != null);
        return x;
    }
    public static Session CreateSession()
    {
        Session s = new Session
        {
            Code = CreateSessionCode(),
            ExpireDate = DateTime.Now.AddHours(1)
        };
        Sessions.Add(s);
        return s;
    }
    public static Session RenewSession(Session oldS)
    {
        if (oldS != null)
            Sessions.Remove(oldS);
        return CreateSession();
    }

    public static void ProlongSession(Session s)
    {
        s.ExpireDate = DateTime.Now.AddHours(1);
    }

    public static bool SetSessionUser(string sCode, int id)
    {
        Session? s = Get(sCode);
        if (s == null)
            return false;
        s.UserId = id;
        return true;
    }
}