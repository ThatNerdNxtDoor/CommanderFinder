using CF_API.Models;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;

namespace CF_API.Services
{
    public static class CFCollectionService
    {
        public static List<CFCollection> Collections { get; set;}
        static int nextId = 0;
        static CFCollectionService() //Constructor
        {
            Console.WriteLine("Initializing Collection Service");
            FetchDataStorage();
        }

        public static void FetchDataStorage() //Collects/Refreshes the data in the json storage
        {
            try {
                string JsonFile = System.IO.File.ReadAllText("./Resources/Collections.json");
                List<CFCollection> collectionData = JsonSerializer.Deserialize<List<CFCollection>>(JsonFile,
                    new JsonSerializerOptions {PropertyNameCaseInsensitive = true});
                if (collectionData != null)
                {
                    Collections = collectionData;
                } else
                {
                    Console.WriteLine("Creating new list.");
                    Collections = new List<CFCollection>();
                }
            } catch (Exception e)
            {
                Console.WriteLine(e);
                Collections = new List<CFCollection>();
            }
            Console.WriteLine(string.Join(",", Collections));
            Console.WriteLine(Collections.Find(p => p.id == 0));
            Console.WriteLine(Collections);
        }

        public static List<CFCollection> GetAll() => Collections; //Get the entire list of collections

        public static CFCollection? Get(int id) => Collections.FirstOrDefault(p => p.id == id); //Get a specific list of collections (currently unused).

        public static async Task Add(CFCollection coll) //Add a collection to the database
        {
            //Iterate through the IDs until there is an available one.
            nextId = 0;
            while (Collections.Find(p => p.id == nextId) != null)
            {
                nextId++;
            }
            coll.id = nextId;
            coll.cards = new List<Card>();
            Collections.Add(coll);
            SaveToJSON();
        }

        public static void Add(Card commander) //Create a new collection using a commander and add it to the database. (Technically not needed but is still here just in case.)
        {
            CFCollection coll = new CFCollection();
            coll.commander = commander;
            //Iterate through the IDs until there is an available one.
            nextId = 0;
            while (Collections.Find(p => p.id == nextId) != null)
            {
                nextId++;
            }
            coll.id = nextId++;
            coll.cards = new List<Card>();
            Collections.Add(coll);
            Console.WriteLine("Added to Collections");
            SaveToJSON();
        }

        public static void Delete(int id) //Remove a collection from the list.
        {
            var coll = Get(id);
            if(coll is null) {
                return;
            }
            Collections.Remove(coll);
            SaveToJSON();
        }

        public static void Update(CFCollection coll) //Update the collection (add/remove card)
        {
            var index = Collections.FindIndex(p => p.id == coll.id);
            if(index == -1) {
                return;
            }
            Collections[index] = coll;
            SaveToJSON();
        }

        public static async void SaveToJSON() //Save the service collection to the JSON and sync the memory storage.
        {
            File.WriteAllText("./Resources/Collections.json", JsonSerializer.Serialize(Collections));
            Console.WriteLine("Saved to JSON");
            FetchDataStorage();
        }
    }
}

