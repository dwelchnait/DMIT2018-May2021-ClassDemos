<Query Kind="Program" />

void Main()
{
	List<Student> students = new List<Student>()
						{
						 new Student(){Id=1, Name="Lowand Behold", Age=25},
						 new Student(){Id=2, Name="Shirley Ujest", Age=22},
						 new Student(){Id=3, Name="Jerry Kan", Age=55},
						 new Student(){Id=4, Name="Sia Latter", Age=26},
						 new Student(){Id=5, Name="Iam Stew-Dent", Age=37},
						};
	
	//find students in their 20's
	//query snytax
	var queryResults = from collectionRow in students
						where collectionRow.Age > 19 && collectionRow.Age < 30
					   select collectionRow;
	//method snytax
	var methodResults = students
						.Where(collectionRow => collectionRow.Age > 19 && collectionRow.Age < 30)
						.Select(collectionRow => collectionRow);
						
	//.Dump() is a Linqpad method that display the data of your collection
	//.Dump() is used ONLY in Linqpad
	queryResults.Dump();
	methodResults.Dump();
}

// You can define other methods, fields, classes and namespaces here

public class Student
{
	public int Id {get;set;}
	public string Name {get;set;}
	public int Age {get;set;}
}