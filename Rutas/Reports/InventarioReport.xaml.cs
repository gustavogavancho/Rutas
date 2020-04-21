using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Microsoft.EntityFrameworkCore;
using Rutas.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Rutas.Reports
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InventarioReport : Page
    {
        public List<Inventario> Inventario;

        public InventarioReport()
        {
            InitializeComponent();

            using (var db = new ProjectContext())
            {
                Inventario = db.Inventario
                    .Include(x => x.PartNumbers)
                    .Include(x => x.Almacenes)
                    .Include(x=> x.Locaciones)
                    .OrderBy(x=> x.Almacenes.Descripcion)
                    .ThenBy(x=> x.Locaciones.Descripcion)
                    .ToList();
            }
        }
    }
}
