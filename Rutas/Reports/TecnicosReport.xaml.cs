using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Rutas.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Rutas.Reports
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TecnicosReport : Page
    {
        private List<Tecnicos> Tecnicos;

        public TecnicosReport()
        {
            InitializeComponent();
            using (var db = new ProjectContext())
            {
                Tecnicos = db.Tecnicos.OrderBy(x=> x.Nombre).ToList();
            }
        }
    }
}
