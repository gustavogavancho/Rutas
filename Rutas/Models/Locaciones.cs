using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rutas.Models
{
    public class Locaciones
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string Apocope { get; set; }

        public List<Inventario> Inventario { get; set; }
        public List<SelectedInventario> SelectedInventario { get; set; }
    }
}
