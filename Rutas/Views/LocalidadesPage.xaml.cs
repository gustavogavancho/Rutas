using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Microsoft.EntityFrameworkCore;
using Prueba2.Services;
using Rutas.Models;
using Rutas.Reports;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Rutas.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LocalidadesPage : Page
    {
        private PrintHelper printHelper;

        private int flag = 0;

        private Localidades _lastSelectedItem;

        RandomAccessStreamReference mapIconStreamReference;

        MapIcon mapIcon1 = new MapIcon();

        public LocalidadesPage()
        {
            InitializeComponent();
            mapIconStreamReference = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/MapPin.png"));
        }

        #region Helper Methods

        #region ValidateSaveButton
        public void ValidateSaveButton()
        {
            if (DepartamentoValidator.Text == "Valido" && ProvinciaValidator.Text == "Valido" &&
                DistritoValidator.Text == "Valido" && LocalidadValidator.Text == "Valido" &&
                ServicioValidator.Text == "Valido" && ProyectoValidator.Text == "Valido" &&
                VsatidValidator.Text == "Valido" && CostoValidator.Text == "Valido" &&
                LatitudValidator.Text == "Valido" && LongitudValidator.Text == "Valido" &&
                CostoValidator.Text == "Valido")
            {
                SaveButton.IsEnabled = true;
            }
            else
            {
                SaveButton.IsEnabled = false;
            }
        }
        #endregion

        #region CleanInputs
        public void CleanInputs()
        {
            DepartamentoTextBox.Text = string.Empty;
            ProvinciaTextBox.Text = string.Empty;
            DistritoTextBox.Text = string.Empty;
            LocalidadTextBox.Text = string.Empty;
            ServicioComboBox.SelectedItem = null;
            ProyectoComboBox.SelectedItem = null;
            VsatidTextBox.Text = string.Empty;
            CostoTextBox.Text = string.Empty;
            LongitudTextBox.Text = string.Empty;
            LatitudTextBox.Text = string.Empty;
            CostoTextBox.Text = string.Empty;
            TelefonosTextBox.Text = string.Empty;
        }
        #endregion

        #endregion

        #region Validate Methods

        #region Departamento Method
        public void ValidateDepartamento()
        {
            if (DepartamentoTextBox.Text == string.Empty)
            {
                DepartamentoValidator.Text = "Campo requerido";
                DepartamentoRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveButton.IsEnabled = false;
            }
            else
            {
                DepartamentoValidator.Text = "Valido";
                DepartamentoRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                ValidateSaveButton();
            }
        }
        #endregion

        #region Provincia Method
        public void ValidateProvincia()
        {
            if (ProvinciaTextBox.Text == string.Empty)
            {
                ProvinciaValidator.Text = "Campo requerido";
                ProvinciaRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveButton.IsEnabled = false;
            }
            else
            {
                ProvinciaValidator.Text = "Valido";
                ProvinciaRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                ValidateSaveButton();
            }
        }
        #endregion

        #region Distrito Method
        public void ValidateDistrito()
        {
            if (DistritoTextBox.Text == string.Empty)
            {
                DistritoValidator.Text = "Campo requerido";
                DistritoRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveButton.IsEnabled = false;
            }
            else
            {
                DistritoValidator.Text = "Valido";
                DistritoRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                ValidateSaveButton();
            }
        }
        #endregion

        #region Localidad Method
        public void ValidateLocalidad()
        {
            if (LocalidadTextBox.Text == string.Empty)
            {
                LocalidadValidator.Text = "Campo requerido";
                LocalidadRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveButton.IsEnabled = false;
            }
            else
            {
                LocalidadValidator.Text = "Valido";
                LocalidadRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                ValidateSaveButton();
            }
        }
        #endregion

        #region Servicio Method
        public void ValidateServicio()
        {
            if (ServicioComboBox.SelectedItem == null)
            {
                ServicioValidator.Text = "Campo requerido";
                ServicioRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveButton.IsEnabled = false;
            }
            else
            {
                ServicioValidator.Text = "Valido";
                ServicioRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                ValidateSaveButton();
            }
        }
        #endregion

        #region Proyecto Method
        public void ValidateProyecto()
        {
            if (ProyectoComboBox.SelectedItem == null)
            {
                ProyectoValidator.Text = "Campo requerido";
                ProyectoRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveButton.IsEnabled = false;
            }
            else
            {
                ProyectoValidator.Text = "Valido";
                ProyectoRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                ValidateSaveButton();
            }
        }
        #endregion

        #region Vsatid Method
        public void ValidateVsatid()
        {
            int outParse;
            bool isNum = Int32.TryParse(VsatidTextBox.Text, out outParse);

            if (VsatidTextBox.Text == string.Empty)
            {
                VsatidValidator.Text = "Campo requerido";
                VsatidRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveButton.IsEnabled = false;
            }

            else if (isNum && Convert.ToInt32(VsatidTextBox.Text) > 0)
            {
                VsatidValidator.Text = "Valido";
                VsatidRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                ValidateSaveButton();
            }

            else
            {
                VsatidValidator.Text = "Campo requerido";
                VsatidRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveButton.IsEnabled = false;
            }
        }
        #endregion

        #region Costo Method
        public void ValidateCosto()
        {
            double outParse;
            bool isNum = double.TryParse(CostoTextBox.Text, out outParse);

            if (CostoTextBox.Text == string.Empty)
            {
                CostoValidator.Text = "Campo requerido";
                CostoRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveButton.IsEnabled = false;
            }

            else if (isNum && Convert.ToDouble(CostoTextBox.Text) > 0)
            {
                CostoValidator.Text = "Valido";
                CostoRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                ValidateSaveButton();
            }

            else
            {
                CostoValidator.Text = "Campo requerido";
                CostoRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveButton.IsEnabled = false;
            }
        }
        #endregion

        #region Latitud Method
        public void ValidateLatitud()
        {
            double outParse;
            bool isNum = double.TryParse(LatitudTextBox.Text, out outParse);

            if (LatitudTextBox.Text == string.Empty)
            {
                LatitudValidator.Text = "Campo requerido";
                LatitudRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveButton.IsEnabled = false;
            }

            else if (isNum)
            {
                LatitudValidator.Text = "Valido";
                LatitudRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                ValidateSaveButton();
            }

            else
            {
                LatitudValidator.Text = "Campo requerido";
                LatitudRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveButton.IsEnabled = false;
            }
        }
        #endregion

        #region Longitud Method
        public void ValidateLongitud()
        {
            double outParse;
            bool isNum = double.TryParse(LongitudTextBox.Text, out outParse);

            if (LongitudTextBox.Text == string.Empty)
            {
                LongitudValidator.Text = "Campo requerido";
                LongitudRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveButton.IsEnabled = false;
            }

            else if (isNum)
            {
                LongitudValidator.Text = "Valido";
                LongitudRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                ValidateSaveButton();
            }

            else
            {
                LongitudValidator.Text = "Campo requerido";
                LongitudRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveButton.IsEnabled = false;
            }
        }
        #endregion

        #region Telefonos Method
        public void ValidateTelefonos()
        {
            if (TelefonosTextBox.Text == string.Empty)
            {
                TelefonosValidator.Text = "Campo requerido";
                TelefonosRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveButton.IsEnabled = false;
            }
            else
            {
                TelefonosValidator.Text = "Valido";
                TelefonosRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                ValidateSaveButton();
            }
        }
        #endregion

        #endregion

        #region Validate Events

        //LocalidadTextBox TextChanged
        private void LocalidadTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateLocalidad();
        }

        //DistritoTextBox TextChanged
        private void DistritoTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateDistrito();
        }

        //ProvinciaTextBox TextChanged
        private void ProvinciaTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateProvincia();
        }

        //DepartamentoTextBox TextChanged
        private void DepartamentoTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateDepartamento();
        }

        //VsatIdTextBox TextChanged
        private void VsatidTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateVsatid();
        }

        //ProyectoComboBox SelectionChanged
        private void ProyectoComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ValidateProyecto();
        }

        //ServicioComboBox SelectionChanged
        private void ServicioComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ValidateServicio();
        }

        //LatitudTextBox TextChanged
        private void LatitudTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateLatitud();
        }

        //LongitudTextBox TextChanged
        private void LongitudTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateLongitud();
        }

        //CostoTextBox TextChanged
        private void CostoTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateCosto();
        }

        //TelefonoTextBox TextChanged
        private void TelefonosTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateTelefonos();
        }

        #endregion

        #region CRUD

        #region Save
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new ProjectContext())
            {
                try
                {
                    //Save
                    if (flag == 0)
                    {
                        var localidad = new Localidades
                        {
                            Departamento = DepartamentoTextBox.Text,
                            Provincia = ProvinciaTextBox.Text,
                            Distrito = DistritoTextBox.Text,
                            Localidad = LocalidadTextBox.Text,
                            Serviciosid = (int)ServicioComboBox.SelectedValue,
                            Proyectoid = (int)ProyectoComboBox.SelectedValue,
                            Vsatid = Convert.ToInt32(VsatidTextBox.Text),
                            Costo = double.Parse(CostoTextBox.Text, System.Globalization.CultureInfo.InvariantCulture),
                            Latitud = double.Parse(LatitudTextBox.Text, System.Globalization.CultureInfo.InvariantCulture),
                            Longitud = double.Parse(LongitudTextBox.Text, System.Globalization.CultureInfo.InvariantCulture),
                            Telefonos = TelefonosTextBox.Text,
                        };
                        db.Localidades.Add(localidad);
                        await db.SaveChangesAsync();

                        cvs.Source = await
                            db.Localidades
                            .Include(x => x.Proyectos)
                            .Include(x => x.Servicios)
                            .GroupBy(x => x.Proyectos.Descripcion)
                            .ToListAsync();

                        InputModalDialog.IsModal = false;
                        MasterListView.SelectedItem = localidad;
                        MasterListView.ScrollIntoView(localidad);
                    }
                    //Edit
                    else if (flag == 1)
                    {
                        var itemSelected = (Localidades)MasterListView.SelectedItem;

                        if (MasterListView.SelectedItem != null)
                        {
                            itemSelected.Departamento = DepartamentoTextBox.Text;
                            itemSelected.Provincia = ProvinciaTextBox.Text;
                            itemSelected.Distrito = DistritoTextBox.Text;
                            itemSelected.Localidad = LocalidadTextBox.Text;
                            itemSelected.Serviciosid = (int)ServicioComboBox.SelectedValue;
                            itemSelected.Proyectoid = (int)ProyectoComboBox.SelectedValue;
                            itemSelected.Vsatid = Convert.ToInt32(VsatidTextBox.Text);
                            itemSelected.Costo = double.Parse(CostoTextBox.Text, System.Globalization.CultureInfo.InvariantCulture);
                            itemSelected.Latitud = double.Parse(LatitudTextBox.Text, System.Globalization.CultureInfo.InvariantCulture);
                            itemSelected.Longitud = double.Parse(LongitudTextBox.Text, System.Globalization.CultureInfo.InvariantCulture);
                            itemSelected.Telefonos = TelefonosTextBox.Text;

                            db.Localidades.Update(itemSelected);
                            await db.SaveChangesAsync();

                            cvs.Source = await
                                db.Localidades
                                .Include(x => x.Proyectos)
                                .Include(x => x.Servicios)
                                .GroupBy(x => x.Proyectos.Descripcion)
                                .ToListAsync();

                            InputModalDialog.IsModal = false;
                            MasterListView.SelectedItem = itemSelected;
                            MasterListView.ScrollIntoView(itemSelected);
                        }
                    }
                }
                catch (Exception ex)
                {

                    var message = new MessageDialog(ex.InnerException.ToString());
                    await message.ShowAsync();
                }
                var query = db.Localidades.Count();
                if (query <= 0)
                {
                    EmptyItemsBorder.Visibility = Visibility.Visible;
                    EmptyItemsBorder2.Visibility = Visibility.Visible;
                }
                else
                {
                    EmptyItemsBorder.Visibility = Visibility.Collapsed;
                }
            }
            //Repopulate Print Helper
            if (printHelper != null)
            {
                printHelper.UnregisterForPrinting();
                // Initalize common helper class and register for printing
                printHelper = new PrintHelper(this);
                printHelper.RegisterForPrinting();

                // Initialize print content for this scenario
                printHelper.PreparePrintContent(new LocalidadesReport());
            }
            //Enable
            SearchAutoSuggestBox.IsEnabled = true;
            MasterListView.IsEnabled = true;
            ListViewCommandBar.IsEnabled = true;
        }
        #endregion

        #region EditNav
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            flag = 1;
            using (var db = new ProjectContext())
            {
                var selectedLocalidad = (Localidades)MasterListView.SelectedItem;

                if (selectedLocalidad != null)
                {
                    LocalidadTextBox.Text = selectedLocalidad.Localidad;
                    DistritoTextBox.Text = selectedLocalidad.Distrito;
                    ProvinciaTextBox.Text = selectedLocalidad.Provincia;
                    DepartamentoTextBox.Text = selectedLocalidad.Departamento;
                    VsatidTextBox.Text = selectedLocalidad.Vsatid.ToString();
                    ProyectoComboBox.SelectedValue = selectedLocalidad.Proyectoid;
                    ServicioComboBox.SelectedValue = selectedLocalidad.Serviciosid;
                    LatitudTextBox.Text = selectedLocalidad.Latitud.ToString();
                    LongitudTextBox.Text = selectedLocalidad.Longitud.ToString();
                    CostoTextBox.Text = selectedLocalidad.Costo.ToString();
                    TelefonosTextBox.Text = selectedLocalidad.Telefonos.ToString();

                    InputModalDialog.IsModal = true;

                    ValidateLocalidad();
                    ValidateDistrito();
                    ValidateProvincia();
                    ValidateDepartamento();
                    ValidateVsatid();
                    ValidateLatitud();
                    ValidateLongitud();
                    ValidateCosto();
                    ValidateTelefonos();

                    ValidateSaveButton();

                }

            }
            //Disable
            SearchAutoSuggestBox.IsEnabled = false;
            MasterListView.IsEnabled = false;
            ListViewCommandBar.IsEnabled = false;
        }
        #endregion

        #region Delete
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new ProjectContext())
            {
                var itemToDelete = (Localidades)MasterListView.SelectedItem;
                if (MasterListView.SelectedItem != null)
                {
                    //MessageDialog
                    var dialog = new MessageDialog("¿Desea eliminar la localidad seleccionada?", "Advertencia");
                    dialog.Commands.Add(new UICommand("Si") { Id = 0 });
                    dialog.Commands.Add(new UICommand("No") { Id = 1 });

                    var result = await dialog.ShowAsync();

                    if (result != null && result.Label == "Si")
                    {
                        db.Localidades.Remove(itemToDelete);
                        await db.SaveChangesAsync();

                        cvs.Source = await
                            db.Localidades
                            .Include(x => x.Proyectos)
                            .Include(x => x.Servicios)
                            .GroupBy(x => x.Proyectos.Descripcion)
                            .ToListAsync();

                        MasterListView.SelectedItem = await
                            db.Localidades
                            .FirstOrDefaultAsync();
                    }
                }
                var query = db.Localidades.Count();
                if (query <= 0)
                {
                    EmptyItemsBorder.Visibility = Visibility.Visible;
                    EmptyItemsBorder2.Visibility = Visibility.Visible;
                }
                else
                {
                    EmptyItemsBorder.Visibility = Visibility.Collapsed;
                }
            }
            //Repopulate Print Helper
            if (printHelper != null)
            {
                printHelper.UnregisterForPrinting();
                // Initalize common helper class and register for printing
                printHelper = new PrintHelper(this);
                printHelper.RegisterForPrinting();

                // Initialize print content for this scenario
                printHelper.PreparePrintContent(new LocalidadesReport());
            }
        }
        #endregion

        #endregion

        #region Search
        private async void SearchAutoSuggestBox_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            using (var db = new ProjectContext())
            {
                cvs.Source = await
                    db.Localidades
                   .Include(x => x.Proyectos)
                   .Where(x => x.Localidad.Contains(SearchAutoSuggestBox.Text))
                   .GroupBy(x => x.Proyectos.Descripcion)
                   .ToListAsync();

                MasterListView.SelectedItem = await
                    db.Localidades
                    .Include(x => x.Proyectos)
                    .Where(x => x.Localidad.Contains(SearchAutoSuggestBox.Text))
                    .GroupBy(x => x.Proyectos.Descripcion)
                    .FirstOrDefaultAsync();
            }
        }
        #endregion

        #region Print
        private async void PrintButton_OnClick(object sender, RoutedEventArgs e)
        {
            await printHelper.ShowPrintUIAsync();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Initalize common helper class and register for printing
            printHelper = new PrintHelper(this);
            printHelper.RegisterForPrinting();

            // Initialize print content for this scenario
            printHelper.PreparePrintContent(new LocalidadesReport());
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (printHelper != null)
            {
                printHelper.UnregisterForPrinting();
            }
        }
        #endregion

        //Enable Content Transitions
        private void EnableContentTransitions()
        {
            DetailContentPresenter.ContentTransitions.Clear();
            DetailContentPresenter.ContentTransitions.Add(new EntranceThemeTransition());
        }

        //Disable Content Transitions
        private void DisableContentTransitions()
        {
            if (DetailContentPresenter != null)
            {
                DetailContentPresenter.ContentTransitions.Clear();
            }
        }

        //LayoutRoot Loaded
        private void LayoutRoot_OnLoaded(object sender, RoutedEventArgs e)
        {
            //// Assure we are displaying the correct item. This is necessary in certain adaptive cases.
            //MasterListView.SelectedItem = _lastSelectedItem;
        }

        //Open ModalDialog
        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            flag = 0;
            CleanInputs();
            ValidateLocalidad();
            ValidateDistrito();
            ValidateProvincia();
            ValidateDepartamento();
            ValidateLongitud();
            ValidateCosto();
            ValidateLatitud();
            ValidateProyecto();
            ValidateServicio();
            ValidateTelefonos();
            ValidateVsatid();
            ValidateLatitud();

            InputModalDialog.IsModal = true;

            //Disable
            SearchAutoSuggestBox.IsEnabled = false;
            MasterListView.IsEnabled = false;
            ListViewCommandBar.IsEnabled = false;
        }

        //Close ModalDialog
        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            flag = 0;
            CleanInputs();
            InputModalDialog.IsModal = false;

            //Enable
            MasterListView.IsEnabled = true;
            ListViewCommandBar.IsEnabled = true;
            SearchAutoSuggestBox.IsEnabled = true;
        }

        //Load Page
        private async void LocalidadesPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            using (var db = new ProjectContext())
            {
                ProyectoComboBox.ItemsSource = await db.Proyectos.ToListAsync();
                ProyectoComboBox.SelectedValuePath = "Id";
                ProyectoComboBox.DisplayMemberPath = "Descripcion";

                ServicioComboBox.ItemsSource = await db.Servicios.ToListAsync();
                ServicioComboBox.SelectedValuePath = "Id";
                ServicioComboBox.DisplayMemberPath = "Descripcion";

                cvs.Source = await
                    db.Localidades
                    .Include(x => x.Proyectos)
                    .GroupBy(x => x.Proyectos.Descripcion)
                    .ToListAsync();

                MasterListView.SelectedItem = await
                    db.Localidades
                    .GroupBy(x => x.Proyectos.Descripcion)
                    .FirstOrDefaultAsync();

                var query = db.Localidades.Count();
                if (query <= 0)
                {
                    EmptyItemsBorder.Visibility = Visibility.Visible;
                    EmptyItemsBorder2.Visibility = Visibility.Visible;
                }
                else
                {
                    EmptyItemsBorder.Visibility = Visibility.Collapsed;
                }
            }
        }

        //MasterListView SelectionChanged
        private void MasterListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var clickedItem = (Localidades)MasterListView.SelectedItem;
            _lastSelectedItem = clickedItem;
            EnableContentTransitions();
            if (MasterListView.SelectedItem != null)
            {
                EditButton.IsEnabled = true;
                DeleteButton.IsEnabled = true;
                DetailContentPresenter.Visibility = Visibility.Visible;
                EmptyItemsBorder2.Visibility = Visibility.Collapsed;
            }
            else
            {
                EditButton.IsEnabled = false;
                DeleteButton.IsEnabled = false;
                DetailContentPresenter.Visibility = Visibility.Collapsed;
                EmptyItemsBorder2.Visibility = Visibility.Visible;
            }
        }

        //MasterListView DoubleTapped
        private async void MasterListView_OnDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            bool isInternetConnected = NetworkInterface.GetIsNetworkAvailable();
            try
            {
                using (var db = new ProjectContext())
                {
                    var selectedItem = (Localidades)MasterListView.SelectedItem;

                    if (isInternetConnected)
                    {


                        myMap.Center =
                            new Geopoint(new BasicGeoposition()
                            {
                                //Geopoint for Seattle 
                                Latitude = selectedItem.Latitud,
                                Longitude = selectedItem.Longitud,
                            });
                        myMap.ZoomLevel = 8;
                        mapIcon1.Location = myMap.Center;
                        mapIcon1.NormalizedAnchorPoint = new Point(0.5, 1.0);
                        mapIcon1.Title = $"'{selectedItem.Localidad}'";
                        mapIcon1.Image = mapIconStreamReference;
                        mapIcon1.ZIndex = 0;
                        myMap.MapElements.Add(mapIcon1);

                        MapasModalDialog.IsModal = true;


                        //Disable
                        SearchAutoSuggestBox.IsEnabled = false;
                        MasterListView.IsEnabled = false;
                        ListViewCommandBar.IsEnabled = false;
                    }
                    else
                    {
                        MapasModalDialog.IsModal = false;
                        var message = new MessageDialog("Por favor verifique su conexion a internet", "Advertencia");
                        await message.ShowAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                MapasModalDialog.IsModal = false;
                var message = new MessageDialog("Por favor verifique sus cordenadas", "Advertencia");
                await message.ShowAsync();
            }
        }

        //Close Maps ModalDialog
        private void CloseMapButton_OnClick(object sender, RoutedEventArgs e)
        {
            MapasModalDialog.IsModal = false;
            //Enable
            MasterListView.IsEnabled = true;
            ListViewCommandBar.IsEnabled = true;
            SearchAutoSuggestBox.IsEnabled = true;

            myMap.MapElements.Remove(mapIcon1);
        }
    }
}
