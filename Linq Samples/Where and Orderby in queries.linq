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

//simpke complete list
//query syntax
from x in Albums
select x

//method syntax
Albums
	.Select(x => x)

//add a filter condition to the query
// where and its C# conditions
//List all albums released in 2000

from x in Albums
where x.ReleaseYear == 2000
select x

Albums
	.Where( x => x.ReleaseYear == 2000)
	.Select(x => x)
	
//List all albums released in the 80's

from x in Albums
where x.ReleaseYear > 1979 && x.ReleaseYear < 1990
select x

Albums
	.Where( x => x.ReleaseYear > 1979 && x.ReleaseYear < 1990)
	.Select(x => x)
	
//List all albums released in the 80's ordered by release year (descending), ReleaseLabel, Title
Albums
	.OrderByDescending(x => x.ReleaseYear)
	.ThenBy(x => x.ReleaseLabel)
	.ThenBy(x => x.Title)
	.Where( x => x.ReleaseYear > 1979 && x.ReleaseYear < 1990)
//	.OrderBy(x => x.ReleaseYear)
	.Select(x => x)
	
from x in Albums
orderby x.ReleaseYear descending, x.ReleaseLabel, x.Title
where x.ReleaseYear > 1979 && x.ReleaseYear < 1990
//orderby x.ReleaseYear
select x

