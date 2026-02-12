using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList
{
    public class GestorTareas
    {
        private List<Tarea> tareas;

        public IReadOnlyList<Tarea> Tareas => tareas;

        public GestorTareas()
        {
            tareas = new List<Tarea>();
        }

        public void AgregarTarea(Tarea tarea)
        {
            tareas.Add(tarea);
        }

        public void EliminarTarea(Tarea tarea)
        {
            tareas.Remove(tarea);
        }

        public void MarcarCompleta(Tarea tarea)
        {
            tarea.MarcarCompleta();
        }

        public List<Tarea> ObtenerPendientes()
        {
            return tareas.Where(t => !t.Completada).ToList();
        }

        public List<Tarea> ObtenerCompletadas()
        {
            return tareas.Where(t => t.Completada).ToList();
        }
    }
}
