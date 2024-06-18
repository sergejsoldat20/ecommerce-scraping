using backend.Entities;
using backend.Entities.DTO;
using backend.Enums;
using backend.Security;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace backend.Controllers
{

	[ApiController]
	[Route("[controller]")]
	public class AccountController : ControllerBase
	{
		
		private readonly IProductsService _service;
		private readonly HttpClient _httpClient;
		private readonly IAccountService _accountService;
		private readonly IAuthenticationService _authenticationService;
		private readonly IJwtProvider jwtProvider;
		public AccountController(IProductsService productsService,
			HttpClient httpClient,
			IAccountService accountService,
			IAuthenticationService authenticationService,
			IJwtProvider jwtProvider)
		{
			_service = productsService;
			_httpClient = httpClient;
			_accountService = accountService;
			_authenticationService = authenticationService;
			this.jwtProvider = jwtProvider;
		}

		[HttpGet("Scraped")]
		public ActionResult<List<Product>> GetProducts()
		{
			var result = _service.GetAllProductsFromDb();
			return Ok(result);
		}

		[AllowAnonymous]
		[HttpPost("authenticate")]
		public ActionResult<AuthResponse> Authenticate(LoginRequest request)
		{
			var response = _accountService.Authenticate(request);
			return Ok(response);
		}

		[AllowAnonymous]
		[HttpPost("register")]
		public async Task<IActionResult> Register(RegisterRequest request)
		{
			var response = await _accountService.Register(request);
			if (response.Equals(RegisterEnum.Success))
			{
				return Ok(new { message = "User registered successfuly." });
			}
			else if (response.Equals(RegisterEnum.ExistingEmail))
			{
				return BadRequest(new { message = "Email already exists!" });
			}
			else
			{
				return BadRequest(new { message = "Error" });
			}
		}

		[AllowAnonymous]
		[HttpPost("register-firebase")]
		public async Task<IActionResult> RegisterFirebase(RegisterRequest request)
		{
			var response = await _authenticationService.RegisterAsync(request);

			if (response.Equals(RegisterEnum.Success))
			{
				return Ok(new { message = "User registered successfuly." });
			}
			else if (response.Equals(RegisterEnum.ExistingEmail))
			{
				return BadRequest(new { message = "Email already exists!" });
			}
			else
			{
				return BadRequest(new { message = "Error" });
			}
		}

		[AllowAnonymous]
		[HttpPost("login-firebase")]
		public async Task<ActionResult<AuthResponse>> LoginFirebase(LoginRequest request)
		{
			var response = await jwtProvider.GetForCredentialsAsync(request.Email, request.Password);
			if (response != null)
			{
				return Ok(response);
			}
			return Unauthorized(new { message = "Login failed" });
		}
	}
}
