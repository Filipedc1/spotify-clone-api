using Microsoft.AspNetCore.Mvc;
using SpotifyCloneApi.Extensions;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SpotifyCloneApi.Controllers
{
    [Route("[controller]")]
    public class MusicController : ControllerBase
    {
        private readonly HttpClient _client;

        public MusicController(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient("lyrics");
        }

        [HttpGet("lyrics/{artist}/{track}")]
        public async Task<ActionResult> Lyrics(string artist, string track)
        {
            artist = artist.RemoveWhiteSpaceAndSpecialCharacters();

            track = track.TakeWhile(c => c != '(')
                         .RemoveWhiteSpaceAndSpecialCharacters();

            var result = await _client.GetAsync($"{artist}/{track}.html");
            if (!result.IsSuccessStatusCode)
                return BadRequest();

            string response = await result.Content.ReadAsStringAsync();
            string lyrics = ParseLyrics(response);

            return Ok(lyrics);
        }

        private string ParseLyrics(string html)
        {
            try
            {
                string start = "<!-- content -->";
                int inxOf = html.IndexOf("<!-- content -->");
                int startPos = inxOf + start.Length + 1;

                int lastInxOf = html.LastIndexOf(start);
                int length = lastInxOf - startPos;
                string sub = html.Substring(startPos, length);

                var arr = sub.Split(new string[] { "\n", "\r\n", "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                var list = arr.ToList();
                var st = list.IndexOf("<div>") + 2;
                var end = list.IndexOf("</div>", st);

                var result = arr[st..end].ToList();

                // Format lyrics
                for (int i = 0; i < result.Count; i++)
                {
                    result[i] = result[i] + "\n";
                    result[i] = result[i].Replace("<br>", "");
                }

                return string.Join("", result);
            }
            catch (Exception ex)
            {
                return "No Lyrics";
            }
        }
    }
}
