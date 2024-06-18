using AutoMapper;
using backend.Data;
using backend.Entities;
using backend.Entities.DTO;
using backend.Enums;
using backend.Security;
using FirebaseAdmin.Auth;
using BC = BCrypt.Net.BCrypt;

namespace backend.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AuthenticationService(ApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<RegisterEnum> RegisterAsync(RegisterRequest request)
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

            
            // before writing to database register account to firebase and store uid to database
            var userArgs = new UserRecordArgs
            {
                Email = request.Email,
                Password = request.Password
            };

            var userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(userArgs);

            // account.IdentityId = userRecord.Uid;

            await _context.AddAsync(account);
            await _context.SaveChangesAsync();

            return RegisterEnum.Success;
        }
    }
}