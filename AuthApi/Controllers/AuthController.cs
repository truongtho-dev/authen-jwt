using AuthApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		[HttpPost("login")]
		public IActionResult Login([FromBody] LoginModel user)
		{
			if (user is null) return BadRequest("Invalid client request");

			if (user.UserName == "david" && user.Password == "123456")
			{
				var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is the super secret key"));
				var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
				var tokeOptions = new JwtSecurityToken(
					issuer: "https://localhost:5001",
					audience: "https://localhost:5001",
					claims: new List<Claim>(),
					expires: DateTime.Now.AddMinutes(5),
					signingCredentials: signinCredentials
				);
				var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
				return Ok(new AuthenticatedResponse { Token = tokenString });
			}

			return Unauthorized();
		}
	}
}
