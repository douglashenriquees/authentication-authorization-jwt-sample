using AuthSample.Models;

namespace AuthSample.Repositories;

public static class UserRepository
{
    public static User Get(string userName, string password)
    {
        var users = new List<User>();
        return users.First();
    }
}