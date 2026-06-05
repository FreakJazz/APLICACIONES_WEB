using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionRecetas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecetasController : ControllerBase
    {
        private readonly GestionRecetasContext _context;

        public RecetasController(GestionRecetasContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Receta>>> GetRecetas()
        {
            return await _context.Recetas.Include(r => r.Ingredientes).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Receta>> GetReceta(int id)
        {
            var receta = await _context.Recetas
                .Include(r => r.Ingredientes)
                .FirstOrDefaultAsync(r => r.RecetaId == id);

            if (receta == null)
                return NotFound();

            return receta;
        }

        [HttpPost]
        public async Task<ActionResult<Receta>> PostReceta(Receta receta)
        {
            _context.Recetas.Add(receta);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetReceta", new { id = receta.RecetaId }, receta);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutReceta(int id, Receta receta)
        {
            if (id != receta.RecetaId)
                return BadRequest();

            _context.Entry(receta).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecetaExists(id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReceta(int id)
        {
            var receta = await _context.Recetas.FindAsync(id);
            if (receta == null)
                return NotFound();

            _context.Recetas.Remove(receta);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool RecetaExists(int id)
        {
            return _context.Recetas.Any(e => e.RecetaId == id);
        }
    }
}
