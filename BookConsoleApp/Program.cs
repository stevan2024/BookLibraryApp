// See https://aka.ms/new-console-template for more information



Console.WriteLine("Press ESC to stop");

 string _uri = System.Configuration.ConfigurationManager.AppSettings.Get("uri");
 using HttpClient _client = new HttpClient();

do
{
    while (!Console.KeyAvailable)
    {
        Console.Write("Please enter your command - ");
        string input = Console.ReadLine();
        await ProcessCommand(input);

   }
} while (Console.ReadKey(true).Key != ConsoleKey.Escape);

//Gets List of all books in library
 async Task GetBooks()
{
    string responseBody = string.Empty;
    try
    {
        responseBody = await _client.GetStringAsync(_uri);
    }
    catch (Exception execption)
    {
        Console.WriteLine(execption.Message);
    }

    Console.WriteLine(responseBody);
}


//Get just one book from library
async Task GetBook(string isbn)
{
    string responseBody = string.Empty;
    HttpResponseMessage response = new HttpResponseMessage();


    try
    {

        responseBody = await _client.GetStringAsync(_uri + "/" + isbn);
    }
    catch (Exception execption)
    {
        Console.WriteLine(execption.Message);
    }

    Console.WriteLine(responseBody);
}

// Updates one book
 async Task UpdateBook(string bookString)
{
    HttpResponseMessage response = new HttpResponseMessage();

    try
    {

        StringContent httpContent = new StringContent(bookString, System.Text.Encoding.UTF8, "application/json");
        response = await _client.PutAsync(_uri, httpContent);

    }
    catch (Exception execption)
    {
        Console.WriteLine(execption.Message);
    }

    Console.WriteLine(response.StatusCode.ToString());
}

// Adds a new book
 async Task AddBook(string bookString)
{
    HttpResponseMessage response = new HttpResponseMessage();

    try
    {
        StringContent httpContent = new StringContent(bookString, System.Text.Encoding.UTF8, "application/json");
        response = await _client.PostAsync(_uri, httpContent);
    }
    catch (Exception execption)
    {
        Console.WriteLine(execption.Message);
    }

    Console.WriteLine(response.StatusCode.ToString());
}

// delete a book
 async Task DeleteBook(string isbn)
{
    HttpResponseMessage response = new HttpResponseMessage();

    try
    {
        response = await _client.DeleteAsync(_uri + "/" + isbn);
    }
    catch (Exception execption)
    {
        Console.WriteLine(execption.Message);
    }

    Console.WriteLine(response.StatusCode.ToString());
}



// parse commands in console window
 async Task ProcessCommand(string input)
{
    if (string.IsNullOrEmpty(input))
    {
        return;
    }

    string commandName = string.Empty;
    string commandValue = string.Empty;

    int pos = input.IndexOf(" ");

    if (pos == -1)
    {
        commandName = input;
    }
    else
    {
        commandName = input.Substring(0, pos);
        commandValue = input.Substring(pos + 1, input.Length - pos - 1);
    }


    switch (commandName.ToLower())
    {
        case "getbooks":
            await GetBooks();
            break;
        case "getbook":
            await GetBook(commandValue);
            break;
        case "updatebook":
            await UpdateBook(commandValue);
            break;
        case "addbook":
            await AddBook(commandValue);
            break;
        case "deletebook":
            await DeleteBook(commandValue);
            break;
    }


    return;
}







