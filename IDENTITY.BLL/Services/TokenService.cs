using IDENTITY.BLL.Factories.Interfaces;
using IDENTITY.BLL.Services.Interfaces;
using IDENTITY.DAL.Entities;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace IDENTITY.BLL.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;
        private readonly IJwtSecurityTokenFactory tokenFactory;


        public TokenService(IJwtSecurityTokenFactory tokenFactory, IConfiguration configuration)
        {

            this.configuration = configuration;
            this.tokenFactory = tokenFactory;
        }
        public string SerializeToken(JwtSecurityToken jwtToken) =>
    new JwtSecurityTokenHandler().WriteToken(jwtToken);

        public JwtSecurityToken BuildToken(User client) => tokenFactory.BuildToken(client);

    }
}
