using System;
using System.Collections.Generic;

namespace Rutas.Models
{
    public class Tecnicos
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Apocope { get; set; }
        public string Dni { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }
        public int Telefono { get; set; }
        public DateTime FechaNacimiento { get; set; }

        public List<Rutas> Rutas { get; set; }

        public override string ToString() => $"{Nombre} {Apellido}";
    }
}
