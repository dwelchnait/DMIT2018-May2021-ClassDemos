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

//Grouping

//a) by a column(field, attribute,property)		groupname.Key
//b) by multiple columns						groupname.Key.attribute
//c) by an entity								groupname.Key.attribute

//groups have 2 components
//a) Key component (group by); reference this component using groupname.Key[.attribute]
//b) data (instances in the group)

//process
//start with a "pile" of data (original collection)
//specific the grouping attribute(s)
//result in smaller "piles" of data determined by the attribute(s) which
//		can be "reported" upon (grouped collections)

//grouping is different the ordering
//Ordering re-sequences a collection of data then displays
//grouping re-organizes a collection into separate, usually smaller, collections

//display albums by RelaseYear
//orderby
//var resultsorderby = from x in Albums
//					orderby x.ReleaseYear
//					select x;
//resultsorderby.Dump("Order by");

//groupby
//var resultsgroupby = from x in Albums
//					group x by x.ReleaseYear;
//resultsgroupby.Dump("Group by");
				
//filter to get a specific group
//one must save the groups into a named collection
//the named collection is a temporary name that
//	exists as long as the query itself
//IMPORTANT!!!!
//Any clause AFTER using a group by into requires one to use the 
//		groupname instead of the original instance placeholder
//var resultsgroupby = from x in Albums
//					//where x.ReleaseYear > 1969 && x.ReleaseYear < 1980
//					group x by x.ReleaseYear into gTemp
//					//where gTemp.Key > 1969 && gTemp.Key < 1980
//					select gTemp;
//resultsgroupby.Dump("Group by");
				
//Group using multiple columns
//you will create a new temporary class for use within the query
//this temporary class DOES NOT need to have a physical class coded.
//var resultsgroupbycolumns = from x in Albums
//							group x by new {x.Artist.Name, x.ReleaseYear} into gTemp
//							where gTemp.Count() > 1
//							select gTemp;
//resultsgroupbycolumns.Dump("multiple columns");
				
//Group by an entity, reference that entity in your group by clause
//When would one maybe wish to use the entity, possibly when you wish
//   to use multiple attributes out of the entity
//var resultsgroupbyentity = from x in Tracks
//							group x by x.Album;
//resultsgroupbyentity.Dump("entity piles");
				
//var resultsgroupbyentity = from x in Customers
//							group x by x.SupportRep;
//resultsgroupbyentity.Dump("entity piles");				
				
//Using the groups in "reporting"

//for query syntax
//your temporary dataset groupname is created by using ->  into gName

//for method syntax
//your temporary dataset groupname is the placehold of your Select
//   ->  .Select(gName => ...

//group by ReleaseYear
//var resultsgroupby = from x in Albums
//						group x by x.ReleaseYear into gGummyBear
//						//select gGummyBear;
//						select new
//						{
//							KeyValue = gGummyBear.Key,
//							numberofAlbums = gGummyBear.Count(),
//							ArtistAndAlbums = from y in gGummyBear
//											select new
//											{
//												Title = y.Title,
//												Year = y.ReleaseYear,
//												Artist = y.Artist.Name
//											}
//						};
//
//resultsgroupby.Dump("Using group in select");

//group using an entity
var resultsgentity= from x in Albums
					where x.ReleaseYear > 1969 && x.ReleaseYear < 1980
					group x by x.Artist into gTemp
					orderby gTemp.Key.Name
					where gTemp.Count() > 1
					//select gTemp;
					select new
					{
						KeyValue = gTemp.Key.Name,
						numberofAlbums = gTemp.Count(),
						GroupAlbums = from y in gTemp
									  orderby y.ReleaseYear
									  select new
									  {
									  	Title = y.Title,
										Year = y.ReleaseYear
									  }
					};
resultsgentity.Dump();

//Create a query which will report the employee and their customer base.
//List the employee fullname (phone), number of customer in their base.
//List the fullname, city and state for the customer base.

//how to attack this question
//tips:
//What is the detail of the query? What is reported on most?
//       Customers base (big pile of data)
//Is the report one complete report or is it in smaller components?
//      order by vs group by?
//Can I subdivide (group) my details into specific piles? If so, on what?
//      Employee (smaller piles of data on xxxxxx)
//Is ther an association between Customers and Employees?
//      nav property SupportRep











