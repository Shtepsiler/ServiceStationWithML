using IDENTITY.BLL.Configurations;
using IDENTITY.BLL.Factories.Interfaces;
using IDENTITY.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IDENTITY.BLL.Factories
{
    public class JwtSecurityTokenFactory : IJwtSecurityTokenFactory
    {
        private readonly JwtTokenConfiguration jwtTokenConfiguration;
        private readonly UserManager<User> userManager;

        public JwtSecurityToken BuildToken(User user) => new JwtSecurityToken(
            issuer: jwtTokenConfiguration.Issuer,
            audience: jwtTokenConfiguration.Audience,
            claims: GetClaims(user),
            expires: jwtTokenConfiguration.ExpirationDate,
            signingCredentials: jwtTokenConfiguration.Credentials);

        private IEnumerable<Claim> GetClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Authentication, user.UserName)
            };

            // Add user roles as claims
            var role = userManager.GetRolesAsync(user).Result.Order().First();
       
                claims.Add(new Claim(ClaimTypes.Role, role));

            return claims;
        }

        public JwtSecurityTokenFactory(JwtTokenConfiguration jwtTokenConfiguration, UserManager<User> userManager)
        {
            this.jwtTokenConfiguration = jwtTokenConfiguration;
            this.userManager = userManager;
        }
    }
}
