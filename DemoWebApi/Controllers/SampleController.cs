using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DemoWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SampleController : ControllerBase
    {
        private readonly List<string> _items;

        public SampleController(List<string>? items = null)
        {
            _items = items ?? new List<string> { "Apple", "Banana", "Carrot" };
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_items);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            if (id < 0 || id >= _items.Count)
                return NotFound();
            return Ok(_items[id]);
        }

        [HttpPost]
        public IActionResult Add([FromBody] string item)
        {
            if (string.IsNullOrWhiteSpace(item))
                return BadRequest("Item cannot be empty");

            _items.Add(item);
            return CreatedAtAction(nameof(GetById), new { id = _items.Count - 1 }, item);
        }
    }
}
