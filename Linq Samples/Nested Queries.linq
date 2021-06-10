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
	//Nested Queries
	//sometimes referred to as subqueries
	
	//simply put: it is a query within a query
	
	//List all sales support employees showing their fullname
	//(lastname, firstname), title, and the number of customers
	//each supports. Order by fullname.
	//In addition, show a list of the customers for each employee.
	//List the customer fullname, phone, city and state.
	
	//there will be 2 separate lists within the same final
	//   dataset collection
	//one for employees
	//one for customers
	
	//C# point of view in a class definition
	//classname
	//	property
	//  property
	// ....
	//  collection<T> (set of records)
	
	//query within a query
	//using navigational properties
	
	var resultsq = from x in Employees
					where x.Title.Contains("Sales Support")
					orderby x.LastName,x.FirstName
					select new EmployeeItem
					{
						FullName = x.LastName + ", " + x.FirstName,
						Title = x.Title,
						NumberOfCustomers = x.SupportRepCustomers.Count(),
						CustomerList = (from y in x.SupportRepCustomers
									   select new CustomerItem
									   {
									   		FullName = y.LastName + ", " + y.FirstName,
											Phone = y.Phone,
											City = y.City,
											State = y.State
									   }).ToList()
					};
	resultsq.Dump("Sample of Nested Query : query");
	
	//Sample of Nested Query

	var resultsm = Employees
	   .Where (x => x.Title.Contains ("Sales Support"))
	   .OrderBy (x => x.LastName)
	   .ThenBy (x => x.FirstName)
	   .Select (
	      x => 
	         new EmployeeItem()
	         {
	            FullName = ((x.LastName + ", ") + x.FirstName), 
	            Title = x.Title, 
	            NumberOfCustomers = x.SupportRepCustomers.Count (), 
	            CustomerList = x.SupportRepCustomers
	               .Select (
	                  y => 
	                     new CustomerItem()
	                     {
	                        FullName = ((y.LastName + ", ") + y.FirstName), 
	                        Phone = y.Phone, 
	                        City = y.City, 
	                        State = y.State
	                     }
	               )
	               .ToList ()
	         });
	resultsm.Dump("Sample of Nested Query :method");
   
//Create a list of albums showing its title and artist.
//Show albums with 25 or more tracks only.
//Show the songs on the album listing the name and song length in seconds.
	var numberoftracks = 25;
	var albumlistq = from x in Albums
					where x.Tracks.Count >= numberoftracks
					select new AlbumOfArtist
					{
						Title = x.Title,
						Artist = x.Artist.Name,
						AlbumSongs = (from y in x.Tracks
										select new Song
										{
											SongName = y.Name,
											LengthInSeconds = y.Milliseconds / 1000.0
										}
									  ).ToList()
					};
	albumlistq.Dump("Artist Album with more than 25 tracks");
	
	// Artist Album with more than 25 tracks

	var albumlistm = Albums
   					.Where (x => (x.Tracks.Count >= numberoftracks))
				    .Select (x => new AlbumOfArtist()
				         {
				            Title = x.Title, 
				            Artist = x.Artist.Name, 
				            AlbumSongs = x.Tracks
							               .Select (y => new Song()
							                     {
							                        SongName = y.Name, 
							                        LengthInSeconds = y.Milliseconds / 1000.0
							                     }
							               ).ToList ()
				         }
				   		);
	albumlistm.Dump("Artist Album with more than 25 tracks");
}

// You can define other methods, fields, classes and namespaces here
// class for the customer info

//these would be your ViewModel classes
//the queries would be in your BLL methods.
public class CustomerItem
{
	public string FullName{get;set;}
	public string Phone{get;set;}
	public string City{get;set;}
	public string State{get;set;}
}

// class for the employee info
public class EmployeeItem
{
	public string FullName{get;set;}
	public string Title{get;set;}
	public int NumberOfCustomers{get;set;}
	//the list of customers associated with the employee
	public List<CustomerItem> CustomerList{get;set;}
}

public class AlbumOfArtist
{
	public string Title{get;set;}
	public string Artist{get;set;}
	public List<Song> AlbumSongs{get;set;}
}

public class Song
{
	public string SongName{get;set;}
	public double LengthInSeconds{get;set;}
}