
using backend.Data;
using backend.Entities.DTO;
using backend.Helpers;
using backend.Security;
using FirebaseAdmin.Auth;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace backend.Services
{
	public class JwtProvider : IJwtProvider
	{
		private readonly HttpClient _httpClient;
		private readonly ApplicationDbContext _context;

		public JwtProvider(HttpClient httpClient,
			ApplicationDbContext context)
		{
			this._httpClient = httpClient;
			this._context = context;	
		}

		public async Task<AuthResponse> GetForCredentialsAsync(string email, string password)
		{
			var payload = new
			{
				email = email,
				password = password,
				returnSecureToken = true
			};

			try
			{

				// find user id from firebase
				var userRecord = await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(email);
				var uid = userRecord.Uid;

				// find user role by email
				var userRole = _context.Accounts
					.Where(x => x.Email == email)
					.Select(x => x.Role)
					.FirstOrDefault();

				if (userRole != null) 
				{
					var claims = new Dictionary<string, object>
					{
						{ "roles", userRole }
					};

					// set role in firebase token
					await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(uid, claims);
				}

				// generate token with firebase
				var response = await _httpClient.PostAsJsonAsync("", payload);

				var authToken = await response.Content.ReadFromJsonAsync<AuthToken>();

				return new AuthResponse
				{
					AccessToken = authToken.IdToken,
				};
			} catch (Exception ex) 
			{
				return null;
			}

			throw new AppException("Login error!");
			
		}

	}

	public class AuthToken
	{
		[JsonPropertyName("idToken")]
		public string IdToken { get; set; }
	}
}