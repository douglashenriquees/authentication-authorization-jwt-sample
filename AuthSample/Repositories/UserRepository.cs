using AuthSample.Models;

namespace AuthSample.Repositories;

public static class UserRepository
{
	public static User? Get(string userName, string password)
	{
		var users = new List<User>();

		users.Add(new User()
		{
			Id = 1,
			UserName = "batman",
			Password = "batman",
			Role = "manager"
		});

		users.Add(new User()
		{
			Id = 2,
			UserName = "robin",
			Password = "robin",
			Role = "employee"
		});

		return users.FirstOrDefault(x =>
			x.UserName.ToLower().Equals(userName.ToLower()) &&
			x.Password.ToLower().Equals(password.ToLower()));
	}
}