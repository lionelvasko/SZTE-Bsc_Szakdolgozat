using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Szakdoga.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await SecureStorage.Default.GetAsync(AuthenticationService.JWT_AUTH_TOKEN);

            if (token is null)
            {
                return new AuthenticationState(_anonymous);
            }
            try
            {
                var user = DecodeJwtToken(token);
                return new AuthenticationState(user);
            }
            catch
            {
                return new AuthenticationState(_anonymous);
            }
        }

        public void NotifyAuthenticationStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        private ClaimsPrincipal DecodeJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            if (jwtToken == null)
            {
                return _anonymous;
            }

            var claims = jwtToken.Claims.ToList();
            var identity = new ClaimsIdentity(claims, "userinfos");
            return new ClaimsPrincipal(identity);
        }

    }

}
