using Microsoft.EntityFrameworkCore;
using Rutas.Models;
using System;
using System.Linq;
using Template10.Utils;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Rutas.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;

            var appointment = new Windows.ApplicationModel.Appointments.Appointment();
        }

        private async void RutasPageButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var service = Frame.GetNavigationService();
            await service.NavigateAsync(typeof(Views.RutasPage));
        }

        private async void LoalidadesPageButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var service = Frame.GetNavigationService();
            await service.NavigateAsync(typeof(Views.LocalidadesPage));
        }

        private async void TecnicosPageButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var service = Frame.GetNavigationService();
            await service.NavigateAsync(typeof(Views.TecnicosPage));
        }

        private async void InventarioPageButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var service = Frame.GetNavigationService();
            await service.NavigateAsync(typeof(Views.InventarioPage));
        }

        private async void AlmacenPageButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var service = Frame.GetNavigationService();
            await service.NavigateAsync(typeof(Views.AlmacenesPage));
        }

        private async void LocacionPageButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var service = Frame.GetNavigationService();
            await service.NavigateAsync(typeof(Views.LocacionesPage));
        }

        private async void PartNumberPageButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var service = Frame.GetNavigationService();
            await service.NavigateAsync(typeof(Views.PartNumbersPage));
        }

        private async void IndicadoresPageButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var service = Frame.GetNavigationService();
            await service.NavigateAsync(typeof(Views.Página_Indicadores));
        }
    }
}