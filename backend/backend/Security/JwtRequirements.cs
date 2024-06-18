using Microsoft.AspNetCore.Authorization;

namespace backend.Security
{
	public class JwtRequirements : IAuthorizationRequirement
	{
		public List<string> Roles { get; set; }

        public JwtRequirements(List<string> roles)
        {
            Roles = roles;
        }
    }
}
