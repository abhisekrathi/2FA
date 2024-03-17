using _2FA.Context;
using _2FA.Extensions;
using _2FA.Models;
using _2FA.Utilities;
using Microsoft.EntityFrameworkCore;
namespace _2FA.Services
{
	public static class UserService
	{
		private static int loginKeyValidityInMinutes { get; } = AppConfiguration.GetValue<int>("AppSettings:LoginKeyValidityInMinutes");
		public static IResult Register(string mobile, string password)
		{
			if (string.IsNullOrEmpty(mobile) || string.IsNullOrEmpty(password))
				return Results.Problem("Both Mobile Number and Password Mandatory", null, StatusCodes.Status406NotAcceptable);

			using (DatabaseContext _dbContext = new())
			{
				if (_dbContext.Users.Any(u => u.Mobile == mobile))
					return Results.Problem("Mobile Number already registered", null, StatusCodes.Status406NotAcceptable);

				User user = new()
				{
					Mobile = mobile,
					Password = password.ComputeSHA256Hash()
				};
				_dbContext.Users.Add(user);
				_dbContext.SaveChanges();
				return Results.StatusCode(StatusCodes.Status201Created);
			}
		}
		public static IResult Login(string mobile, string password)
		{
			if (string.IsNullOrEmpty(mobile) || string.IsNullOrEmpty(password))
				return Results.Problem("Both Mobile Number and Password Mandatory", null, StatusCodes.Status406NotAcceptable);

			using (DatabaseContext _dbContext = new())
			{
				User loginUser = _dbContext.Users.FirstOrDefault(u => u.Mobile == mobile && u.Password == password.ComputeSHA256Hash());
				if (loginUser == null)
					return Results.Problem("Invalid Mobile Number or Password", null, StatusCodes.Status406NotAcceptable);

				string authenticationString = loginUser.Mobile + "~" + loginUser.Password + "~" + DateTime.Now.AddMinutes(loginKeyValidityInMinutes);
				return Results.Ok(authenticationString.Encrypt());
			}
		}
		public static User UserFromToken(string token, bool withCodes = true)
		{
			User loginUser = null;
			string decryptedToken = token.Decrypt();
			string[] decryptedTokenArray = decryptedToken.Split("~");
			if (decryptedTokenArray.Length >= 2)
			{
				string mobile = decryptedTokenArray[0];
				string password = decryptedTokenArray[1];
				string validTillString = decryptedTokenArray[2];
				if (DateTime.TryParse(validTillString, out DateTime validTill) && validTill >= DateTime.Now)
					using (DatabaseContext _dbContext = new())
						if (withCodes)
							loginUser = _dbContext.Users.Where(u => u.Mobile == mobile && u.Password == password).Include("Codes").FirstOrDefault();
						else
							loginUser = _dbContext.Users.FirstOrDefault(u => u.Mobile == mobile && u.Password == password);
			}
			return loginUser;
		}
	}
}