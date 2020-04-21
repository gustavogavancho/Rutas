using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rutas.Models
{
    public class PartNumbers
    {
        public int Id { get; set; }
        public Categoria Categoria { get; set; }
        public string PartNumber { get; set; }
        public string Descripcion { get; set; }

        public List<Inventario> Inventario { get; set; }
        public List<SelectedInventario> SelectedInventario { get; set; }
    }

    public enum Categoria
    {
        Equipos = 1,
        Consumibles = 2,
        Estructuras = 3,
    }
}
