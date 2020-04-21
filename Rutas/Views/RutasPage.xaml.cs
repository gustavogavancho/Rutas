using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.EntityFrameworkCore;
using Prueba2.Services;
using Rutas.Models;
using Rutas.Reports;
using Rutas = Rutas.Models.Rutas;
using Windows.UI.Xaml.Media.Animation;
using Template10.Utils;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Rutas.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RutasPage : Page
    {
        public RutasPage()
        {
            this.InitializeComponent();
        }

        int flag = 0;

        private PrintHelper printHelper;

        private Models.Rutas _lastSelectedItem;

        #region Helper Methods

        #region Validate SaveButton
        public void ValidateSaveButton()
        {
            if (TecnicoValidator.Text == "Valido" && FechaInicioValidator.Text == "Valido" &&
                FechaFinValidator.Text == "Valido" && EstadoValidator.Text == "Valido" )
            {
                SaveButton.IsEnabled = true;
            }
            else
            {
                SaveButton.IsEnabled = false;
            }
        }
        #endregion

        #region ValidateSaveInventarioButton
        public void ValidateSaveInventarioButton()
        {
            if (BalanceValidator.Text == "Valido" && SalienteValidator.Text == "Valido")
            {
                SaveInventarioButton.IsEnabled = true;
            }
            else
            {
                SaveInventarioButton.IsEnabled = false;
            }
        }
        #endregion

        #region ValidateChangeInventarioButton
        public void ValidateChangeInventarioButton()
        {
            if (PartNumberToReplaceValidator.Text == "Valido" && DescripcionToReplaceValidator.Text == "Valido" &&
                TotalToReplaceValidator.Text == "Valido" && CantidadToReplaceValidator.Text == "Valido")
            {
                ChangeInventarioButton.IsEnabled = true;
            }
            else
            {
                ChangeInventarioButton.IsEnabled = false;
            }
        }
        #endregion

        #region ValidateReturnInventarioButton
        public void ValidateReturnInventarioButton()
        {
            if (BalanceToReturnValidator.Text == "Valido" && CantidadToReturnValidator.Text == "Valido" &&
                LocacionToReturnValidator.Text == "Valido")
            {
                ReturnInventarioButton.IsEnabled = true;
            }
            else
            {
                ReturnInventarioButton.IsEnabled = false;
            }

        }
        #endregion

        #region Clean Inputs

        public void CleanInputs()
        {
            TecnicoAutoSuggestBox.Text = string.Empty;
            FechaInicioDatePicker.Date = DateTimeOffset.Now;
            FechaFinDatePicker.Date = DateTimeOffset.Now;
            EstadoComboBox.SelectedItem = null;
        }

        #endregion

        #endregion

        #region Validate Methods

        #region Tecnico Method
        public void ValidateTecnico()
        {
            if (TecnicoAutoSuggestBox.Text == string.Empty)
            {
                TecnicoValidator.Text = "Campo requerido";
                TecnicoRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveButton.IsEnabled = false;
            }
            else
            {
                TecnicoValidator.Text = "Valido";
                TecnicoRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                ValidateSaveButton();
            }
        }
        #endregion

        #region FechaInicio Method
        public void ValidateFechaInicio()
        {
            if (FechaInicioDatePicker.Date > FechaFinDatePicker.Date)
            {
                FechaInicioValidator.Text = "Campo requerido";
                FechaInicioRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveButton.IsEnabled = false;
            }
            else
            {
                FechaInicioValidator.Text = "Valido";
                FechaInicioRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                FechaFinValidator.Text = "Valido";
                FechaFinRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                ValidateSaveButton();
            }
        }
        #endregion

        #region FechaFin Method
        public void ValidateFechaFin()
        {
            if (FechaFinDatePicker.Date < FechaInicioDatePicker.Date)
            {
                FechaFinValidator.Text = "Campo requerido";
                FechaFinRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveButton.IsEnabled = false;
            }
            else
            {
                FechaFinValidator.Text = "Valido";
                FechaFinRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                FechaInicioValidator.Text = "Valido";
                FechaInicioRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                ValidateSaveButton();
            }
        }
        #endregion

        #region Estado Method
        
        public void ValidateEstado()
        {
            if (EstadoComboBox.SelectedItem == null)
            {
                EstadoValidator.Text = "Campo requerido";
                EstadoRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveButton.IsEnabled = false;
            }
            else
            {
                EstadoValidator.Text = "Valido";
                EstadoRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                ValidateSaveButton();
            }
        }

        #endregion

        #region SeachLocalidades Method
        public void ValidateSearchLocalidad()
        {
            if (SearchLocalidadesSuggestBox.Text == null)
            {
                SearchLocalidadesValidator.Text = "Campo requerido";
                SearchLocalidadesRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                AddLocalidadSelectedButton.IsEnabled = false;
            }
        }
        #endregion

        #region Balance Method
        public void ValidateBalance()
        {
            int outParse;
            bool isNum = Int32.TryParse(BalanceTextBox.Text, out outParse);

            if (BalanceTextBox.Text == string.Empty)
            {
                BalanceValidator.Text = "Campo requerido";
                BalanceRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveInventarioButton.IsEnabled = false;
            }

            else if (isNum && outParse >= 0)
            {
                BalanceValidator.Text = "Valido";
                BalanceRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                ValidateSaveInventarioButton();
            }

            else
            {
                BalanceValidator.Text = "Campo requerido";
                BalanceRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveInventarioButton.IsEnabled = false;
            }
        }
        #endregion

        #region Saliente Method
        public void ValidateSaliente()
        {
            int outParse;
            bool isNum = Int32.TryParse(SalienteTextBox.Text, out outParse);
            int saliente;
            int.TryParse(SalienteTextBox.Text, out saliente);
            int balance;
            int.TryParse(BalanceTextBox.Text, out balance);

            if (SalienteTextBox.Text == string.Empty)
            {
                SalienteValidator.Text = "Campo requerido";
                SalienteRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveInventarioButton.IsEnabled = false;
            }

            else if (isNum && saliente > 0 && saliente <= balance)
            {
                SalienteValidator.Text = "Valido";
                SalienteRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                ValidateSaveInventarioButton();
            }

            else
            {
                SalienteValidator.Text = "Campo requerido";
                SalienteRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveInventarioButton.IsEnabled = false;
            }
        }
        #endregion

        #region PartNumnberToReplace Method
        public void ValidatePartNumberToReplace()
        {
            if (PartNumberToReplaceAutoSuggestBox.Text != string.Empty)
            {
                PartNumberToReplaceRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                PartNumberToReplaceValidator.Text = "Valido";
                ValidateChangeInventarioButton();
            }
            else
            {
                PartNumberToReplaceRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                PartNumberToReplaceValidator.Text = "Campo requerido";
                ChangeInventarioButton.IsEnabled = false;
            }
        }
        #endregion

        #region DescripcionToReplace Method
        public void ValidateDescripcionToReplace()
        {
            if (DescripcionToReplaceTextBox.Text != string.Empty)
            {
                DescripcionToReplaceRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                DescripcionToReplaceValidator.Text = "Valido";
                ValidateChangeInventarioButton();
            }
            else
            {
                DescripcionToReplaceRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                DescripcionToReplaceValidator.Text = "Campo requerido";
                ChangeInventarioButton.IsEnabled = false;
            }
        }
        #endregion

        #region TotalToReplace Method
        public void ValidateTotalToReplace()
        {
            int outParse;
            bool isNum = Int32.TryParse(TotalToReplaceTextBox.Text, out outParse);

            if (TotalToReplaceTextBox.Text == string.Empty)
            {
                TotalToReplaceValidator.Text = "Campo requerido";
                TotalToReplaceRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                ChangeInventarioButton.IsEnabled = false;
            }

            else if (isNum && outParse >= 0)
            {
                TotalToReplaceValidator.Text = "Valido";
                TotalToReplaceRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                ValidateChangeInventarioButton();
            }

            else
            {
                TotalToReplaceValidator.Text = "Campo requerido";
                TotalToReplaceRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                ChangeInventarioButton.IsEnabled = false;
            }
        }
        #endregion

        #region CantidadToReplace Method
        public void ValidateCantidadToReplace()
        {
            int outParse;
            bool isNum = Int32.TryParse(CantidadToReplaceTextBox.Text, out outParse);

            int balance;
            Int32.TryParse(TotalToReplaceTextBox.Text, out balance);
            int cantidad;
            Int32.TryParse(CantidadToReplaceTextBox.Text, out cantidad);

            if (CantidadToReplaceTextBox.Text == string.Empty)
            {
                CantidadToReplaceValidator.Text = "Campo requerido";
                CantidadToReplaceRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                ChangeInventarioButton.IsEnabled = false;
            }

            else if (isNum && cantidad > 0 && cantidad <= balance)
            {
                CantidadToReplaceValidator.Text = "Valido";
                CantidadToReplaceRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                ValidateChangeInventarioButton();
            }

            else
            {
                CantidadToReplaceValidator.Text = "Campo requerido";
                CantidadToReplaceRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                ChangeInventarioButton.IsEnabled = false;
            }
        }
        #endregion

        #region BalanceToReturn Method
        public void ValidateBalanceToReturn()
        {
            int outParse;
            bool isNum = Int32.TryParse(BalanceToReturnTextBox.Text, out outParse);

            if (BalanceToReturnTextBox.Text == string.Empty)
            {
                BalanceToReturnValidator.Text = "Campo requerido";
                BalanceToReturnRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                ReturnInventarioButton.IsEnabled = false;
            }

            else if (isNum && outParse >= 0)
            {
                BalanceToReturnValidator.Text = "Valido";
                BalanceToReturnRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                ValidateReturnInventarioButton();
            }

            else
            {
                BalanceToReturnValidator.Text = "Campo requerido";
                BalanceToReturnRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                ReturnInventarioButton.IsEnabled = false;
            }

        }
        #endregion

        #region CantidadToReturn Method
        public void ValidateCantidadToReturn()
        {
            int outParse;
            bool isNum = Int32.TryParse(CantidadToReturnTextBox.Text, out outParse);

            int balance;
            Int32.TryParse(BalanceToReturnTextBox.Text, out balance);
            int cantidad;
            Int32.TryParse(CantidadToReturnTextBox.Text, out cantidad);

            if (CantidadToReturnTextBox.Text == string.Empty)
            {
                CantidadToReturnValidator.Text = "Campo requerido";
                CantidadToReturnRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                ReturnInventarioButton.IsEnabled = false;
            }

            else if (isNum && cantidad > 0 && cantidad <= balance)
            {
                CantidadToReturnValidator.Text = "Valido";
                CantidadToReturnRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                ValidateReturnInventarioButton();
            }

            else
            {
                CantidadToReturnValidator.Text = "Campo requerido";
                CantidadToReturnRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                ReturnInventarioButton.IsEnabled = false;
            }
        }
        #endregion

        #region LocacacionToRetun Method
        public void ValidateLocacionToReturn()
        {
            if (LocacionToReturnComboBox.SelectedItem != null)
            {
                LocacionToReturnValidator.Text = "Valido";
                LocacionToReturnRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                ValidateReturnInventarioButton();
            }
            else
            {
                LocacionToReturnValidator.Text = "Campo requerido";
                LocacionToReturnRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                ReturnInventarioButton.IsEnabled = false;
            }
        }
        #endregion

        #endregion

        #region Validate Events

        //TecnicosAutoSuggestBox TextChanged
        private async void TecnicoAutoSuggestBox_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            using (var db = new ProjectContext())
            {
                if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
                {
                    sender.ItemsSource =
                        await
                            db.Tecnicos.OrderBy(x => x.Nombre)
                                .Where(x => x.Nombre.Contains(TecnicoAutoSuggestBox.Text))
                                .Select(x => x.ToString())
                                .ToListAsync();
                }

                var query =
                    await
                        db.Tecnicos
                            .Where(x => x.ToString() == TecnicoAutoSuggestBox.Text)
                            .Select(x => x.ToString())
                            .SingleOrDefaultAsync();

                if (TecnicoAutoSuggestBox.Text == string.Empty || query == null)
                {
                    TecnicoValidator.Text = "Campo requerido";
                    TecnicoRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                    SaveButton.IsEnabled = false;
                }
                else if (query != null)
                {
                    TecnicoValidator.Text = "Valido";
                    TecnicoRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                    ValidateSaveButton();
                }
            }
        }

        //FechaInicioDatePicker DateChanged
        private void FechaInicioDatePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            ValidateFechaInicio();
        }

        //FechaFinValidator DateChanged
        private void FechaFinDatePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            ValidateFechaFin();
        }

        //EstadoComboBox SelectionChanged
        private void EstadoComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ValidateEstado();
        }

        //BalanceTextBox TextChanged
        private void BalanceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateBalance();
        }

        //SalienteTextBox TextChanged
        private void SalienteTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateSaliente();
        }

        //PartNumberToReplaceAutoSuggestBox TextChanged
        private async void PartNumberToReplaceAutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            ValidatePartNumberToReplace();
            using (var db = new ProjectContext())
            {
                if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
                {
                    sender.ItemsSource =
                        await
                            db.PartNumbers.AsNoTracking().OrderBy(x => x.PartNumber)
                                .Where(x => x.PartNumber.Contains(PartNumberToReplaceAutoSuggestBox.Text))
                                .Select(x => x.PartNumber)
                                .ToListAsync();
                }

                var query =
                    await
                        db.PartNumbers.AsNoTracking()
                            .OrderBy(x => x.PartNumber)
                            .Where(x => x.PartNumber == PartNumberToReplaceAutoSuggestBox.Text)
                            .Select(x => x.Descripcion)
                            .SingleOrDefaultAsync();

                if (PartNumberToReplaceAutoSuggestBox.Text == string.Empty || query == null)
                {
                    PartNumberToReplaceValidator.Text = "Campo requerido";
                    PartNumberToReplaceRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                    DescripcionToReplaceTextBox.Text = string.Empty;
                }
                else if (query != null)
                {
                    PartNumberToReplaceValidator.Text = "Valido";
                    PartNumberToReplaceRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                    DescripcionToReplaceTextBox.Text = query;
                }
            }

        }

        //DescripcionToReplaceTextBox TextChanged
        private void DescripcionToReplaceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateDescripcionToReplace();
        }

        //TotalToReplaceTextBox TextChanged
        private void TotalToReplaceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateTotalToReplace();
        }

        //CantidadToReplaceTextBox TextChanged
        private void CantidadToReplaceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateCantidadToReplace();
        }

        //BalanceToRetunTextBox TextChanged
        private void BalanceToReturnTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateBalanceToReturn();
        }

        //CantidadToRetunTextBox TextChanged
        private void CantidadToReturnTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateCantidadToReturn();
        }

        //LocacionToReturnComboBox SelectionChanged
        private void LocacionToReturnComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ValidateLocacionToReturn();
        }

        #endregion

        #region CRUD

        #region Save

        private async void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new ProjectContext())
                {
                    //Save
                    if (flag == 0)
                    {
                        var ruta = new Models.Rutas
                        {
                            FechaInicio = FechaInicioDatePicker.Date.DateTime,
                            FechaFin = FechaFinDatePicker.Date.DateTime,
                            TecnicosId = await
                                db.Tecnicos.AsNoTracking()
                                    .Where(x => x.ToString() == TecnicoAutoSuggestBox.Text)
                                    .Select(x => x.Id)
                                    .SingleOrDefaultAsync(),
                            Color = "Yellow",
                            Estado = (Estado)EstadoComboBox.SelectedItem,
                        };

                        db.Rutas.Add(ruta);

                        await db.SaveChangesAsync();

                        cvs.Source = await db.Rutas
                            .Include(x => x.Tecnicos)
                            .OrderBy(x => x.Estado)
                            .ThenBy(x => x.Tecnicos.Nombre)
                            .ThenBy(x => x.Tecnicos.Apellido)
                            .ThenByDescending(x => x.FechaInicio.Date)
                            .GroupBy(x => x.Estado)
                            .ToListAsync();

                        InputModalDialog.IsModal = false;

                        //MasterListView.SelectedItem = ruta;
                        //MasterListView.ScrollIntoView(ruta);

                        var appointment = new Windows.ApplicationModel.Appointments.Appointment();

                        // Get the selection rect of the button pressed to add this appointment
                        var rect = GetElementRect(sender as FrameworkElement);

                        appointment.AllDay.Equals(true);
                        appointment.Subject = ruta.Tecnicos.ToString();
                        appointment.Details = "Fecha Fin: " + ruta.FechaFin.ToString();
                        appointment.StartTime = ruta.FechaInicio;
                        appointment.BusyStatus = Windows.ApplicationModel.Appointments.AppointmentBusyStatus.Busy;
                        appointment.Reminder = TimeSpan.FromDays(1);
                        await Windows.ApplicationModel.Appointments.AppointmentManager.ShowAddAppointmentAsync(appointment, rect, Windows.UI.Popups.Placement.Default);
                    }
                    //Edit
                    else if (flag == 1)
                    {
                        var itemSelected = (Models.Rutas)MasterListView.SelectedItem;

                        if (MasterListView.SelectedItem != null)
                        {
                            itemSelected.TecnicosId = await
                                db.Tecnicos.AsNoTracking()
                                    .Where(x => x.ToString() == TecnicoAutoSuggestBox.Text)
                                    .Select(x => x.Id)
                                    .SingleOrDefaultAsync();
                            itemSelected.Tecnicos = await db.Tecnicos.Where(x => x.ToString() == TecnicoAutoSuggestBox.Text).SingleOrDefaultAsync();
                            itemSelected.FechaInicio = FechaInicioDatePicker.Date.DateTime;
                            itemSelected.FechaFin = FechaFinDatePicker.Date.DateTime;
                            itemSelected.Estado = (Estado)EstadoComboBox.SelectedItem;

                            if ((Estado)EstadoComboBox.SelectedItem == Estado.Pendiente)
                            {
                                itemSelected.Color = "Yellow";

                                db.Rutas.Update(itemSelected);
                                await db.SaveChangesAsync();

                                cvs.Source = await db.Rutas
                                    .Include(x => x.Tecnicos)
                                    .OrderBy(x => x.Estado)
                                    .ThenBy(x => x.Tecnicos.Nombre)
                                    .ThenBy(x => x.Tecnicos.Apellido)
                                    .ThenByDescending(x => x.FechaInicio.Date)
                                    .GroupBy(x => x.Estado)
                                    .ToListAsync();

                                InputModalDialog.IsModal = false;


                            }
                            else if ((Estado)EstadoComboBox.SelectedItem == Estado.Finalizado)
                            {
                                itemSelected.Color = "DarkGreen";
                                var queryCount = db.SelectedInventario.Where(x => x.Rutasid == itemSelected.Id).Count();

                                if (queryCount >= 1)
                                {
                                    var message = new MessageDialog("Se requiere despachar items al inventario", "Advertencia");
                                    await message.ShowAsync();
                                }
                                else if (queryCount == 0)
                                {
                                    db.Rutas.Update(itemSelected);
                                    await db.SaveChangesAsync();

                                    cvs.Source = await db.Rutas
                                        .Include(x => x.Tecnicos)
                                        .OrderBy(x => x.Estado)
                                        .ThenBy(x => x.Tecnicos.Nombre)
                                        .ThenBy(x => x.Tecnicos.Apellido)
                                        .ThenByDescending(x => x.FechaInicio.Date)
                                        .GroupBy(x => x.Estado)
                                        .ToListAsync();

                                    InputModalDialog.IsModal = false;
                                }
                            }

                            //MasterListView.SelectedItem = itemSelected;
                            ////MasterListView.ScrollIntoView(itemSelected);
                        }
                    }
                    var query1 = db.Rutas.Count();
                    if (query1 <= 0)
                    {
                        EmptyItemsBorder.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        EmptyItemsBorder.Visibility = Visibility.Collapsed;
                    }
                }
            }
            catch (Exception ex)
            {
                var message = new MessageDialog(ex.Message);
                await message.ShowAsync();
            }
            //Repopulate Print Helper
            if (printHelper != null)
            {
                printHelper.UnregisterForPrinting();
                // Initalize common helper class and register for printing
                printHelper = new PrintHelper(this);
                printHelper.RegisterForPrinting();

                // Initialize print content for this scenario
                printHelper.PreparePrintContent(new RutasReport());
            }

            //Disable
            EstadoComboBox.IsEnabled = false;
            //Enable
            MasterListView.IsEnabled = true;
            ListViewCommandBar.IsEnabled = true;
            SearchAutoSuggestBox.IsEnabled = true;
        }

        #endregion

        #region Edit

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            CleanInputs();
            using (var db = new ProjectContext())
            {
                flag = 1;

                var editRuta = (Models.Rutas)MasterListView.SelectedItem;

                if (MasterListView.SelectedItem != null)
                {
                    TecnicoAutoSuggestBox.Text = editRuta.Tecnicos.Nombre + " " + editRuta.Tecnicos.Apellido;
                    FechaInicioDatePicker.Date = editRuta.FechaInicio;
                    FechaFinDatePicker.Date = editRuta.FechaFin;
                    EstadoComboBox.SelectedItem = editRuta.Estado;
                    InputModalDialog.IsModal = true;

                    ValidateTecnico();
                    ValidateFechaFin();
                    ValidateFechaInicio();

                    SaveButton.IsEnabled = true;
                }
            }
            //Enable
            EstadoComboBox.IsEnabled = true;
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
                var itemToDelete = (Models.Rutas)MasterListView.SelectedItem;
                if (MasterListView.SelectedItem != null)
                {
                    //MessageDialog
                    var dialog = new MessageDialog("¿Desea eliminar el item seleccionado?", "Advertencia");
                    dialog.Commands.Add(new UICommand("Si") { Id = 0 });
                    dialog.Commands.Add(new UICommand("No") { Id = 1 });

                    var result = await dialog.ShowAsync();

                    if (result != null && result.Label == "Si")
                    {
                        db.Rutas.Remove(itemToDelete);
                        await db.SaveChangesAsync();

                        cvs.Source = await db.Rutas
                            .Include(x => x.Tecnicos)
                            .OrderBy(x => x.Estado)
                            .ThenBy(x => x.Tecnicos.Nombre)
                            .ThenBy(x => x.Tecnicos.Apellido)
                            .ThenByDescending(x => x.FechaInicio.Date)
                            .GroupBy(x => x.Estado)
                            .ToListAsync();

                        MasterListView.SelectedItem = await db.Rutas
                            .Include(x => x.Tecnicos)
                            .OrderBy(x => x.Estado)
                            .ThenBy(x => x.Tecnicos.Nombre)
                            .ThenBy(x => x.Tecnicos.Apellido)
                            .ThenByDescending(x => x.FechaInicio.Date)
                            .GroupBy(x => x.Estado)
                            .FirstOrDefaultAsync();
                    }
                }
                var query = db.Rutas.Count();
                if (query <= 0)
                {
                    EmptyItemsBorder.Visibility = Visibility.Visible;
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
                printHelper.PreparePrintContent(new RutasReport());
            }
        }

        #endregion

        #endregion

        #region Search

        private async void SearchAutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            using (var db = new ProjectContext())
            {
                cvs.Source = await db.Rutas
                    .Include(x => x.Tecnicos)
                    .Where(x=> x.Tecnicos.Nombre.Contains(SearchAutoSuggestBox.Text) || x.Tecnicos.Apellido.Contains(SearchAutoSuggestBox.Text))
                    .OrderBy(x => x.Estado)
                    .ThenBy(x => x.Tecnicos.Nombre)
                    .ThenBy(x => x.Tecnicos.Apellido)
                    .ThenByDescending(x => x.FechaInicio.Date)
                   .GroupBy(x => x.Estado)
                    .ToListAsync();

                MasterListView.SelectedItem = await db.Rutas
                    .Include(x => x.Tecnicos)
                    .Where(x => x.Tecnicos.Nombre.Contains(SearchAutoSuggestBox.Text) || x.Tecnicos.Apellido.Contains(SearchAutoSuggestBox.Text))
                    .OrderBy(x => x.Estado)
                    .ThenBy(x => x.Tecnicos.Nombre)
                    .ThenBy(x => x.Tecnicos.Apellido)
                    .ThenByDescending(x => x.FechaInicio.Date)
                    .GroupBy(x => x.Estado)
                    .FirstOrDefaultAsync();
            }
        }

        #endregion

        #region Print
        private async void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            await printHelper.ShowPrintUIAsync();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Initalize common helper class and register for printing
            printHelper = new PrintHelper(this);
            printHelper.RegisterForPrinting();

            // Initialize print content for this scenario
            printHelper.PreparePrintContent(new RutasReport());
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (printHelper != null)
            {
                printHelper.UnregisterForPrinting();
            }
        }

        #endregion

        //Appointments
        public static Rect GetElementRect(Windows.UI.Xaml.FrameworkElement element)
        {
            Windows.UI.Xaml.Media.GeneralTransform transform = element.TransformToVisual(null);
            Point point = transform.TransformPoint(new Point());
            return new Rect(point, new Size(element.ActualWidth, element.ActualHeight));
        }

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

        //Open ModalDialog
        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            flag = 0;
            CleanInputs();
            ValidateTecnico();
            ValidateFechaInicio();
            ValidateFechaInicio();

            InputModalDialog.IsModal = true;

            EstadoComboBox.SelectedItem = Estado.Pendiente;

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

            EstadoComboBox.IsEnabled = false;

            //Enable
            MasterListView.IsEnabled = true;
            ListViewCommandBar.IsEnabled = true;
            SearchAutoSuggestBox.IsEnabled = true;
        }

        //Load Page
        private async void RutasPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            using (var db = new ProjectContext())
            {
                cvs.Source = await db.Rutas
                    .Include(x => x.Tecnicos)
                    .OrderBy(x => x.Estado)
                    .ThenBy(x => x.Tecnicos.Nombre)
                    .ThenBy(x => x.Tecnicos.Apellido)
                    .ThenByDescending(x => x.FechaInicio.Date)
                    .GroupBy(x => x.Estado)
                    .ToListAsync();

                MasterListView.SelectedItem = await db.Rutas
                    .Include(x => x.Tecnicos)
                    .OrderBy(x => x.Estado)
                    .ThenBy(x => x.Tecnicos.Nombre)
                    .ThenBy(x => x.Tecnicos.Apellido)
                    .ThenByDescending(x => x.FechaInicio.Date)
                    .GroupBy(x => x.Estado)
                    .FirstOrDefaultAsync();

                cvs1.Source = await db.Inventario
                    .Include(x => x.Almacenes)
                    .Include(x => x.Locaciones)
                    .Include(x => x.PartNumbers)
                    .OrderBy(x => x.Locaciones.Descripcion)
                    .GroupBy(x => x.Almacenes.Descripcion)
                    .ToListAsync();


                InventarioToSelectListView.SelectedItem = await db.Inventario
                    .Include(x => x.Almacenes)
                    .Include(x => x.Locaciones)
                    .Include(x => x.PartNumbers)
                    .OrderBy(x => x.Locaciones.Descripcion)
                    .GroupBy(x => x.Almacenes.Descripcion)
                    .FirstOrDefaultAsync();


                EstadoComboBox.ItemsSource = Enum.GetValues(typeof(Estado));

                LocacionToReturnComboBox.ItemsSource = await db.Locaciones.ToListAsync();
                LocacionToReturnComboBox.SelectedValuePath = "Id";
                LocacionToReturnComboBox.DisplayMemberPath = "Descripcion";
                var query = db.Rutas.AsNoTracking().Count();
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

        //MasterListView Selection Changed
        private async void MasterListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var clickedItem = (Models.Rutas)MasterListView.SelectedItem;
            _lastSelectedItem = clickedItem;
            EnableContentTransitions();
            if (MasterListView.SelectedItem != null)
            {
                EditButton.IsEnabled = true;
                DeleteButton.IsEnabled = true;
                AllDetailGrid.Visibility = Visibility.Visible;
                EmptyItemsBorder2.Visibility = Visibility.Collapsed;
                ValidateSearchLocalidad();

                using (var db = new ProjectContext())
                {
                  LocalidadesSelectedGridView.ItemsSource = await
                    db.SelectedLocalidades
                    .Include(x => x.Rutas)
                    .Include(x => x.Proyectos)
                    .Include(x => x.Servicios)
                    .Where(x=> x.Rutasid == clickedItem.Id)
                    .ToListAsync();

                  InventarioSelectedListView.ItemsSource = await
                    db.SelectedInventario
                    .Include(x => x.Rutas)
                    .Include(x => x.PartNumbers)
                    .Include(x => x.Locaciones)
                    .Include(x=> x.Almacenes)
                    .Where(x=> x.Rutasid == clickedItem.Id)
                    .ToListAsync();

                }
            }
            else
            {
                EditButton.IsEnabled = false;
                DeleteButton.IsEnabled = false;
                AllDetailGrid.Visibility = Visibility.Collapsed;
                EmptyItemsBorder2.Visibility = Visibility.Visible;
                LocalidadesSelectedGridView.ItemsSource = null;
                InventarioSelectedListView.ItemsSource = null;
            }
        }

        //SearchLocalidades TextChanged
        private async void SearchLocalidadesSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            using (var db = new ProjectContext())
            {
                if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
                {
                    sender.ItemsSource =
                        await
                            db.Localidades.OrderBy(x => x.Localidad)
                                .Where(x => x.Localidad.Contains(SearchLocalidadesSuggestBox.Text))
                                .Select(x => x.ToString())
                                .ToListAsync();
                }
                 var query =
                    await
                        db.Localidades.OrderBy(x => x.Localidad)
                                .Where(x => x.ToString() == (SearchLocalidadesSuggestBox.Text))
                                .Select(x => x.Localidad)
                                .FirstOrDefaultAsync();

                if (SearchLocalidadesSuggestBox.Text == string.Empty || query == null)
                {
                    SearchLocalidadesValidator.Text = "Campo requerido";
                    SearchLocalidadesRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                    AddLocalidadSelectedButton.IsEnabled = false;
                }
                else if (query != null)
                {
                    SearchLocalidadesValidator.Text = "Valido";
                    SearchLocalidadesRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                    AddLocalidadSelectedButton.IsEnabled = true; ;
                }
            }
        }

        //Add LocalidadSelected 
        private async void AddLocalidadSelectedButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new ProjectContext())
            {
                var rutas = (Models.Rutas)MasterListView.SelectedItem;

                var localidad = await db.Localidades.Where(x => x.ToString() == SearchLocalidadesSuggestBox.Text).SingleOrDefaultAsync();

                var localidadSelected = new SelectedLocalidades
                {
                    Departamento = localidad.Departamento,
                    Provincia = localidad.Provincia,
                    Distrito = localidad.Distrito,
                    Localidad = localidad.Localidad,
                    Serviciosid = localidad.Serviciosid,
                    Proyectoid = localidad.Proyectoid,
                    Vsatid = localidad.Vsatid,
                    Costo = localidad.Costo,
                    Latitud = localidad.Latitud,
                    Longitud = localidad.Longitud,
                    Telefonos = localidad.Telefonos,
                    Rutasid = rutas.Id,
                    Estado = Estado.Pendiente,
                    Color = "Yellow",
                };

                var query = db.SelectedLocalidades.Where(x => x.Vsatid == localidadSelected.Vsatid && x.Rutasid == localidadSelected.Rutasid).Count();

                if (query >= 1)
                {
                    var message = new MessageDialog("La localidad ya se encuentra seleccionada", "Adventencia");
                    await message.ShowAsync();
                    SearchLocalidadesSuggestBox.Text = string.Empty;
                    ValidateSearchLocalidad();
                }

                else if (query == 0)
                {
                    db.SelectedLocalidades.Add(localidadSelected);
                    await db.SaveChangesAsync();

                    LocalidadesSelectedGridView.ItemsSource = await
                        db.SelectedLocalidades
                        .Include(x => x.Rutas)
                        .Include(x => x.Proyectos)
                        .Include(x => x.Servicios)
                        .Where(x => x.Rutasid == localidadSelected.Rutasid)
                        .ToListAsync();

                    SearchLocalidadesSuggestBox.Text = string.Empty;
                    ValidateSearchLocalidad();
                    LocalidadesSelectedGridView.SelectedItem = localidadSelected;
                    LocalidadesSelectedGridView.ScrollIntoView(localidadSelected);
                }
            }
        }

        //LocalidadesSelection SeletionChanged
        private void LocalidadesSelectedGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LocalidadesSelectedGridView.SelectedItem != null)
            {
                DeleteLocalidadSelectedButton.IsEnabled = true;
                CheckLocalidadSelectedButton.IsEnabled = true;
            }
            else
            {
                DeleteLocalidadSelectedButton.IsEnabled = false;
                CheckLocalidadSelectedButton.IsEnabled = false;
            }
        }

        //Delete LocalidadeSelected
        private async void DeleteLocalidadSelectedButton_Click(object sender, RoutedEventArgs e)
        {
            var localidad = (SelectedLocalidades) LocalidadesSelectedGridView.SelectedItem;
            var ruta = (Models.Rutas)MasterListView.SelectedItem;
            using (var db = new ProjectContext())
            {
                db.SelectedLocalidades.Remove(localidad);
                await db.SaveChangesAsync();

                LocalidadesSelectedGridView.ItemsSource = await
                    db.SelectedLocalidades
                    .Include(x => x.Rutas)
                    .Include(x => x.Proyectos)
                    .Include(x => x.Servicios)
                    .Where(x=> x.Rutasid == ruta.Id)
                    .ToListAsync();
            }
        }
        
        //Check LocalidadesSelected
        private async void CheckLocalidadSelectedButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new ProjectContext())
            {
                var localidad = (SelectedLocalidades)LocalidadesSelectedGridView.SelectedItem;
                var ruta = (Models.Rutas)MasterListView.SelectedItem;

                if (localidad.Estado == Estado.Pendiente)
                {
                    localidad.Estado = Estado.Finalizado;
                    localidad.Color = "DarkGreen";

                    db.SelectedLocalidades.Update(localidad);
                }
                else if (localidad.Estado == Estado.Finalizado)
                {
                    localidad.Estado = Estado.Pendiente;
                    localidad.Color = "Yellow";

                    db.SelectedLocalidades.Update(localidad);

                }
                await db.SaveChangesAsync();

                LocalidadesSelectedGridView.ItemsSource = await
                    db.SelectedLocalidades
                    .Include(x => x.Rutas)
                    .Include(x => x.Proyectos)
                    .Include(x => x.Servicios)
                    .Where(x => x.Rutasid == ruta.Id)
                    .ToListAsync();

                LocalidadesSelectedGridView.SelectedItem = localidad;
            }
        }

        //Open InventarioModalDialog
        private void AddInventarioSelectedButton_Click(object sender, RoutedEventArgs e)
        {
            InventarioModalDialog.IsModal = true;
            var inventarioToSelect = (Inventario)InventarioToSelectListView.SelectedItem;

            BalanceTextBox.Text = inventarioToSelect.Balance.ToString();

            ValidateBalance();
            ValidateSaliente();
        }

        //Close InventarioModalDialog
        private void CloseInventarioButton_Click(object sender, RoutedEventArgs e)
        {
            InventarioModalDialog.IsModal = false;
            SalienteTextBox.Text = string.Empty;
        }

        //InventarioToSelectListView SeletionChanged
        private void InventarioToSelectListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (InventarioToSelectListView.SelectedItem != null)
            {
                var inventarioToSelect = (Inventario)InventarioToSelectListView.SelectedItem;

                BalanceTextBox.Text = inventarioToSelect.Balance.ToString();
            }
            else
            {
                BalanceTextBox.Text = string.Empty;
            }
        }

        //InventarioToSearchAutoSuggestBox TextChaneg Text
        private async void InventarioToSearchAutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            using (var db = new ProjectContext())
            {
                cvs1.Source = await db.Inventario
                    .Include(x => x.Almacenes)
                    .Include(x => x.Locaciones)
                    .Include(x => x.PartNumbers)
                    .Where(x => x.PartNumbers.PartNumber.Contains(InventarioToSearchAutoSuggestBox.Text) || x.PartNumbers.Descripcion.Contains(InventarioToSearchAutoSuggestBox.Text))
                    .OrderBy(x => x.Locaciones.Descripcion)
                    .GroupBy(x => x.Almacenes.Descripcion)
                    .ToListAsync();

                InventarioToSelectListView.SelectedItem = await db.Inventario
                    .Include(x => x.Almacenes)
                    .Include(x => x.Locaciones)
                    .Include(x => x.PartNumbers)
                    .Where(x => x.PartNumbers.PartNumber.Contains(InventarioToSearchAutoSuggestBox.Text) || x.PartNumbers.Descripcion.Contains(InventarioToSearchAutoSuggestBox.Text))
                    .OrderBy(x => x.Locaciones.Descripcion)
                    .GroupBy(x => x.Almacenes.Descripcion)
                    .FirstOrDefaultAsync();
            }
        }

        //Add InventartioSelected
        private async void SaveInventarioButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new ProjectContext())
            {
                var rutaSelected = (Models.Rutas)MasterListView.SelectedItem;
                var itemSelected = (Inventario)InventarioToSelectListView.SelectedItem;
                
                
                var selectedInventario = new SelectedInventario
                {
                    Balance = Convert.ToInt32(SalienteTextBox.Text),
                    AlmacenesId = itemSelected.AlmacenesId,
                    LocacionesId = itemSelected.LocacionesId,
                    PartNumberId = itemSelected.PartNumberId,
                    Rutasid = rutaSelected.Id,
                };

                db.SelectedInventario.Add(selectedInventario);
                int balance1 = itemSelected.Balance;
                int balance2 = selectedInventario.Balance;
                itemSelected.Balance = balance1 - balance2;

                db.Inventario.Update(itemSelected);

                await db.SaveChangesAsync();

                InventarioSelectedListView.ItemsSource = await db.SelectedInventario
                    .Include(x => x.Almacenes)
                    .Include(x => x.Locaciones)
                    .Include(x => x.PartNumbers)
                    .Include(x => x.Rutas)
                    .ToListAsync();
                InventarioModalDialog.IsModal = false;

                InventarioSelectedListView.SelectedItem = selectedInventario;
                InventarioSelectedListView.ScrollIntoView(selectedInventario);

                //Repopulate InventarioToAdd
                cvs1.Source = await db.Inventario
                    .Include(x => x.Almacenes)
                    .Include(x => x.Locaciones)
                    .Include(x => x.PartNumbers)
                    .OrderBy(x => x.Locaciones.Descripcion)
                    .GroupBy(x => x.Almacenes.Descripcion)
                    .ToListAsync();


                InventarioToSelectListView.SelectedItem = await db.Inventario
                    .Include(x => x.Almacenes)
                    .Include(x => x.Locaciones)
                    .Include(x => x.PartNumbers)
                    .OrderBy(x => x.Locaciones.Descripcion)
                    .GroupBy(x => x.Almacenes.Descripcion)
                    .FirstOrDefaultAsync();

                BalanceTextBox.Text = itemSelected.Balance.ToString();
                SalienteTextBox.Text = string.Empty;
            }
        }

        //InventarioSelectedListView SelectionChanged
        private void InventarioSelectedListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (InventarioSelectedListView.SelectedItem != null)
            {
                ReturnInventarioSelectedButton.IsEnabled = true;
                CambiarInventarioSelectedButton.IsEnabled = true;
            }
            else
            {
                ReturnInventarioSelectedButton.IsEnabled = false;
                CambiarInventarioSelectedButton.IsEnabled = false;
            }
        }

        //CambiarInventario SelectedButton
        private void CambiarInventarioSelectedButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeInventarioModalDialog.IsModal = true;
            var itemToChange = (SelectedInventario)InventarioSelectedListView.SelectedItem;
            TotalToReplaceTextBox.Text = itemToChange.Balance.ToString();
            ValidateTotalToReplace();
            ValidatePartNumberToReplace();
            ValidateDescripcionToReplace();
            ValidateCantidadToReplace();
        }

        //Close ChangeInventarioButton
        private void CloseChangeInventarioButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeInventarioModalDialog.IsModal = false;
            //Clean Inputs
            PartNumberToReplaceAutoSuggestBox.Text = string.Empty;
            DescripcionToReplaceTextBox.Text = string.Empty;
            TotalToReplaceTextBox.Text = string.Empty;
            CantidadToReplaceTextBox.Text = string.Empty;
        }

        //ChangeInventario Event
        private async void ChangeInventarioButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new ProjectContext())
                {
                    var selectedItem = (SelectedInventario)InventarioSelectedListView.SelectedItem;
                    var selectedItemDelete = (SelectedInventario)InventarioSelectedListView.SelectedItem;
                    var selectedRuta = (Models.Rutas)MasterListView.SelectedItem;

                    var newItem = new SelectedInventario
                    {
                        Balance = Convert.ToInt32(CantidadToReplaceTextBox.Text),
                        AlmacenesId = selectedItem.AlmacenesId,
                        LocacionesId = selectedItem.LocacionesId,
                        PartNumberId = await
                                db.PartNumbers
                                    .Where(x => x.PartNumber == PartNumberToReplaceAutoSuggestBox.Text)
                                    .Select(x => x.Id)
                                    .SingleOrDefaultAsync(),
                        Rutasid = selectedRuta.Id,
                    };

                    if (selectedItem.PartNumberId == newItem.PartNumberId)
                    {
                        var message = new MessageDialog("No es posible reemplazar el mismo item", "Advertencia");
                        await message.ShowAsync();
                    }
                    else
                    {
                        if (selectedItem.Balance == Convert.ToInt32(CantidadToReplaceTextBox.Text))
                        {
                            db.SelectedInventario.Remove(selectedItemDelete);
                            db.SelectedInventario.Add(newItem);
                            await db.SaveChangesAsync();

                            ChangeInventarioModalDialog.IsModal = false;

                            InventarioSelectedListView.ItemsSource = await
                            db.SelectedInventario
                            .Include(x => x.Rutas)
                            .Include(x => x.PartNumbers)
                            .Include(x => x.Locaciones)
                            .Include(x => x.Almacenes)
                            .Where(x => x.Rutasid == selectedRuta.Id)
                            .ToListAsync();
                        }

                        else if (selectedItem.Balance > Convert.ToInt32(CantidadToReplaceTextBox.Text))
                        {
                            selectedItemDelete.Balance = selectedItemDelete.Balance - newItem.Balance;
                            db.SelectedInventario.Update(selectedItemDelete);
                            db.SelectedInventario.Add(newItem);
                            await db.SaveChangesAsync();
                            ChangeInventarioModalDialog.IsModal = false;

                            InventarioSelectedListView.ItemsSource = await
                            db.SelectedInventario
                            .Include(x => x.Rutas)
                            .Include(x => x.PartNumbers)
                            .Include(x => x.Locaciones)
                            .Include(x => x.Almacenes)
                            .Where(x => x.Rutasid == selectedRuta.Id)
                            .ToListAsync();
                        }
                        //Clean Inputs
                        PartNumberToReplaceAutoSuggestBox.Text = string.Empty;
                        DescripcionToReplaceTextBox.Text = string.Empty;
                        TotalToReplaceTextBox.Text = string.Empty;
                        CantidadToReplaceTextBox.Text = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                var message = new MessageDialog(ex.InnerException.ToString());
                await message.ShowAsync();
            }
        }

        //Open ReturnInventarioModalDialog
        private void ReturnInventarioSelectedButton_Click(object sender, RoutedEventArgs e)
        {
            ReturnInventarioModalDialog.IsModal = true;

            var selectedItem = (SelectedInventario)InventarioSelectedListView.SelectedItem;

            BalanceToReturnTextBox.Text = selectedItem.Balance.ToString();

            ValidateBalanceToReturn();
            ValidateCantidadToReturn();
            ValidateLocacionToReturn();
        }

        //Close ReturnInventarioModalDialog
        private void CloseReturnInventarioButton_Click(object sender, RoutedEventArgs e)
        {
            ReturnInventarioModalDialog.IsModal = false;

            //Clean Inputs
            BalanceToReturnTextBox.Text = string.Empty;
            CantidadToReturnTextBox.Text = string.Empty;
            LocacionToReturnComboBox.SelectedItem = null;
        }

        //ReturnInventario Event
        private async void ReturnInventarioButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new ProjectContext())
            {
                var selectedItem = (SelectedInventario)InventarioSelectedListView.SelectedItem;
                var selectedRuta = (Models.Rutas)MasterListView.SelectedItem;

                var query = db.Inventario
                    .Include(x => x.Almacenes)
                    .Include(x => x.Locaciones)
                    .Include(x => x.PartNumbers)
                    .Where(x => x.AlmacenesId == selectedItem.AlmacenesId &&
                    x.LocacionesId == (int)LocacionToReturnComboBox.SelectedValue &&
                    x.PartNumberId == selectedItem.PartNumberId).Count();

                if (query < 1)
                {
                    if (selectedItem.Balance == Convert.ToInt32(CantidadToReturnTextBox.Text))
                    {
                        var inventario = new Inventario
                        {
                            Balance = Convert.ToInt32(CantidadToReturnTextBox.Text),
                            AlmacenesId = selectedItem.AlmacenesId,
                            LocacionesId = (int)LocacionToReturnComboBox.SelectedValue,
                            PartNumberId = selectedItem.PartNumberId,
                        };

                        db.SelectedInventario.Remove(selectedItem);
                        db.Inventario.Add(inventario);
                        await db.SaveChangesAsync();

                        ReturnInventarioModalDialog.IsModal = false;

                        InventarioSelectedListView.ItemsSource = await
                        db.SelectedInventario
                        .Include(x => x.Rutas)
                        .Include(x => x.PartNumbers)
                        .Include(x => x.Locaciones)
                        .Include(x => x.Almacenes)
                        .Where(x => x.Rutasid == selectedRuta.Id)
                        .ToListAsync();
                    }
                    else if (selectedItem.Balance > Convert.ToInt32(CantidadToReturnTextBox.Text))
                    {
                        var inventario = new Inventario
                        {
                            Balance = Convert.ToInt32(CantidadToReturnTextBox.Text),
                            AlmacenesId = selectedItem.AlmacenesId,
                            LocacionesId = (int)LocacionToReturnComboBox.SelectedValue,
                            PartNumberId = selectedItem.PartNumberId,
                        };

                        selectedItem.Balance = selectedItem.Balance - inventario.Balance;
                        db.SelectedInventario.Update(selectedItem);
                        db.Inventario.Add(inventario);
                        await db.SaveChangesAsync();

                        ReturnInventarioModalDialog.IsModal = false;

                        InventarioSelectedListView.ItemsSource = await
                        db.SelectedInventario
                        .Include(x => x.Rutas)
                        .Include(x => x.PartNumbers)
                        .Include(x => x.Locaciones)
                        .Include(x => x.Almacenes)
                        .Where(x => x.Rutasid == selectedRuta.Id)
                        .ToListAsync();
                    }
                }
                else if (query >= 1)
                {
                    if (selectedItem.Balance == Convert.ToInt32(CantidadToReturnTextBox.Text))
                    {
                        var inventarioExistente = await db.Inventario
                            .Include(x => x.Almacenes)
                            .Include(x => x.Locaciones)
                            .Include(x => x.PartNumbers)
                            .Where(x => x.AlmacenesId == selectedItem.AlmacenesId &&
                            x.LocacionesId == (int)LocacionToReturnComboBox.SelectedValue &&
                            x.PartNumberId == selectedItem.PartNumberId).SingleOrDefaultAsync();

                        inventarioExistente.Balance = inventarioExistente.Balance + Convert.ToInt32(CantidadToReturnTextBox.Text);

                        db.Inventario.Update(inventarioExistente);
                        db.SelectedInventario.Remove(selectedItem);
                        await db.SaveChangesAsync();

                        ReturnInventarioModalDialog.IsModal = false;

                        InventarioSelectedListView.ItemsSource = await
                        db.SelectedInventario
                        .Include(x => x.Rutas)
                        .Include(x => x.PartNumbers)
                        .Include(x => x.Locaciones)
                        .Include(x => x.Almacenes)
                        .Where(x => x.Rutasid == selectedRuta.Id)
                        .ToListAsync();
                    }
                    else if (selectedItem.Balance > Convert.ToInt32(CantidadToReturnTextBox.Text))
                    {
                        var inventarioExistente = await db.Inventario
                            .Include(x => x.Almacenes)
                            .Include(x => x.Locaciones)
                            .Include(x => x.PartNumbers)
                            .Where(x => x.AlmacenesId == selectedItem.AlmacenesId &&
                            x.LocacionesId == (int)LocacionToReturnComboBox.SelectedValue &&
                            x.PartNumberId == selectedItem.PartNumberId).SingleOrDefaultAsync();

                        inventarioExistente.Balance = inventarioExistente.Balance + Convert.ToInt32(CantidadToReturnTextBox.Text);
                        selectedItem.Balance = selectedItem.Balance - Convert.ToInt32(CantidadToReturnTextBox.Text);

                        db.Inventario.Update(inventarioExistente);
                        db.SelectedInventario.Update(selectedItem);
                        await db.SaveChangesAsync();

                        ReturnInventarioModalDialog.IsModal = false;

                        InventarioSelectedListView.ItemsSource = await
                        db.SelectedInventario
                        .Include(x => x.Rutas)
                        .Include(x => x.PartNumbers)
                        .Include(x => x.Locaciones)
                        .Include(x => x.Almacenes)
                        .Where(x => x.Rutasid == selectedRuta.Id)
                        .ToListAsync();
                    }
                }
                CantidadToReturnTextBox.Text = string.Empty;
                LocacionToReturnComboBox.SelectedItem = null;
            }
        }

        //Go to IndicadoresPage
        private async void GoToIndicadoresButton_Click(object sender, RoutedEventArgs e)
        { 
            var service = Frame.GetNavigationService();
            await service.NavigateAsync(typeof(Views.Página_Indicadores));

        }
    }
}
