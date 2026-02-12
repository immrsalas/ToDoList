using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList
{
        public class Tarea
        {
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public bool Completada { get; private set; }
            public DateTime FechaCreacion { get; }

            public Tarea(string titulo, string descripcion)
            {
                if (string.IsNullOrWhiteSpace(titulo))
                    throw new ArgumentException("El título no puede estar vacío");

                Titulo = titulo;
                Descripcion = descripcion;
                FechaCreacion = DateTime.Now;
                Completada = false;
            }

            public void MarcarCompleta()
            {
                Completada = true;
            }

            public override string ToString()
            {
                string estado = Completada ? "✓" : "Pendiente";
                return $"{Titulo} - {estado}";
            }
        }
}
