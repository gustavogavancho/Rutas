using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rutas.Models
{
    public class Proyectos
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }

        public List<Localidades> Localidades { get; set; }
        public List<SelectedLocalidades> SelectedLocalidades { get; set; }
    }
}
