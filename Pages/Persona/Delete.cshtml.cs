using HelloWorld.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PersonaEntity = HelloWorld.Models.Persona;

namespace HelloWorld.Pages.Persona
{
    public class DeleteModel : PageModel
    {
        private readonly HWBDContext _context;

        public DeleteModel(HWBDContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PersonaEntity Persona { get; set; } = null!;

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

        public async Task<IActionResult> OnPostAsync()
        {
            var persona = await _context.Personas.FindAsync(Persona.Id);

            if (persona is null)
            {
                return NotFound();
            }

            _context.Personas.Remove(persona);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Persona eliminada correctamente.";
            return RedirectToPage("Index");
        }
    }
}
