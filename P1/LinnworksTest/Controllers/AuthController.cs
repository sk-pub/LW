using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Linnworks.Api;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinnworksTest.Controllers
{
	[Produces("application/json")]
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : Controller
	{
		public class Account
		{
			public string Token { get; set; }
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> Login([FromBody] Account account)
		{
			if (account == null
				|| String.IsNullOrEmpty(account.Token)
				|| !(await Client.CheckToken(account.Token)))
			{
				ModelState.AddModelError("login_failure", "Invalid token.");
			}

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, account.Token)
			};
			var userIdentity = new ClaimsIdentity(claims, "login");
			var principal = new ClaimsPrincipal(userIdentity);
			await HttpContext.SignInAsync(principal);

			return new OkObjectResult(account.Token);
		}
	}
}
