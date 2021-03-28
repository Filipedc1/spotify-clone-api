using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SpotifyClone.Core.Dtos;
using SpotifyClone.Core.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace SpotifyCloneApi.Controllers
{
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly AppSettings _appSettings;

        private readonly HttpClient _client;

        public AccountController(IAccountService accountService, IHttpClientFactory clientFactory, IOptions<AppSettings> appSettings)
        {
            _accountService = accountService;
            _appSettings = appSettings.Value;
            _client = clientFactory.CreateClient("lyrics");
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto model)
        {
            var result = await _accountService.LoginAsync(model.Code, _appSettings.SpotifyClientId, _appSettings.SpotifyClientSecret);
            if (result is null) return BadRequest();

            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult> RefreshToken(LoginDto model)
        {
            var result = await _accountService.RefreshTokenAsync(model.RefreshToken, _appSettings.SpotifyClientId, _appSettings.SpotifyClientSecret);
            if (result is null) return BadRequest();

            return Ok(result);
        }
    }
}
