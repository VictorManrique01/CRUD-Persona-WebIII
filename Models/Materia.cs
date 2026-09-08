namespace HelloWorld.Models
{
    public class Materia
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }

        public List<Persona> Personas { get; set; } = new List<Persona>();
    }
}
