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
	 IEnumerable<ArtistAlbumsByTitleandYear> results = Albums
                           .OrderBy(x => x.Artist.Name)
                           .ThenBy(x => x.Title)
                           .ThenByDescending(x => x.ReleaseYear)
                           .Where(x => x.ReleaseYear > 1979 && x.ReleaseYear < 1990)
                           .Select(x => new ArtistAlbumsByTitleandYear
                           {
                               Artist = x.Artist.Name,
                               Title = x.Title,
                               Year = x.ReleaseYear,
                               Label = x.ReleaseLabel == null ? "Unknown" : x.ReleaseLabel
                           });
		results.Dump();
}

// You can define other methods, fields, classes and namespaces here
public class ArtistAlbumsByTitleandYear
    {

        public string Artist { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Label { get; set; }
    }