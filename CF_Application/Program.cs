using System.Text.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using CF_Console.Models;
using System.ComponentModel;

public static class CommanderFinderConsole {
    static HttpClient localClient;

    static CommanderFinderConsole()
    {
        //Creates a client for accessing the local api
        localClient = new HttpClient();
        localClient.BaseAddress = new Uri("https://localhost:7277");
    }

    public static async Task Main()
    {
        //Creates a client for accessing the local api
        //localClient = new HttpClient();
        //localClient.BaseAddress = new Uri("https://localhost:7277");

        //Main Menu Loop
        bool runtime = true;
        do {
            List<CFCollection> collections = await FetchApiData();
            Console.WriteLine($"{collections}");
            Console.WriteLine("======================================================================");
            Console.WriteLine("CommanderFinder by Isaiah Thompson");
            Console.WriteLine("(P)ick out a new random commander.");
            Console.WriteLine("(O)pen the list of collections.");
            Console.WriteLine("(Q)uit");
            Console.WriteLine("Enter your letter command:");
            string input = Console.ReadLine().ToUpper();
            switch (input)
            {
                case "P":
                    Card commander = await RetrieveCard(null);
                    //commander.printCard();
                    Console.WriteLine("Do you wish to create a collection with this commander? (Y/N)");
                    bool validInput = false;
                    do
                    {
                        switch(Console.ReadLine().ToUpper())
                        {
                            case "Y":
                                
                                HttpResponseMessage response = await localClient.PostAsJsonAsync<Card>("api/CFCollection", commander);
                                if (response.IsSuccessStatusCode)
                                {
                                    Console.WriteLine("Collection Creation Successful");
                                } else
                                {
                                    Console.WriteLine($"Error - {response.StatusCode}");
                                }
                                validInput = true;
                                break;
                            case "N":
                                validInput = true;
                                break;
                        }
                    } while(!validInput);
                    break;
                case "O":
                    foreach(CFCollection coll in collections)
                    {
                        coll.printHead();
                    }
                    break;
                case "Q":
                    runtime = false;
                    break;
                default:
                    
                    break;
            }
        } while (runtime);
    }

    public static async Task<List<CFCollection>> FetchApiData() //Fetches the data from the API
    {
        HttpResponseMessage response = await localClient.GetAsync("/api/CFCollection");
        
        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(jsonResponse);
            var collections = JsonSerializer.Deserialize<List<CFCollection>>(jsonResponse, 
                new JsonSerializerOptions {PropertyNameCaseInsensitive = true});

            return collections;
        } else
        {
            Console.WriteLine($"Error - {response.StatusCode}");
            return null;
        }
    }

    public static async Task<Card> RetrieveCard(Card? commander) //Retrieves a random card from the Scryfall API
    {
        using (HttpClient scryfallClient = new HttpClient()) //Creates a client for accessing the Scyfall API
            {
                //Changes User-Agent and Accept header to match the ones required to access the API (detailed here: https://scryfall.com/docs/api)
                scryfallClient.DefaultRequestHeaders.UserAgent.Clear();
                scryfallClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue("CommanderFinder", "1.0")));
                scryfallClient.DefaultRequestHeaders.Accept.Clear();
                scryfallClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Card data = default(Card);
                try
                {
                    //Use the commander argument to determine the search query.
                    //If there is no commander provided, it will retrieve a random card that can be a commander.
                    //If there is a commander provided, it will find a card that is within the commander's color identity.
                    string cardArgument = commander is null? "is%3Acommander" : $"identity<%3D{commander.ColorsToString()}";
                    //Unicode Translations: %3A = " " , %3D = "="

                    HttpResponseMessage response = await scryfallClient.GetAsync($"https://api.scryfall.com/cards/random?q={cardArgument}"); //"q=" is the search query argument for the Scryfall API
                    response.EnsureSuccessStatusCode(); //Checks that the HTTP response is valid
                    string responseBody = await response.Content.ReadAsStringAsync(); //JSON content to deserialize
                    //Console.WriteLine($"Response: {responseBody}");
                    data = JsonSerializer.Deserialize<Card>(responseBody);
                }
                catch (HttpRequestException e) //Catch exception if the api request fails
                {
                    Console.WriteLine($"Request error: {e.Message}");
                }
                return data;
            }
    }
}