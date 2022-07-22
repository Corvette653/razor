using razor.Models;
namespace razor.Services;

public static class AuthentificationService
{
    static List<User> Users { get; }
    static int nextId = 4;

    static AuthentificationService()
    {
        Users = new List<User>
        {
            new User { Id = 1, Role = UserRole.SuperAdmin , Username = "Superadmin", Password = "zy!fmk9Z>81R" },
            new User { Id = 2, Role = UserRole.Admin, Username = "Admin", Password = "GtCp/z9*hZaR" },
            new User { Id = 3, Role = UserRole.User, Username = "User", Password = "AYqZ)3EN8?MN" }
        };
    }
    public static User? Get(string username)
    {
        return Users.FirstOrDefault(u => u.Username == username, null);
    }
    public static User? Get(int id)
    {
        return Users.FirstOrDefault(u => u.Id == id, null);
    }
    public static User? AddUser(UserRole role, string username, string password)
    {
        if (Get(username) != null)
            return null;
        User newUser = new User
        {
            Id = nextId,
            Role = role,
            Username = username,
            Password = password
        };
        nextId++;
        Users.Add(newUser);
        return newUser;
    }
    public static bool DeleteUser(string username)
    {
        User? user = Get(username);
        if (user == null)
            return false;
        Users.Remove(user);
        return true;
    }
    public static bool LoginUser(string sCode, User AnonymousU)
    {
        User? LoggedU = Get(AnonymousU.Username);
        if (LoggedU == null || LoggedU.Password != AnonymousU.Password)
            return false;

        SessionService.SetSessionUser(sCode, LoggedU.Id);
        return true;
    }
    public static void LogOutUser(string sCode)
    {
        SessionService.SetSessionUser(sCode, -1);
    }
}