using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using System.Linq;
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
			/* Most of this uses the Spotify-API */
			var result = new Search();

			// new authorization so we can search without restrictions
			var auth = new ClientCredentialsAuth()
			{
				//Your client Id
				ClientId = "c99b06725565434cab71dae37925376c",
				ClientSecret = "ccb0bc68f03647749a970bb8987b29b0",
				Scope = Scope.UserReadPrivate
			};

			// get a token for the spotifywebapi object
			Token token = auth.DoAuth();

			// create the new spotify web api object
			var spotify = new SpotifyWebAPI()
			{
				TokenType = token.TokenType,
				AccessToken = token.AccessToken,
				UseAuth = false
			};

			// search for the artist specified by the user and get the results
			var artistResult = spotify.SearchItems(Artist, SearchType.Artist);

			string albumID;
			FullAlbum albumResult;

			// create a new empty sorted list
			result.sortedList = new System.Collections.Generic.SortedList<FullAlbum, int>(new AlbumPopularityComparer());

			// make sure search returned results
			if (artistResult.Artists.Items.Count > 0)
			{
				// get the first artist returned
				var artistObj = artistResult.Artists.Items[0];
				
				// search for the albums of that artist
				var albums = spotify.GetArtistsAlbums(artistObj.Id, limit: 300);
				bool albumInList = false;
				// since the album search only returns a simple album with no
				// popularity variable, we must search for the album by ID. This
				// gives us a FullAlbum object with the popularity variable.
				for (int i = 0; i < albums.Items.Count; ++i)
				{
					albumID = albums.Items[i].Id;
					albumResult = spotify.GetAlbum(albumID);

					// if list empty, go ahead and add the first album
					if(result.sortedList.Count > 0)
					{
						// check to see if the album is already in the list
						for (int j = 0; j < result.sortedList.Count; ++j)
						{
							// if it IS in the list
							if (albumResult.Name == result.sortedList.ElementAt(j).Key.Name)
							{
								// set to true
								albumInList = true;
							}
						}

						// now that we know if the album is in the list already or not
						// we can decide if we need to add the album
						if (albumInList != true)
						{
							// The code above was not perfect and this helps
							// double check that the album object is not already in the list
							if (result.sortedList.ContainsKey(albumResult) != true)
							{
								result.sortedList.Add(albumResult, albumResult.Popularity);
							}
						}

						// reset to false
						albumInList = false;
					} // if
					else
					{	// add the first album
						result.sortedList.Add(albumResult, albumResult.Popularity);
					}
				} // for

				// set the artist object in the model to the artist object
				// from the search
				result.Artist = artistObj;
			} // if
			else
			{
				// no result found, go to error page
				return RedirectToAction("errorPage");
			}

			return View(result);
			//return View();
		}

		public ActionResult errorPage()
		{
			return View();
		}
	}
}