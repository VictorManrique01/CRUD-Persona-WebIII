using System.ComponentModel.DataAnnotations;

namespace HelloWorld.Pages.Persona
{
    public class PersonaFormInput
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de nacimiento")]
        public DateOnly FechaNacimiento { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un país.")]
        [Display(Name = "País")]
        public int PaisId { get; set; }

        [Display(Name = "Número de pasaporte")]
        public string? NumeroPasaporte { get; set; }

        [Required(ErrorMessage = "Debe seleccionar al menos una materia.")]
        [Display(Name = "Materias")]
        public List<int> MateriasIds { get; set; } = new List<int>();
    }
}
