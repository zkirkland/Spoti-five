using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using System.Web.Mvc;
using Task2.Models;

namespace Task2.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Contact()
		{
			return View();
		}

		public ActionResult Result(string Artist)
		{
			var result = new Search();

			var auth = new ClientCredentialsAuth()
			{
				//Your client Id
				ClientId = "c99b06725565434cab71dae37925376c",
				ClientSecret = "ccb0bc68f03647749a970bb8987b29b0",
				Scope = Scope.UserReadPrivate
			};

			Token token = auth.DoAuth();
			var spotify = new SpotifyWebAPI()
			{
				TokenType = token.TokenType,
				AccessToken = token.AccessToken,
				UseAuth = false
			};

			var artistResult = spotify.SearchItems(Artist, SearchType.Artist);
			string albumID;
			FullAlbum albumResult;
			result.sortedList = new System.Collections.Generic.SortedList<FullAlbum, int>(new AlbumPopularityComparer());

			if (artistResult.Artists.Items.Count > 0)
			{
				var artist = artistResult.Artists.Items[0];
				var albums = spotify.GetArtistsAlbums(artist.Id, limit: 300);
				for (int i = 0; i < albums.Items.Count; ++i)
				{
					albumID = albums.Items[i].Id;
					albumResult = spotify.GetAlbum(albumID);
					if(result.sortedList.ContainsKey(albumResult) != true)
					{
						result.sortedList.Add(albumResult, albumResult.Popularity);
					}
				}

				result.Artist = artist;
			}

			return View(result);
			//return View();
		}
	}
}