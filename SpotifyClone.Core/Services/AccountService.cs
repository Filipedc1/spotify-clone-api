using Microsoft.AspNetCore.Http;
using SpotifyClone.Core.Interfaces;
using SpotifyAPI.Web;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using SpotifyClone.Core.Dtos;

namespace SpotifyClone.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly SpotifyClientConfig _spotifyClientConfig;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(SpotifyClientConfig spotifyClientConfig, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _spotifyClientConfig = spotifyClientConfig;
        }

        public async Task<SpotifyTokenResponse> LoginAsync(string code, string spotifyClientId, string spotifyClientSecret)
        {
            try
            {
                var request = new AuthorizationCodeTokenRequest(spotifyClientId, spotifyClientSecret, code, new Uri("http://localhost:3000/"));
                var response = await new OAuthClient().RequestToken(request);

                return new SpotifyTokenResponse(response.AccessToken, response.RefreshToken, response.ExpiresIn);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<SpotifyTokenResponse> RefreshTokenAsync(string refreshToken, string spotifyClientId, string spotifyClientSecret)
        {
            try
            {
                var request = new AuthorizationCodeRefreshRequest(spotifyClientId, spotifyClientSecret, refreshToken);
                var response = await new OAuthClient().RequestToken(request);

                return new SpotifyTokenResponse(response.AccessToken, response.ExpiresIn);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<SpotifyClient> BuildSpotifyClientAsync()
        {
            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("Bearer", "access_token");
            var token2 = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");

            return new SpotifyClient(_spotifyClientConfig.WithToken(token));
        }
    }
}
