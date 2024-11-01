using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObject.SharedModels;
using BusinessObject.SharedModels.Models;

namespace Ponds.Microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PondsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PondsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Ponds
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pond>>> GetPonds()
        {
            return await _context.Ponds.ToListAsync();
        }

        // GET: api/Ponds/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pond>> GetPond(Guid id)
        {
            var pond = await _context.Ponds.FindAsync(id);

            if (pond == null)
            {
                return NotFound();
            }

            return pond;
        }

        // PUT: api/Ponds/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPond(Guid id, Pond pond)
        {
            if (id != pond.Id)
            {
                return BadRequest();
            }

            _context.Entry(pond).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PondExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Ponds
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pond>> PostPond(Pond pond)
        {
            _context.Ponds.Add(pond);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPond", new { id = pond.Id }, pond);
        }

        // DELETE: api/Ponds/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePond(Guid id)
        {
            var pond = await _context.Ponds.FindAsync(id);
            if (pond == null)
            {
                return NotFound();
            }

            _context.Ponds.Remove(pond);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PondExists(Guid id)
        {
            return _context.Ponds.Any(e => e.Id == id);
        }
    }
}
