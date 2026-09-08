using HelloWorld.Data;
using HelloWorld.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HelloWorld.Pages
{
    public class ContactModel : PageModel

    {
        private readonly HWBDContext _context;
        private static List<Contacto> ListaContactos = new List<Contacto>();
        [BindProperty]
        public ContactoInput Input { get; set; } = new ContactoInput();
        
        public List<Contacto> Contactos => ListaContactos;
        private static int siguienteID = 1;

        public ContactModel(HWBDContext context)
        {
            _context = context;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {

            Contacto nuevoContacto = new Contacto
            {
                Nombre = Input.Nombre,
                Email = Input.Email,
                Mensaje = Input.Mensaje
            };
            _context.Contactos.Add(nuevoContacto);
            await _context.SaveChangesAsync();
            return RedirectToPage();
        }
        public IActionResult OnPostEliminar(int Id)
        {
            Contacto? contacto = ListaContactos.FirstOrDefault (x => x.Id == Id);

            if (contacto != null) 
            {
                ListaContactos.Remove(contacto);
            }
            return RedirectToPage();
        }
        public class ContactoInput
        {
            public int Id { get; set; } = 0;
            public string Nombre { get; set; } = "";
            public string Email { get; set; } = "";
            public string Mensaje { get; set; } = "";
        }

    }
}
