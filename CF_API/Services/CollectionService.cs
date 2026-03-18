using CF_API.Models;

namespace CF_API.Services
{
    public static class CFCollectionService
    {
        static List<CFCollection> Collections { get; }
        static int nextId = 0;
        static CFCollectionService()
        {
            Collections = new List<CFCollection>
            {
                
            };
        }

        public static List<CFCollection> GetAll() => Collections;

        public static CFCollection? Get(int id) => Collections.FirstOrDefault(p => p.Id == id);

        public static void Add(CFCollection coll)
        {
            coll.Id = nextId++;
            Collections.Add(coll);
        }

        public static void Delete(int id)
        {
            var coll = Get(id);
            if(coll is null)
                return;

            Collections.Remove(coll);
        }

        public static void Update(CFCollection coll)
        {
            var index = Collections.FindIndex(p => p.Id == coll.Id);
            if(index == -1)
                return;

            Collections[index] = coll;
        }
    }
}

