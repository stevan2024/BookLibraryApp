The solution consists on a book console app, a books api (extra bonus).
Inside the books api project there is a books repository, unit tests for the repository, web api controller class.
The project uses an in memory Entity Framework DB to store data. This is initially populated with a few books in the Database\BooksDataGenerator.cs


To Run the app go into the BookApi directory and open the BooksApi.sln file

Press the run button for the project this will load the console app and book API.

A swagger browser window will open. Check the base address of the swagger page and make sure its the same one in the console apps app.config file.

In the app.config file the uri setting should look something like "https://localhost:7058/" with the string "Books" added so something like "https://localhost:7058/Books" 

The following commands can be used in the console app
	GetBooks - example: GetBooks
	GetBook <isbn number> - example: GetBook 9780671612979
	UpdateBook <book json> - example: UpdateBook {"isbn": 9780671612979,"title": "t3","publisher": "p3","authors": "a3"}
	AddBook <book json> - example: AddBook {"isbn": 9781451683219,"title": "t4","publisher": "p4","authors": "a4"}
	Delete <isbn number> - example: Delete 9781451683219

The swagger page can also be used to preform these operations.

Unit tests can be found in the books api in the Unit Tests folder