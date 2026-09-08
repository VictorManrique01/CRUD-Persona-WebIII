namespace HelloWorld.Models
{
    public class Pasaporte
    {
        public int Id { get; set; }
        public string Numero { get; set; } = string.Empty;

        public int personaId { get; set; }
        public Persona Persona { get; set; } = null!;
    }
}
