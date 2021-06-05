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

//Aggregates
//.Count(), .Sum(), .Max(), .Min(), .Average()

//aggregates can be coded using query or method syntax
//.Sum(), Max(), Min(), .Average() need to excute against a collection of a single column
//.Count() returns the number of records therefore is not restricted to a single column

//query syntax
// (from ....
//   ....
//  select expression).Max()

//Method  syntax
// collectionset.Max(x => expression)
//or
// collectionset.Select(x => expression).Max()

//IMPORTANT!!!!
//AGGREGATES WORK ONLY ON A COLLECTION OF DATA
//	NOT ON A SINGLE ROW

//A collection CAN have 0, 1 or more rows
//the expression in your aggregate of Sum, Max, Min, Average CANNOT be null
//

//bad example of using aggregate
//aggregate is against a single row
from x in Tracks
select new
{
	Name = x.Name,
	AvgLength = x.Average(x => x.Milliseconds) //wrong, single row
}

//good example
//collection (from x in Tracks select x.Milliseconds)
(from x in Tracks
select x.Milliseconds).Average()

//collection Tracks.Select(x => x.Milliseconds)
Tracks.Select(x => x.Milliseconds).Average()

//collection Tracks and the aggregate delagate is x => x.Milliseconds
Tracks.Average(x => x.Milliseconds)

//List all Albums showing the title, artist name and various aggregate values
//for albums containing tracks. For each album show the number of tracks,
//the longest track length, the shortest track length, the total price of the
//tracks, and the average track length.









