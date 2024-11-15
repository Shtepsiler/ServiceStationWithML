using IDENTITY.DAL.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace IDENTITY.BLL.Factories.Interfaces
{
    public interface IJwtSecurityTokenFactory
    {
        JwtSecurityToken BuildToken(User User);
    }
}
