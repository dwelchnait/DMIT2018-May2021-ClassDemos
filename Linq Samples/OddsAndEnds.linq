<Query Kind="Program">
  <Connection>
    <ID>31f51b8c-6290-4481-9153-20fb85dae524</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Chinook</Database>
  </Connection>
</Query>

void Main()
{
	AlbumItem results = (from x in Albums
				    	where x.AlbumId == 1000
						select new AlbumItem
						{
							AlbumId = x.AlbumId,
							Title = x.Title,
							ArtistId = x.ArtistId,
							ReleaseYear = x.ReleaseYear,
							ReleaseLabel = x.ReleaseLabel
						}).FirstOrDefault();
	//results.Dump();
	
	//.Distinct()
	var distinctmq = Customers
						.Select(x => x.Country)
						.Distinct();
	//distinctmq.Dump();
	
	var distinctsq = (from x in Customers
						select x.Country).Distinct();
	//distinctsq.Dump();
	
	//Any and All
	//Genres.Count().Dump();
	
	//Show Genres that have tracks which are not on any playlist
	var genreTrackAny = from g in Genres
							where g.Tracks.Any(tr => tr.PlaylistTracks.Count == 0)
							select g;
	//genreTrackAny.Dump();
	
	//show genres that have all their tracks appearing at least once on
	//a playlist. What are the popular genres?
	//show the genre name, and list of genre tracks and numbe of playlists.
	var genreTrackAll = from g in Genres
					    where g.Tracks.All(tr => tr.PlaylistTracks.Count() > 0 )
						select new
						{
						 	name = g.Name,
							thetracks = g.Tracks
										 .Where(y => y.PlaylistTracks.Count() > 0)
										 .Select(y => new {
										 			song = y.Name,
													count = y.PlaylistTracks.Count()
										 })
						};
	//genreTrackAll.Dump();
	
	//another scenario for Any or ALL is comparing files
	//but there is another filter Exclude()
	
	var almeida = (from x in PlaylistTracks
				where x.Playlist.UserName.Contains("AlmeidaR")
				select new
				{
					genre = x.Track.Genre.Name,
					id = x.TrackId,
					song = x.Track.Name,
					artist = x.Track.Album.Artist.Name
				}).Distinct().OrderBy(y => y.song);
	var brooks = (from x in PlaylistTracks
				where x.Playlist.UserName.Contains("BrooksM")
				select new
				{
					genre = x.Track.Genre.Name,
					id = x.TrackId,
					song = x.Track.Name,
					artist = x.Track.Album.Artist.Name
				}).Distinct().OrderBy(y => y.song);
				
	//Roberto has 110 tracks and Michelle has 88
	
	//list the tracks that both Roberto and Michelle like.
	//What's happening: comparing 2 datasets together
	//                  data in listA that is also in listB
	var likes = almeida
				.Where(rob => brooks.Any(mic => mic.id == rob.id))
				.OrderBy(rob => rob.song)
				.Select(rob => rob.song);
	//likes.Dump();
	
	//list the tracks that Roberto likes but Michelle does not listen to
	var Robertolikes = almeida
				.Where(rob => !brooks.Any(mic => mic.id == rob.id))
				.OrderBy(rob => rob.song)
				.Select(rob => rob.song);
	//Robertolikes.Dump();
	
	//list the tracks that Michelle likes but Roberto does not listen to
	var MichellelikesALL = brooks
				.Where(mic => almeida.All(rob => mic.id != rob.id))
				.OrderBy(mic => mic.song)
				.Select(mic => mic.song);
	//MichellelikesALL.Dump();
	
	var MichellelikesANY = brooks
				.Where(mic => !almeida.Any(rob => mic.id == rob.id))
				.OrderBy(mic => mic.song)
				.Select(mic => mic.song);
	//MichellelikesANY.Dump();
	
	//.Exclude() this is suppose to exclude records on one list that appear
	//				on another list.
	
	//Union in linq
	//the joining of multiple results into a single query dataset
	//syntax (queryA).Union(queryB).Union(queryX)....
	//rules same as sql
	//	number of columns must be the same
	//	datatype of columns must be the same
	//	ordering should be done as a method( on the unioned dataset
	
	//List the stats of Albums on Tracks (Count, $cost, average track length)
	//Note: for cost and average, one will need an instance (track on album) to
	//       actually proces the method
	//      if an album contains no tracks then no Sum() or Average() can be physical done

	//to do this example, you will need an Album with no Tracks on your database
	
	//Albums.Where(x => x.Tracks.Count() == 0).Select(x => x).Dump();
	
	//IMPORTANT result: It seems in LinqPad6 that the query using literal as the
	//					numeric values (not the Aggregrates) must be in the first
	//					query.
	
	var unionresults = (from y in Albums
								where y.Tracks.Count() == 0
								select new
								{
									title = y.Title,
									totalTracks = 0,
									totalPrice = 0.00m,
									AverageLength = 0.0
								}).Union(Albums
									.Where(x => x.Tracks.Count() > 0)
									.Select(x => new
									{
										title = x.Title,
										totalTracks = x.Tracks.Count(),
										totalPrice = x.Tracks.Sum(tr => tr.UnitPrice),
										AverageLength = x.Tracks.Average(tr => tr.Milliseconds)
									})).OrderBy(u => u.totalTracks);
	unionresults.Dump();
}

// You can define other methods, fields, classes and namespaces here
public class AlbumItem
{
	public int AlbumId{get;set;}
	public string Title{get;set;}
	public int ArtistId{get;set;}
	public int ReleaseYear{get;set;}
	public string ReleaseLabel{get;set;}
}
	