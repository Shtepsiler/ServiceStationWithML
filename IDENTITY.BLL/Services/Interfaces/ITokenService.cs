using IDENTITY.DAL.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace IDENTITY.BLL.Services.Interfaces
{
    public interface ITokenService
    {
        string SerializeToken(JwtSecurityToken jwtToken);
        //bool IsValid(JwtResponse response, out string username);
        JwtSecurityToken BuildToken(User client);

    }
}
