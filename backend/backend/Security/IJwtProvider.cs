using backend.Entities.DTO;

namespace backend.Security
{
	public interface IJwtProvider
	{
		Task<AuthResponse> GetForCredentialsAsync(string email, string password);
	}
}
