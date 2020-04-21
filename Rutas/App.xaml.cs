using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Microsoft.EntityFrameworkCore;
using Rutas.Models;
using Rutas.Services.SettingsServices;
using Rutas.Views;
using Template10.Common;
using Template10.Controls;

namespace Rutas
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki
    [Bindable]
    public sealed partial class App : BootStrapper
    {
        public App()
        {
            InitializeComponent();
            SplashFactory = e => new Splash(e);

            #region App settings

            var _settings = SettingsService.Instance;
            RequestedTheme = _settings.AppTheme;
            CacheMaxDuration = _settings.CacheMaxDuration;
            ShowShellBackButton = _settings.UseShellBackButton;

            #endregion

            #region Migrate DataBase
            using (var db = new ProjectContext())
            {
                db.Database.Migrate();
            }
            #endregion

            #region Inicializar
            using (var db = new ProjectContext())
            {
                var query = db.Servicios.Count();
                var query1 = db.Proyectos.Count();
                if (query <= 0 && query1 <= 0)
                {
                    db.Servicios.Add(new Servicios { Descripcion = "Telefonía" });
                    db.Servicios.Add(new Servicios { Descripcion = "Internet" });
                    db.Servicios.Add(new Servicios { Descripcion = "Datos" });
                    db.Servicios.Add(new Servicios { Descripcion = "Telefonía e Internet" });
                    db.Servicios.Add(new Servicios { Descripcion = "Telefonia, Internet y Datos" });
                    db.Servicios.Add(new Servicios { Descripcion = "Telefonía y Datos" });
                    db.Servicios.Add(new Servicios { Descripcion = "Datos e Internet" });

                    db.Proyectos.Add(new Proyectos { Descripcion = "Fitel I" });
                    db.Proyectos.Add(new Proyectos { Descripcion = "Filte II" });
                    db.Proyectos.Add(new Proyectos { Descripcion = "Filtel III" });
                    db.Proyectos.Add(new Proyectos { Descripcion = "Filtel IV" });
                    db.Proyectos.Add(new Proyectos { Descripcion = "Filtel VIII" });
                    db.Proyectos.Add(new Proyectos { Descripcion = "Privados" });
                    db.Proyectos.Add(new Proyectos { Descripcion = "Banco de la Nación" });
                    db.Proyectos.Add(new Proyectos { Descripcion = "Cherry Picking" });
                    db.Proyectos.Add(new Proyectos { Descripcion = "Otros" });

                    db.SaveChanges();
                }
            }
            #endregion

        }

        public override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            if (!(Window.Current.Content is ModalDialog))
            {
                // create a new frame 
                var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);

                // create modal root
                Window.Current.Content = new ModalDialog
                {
                    DisableBackButtonWhenModal = true,
                    Content = new Shell(nav),
                    ModalContent = new Busy()
                };
            }
            await Task.CompletedTask;
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            // long-running startup tasks go here
            await Task.Delay(4000);

            NavigationService.Navigate(typeof(MainPage));
            await Task.CompletedTask;
        }
    }
}