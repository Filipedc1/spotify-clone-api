namespace SpotifyClone.Core.Dtos
{
    public class SpotifyTokenResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresIn { get; set; }

        public SpotifyTokenResponse(string access, string refresh, int expires)
        {
            AccessToken = access;
            RefreshToken = refresh;
            ExpiresIn = expires;
        }

        public SpotifyTokenResponse(string access, int expires)
        {
            AccessToken = access;
            ExpiresIn = expires;
        }
    }
}
