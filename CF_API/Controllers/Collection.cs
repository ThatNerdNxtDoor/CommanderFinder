using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using CF_API.Models;
using CF_API.Services;

//Controller is the middleman for the API commands, the Service is the one that actually modifies the information.

namespace CF_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CFCollectionController : ControllerBase
    {

        public CFCollectionController()
        {
            
        }

        [HttpGet]
        public ActionResult<List<CFCollection>> GetCollection()
        {
            return Ok(CFCollectionService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<CFCollection> GetCollection(int id)
        {
            var collection = CFCollectionService.Get(id);

            if (collection == null)
            {
                return NotFound();
            }

            return Ok(collection);
        }

        [Route("")] //Don't entirely understand why, but putting a route over the POST function allows it to take the post request instead or returning a 405 error
        [HttpPost("{commander}")]
        public async Task<IActionResult> Create(Card commander)
        {
            Console.WriteLine(commander);
            CFCollection coll = new CFCollection();
            coll.commander = commander;
            CFCollectionService.Add(coll);
            return CreatedAtAction(nameof(GetCollection), new { id = coll.id }, coll);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CFCollection coll)
        {
            //CFCollection deserialColl = JsonSerializer.Deserialize<CFCollection>(coll,
            //    new JsonSerializerOptions {PropertyNameCaseInsensitive = true});
            if (id != coll.id)
            return BadRequest();
            
            var existingColl = CFCollectionService.Get(id);
            if(existingColl is null) {
                return NotFound();
            }
            CFCollectionService.Update(coll);           
        
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var coll = CFCollectionService.Get(id);
            
            if (coll is null)
                return NotFound();
            
            CFCollectionService.Delete(id);
        
            return NoContent();
        }
    }
}