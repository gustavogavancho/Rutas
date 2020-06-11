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
using System.Net;
using System;

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

                if (!db.Tecnicos.Any())
                {
                    var tecnicos = new Tecnicos
                    {
                        Id = 1,
                        Nombre = "Gustavo",
                        Apellido = "Gavancho León",
                        Apocope = "G",
                        Dni = "73215945",
                        Direccion = "Psje. Limatambo 121",
                        Email = "gustavo.gavancho.l@gmail.com",
                        Telefono = 993704915,
                        FechaNacimiento = DateTime.Now,
                    };
                    db.Add(tecnicos);
                    db.SaveChangesAsync();
                }
                if (!db.Almacenes.Any())
                {
                    var almacen = new Almacenes
                    {
                        Id = 1,
                        Descripcion = "General",
                        Apocope = "G",
                    };
                    db.Add(almacen);
                    db.SaveChangesAsync();
                }
                if (!db.Locaciones.Any())
                {
                    var locacion1 = new Locaciones
                    {
                        Id = 1,
                        Descripcion = "Malogrados",
                        Apocope = "M",
                    };
                    var locacion2 = new Locaciones
                    {
                        Id = 2,
                        Descripcion = "Robados",
                        Apocope = "M",
                    };
                    var locacion3 = new Locaciones
                    {
                        Id = 3,
                        Descripcion = "Normal",
                        Apocope = "N",
                    };
                    db.AddRange(locacion1, locacion2, locacion3);
                    db.SaveChangesAsync();
                }
                if (!db.PartNumbers.Any())
                {
                    var partnumber = new PartNumbers
                    {
                        Id = 1,
                        Categoria = Categoria.Equipos,
                        PartNumber = "541210-v",
                        Descripcion = "VSAT DUAL BAND",
                    };
                    db.Add(partnumber);
                    db.SaveChangesAsync();
                }
                if (!db.Localidades.Any())
                {
                    var localidad = new Localidades
                    {
                        Id = 1,
                        Localidad = "Nueva Union",
                        Distrito = "La Banda de Shilcayo",
                        Provincia = "San Martin",
                        Departamento = "San Martin",
                        Vsatid = 541310,
                        Telefonos = "9784984516",
                        Serviciosid = 1,
                        Proyectoid = 1,
                        Latitud = 45.87984984948,
                        Longitud = 45.98498984949,
                        Costo = 1100.77,
                    };
                    db.Add(localidad);
                    db.SaveChangesAsync();
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