using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Szakdoga.Services
{
    internal class GetUserInfos
    {
        private CustomAuthenticationStateProvider _customAuthenticationStateProvider;
        public GetUserInfos(CustomAuthenticationStateProvider customAuthenticationStateProvider)
        {
            _customAuthenticationStateProvider = customAuthenticationStateProvider;
        }

        public async Task<string?> GetUserEmailAsync()
        {
            var authState = await _customAuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity is not { IsAuthenticated: true })
            {
                return null;
            }

            return user.FindFirst(ClaimTypes.Email)?.Value;
        }

        public async Task<string?> GetUserNameAsync()
        {
            var authState = await _customAuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity is not { IsAuthenticated: true })
            {
                return null;
            }

            return user.FindFirst(ClaimTypes.Name)?.Value;
        }
    }
}
