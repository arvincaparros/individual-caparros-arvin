using Nexus.Core;

namespace Nexus.API.Services
{
    public interface ITokenServices
    {
        string CreateToken(ApplicationUser user);
    }
}
