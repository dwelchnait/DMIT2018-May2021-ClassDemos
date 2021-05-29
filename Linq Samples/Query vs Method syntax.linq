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

////query syntax
//from x in Albums
//select x
//
////method syntax
//Albums
//   .Select (x => x)

//TIP to comment out lines use ctrl,K then ctrl,C
//    to uncomment lines use ctrl,K then ctrl, U
//    one could also just use ctrl/ as a toogle comment

//find all albums release in 2000

//query syntax
//from x in Albums
//where x.ReleaseYear == 2000
//select x

//method syntax
Albums
   .Where (x => (x.ReleaseYear == 2000))
   .Select (x => x)
   
   