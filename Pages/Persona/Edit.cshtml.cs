using HelloWorld.Data;
using HelloWorld.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HelloWorld.Pages.Persona
{
    public class EditModel : PageModel
    {
        private readonly HWBDContext _context;

        public EditModel(HWBDContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public PersonaFormInput Input { get; set; } = new PersonaFormInput();

        public List<SelectListItem> Paises { get; private set; } = new List<SelectListItem>();
        public List<SelectListItem> Materias { get; private set; } = new List<SelectListItem>();

        public async Task<IActionResult> OnGetAsync()
        {
            var persona = await _context.Personas
                .AsNoTracking()
                .Include(p => p.Pasaporte)
                .Include(p => p.Materias)
                .FirstOrDefaultAsync(p => p.Id == Id);

            if (persona is null)
            {
                return NotFound();
            }

            Input = new PersonaFormInput
            {
                Nombre = persona.Name,
                FechaNacimiento = persona.dob,
                PaisId = persona.PaisId,
                NumeroPasaporte = persona.Pasaporte?.Numero,
                MateriasIds = persona.Materias.Select(m => m.Id).ToList()
            };

            await CargarCatalogosAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ValidarFechaNacimiento();

            if (!await _context.Paises.AnyAsync(p => p.Id == Input.PaisId))
            {
                ModelState.AddModelError("Input.PaisId", "El país seleccionado no existe.");
            }

            if (!ModelState.IsValid)
            {
                await CargarCatalogosAsync();
                return Page();
            }

            var persona = await _context.Personas
                .Include(p => p.Pasaporte)
                .Include(p => p.Materias)
                .FirstOrDefaultAsync(p => p.Id == Id);

            if (persona is null)
            {
                return NotFound();
            }

            var materiasIds = Input.MateriasIds.Distinct().ToList();
            var materiasSeleccionadas = await _context.Materias
                .Where(m => materiasIds.Contains(m.Id))
                .ToListAsync();

            if (materiasSeleccionadas.Count != materiasIds.Count)
            {
                ModelState.AddModelError("Input.MateriasIds", "Una de las materias seleccionadas no existe.");
                await CargarCatalogosAsync();
                return Page();
            }

            persona.Name = Input.Nombre.Trim();
            persona.dob = Input.FechaNacimiento;
            persona.PaisId = Input.PaisId;

            persona.Materias.Clear();
            foreach (var materia in materiasSeleccionadas)
            {
                persona.Materias.Add(materia);
            }

            if (string.IsNullOrWhiteSpace(Input.NumeroPasaporte))
            {
                if (persona.Pasaporte is not null)
                {
                    _context.Pasaportes.Remove(persona.Pasaporte);
                    persona.Pasaporte = null;
                }
            }
            else if (persona.Pasaporte is null)
            {
                persona.Pasaporte = new Pasaporte
                {
                    Numero = Input.NumeroPasaporte.Trim()
                };
            }
            else
            {
                persona.Pasaporte.Numero = Input.NumeroPasaporte.Trim();
            }

            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Persona actualizada correctamente.";
            return RedirectToPage("Index");
        }

        private void ValidarFechaNacimiento()
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);

            if (Input.FechaNacimiento == default || Input.FechaNacimiento > hoy)
            {
                ModelState.AddModelError(
                    "Input.FechaNacimiento",
                    "Ingrese una fecha de nacimiento válida.");
            }
        }

        private async Task CargarCatalogosAsync()
        {
            Paises = await _context.Paises
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .Select(p => new SelectListItem(p.Name, p.Id.ToString()))
                .ToListAsync();

            Materias = await _context.Materias
                .AsNoTracking()
                .OrderBy(m => m.Name)
                .Select(m => new SelectListItem(m.Name, m.Id.ToString()))
                .ToListAsync();
        }
    }
}
