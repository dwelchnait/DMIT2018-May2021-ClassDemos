<Query Kind="Statements">
  <Connection>
    <ID>31f51b8c-6290-4481-9153-20fb85dae524</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Chinook</Database>
  </Connection>
</Query>

//Using the Ternary operator
//
//   condition(s) ? true value : false value
//
//compare to a conditional statement
//
//  if (condition(s))
//  [{]
//     true path (complex logic)
//  [}]
//  [else
//	[{]
//     false path (complex logic)
//  [}]]

//List all albums by release label. Any album with no label should
//  be indicated as Unknown. List title and label.

var ternaryResults =  from x in Albums
						orderby x.ReleaseLabel
						select new
						{
							title = x.Title,
							label = x.ReleaseLabel != null ? x.ReleaseLabel : "Unknown"
						};
//ternaryResults.Dump();

var methodternaryResults = Albums
						   .OrderBy (x => x.ReleaseLabel)
						   .Select (x => new  
						         {
						            title = x.Title, 
						            label = (x.ReleaseLabel != null) ? x.ReleaseLabel : "Unknown"
						         }
						   );
//methodternaryResults.Dump();

//List all albums showing the Title, ArtistName, and
//decade of release (oldies, 70's, 80's, 90's and modern). Order by Artist

var nestedOperatorResults = from x in Albums
							orderby x.Artist.Name
							select new
							{
								Title = x.Title,
								Artist = x.Artist.Name,
								year = x.ReleaseYear,
								Decade = x.ReleaseYear < 1970 ? "Oldies" : 
										 x.ReleaseYear < 1980 ? "70's" :
										 x.ReleaseYear < 1990 ? "80's" :
										 x.ReleaseYear < 2000 ? "90's" : "Modern"
							};
//nestedOperatorResults.Dump();

var nestedOperatorResultsM = Albums
							   .OrderBy (x => x.Artist.Name)
							   .Select (
							      x => 
							         new  
							         {
							            Title = x.Title, 
							            Artist = x.Artist.Name, 
							            year = x.ReleaseYear, 
							            Decade = (x.ReleaseYear < 1970) ? "Oldies"
							             : (x.ReleaseYear < 1980) ? "70's"
							             : (x.ReleaseYear < 1990) ? "80's" 
										 : (x.ReleaseYear < 2000) ? "90's" 
										 : "Modern"
							         }
							   );
//nestedOperatorResultsM.Dump();

//List all tracks indicating whether they are longer, shorter or equal to the 
//average of all track lengths. Show track name and length.


//example of using multiple queries to answer a question
//query 1, Find the average Track length (using an Aggregrate query: Average)
var resultavg = Tracks.Average(x => x.Milliseconds);
resultavg.Dump();
//now you can use the variable resultavg in your linq query
var ternaryAverage = from x in Tracks
					 select new
					 {
					 	Song = x.Name,
						Length = x.Milliseconds < resultavg ? "shorter"
								: x.Milliseconds > resultavg ? "longer"
								: "average",
						ActualLength = x.Milliseconds
					 };
ternaryAverage.Dump();









