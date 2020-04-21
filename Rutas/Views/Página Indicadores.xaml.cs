using Microsoft.EntityFrameworkCore;
using Rutas.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Rutas.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Página_Indicadores : Page
    {
        public int cantidad1;

        public Página_Indicadores()
        {
            this.InitializeComponent();

            using (var db = new ProjectContext())
            {
                var query = db.SelectedLocalidades.Include(x => x.Rutas).Where(x => x.Estado == Estado.Finalizado && x.Rutas.Estado == Estado.Finalizado && x.Rutas.FechaFin.Month == SelectDateDatePicker.Date.Month && x.Rutas.FechaFin.Year == SelectDateDatePicker.Date.Year).Count();
                RadialGauge1.Value = query;

                var query1 = db.SelectedLocalidades.Include(x => x.Rutas).Where(x => x.Estado == Estado.Pendiente && x.Rutas.Estado == Estado.Finalizado && x.Rutas.FechaFin.Month == SelectDateDatePicker.Date.Month && x.Rutas.FechaFin.Year == SelectDateDatePicker.Date.Year).Count();
                RadialGauge2.Value = query1;

            }
        }

        private void SelectDateDatePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            using (var db = new ProjectContext())
            {
                var query = db.SelectedLocalidades.Include(x => x.Rutas).Where(x => x.Estado == Estado.Finalizado && x.Rutas.Estado == Estado.Finalizado && x.Rutas.FechaFin.Month == SelectDateDatePicker.Date.Month && x.Rutas.FechaFin.Year == SelectDateDatePicker.Date.Year).Count();
                RadialGauge1.Value = query;

                var query1 = db.SelectedLocalidades.Include(x => x.Rutas).Where(x => x.Estado == Estado.Pendiente && x.Rutas.Estado == Estado.Finalizado && x.Rutas.FechaFin.Month == SelectDateDatePicker.Date.Month && x.Rutas.FechaFin.Year == SelectDateDatePicker.Date.Year).Count();
                RadialGauge2.Value = query1;

            }

        }
    }
}
