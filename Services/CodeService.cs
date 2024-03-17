using _2FA.Context;
using _2FA.Extensions;
using _2FA.Models;
using _2FA.Utilities;
namespace _2FA.Services
{
    public static class CodeService
	{
		private static int codeLifetimeInMinutes { get; } = AppConfiguration.GetValue<int>("AppSettings:CodeLifetimeInMinutes");
		private static int maxConcurrentCodes { get; } = AppConfiguration.GetValue<int>("AppSettings:MaximumConcurrentCodes");
		public static bool StoreCodeAfterExpiry { get; } = AppConfiguration.GetValue<bool>("AppSettings:StoreCodeAfterExpiry");
		public static IResult GenerateCode(string authToken, string mobile)
		{
			User user = UserService.UserFromToken(authToken);
			if (user.Mobile != mobile)
				return Results.Unauthorized();
			int sentCodeCount = user.Codes.Count(c =>
				!StoreCodeAfterExpiry || 
				c.GenerationTime > DateTime.Now.AddMinutes(-codeLifetimeInMinutes));
			if (sentCodeCount >= maxConcurrentCodes)
				return Results.Problem("Maximum Codes Issued. Please try later", null, StatusCodes.Status429TooManyRequests);
			using (DatabaseContext _dbContext = new())
			{
				Code newCode = new()
				{
					Mobile = mobile,
					GenerationTime = DateTime.Now,
					CodeValue = GenerateRandomCode()
				};
				_dbContext.Codes.Add(newCode);
				_dbContext.SaveChanges();
				newCode.SendCodeBySMS();
				return Results.Ok();
			}
		}
		public static IResult ValidateCode(string authToken, string mobile, string code)
		{
			User user = UserService.UserFromToken(authToken);
			if (user.Mobile != mobile)
				return Results.Unauthorized();
			Code selectedCode = user.Codes.FirstOrDefault(c => c.CodeValue == code);
			if (selectedCode == null)
				return Results.Problem("Invalid or Expired Code", null, StatusCodes.Status406NotAcceptable);
			selectedCode.UtilizationTime = DateTime.Now;
			using (DatabaseContext _dbContext = new())
			{
				_dbContext.Codes.Update(selectedCode);
				_dbContext.SaveChanges();
				return Results.Ok();
			}
		}
		private static string GenerateRandomCode()
		{
			var chars = "0123456789";
			Random random = new();
			return new(
				Enumerable.Repeat(chars, 6)
						  .Select(s => s[random.Next(s.Length)])
						  .ToArray());
		}
	}
}