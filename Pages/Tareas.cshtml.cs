using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HelloWorld.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HelloWorld.Pages
{
    public class TareasModel : PageModel
    {
        private readonly TareaService _tareaService;

        public TareasModel(TareaService tareaService)
        {
            _tareaService = tareaService;
        }

        // Datos del formulario para registrar una nueva tarea.
        [BindProperty]
        public string Titulo { get; set; } = string.Empty;

        [BindProperty]
        public string Responsable { get; set; } = string.Empty;

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime FechaLimite { get; set; } = DateTime.Today;

        // Lista de tareas que se muestra en la tabla.
        public IReadOnlyList<Tarea> Tareas { get; private set; } = Array.Empty<Tarea>();

        public string? Mensaje { get; private set; }

        public void OnGet()
        {
            CargarTareas();
        }

        // Se ejecuta al presionar "Agregar tarea".
        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(Titulo) || string.IsNullOrWhiteSpace(Responsable))
            {
                Mensaje = "El título y el responsable son obligatorios.";
                CargarTareas();
                return Page();
            }

            _tareaService.Agregar(Titulo.Trim(), Responsable.Trim(), FechaLimite);

            // Redirigimos a GET para evitar reenvíos del formulario y limpiar los campos.
            return RedirectToPage();
        }

        // Cambia el estado de la tarea a Completada.
        public IActionResult OnPostCompletar(int id)
        {
            _tareaService.MarcarCompletada(id);
            return RedirectToPage();
        }

        // Elimina la tarea seleccionada de la lista.
        public IActionResult OnPostEliminar(int id)
        {
            _tareaService.Eliminar(id);
            return RedirectToPage();
        }

        private void CargarTareas()
        {
            Tareas = _tareaService.ObtenerTodas();
        }
    }
}
