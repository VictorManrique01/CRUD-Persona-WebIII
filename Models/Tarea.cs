using System;

namespace HelloWorld.Models
{
    /// <summary>
    /// Representa una tarea registrada por el usuario.
    /// </summary>
    public class Tarea
    {
        public int Id { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public string Responsable { get; set; } = string.Empty;

        public DateTime FechaLimite { get; set; }

        public bool Completada { get; set; }
    }
}
