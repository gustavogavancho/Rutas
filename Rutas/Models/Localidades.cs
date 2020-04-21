using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rutas.Models
{
    public class Localidades
    {
        public int Id { get; set; }
        public string Localidad { get; set; }
        public string Distrito { get; set; }
        public string Provincia { get; set; }
        public string Departamento { get; set; }
        public int Vsatid { get; set; }
        public string Telefonos { get; set; }

        public int Serviciosid { get; set; }
        [ForeignKey("Serviciosid")]
        public Servicios Servicios { get; set; }

        public int Proyectoid { get; set; }
        [ForeignKey("Proyectoid")]
        public Proyectos Proyectos { get; set; }

        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public double Costo { get; set; }

        public override string ToString() => $"{Localidad} {Vsatid}";
    }
}
