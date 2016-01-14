using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task2.Models
{
	public class Search
	{
		public SimpleArtist Artist;
		public SortedList<FullAlbum, int> sortedList;
		public bool HaveResults;
	}
}