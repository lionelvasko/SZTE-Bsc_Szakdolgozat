using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Szakdoga.Services
{
    public class UserInfoService
    {
        public static readonly string NAME_KEY = "name";
        public static readonly string  EMAIL_KEY = "email";

        public async Task<string?> GetName()
        {
           return await SecureStorage.Default.GetAsync(NAME_KEY);
        }

        public void StroreName(string name = "nulla")
        {
            SecureStorage.Default.SetAsync(NAME_KEY, name);
        }

        public async Task<string?> GetEmail()
        {
            return await SecureStorage.Default.GetAsync(EMAIL_KEY);
        }
        public void StroreEmail(string email = "nulla")
        {
            SecureStorage.Default.SetAsync(EMAIL_KEY, email);
        }
    }
}
