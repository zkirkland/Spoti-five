using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task2.Models
{
	public class AlbumPopularityComparer : IComparer<FullAlbum>
	{
		public int Compare(FullAlbum x, FullAlbum y)
		{
			return x.Popularity.CompareTo(y.Popularity);
		}
	}
}