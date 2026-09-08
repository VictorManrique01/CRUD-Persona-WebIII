using HelloWorld.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PersonaEntity = HelloWorld.Models.Persona;

namespace HelloWorld.Pages.Persona
{
    public class DetailsModel : PageModel
    {
        private readonly HWBDContext _context;

        public DetailsModel(HWBDContext context)
        {
            _context = context;
        }

        public PersonaEntity Persona { get; private set; } = null!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var persona = await _context.Personas
                .AsNoTracking()
                .Include(p => p.Pais)
                .Include(p => p.Pasaporte)
                .Include(p => p.Materias)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (persona is null)
            {
                return NotFound();
            }

            Persona = persona;
            return Page();
        }
    }
}
