using System.Text.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using CF_Console.Models;
using System.ComponentModel;

public static class CommanderFinderConsole {
    static HttpClient localClient;
    static List<CFCollection> collections;

    static CommanderFinderConsole()
    {
        //Creates a client for accessing the local api
        localClient = new HttpClient();
        localClient.BaseAddress = new Uri("https://localhost:7277");
    }

    public static async Task Main() //Main Menu Loop
    {
        bool runtime = true;
        do {
            collections = await FetchApiData();
            Console.WriteLine("======================================================================");
            Console.WriteLine("CommanderFinder by Isaiah Thompson");
            Console.WriteLine("(P)ick out a new random commander.");
            Console.WriteLine("(O)pen the list of collections.");
            Console.WriteLine("(Q)uit");
            Console.WriteLine("Enter your letter command:");
            string input = Console.ReadLine().ToUpper();
            switch (input)
            {
                case "P": //Pick a new commander
                    await PickCommander();
                    break;
                case "O": //Open collection list
                    await CollectionListing();
                    break;
                case "Q":
                    runtime = false;
                    break;
                default:
                    Console.WriteLine("ERROR - Invalid Input, Please Try Again.");
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
            //Console.WriteLine(jsonResponse);
            var collections = JsonSerializer.Deserialize<List<CFCollection>>(jsonResponse, 
                new JsonSerializerOptions {PropertyNameCaseInsensitive = true});

            return collections.OrderBy(p => p.id).ToList();
        } else
        {
            Console.WriteLine($"Error - {response.StatusCode}");
            return null;
        }
    }

//==========================SCRYFALL API RETRIEVAL FUNCTION==========================//
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

//==========================COMMANDER PICKING FUNCTION==========================//
    public static async Task PickCommander() //The commander picking process delegated to its own function (the api interactions are already in their own tests.)
    {
        Card commander = await RetrieveCard(null);
        commander.PrintCard();
        Console.WriteLine("Do you wish to create a collection with this commander?");
        Console.WriteLine(" - (Y)es.");
        Console.WriteLine(" - (N)o.");
        Console.WriteLine(" - No, and (G)enerate another commander.");
        
        bool validInput = false;
        do //Repeated generation loop.
        {
            switch(Console.ReadLine().ToUpper())
            {
                 case "Y": //Add card to collection
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
                case "N": //Refuse and go to main menu
                    validInput = true;
                    break;
                case "G": //Refuse and generate a new commander.
                    await PickCommander();
                    validInput = true;
                    break;
                default:
                    Console.WriteLine("ERROR - Invalid Input, Please Try Again.");
                    break;
            }
        } while(!validInput);
    }

//==========================COLLECTION LISTING FUNCTION==========================//
    public static async Task CollectionListing() //Collection interaction, including adding/removing cards and deleting collections.
    {
        do {
            //Print each collection
            foreach(CFCollection coll in collections)
            {
                coll.PrintHead();
            }
            //Have the user decide which collection to select by typing in the id.
            Console.WriteLine("Enter the (ID) of the collection you wish to look at. Exit to the main menu with (-1)");
            int choice;
            try {
                choice = int.Parse(Console.ReadLine());
            } catch
            {
                Console.WriteLine("ERROR - Invalid Input, Please Try Again.");
                continue;
            }

            if (choice == -1)
            {
                break; //exit the loop and return to the main menu.
            } else {
                CFCollection coll = collections.FirstOrDefault(p => p.id == choice);
                if (coll == null) //Collection does not exist.
                {
                    Console.WriteLine($"ERROR - Collection With ID ({choice}) Does Not Exist.");
                } else
                { //Collection found.
                    bool subLoop = false;
                    do //Main loop for interacting with the collection.
                    {
                        coll.PrintCards();
                        Console.WriteLine("(A)dd a new card to the collection.");
                        Console.WriteLine("(R)emove a card from the collection.");
                        Console.WriteLine("(D)elete the collection.");
                        Console.WriteLine("(E)xit to main menu.");
                        Console.WriteLine("Enter your letter command:");
                        string input = Console.ReadLine().ToUpper();
                        switch (input)
                        {
                            case "A": //Add a new card
                                await PickNewCard(coll);
                                await FetchApiData(); //Update collection data
                                break;
                            case "R": //Remove a card
                                await RemoveCard(coll);
                                await FetchApiData(); //Update collection data
                                break;
                            case "D": //Delete Collection
                                await DeleteCollection(coll);
                                await FetchApiData(); //Update collection data
                                return;
                            case "E": //Exit to main
                                subLoop = true;
                                break;
                            default:
                                Console.WriteLine("ERROR - Invalid Input, Please Try Again.");
                                break;
                        }
                    }while(!subLoop);
                }
            }
        } while(true); //Breaks are used to leave the loop when needed
    }

//==========================COLLECTION MANIPULATION FUNCTIONS==========================//
    public static async Task PickNewCard(CFCollection coll) //Pick and add a card to the collection.
    {
        Card newCard = await RetrieveCard(coll.commander);
        newCard.PrintCard();
        Console.WriteLine("Do you wish to add this card to the collection?");
        Console.WriteLine(" - (Y)es.");
        Console.WriteLine(" - Yes, and generate (A)nother card.");
        Console.WriteLine(" - (N)o.");
        Console.WriteLine(" - No, and (G)enerate another card.");

        bool validInput = false;
        do //Repeated generation loop.
        {
            switch(Console.ReadLine().ToUpper())
            {
                case "Y": //Add card to collection
                    coll.cards.Add(newCard);
                    HttpResponseMessage response = await localClient.PutAsJsonAsync<CFCollection>($"api/CFCollection/{coll.id}", coll);
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Card Addition Successful");
                    } else
                    {
                        Console.WriteLine($"Error - {response.StatusCode}");
                    }
                    validInput = true;
                    break;
                case "A": //Accept and generate a new card.
                    coll.cards.Add(newCard);
                    response = await localClient.PutAsJsonAsync<CFCollection>($"api/CFCollection/{coll.id}", coll);
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Card Addition Successful");
                    } else
                    {
                        Console.WriteLine($"Error - {response.StatusCode}");
                    }
                    await PickNewCard(coll);
                    validInput = true;
                    break;
                case "N": //Refuse and go to main menu
                    validInput = true;
                    break;
                case "G": //Refuse and generate a new card.
                    await PickNewCard(coll);
                    validInput = true;
                    break;
                default:
                    Console.WriteLine("ERROR - Invalid Input, Please Try Again.");
                    break;
            }
        } while(!validInput);
    }

    public static async Task RemoveCard(CFCollection coll) //Removes a card from the collection via a PUT call
    {
        Console.WriteLine("Enter the name of the card you wish to delete (partial name finds the first occurrence of the input):");
        string input = Console.ReadLine();
        Card foundCard = coll.cards.Find(p => p.name.Contains(input));
        if (foundCard == null) //Card not found
        {
            Console.WriteLine($"ERROR - Card Containing \"{input}\" Not Found.");
            return;
        }

        //Card found
        Console.WriteLine("--------------------------------------------------------------------------------------");
        foundCard.PrintCard();

        Console.WriteLine("Remove this card from the collection? (Y/N)");
        bool validInput = false;
        do
        {
            switch(Console.ReadLine().ToUpper())
            {
                 case "Y": //Remove the card from the collection.
                    coll.cards.Remove(foundCard);
                    HttpResponseMessage response = await localClient.PutAsJsonAsync<CFCollection>($"api/CFCollection/{coll.id}", coll);
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Card Removal Successful");
                    } else
                    {
                        Console.WriteLine($"Error - {response.StatusCode}");
                    }
                    validInput = true;
                    break;
                case "N": //Refuse to delete
                    validInput = true;
                    break;
                default:
                    Console.WriteLine("ERROR - Invalid Input, Please Try Again.");
                    break;
            }
        } while(!validInput);
    }

    public static async Task DeleteCollection(CFCollection coll)
    {
        Console.WriteLine("--------------------------------------------------------------------------------------");
        coll.commander.PrintCard();

        Console.WriteLine("Delete this Commander's collection? All cards within it will also be deleted. (Y/N)");
        bool validInput = false;
        do
        {
            switch(Console.ReadLine().ToUpper())
            {
                case "Y": //Delete the collection.
                    HttpResponseMessage response = await localClient.DeleteAsync($"api/CFCollection/{coll.id}");
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Collection Deletion Successful");
                    } else
                    {
                        Console.WriteLine($"Error - {response.StatusCode}");
                    }
                    validInput = true;
                    break;
                case "N": //Refuse to delete
                    validInput = true;
                    break;
                default:
                    Console.WriteLine("ERROR - Invalid Input, Please Try Again.");
                    break;
            }
        } while(!validInput);
    }
}