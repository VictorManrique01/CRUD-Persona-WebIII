namespace HelloWorld.Models
{
    public class Pais
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<Persona> Personas { get; set; } = new List<Persona>();
    }
}
