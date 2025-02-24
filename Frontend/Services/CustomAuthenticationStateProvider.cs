using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Szakdoga.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await SecureStorage.Default.GetAsync("jwt_auth_token");

            if (token is null)
            {
                return new AuthenticationState(_anonymous);
            }

            //Store the user's data in securestorage

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
