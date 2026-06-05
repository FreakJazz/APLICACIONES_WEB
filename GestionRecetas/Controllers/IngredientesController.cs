using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionRecetas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngredientesController : ControllerBase
    {
        private readonly GestionRecetasContext _context;

        public IngredientesController(GestionRecetasContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ingrediente>>> GetIngredientes()
        {
            return await _context.Ingredientes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ingrediente>> GetIngrediente(int id)
        {
            var ingrediente = await _context.Ingredientes.FindAsync(id);
            if (ingrediente == null)
                return NotFound();
            return ingrediente;
        }

        [HttpPost]
        public async Task<ActionResult<Ingrediente>> PostIngrediente(Ingrediente ingrediente)
        {
            _context.Ingredientes.Add(ingrediente);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetIngrediente", new { id = ingrediente.IngredienteId }, ingrediente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutIngrediente(int id, Ingrediente ingrediente)
        {
            if (id != ingrediente.IngredienteId)
                return BadRequest();

            _context.Entry(ingrediente).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IngredienteExists(id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngrediente(int id)
        {
            var ingrediente = await _context.Ingredientes.FindAsync(id);
            if (ingrediente == null)
                return NotFound();

            _context.Ingredientes.Remove(ingrediente);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool IngredienteExists(int id)
        {
            return _context.Ingredientes.Any(e => e.IngredienteId == id);
        }
    }
}
