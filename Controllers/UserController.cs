using _2FA.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace _2FA.Controllers
{
	[Route("user")]
	[ApiController]
	[AllowAnonymous]
	public class UserController : Controller
	{
		/// <summary>
		/// Register A New User
		/// </summary>
		/// <param name="user">User Details: Mobile Number & Password</param>
		/// <returns></returns>
		[HttpPost("register")]
		//[ProducesResponseType(StatusCodes.Status201Created)]
		public IResult Register(string mobile, string password) => UserService.Register(mobile, password);
		/// <summary>
		/// Validate A Registered User
		/// </summary>
		/// <param name="user">User Details: Mobile Number & Password</param>
		/// <returns></returns>
		[HttpPost("login")]
		public IResult Login(string mobile, string password) => UserService.Login(mobile, password);
	}
}