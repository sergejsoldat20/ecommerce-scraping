using backend.Entities.DTO;
using backend.Enums;

namespace backend.Security;

public interface IAuthenticationService 
{
    Task<RegisterEnum> RegisterAsync(RegisterRequest registerRequest);
}