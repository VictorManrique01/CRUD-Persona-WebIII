namespace HelloWorld.Models
{
    public class Persona
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateOnly dob { get; set; }

        public Pasaporte? Pasaporte { get; set; }
        public int PaisId { get; set; }
        public Pais Pais { get; set; } = null!;
        public List<Materia> Materias { get; set; } = new List<Materia>();
    }
}
