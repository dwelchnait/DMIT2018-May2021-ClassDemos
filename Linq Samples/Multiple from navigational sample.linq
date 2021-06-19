<Query Kind="Expression">
  <Connection>
    <ID>31f51b8c-6290-4481-9153-20fb85dae524</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Chinook</Database>
  </Connection>
</Query>

//reporting between "grandparent" and "grandchild" without the "parent"

// report from Albums and PlaylistTracks but not from Track

//  Albums --> Tracks --> PlaylistTracks

//possible way of doing the "join" is to physical include a where clause similar
//		to the tablea inner join tableb on condition in sql

//This limits and confinds the optimization that Linq and Sql can create
//It works but you should FIRST ALWAYS consider using navigational properties before
//		doing your own join conditions

from x in Albums
where x.ReleaseYear > 1969 && x.ReleaseYear < 1980
select new
{
	title = x.Title,
	trackcount = x.Tracks.Count(),
	playlists = from tr in x.Tracks
				  from pltr in tr.PlaylistTracks
				  
				  select new
				  {
				  	song = pltr.Track.Name,
					playlist = pltr.Playlist.Name,
					user = pltr.Playlist.UserName
				  }
 }
