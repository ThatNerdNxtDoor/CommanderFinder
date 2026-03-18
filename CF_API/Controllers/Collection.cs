using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using CF_API.Models;
using CF_API.Services;

namespace CF_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CFCollectionController : ControllerBase
    {
        private List<CFCollection> collections {get;} = new List<CFCollection>();

        public CFCollectionController()
        {
            string JsonFile = System.IO.File.ReadAllText("./Resources/Collections.json");
            var collectionData = JsonSerializer.Deserialize<List<CFCollection>>(JsonFile,
                new JsonSerializerOptions {PropertyNameCaseInsensitive = true});
            if (collectionData != null)
            {
                collections = collectionData;
            }
        }

        [HttpGet]
        public ActionResult<List<CFCollection>> GetCollection()
        {
            return Ok(collections);
        }

        [HttpGet("{id}")]
        public ActionResult<CFCollection> GetCollection(int id)
        {
            var Collection = collections.FirstOrDefault(c => c.Id == id);

            if (Collection == null)
            {
                return NotFound();
            }

            return Ok(Collection);
        }

    [HttpPost]
    public IActionResult Create(CFCollection coll)
    {            
        CFCollectionService.Add(coll);
        return CreatedAtAction(nameof(GetCollection), new { id = coll.Id }, coll);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, CFCollection coll)
    {
        if (id != coll.Id)
        return BadRequest();
           
        //var existingPizza = PizzaService.Get(id);
        //if(existingPizza is null)
        //    return NotFound();
        //
        //PizzaService.Update(pizza);           
    
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        //var coll = PizzaService.Get(id);
        
        //if (pizza is null)
        //    return NotFound();
        
        //PizzaService.Delete(id);
    
        return NoContent();
    }
    }
}