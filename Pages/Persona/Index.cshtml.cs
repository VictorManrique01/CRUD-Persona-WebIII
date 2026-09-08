using HelloWorld.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PersonaEntity = HelloWorld.Models.Persona;

namespace HelloWorld.Pages.Persona
{
    public class IndexModel : PageModel
    {
        private readonly HWBDContext _context;

        public IndexModel(HWBDContext context)
        {
            _context = context;
        }

        public IList<PersonaEntity> Personas { get; private set; } = new List<PersonaEntity>();

        [TempData]
        public string? Mensaje { get; set; }

        public async Task OnGetAsync()
        {
            Personas = await _context.Personas
                .AsNoTracking()
                .Include(p => p.Pais)
                .Include(p => p.Pasaporte)
                .Include(p => p.Materias)
                .OrderBy(p => p.Id)
                .ToListAsync();
        }
    }
}
