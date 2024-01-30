using Microsoft.AspNetCore.Identity;

namespace NZWalks2.API.Repository
{
    public interface ITokenRepository
    {
       string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
