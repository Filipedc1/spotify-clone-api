using System.ComponentModel.DataAnnotations;

namespace SpotifyClone.Core.Dtos
{
    public class LoginDto
    {
        public string Code { get; set; }
        public string RefreshToken { get; set; }
    }
}
