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
    public sealed partial class LocalidadesReport : Page
    {
        private List<Localidades> Localidades;

        public LocalidadesReport()
        {
            InitializeComponent();

            using (var db = new ProjectContext())
            {
                Localidades = db.Localidades
                    .Include(x => x.Servicios)
                    .Include(x => x.Proyectos)
                    .ToList();
            }
        }
    }
}
