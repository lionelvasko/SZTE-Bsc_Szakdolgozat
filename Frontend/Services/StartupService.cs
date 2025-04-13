using SomfyAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Szakdoga.Models;
using TuyaAPI.Services;

namespace Szakdoga.Services
{
    public class StartupService
    {
        private readonly TuyaApiService _tuyaApiService = TuyaApiService.GetInstance();
        private readonly SomfyApiService _somfyApiService = SomfyApiService.GetInstance();

        public void SetupTuya(string region, string at, string rt)
        {
            _tuyaApiService.SetUrl(region);
            _tuyaApiService.SetTokens(at, rt);
        }

        public void SetupSomfy(string baseUrl, string username, string password)
        {
            _somfyApiService.Username = username;
            _somfyApiService.Password = password;

        }
    }
}
