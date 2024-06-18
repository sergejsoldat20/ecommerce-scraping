using AutoMapper;
using backend.Data;
using backend.Entities;
using backend.Entities.DTO;
using backend.Enums;
using backend.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BC = BCrypt.Net.BCrypt;
namespace backend.Services;

public interface IAccountService
{
	AuthResponse Authenticate(LoginRequest request);
	Task<RegisterEnum> Register(RegisterRequest request);
}

public class AccountService : IAccountService
{
	private readonly ApplicationDbContext _context;
	private readonly IMapper _mapper;
	private readonly JwtSecret _jwtConfig;
    public AccountService(ApplicationDbContext context,
		IMapper mapper,
		IOptions<JwtSecret> jwtConfig
		)
    {
		_context = context;
		_mapper = mapper;
		_jwtConfig = jwtConfig.Value;
    }

    public AuthResponse Authenticate(LoginRequest request)
	{
		var account = _context
			   .Accounts
			   .FirstOrDefault(x => x.Email == request.Email);

		if (account == null) 
		{
			throw new AppException("This email doesn't exist!");
		}

		if (!BC.Verify(request.Password, account.Password))
		{
			throw new AppException("Password is incorrect!");
		}
		string jwtToken = GenerateToken(account);
		var response = new AuthResponse();
		response.AccessToken = jwtToken;
	
		return response;
	}

	public async Task<RegisterEnum> Register(RegisterRequest request)
	{
		//check email
		if (_context.Accounts.Any(x => x.Email == request.Email))
		{
			return RegisterEnum.ExistingEmail;
		}

		Account account;

		try
		{
			account = _mapper.Map<Account>(request);
		}
		catch 
		{
			return RegisterEnum.Error;
		}

		var isFirstAccount = _context.Accounts.Count() == 0;
		account.Role = isFirstAccount ? Consts.ADMIN : Consts.USER;
		
		// hash password 
		account.Password = BC.HashPassword(request.Password);

		await _context.AddAsync(account);
		await _context.SaveChangesAsync();

		return RegisterEnum.Success;	
	}

	private string GenerateToken(Account account)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.ASCII.GetBytes(_jwtConfig.Key);


		var claims = new List<Claim>
		{
			new Claim("id", account.Id.ToString()),
			new Claim("Role", account.Role)
		};

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(claims),
			Issuer = _jwtConfig.Issuer,
			Audience = _jwtConfig.Audience,
			Expires = DateTime.UtcNow.AddMinutes(_jwtConfig.Minutes),
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), 
			SecurityAlgorithms.HmacSha256Signature),
		};

		var token = tokenHandler.CreateToken(tokenDescriptor);
		return tokenHandler.WriteToken(token);
	}
}
