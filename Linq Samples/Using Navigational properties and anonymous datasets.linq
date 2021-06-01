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

//List all albums released in the 80's ordered by release year (descending), ReleaseLabel, Title
//Show the year, label, title and artist name
Albums
	.OrderByDescending(x => x.ReleaseYear)
	.ThenBy(x => x.ReleaseLabel)
	.ThenBy(x => x.Title)
	.Where( x => x.ReleaseYear > 1979 && x.ReleaseYear < 1990)
	.Select(x => new
				{
					Year = x.ReleaseYear,
					Label = x.ReleaseLabel,
					Title = x.Title,
					Artist = x.Artist.Name
				})
	
from x in Albums
orderby x.ReleaseYear descending, x.ReleaseLabel, x.Title
where x.ReleaseYear > 1979 && x.ReleaseYear < 1990
select new 
{
	Year = x.ReleaseYear,
	Label = x.ReleaseLabel,
	Title = x.Title,
	Artist = x.Artist.Name
}

//List all albums released in the 80's ordered by Artist, Title,release year (descending)
//Show the artist name, Title, year and label

// x.Artist.Name where instance x of Albums is using the navigational property Artiest and
//    accessing the .Name property in the Artist instance which is the parent of x
Albums
	.OrderBy(x => x.Artist.Name)
	.ThenBy(x => x.Title)
	.ThenByDescending(x => x.ReleaseLabel)
	.Where( x => x.ReleaseYear > 1979 && x.ReleaseYear < 1990)
	.Select(x => new
				{
					Artist = x.Artist.Name,
					Title = x.Title,
					Year = x.ReleaseYear,
					Label = x.ReleaseLabel == null ? "Unknown" : x.ReleaseLabel
				})
				
from x in Albums
orderby x.Artist.Name, x.Title, x.ReleaseYear descending
where x.ReleaseYear > 1979 && x.ReleaseYear < 1990
select new 
{
	Artist = x.Artist.Name,
	Title = x.Title,
	Year = x.ReleaseYear,
	Label = x.ReleaseLabel == null ? "Unknown" : x.ReleaseLabel
}				
				