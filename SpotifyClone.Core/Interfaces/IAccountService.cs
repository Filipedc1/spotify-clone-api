using SpotifyClone.Core.Dtos;
using System.Threading.Tasks;

namespace SpotifyClone.Core.Interfaces
{
    public interface IAccountService
    {
        Task<SpotifyTokenResponse> LoginAsync(string code, string spotifyClientId, string spotifyClientSecret);
        Task<SpotifyTokenResponse> RefreshTokenAsync(string refreshToken, string spotifyClientId, string spotifyClientSecret);
    }
}