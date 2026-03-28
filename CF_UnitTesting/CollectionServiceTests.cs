using System.Threading.Tasks;
using CF_API.Services;
using CF_Console.Models;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices;
namespace CF_UnitTesting;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Xunit;

public class CollectionTests
{
    [Fact]
    public async Task CommanderCollectionCreation() //Create a collection with a commander
    {
        //Creates a client for accessing the local api
        HttpClient localClient = new HttpClient();
        localClient.BaseAddress = new Uri("https://localhost:7277");

        //Retrieves a card from scryfall and makes the post request.
        //This tests the respective part of the console's main, the controller's POST function and the service's Add() and SaveToJson() functions.
        CF_Console.Models.Card commander = await CommanderFinderConsole.RetrieveCard(null);
        HttpResponseMessage response = await localClient.PostAsJsonAsync<Card>("api/CFCollection", commander);
 
        Assert.True(response.IsSuccessStatusCode, $"{response.StatusCode}");
    }

    [Fact]
    public async Task CommanderCollectionAppendAndRemove() //Add and a card to a collection and remove it. Both use the controller's PUT and service's Update functions.
    {
        HttpClient localClient = new HttpClient();
        localClient.BaseAddress = new Uri("https://localhost:7277");

        //Fetch a collection from the list and add a card to it
        List<CFCollection> collections = await CommanderFinderConsole.FetchApiData();
        CFCollection newCollection;
        int searchId = 0;
        do
        {
            newCollection = collections.Find(p => p.id == searchId);
            searchId++;
        } while (newCollection == null);
        
        //Add a new card to it.
        Card newCard = await CommanderFinderConsole.RetrieveCard(newCollection.commander);
        newCollection.cards.Add(newCard);
        HttpResponseMessage responseA = await localClient.PutAsJsonAsync<CFCollection>($"api/CFCollection/{newCollection.id}", newCollection);

        //Remove the card from the collection
        newCollection.cards.Remove(newCollection.cards.Find(p => p.name.Contains(newCard.name)));
        HttpResponseMessage responseB = await localClient.PutAsJsonAsync<CFCollection>($"api/CFCollection/{newCollection.id}", newCollection);

        Assert.True((responseA.IsSuccessStatusCode && responseB.IsSuccessStatusCode), $"Test A - {responseA.StatusCode} : Test B - {responseB.StatusCode} ");
    }

    [Fact]
    public async Task CommanderCollectionDelete() //Delete a collection. Tests the controller's DELETE and services's Delete functions.
    {
        HttpClient localClient = new HttpClient();
        localClient.BaseAddress = new Uri("https://localhost:7277");

        //Fetch a collection from the list and remove it.
        List<CFCollection> collections = await CommanderFinderConsole.FetchApiData();
        CFCollection newCollection;
        int searchId = 0;
        do
        {
            newCollection = collections.Find(p => p.id == searchId);
            searchId++;
        } while (newCollection == null);

        HttpResponseMessage response = await localClient.DeleteAsync($"api/CFCollection/{newCollection.id}");
 
        Assert.True(response.IsSuccessStatusCode, $"{response.StatusCode}");
    }
}