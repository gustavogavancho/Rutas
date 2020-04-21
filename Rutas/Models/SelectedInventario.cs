using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rutas.Models
{
    public class SelectedInventario
    {
        public int Id { get; set; }
        public int Balance { get; set; }

        public int AlmacenesId { get; set; }
        [ForeignKey("AlmacenesId")]
        public Almacenes Almacenes { get; set; }

        public int LocacionesId { get; set; }
        [ForeignKey("LocacionesId")]
        public Locaciones Locaciones { get; set; }

        public int PartNumberId { get; set; }
        [ForeignKey("PartNumberId")]
        public PartNumbers PartNumbers { get; set; }

        public int Rutasid { get; set; }
        [ForeignKey("Rutasid")]
        public Rutas Rutas { get; set; }
    }
}
