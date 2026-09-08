using HelloWorld.Data;
using HelloWorld.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PersonaEntity = HelloWorld.Models.Persona;

namespace HelloWorld.Pages.Persona
{
    public class CreateModel : PageModel
    {
        private readonly HWBDContext _context;

        public CreateModel(HWBDContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PersonaFormInput Input { get; set; } = new PersonaFormInput();

        public List<SelectListItem> Paises { get; private set; } = new List<SelectListItem>();
        public List<SelectListItem> Materias { get; private set; } = new List<SelectListItem>();

        public async Task OnGetAsync()
        {
            await CargarCatalogosAsync();
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

            var persona = new PersonaEntity
            {
                Name = Input.Nombre.Trim(),
                dob = Input.FechaNacimiento,
                PaisId = Input.PaisId,
                Materias = materiasSeleccionadas
            };

            if (!string.IsNullOrWhiteSpace(Input.NumeroPasaporte))
            {
                persona.Pasaporte = new Pasaporte
                {
                    Numero = Input.NumeroPasaporte.Trim()
                };
            }

            _context.Personas.Add(persona);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Persona registrada correctamente.";
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
