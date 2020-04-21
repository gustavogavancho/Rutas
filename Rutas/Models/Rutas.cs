using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rutas.Models
{
    public class Rutas
    {
        public int Id { get; set; }

        public int TecnicosId { get; set; }
        [ForeignKey("TecnicosId")]
        public Tecnicos Tecnicos { get; set; }

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public Estado Estado { get; set; }
        public string Color { get; set; }

        public List<SelectedLocalidades> SelectedLocalidades { get; set; }
        public List<SelectedInventario> SelectedInventario { get; set; }

    }

    public enum Estado
    {
        Pendiente = 1,
        Finalizado = 2,
    }
}
