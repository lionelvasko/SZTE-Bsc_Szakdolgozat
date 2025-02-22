using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;


namespace Szakdoga.Services
{
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);

            return Task.FromResult(new AuthenticationState(user));
        }

        internal void AuthenticateUser(string jwt)
        {
            var jwtinfos = GetInfoFromJwt(jwt);
            var identity = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name, jwtinfos["Name"]),
                new Claim(ClaimTypes.Email, jwtinfos["Email"]),
                new Claim(ClaimTypes.Role, jwtinfos["First Name"]),
                new Claim(ClaimTypes.Role, jwtinfos["Last Name"])
             ]);

            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(user)));
        }

        internal void LogoutUser()
        {
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(user)));
        }

        private Dictionary<string, string> GetInfoFromJwt(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadToken(jwt) as JwtSecurityToken;
            return token?.Claims.ToDictionary(c => c.Type, c => c.Value);
        }
    }
}
