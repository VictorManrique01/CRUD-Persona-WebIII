using System;
using System.Collections.Generic;
using System.Linq;

namespace HelloWorld.Models
{
    /// <summary>
    /// Servicio en memoria (singleton) que mantiene la lista temporal de tareas
    /// mientras la aplicación está en ejecución.
    /// </summary>
    public class TareaService
    {
        private readonly List<Tarea> _tareas = new();
        private int _siguienteId = 1;

        public IReadOnlyList<Tarea> ObtenerTodas()
        {
            return _tareas
                .OrderBy(t => t.Id)
                .ToList();
        }

        public Tarea Agregar(string titulo, string responsable, DateTime fechaLimite)
        {
            var nuevaTarea = new Tarea
            {
                Id = _siguienteId++,
                Titulo = titulo,
                Responsable = responsable,
                FechaLimite = fechaLimite,
                Completada = false
            };

            _tareas.Add(nuevaTarea);
            return nuevaTarea;
        }

        public bool MarcarCompletada(int id)
        {
            var tarea = _tareas.FirstOrDefault(t => t.Id == id);
            if (tarea is null)
            {
                return false;
            }

            tarea.Completada = true;
            return true;
        }

        public bool Eliminar(int id)
        {
            var tarea = _tareas.FirstOrDefault(t => t.Id == id);
            if (tarea is null)
            {
                return false;
            }

            return _tareas.Remove(tarea);
        }
    }
}
