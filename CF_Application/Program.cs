using System.Text.Json;
using CF_Console.Models;

HttpClient client = new HttpClient();

client.BaseAddress = new Uri("http://localhost:5143");
HttpResponseMessage response = await client.GetAsync("api/CommanderFinder");