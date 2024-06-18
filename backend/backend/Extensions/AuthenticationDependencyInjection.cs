using backend.Enums;
using backend.Security;
using backend.Services;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Nest;
using System.Text;

namespace backend.Extensions;

public static class AuthenticationDependencyInjection
{
	public static IServiceCollection RegisterAuthentication(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		//var jwtSecretKey = configuration.GetValue<string>("Jwt:Key");
		//var issuer = configuration.GetValue<string>("Jwt:Issuer");
		//var audience = configuration.GetValue<string>("Jwt:Audience");
		//if (jwtSecretKey != null)
		//{
		//	var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecretKey));

		//	services
		//		.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
		//		.AddJwtBearer(options =>
		//		{
		//			options.TokenValidationParameters = new TokenValidationParameters
		//			{
		//				ValidIssuer = issuer,
		//				ValidAudience = audience,
		//				ValidateIssuerSigningKey = true,
		//				IssuerSigningKey = key,
		//				ValidateIssuer = true,
		//				ValidateAudience = true,
		//				// set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
		//				ClockSkew = TimeSpan.Zero
		//			};
		//		});
		//}

		// Add JWT authentication
		services.AddAuthentication()
		.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtOptions =>
		{
			jwtOptions.Audience = configuration["Authentication:Audience"];
			jwtOptions.TokenValidationParameters.ValidIssuer =
				configuration["Authentication:ValidIssuer"];

		});
		return services;
	}

	public static IServiceCollection RegisterAuthorization(this IServiceCollection services)
	{
		services.AddAuthorization(options =>
		{
			// Define policy for 'admin' role
			options.AddPolicy(Consts.ADMIN, policy =>
			{
				var roles = new List<string>
				{
					Consts.ADMIN,
				};
				policy.Requirements.Add(new JwtRequirements(roles));
			});

			// Define policy for 'user' role
			options.AddPolicy(Consts.USER, policy =>
			{
				var roles = new List<string>
				{
					// Consts.ADMIN,
					Consts.USER
				};
				policy.Requirements.Add(new JwtRequirements(roles));
			});

			// Define policy for both roles
			options.AddPolicy(Consts.ALL, policy =>
			{
				var roles = new List<string>
				{
  					Consts.ADMIN,
					Consts.USER
				};
				policy.Requirements.Add(new JwtRequirements(roles));
			});

		});
		return services;
	}

	public static void AddInfrastructure(this IServiceCollection services,
		IConfiguration configuration)
	{
		FirebaseApp.Create(new AppOptions
		{
			Credential = GoogleCredential.FromFile("firebase.json")
		});

		services.AddScoped<IAuthenticationService, AuthenticationService>();

		services.AddHttpClient<IJwtProvider, JwtProvider>((sp, httpClient) =>
		{

			httpClient.BaseAddress = new Uri(configuration["Authentication:TokenUri"]);
		});

		// Configuration for 
		var settings = new ConnectionSettings(new Uri(configuration.GetConnectionString("Els")))
				.DefaultIndex("products");

		var client = new ElasticClient(settings);
		services.AddSingleton<IElasticClient>(client);
	}

}
