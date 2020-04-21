using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rutas.Models
{
    public class SelectedLocalidades
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

        public int Rutasid { get; set; }
        [ForeignKey("Rutasid")]
        public Rutas Rutas { get; set; }

        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public double Costo { get; set; }

        public Estado Estado { get; set; }
        public string Color { get; set; }

        public override string ToString() => $"{Localidad} {Vsatid}";
    }
}
