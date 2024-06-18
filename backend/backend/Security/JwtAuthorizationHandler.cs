
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace backend.Security;

public class JwtAuthorizationHandler : AuthorizationHandler<JwtRequirements>
{
	IHttpContextAccessor _httpContextAccessor = null;

	public JwtAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}
	// Get role from jwt 
	protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, JwtRequirements requirement)
    {

		var httpContext = _httpContextAccessor?.HttpContext;

		string authHeader = httpContext?.Request.Headers["Authorization"];

		if (!string.IsNullOrEmpty(authHeader)) 
		{
			var token = authHeader.Substring("Bearer ".Length).Trim();

			try 
			{
				var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
				Console.WriteLine(decodedToken.Claims);
				var role = decodedToken.Claims.Where(x => x.Key == "roles").Select(x => x.Value).FirstOrDefault();

				if (role != null && requirement.Roles.Contains(role))
				{
					context.Succeed(requirement);
				}
				else
				{
					context.Fail();
				}
			}
			catch (Exception ex) 
			{
				context.Fail();
			}
		} else
		{
			context.Fail();
		}
	}
}
