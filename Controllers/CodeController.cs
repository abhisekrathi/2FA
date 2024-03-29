﻿using _2FA.Filters;
using _2FA.Services;
using Microsoft.AspNetCore.Mvc;
namespace _2FA.Controllers
{
	[Route("code")]
	[ApiController]
	[UserAuthenticate]
	public class CodeController : Controller
	{
		/// <summary>
		/// Generate A Code For Particular Mobile Number (For Authorized User)
		/// </summary>
		/// <param name="mobile"></param>
		/// <returns></returns>
		[HttpGet("generate")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status429TooManyRequests)]
		public IResult GenerateCode(string mobile) => CodeService.GenerateCode(Request.Headers.Authorization.ToString()["Bearer ".Length..].Trim(), mobile);
		/// <summary>
		/// Validate The Code shared by user against the given Mobile Number
		/// </summary>
		/// <param name="mobile"></param>
		/// <param name="code"></param>
		/// <returns></returns>
		[HttpGet("validate")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status406NotAcceptable)]
		public IResult ValidateCode(string mobile, string code) => CodeService.ValidateCode(Request.Headers.Authorization.ToString()["Bearer ".Length..].Trim(), mobile, code);
	}
}