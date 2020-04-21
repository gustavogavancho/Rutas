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
    public sealed partial class TecnicosPage : Page
    {
        public TecnicosPage()
        {
            InitializeComponent();
        }

        private PrintHelper printHelper;

        public List<Tecnicos> TecnicosList { get; set; }

        public class GroupInfoList<T> : List<object>
        {
            public object Key { get; set; }

            public new IEnumerator<object> GetEnumerator()
            {
                return (System.Collections.Generic.IEnumerator<object>)base.GetEnumerator();
            }
        }

        internal List<GroupInfoList<object>> GetGroupsByLetter()
        {
            var groups = new List<GroupInfoList<object>>();

            var query = from item in TecnicosList
                        orderby ((Tecnicos)item).Nombre
                        group item by ((Tecnicos)item).Nombre[0] into g
                        select new { GroupName = g.Key, Items = g };
            foreach (var g in query)
            {
                var info = new GroupInfoList<object>();
                info.Key = g.GroupName;
                foreach (var item in g.Items)
                {
                    info.Add(item);
                }

                groups.Add(info);
            }

            return groups;
        }

        #region Helper Methods

        #region ValidateSaveButton
        public void ValidateSaveButton()
        {
            if (NombreValidator.Text == "Valido" && ApellidoValidator.Text == "Valido" &&
                DniValidator.Text == "Valido" && DireccionValidator.Text == "Valido" &&
                FechaNacimientoValidator.Text == "Valido" && TelefonoValidator.Text == "Valido" &&
                EmailValidator.Text == "Valido")
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
            NombreTextBox.Text = string.Empty;
            ApellidoTextBox.Text = string.Empty;
            DniTextBox.Text = string.Empty;
            DireccionTextBox.Text = string.Empty;
            FechaNacimientoDatePicker.Date = DateTime.Now;
            TelefonoTextBox.Text = string.Empty;
            EmailTextBox.Text = string.Empty;
        }
        #endregion

        #endregion

        #region Validate Methods

        #region Nombre Method
        public void ValidateNombre()
        {
            if (NombreTextBox.Text == string.Empty)
            {
                NombreValidator.Text = "Campo requerido";
                NombreRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveButton.IsEnabled = false;
            }
            else
            {
                NombreValidator.Text = "Valido";
                NombreRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                ValidateSaveButton();
            }
        }
        #endregion

        #region Apellido Method
        public void ValidateApellido()
        {
            if (ApellidoTextBox.Text == string.Empty)
            {
                ApellidoValidator.Text = "Campo requerido";
                ApellidoRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveButton.IsEnabled = false;
            }
            else
            {
                ApellidoValidator.Text = "Valido";
                ApellidoRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                ValidateSaveButton();
            }
        }
        #endregion

        #region Dni Method
        public void ValidateDni()
        {
            int outParse;
            bool isNum = Int32.TryParse(DniTextBox.Text, out outParse);

            if (DniTextBox.Text == string.Empty)
            {
                DniValidator.Text = "Campo requerido";
                DniRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveButton.IsEnabled = false;
            }

            else if (isNum && Convert.ToInt32(DniTextBox.Text) > 0)
            {
                DniValidator.Text = "Valido";
                DniRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                ValidateSaveButton();
            }

            else
            {
                DniValidator.Text = "Campo requerido";
                DniRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveButton.IsEnabled = false;
            }
        }
        #endregion

        #region FechaNacimiento Method
        public void ValidateFechaNacimiento()
        {
            if (FechaNacimientoDatePicker.Date.Year == DateTime.Now.Year)
            {
                FechaNacimientoValidator.Text = "Campo requerido";
                FechaNacimientoRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveButton.IsEnabled = false;
            }
            else
            {
                FechaNacimientoValidator.Text = "Valido";
                FechaNacimientoRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                ValidateSaveButton();
            }
        }

        #endregion

        #region Direccion Method
        public void ValidateDireccion()
        {
            if (DireccionTextBox.Text == string.Empty)
            {
                DireccionValidator.Text = "Campo requerido";
                DireccionRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveButton.IsEnabled = false;
            }
            else
            {
                DireccionValidator.Text = "Valido";
                DireccionRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                ValidateSaveButton();
            }
        }
        #endregion

        #region Email Method
        public void ValidateEmail()
        {
            if (EmailTextBox.Text == string.Empty)
            {
                EmailValidator.Text = "Campo requerido";
                EmailRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveButton.IsEnabled = false;
            }
            else
            {
                EmailValidator.Text = "Valido";
                EmailRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                ValidateSaveButton();
            }
        }
        #endregion

        #region Telefono Method
        public void ValidateTelefono()
        {
            int outParse;
            bool isNum = Int32.TryParse(TelefonoTextBox.Text, out outParse);

            if (TelefonoTextBox.Text == string.Empty)
            {
                TelefonoValidator.Text = "Campo requerido";
                TelefonoRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveButton.IsEnabled = false;
            }

            else if (isNum && Convert.ToInt32(TelefonoTextBox.Text) > 0)
            {
                TelefonoValidator.Text = "Valido";
                TelefonoRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                ValidateSaveButton();
            }

            else
            {
                TelefonoValidator.Text = "Campo requerido";
                TelefonoRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveButton.IsEnabled = false;
            }
        }
        #endregion

        #endregion

        #region Validate Events

        //NombreTextBox TextChanged
        private void NombreTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateNombre();
        }

        //ApellidoTextBox TextChanged
        private void ApellidoTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateApellido();
        }

        //DniTextBox TextChanged
        private void DniTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateDni();
        }

        //DireccionTextBox TextChanged
        private void DireccionTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateDireccion();
        }

        //FechaNacimientoDatePicker DateChanged
        private void FechaNacimientoDatePicker_OnDateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            ValidateFechaNacimiento();
        }

        //TelefonoTextBox TextChanged
        private void TelefonoTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateTelefono();
        }

        //EmailTextBox TextChanged
        private void EmailTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateEmail();
        }

        #endregion

        #region CRUD

        #region Save
        private async void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            using (var db = new ProjectContext())
            {
                //Add
                if (flag == 0)
                {
                    var newTecnico = new Tecnicos
                    {
                        Nombre = NombreTextBox.Text,
                        Apellido = ApellidoTextBox.Text,
                        Dni = DniTextBox.Text,
                        Direccion = DireccionTextBox.Text,
                        FechaNacimiento = FechaNacimientoDatePicker.Date.LocalDateTime,
                        Email = EmailTextBox.Text,
                        Telefono = Int32.Parse(TelefonoTextBox.Text),
                    };
                    string apocope1 = newTecnico.Nombre;
                    string apocope2 = newTecnico.Apellido;
                    newTecnico.Apocope = apocope1.Substring(0, 1).ToUpper() + apocope2.Substring(0, 1).ToUpper();
                    db.Tecnicos.Add(newTecnico);
                    await db.SaveChangesAsync();

                    TecnicosList = await db.Tecnicos.OrderBy(x => x.Nombre).ToListAsync();
                    cvs.Source = GetGroupsByLetter();

                    InputModalDialog.IsModal = false;
                    MasterListView.SelectedItem = newTecnico;
                    MasterListView.ScrollIntoView(newTecnico);
                }
                //Edit
                else if (flag == 1)
                {
                    var itemSelected = (Tecnicos)MasterListView.SelectedItem;

                    if (MasterListView.SelectedItem != null)
                    {
                        itemSelected.Nombre = NombreTextBox.Text;
                        itemSelected.Apellido = ApellidoTextBox.Text;
                        itemSelected.Dni = DniTextBox.Text;
                        itemSelected.Direccion = DireccionTextBox.Text;
                        itemSelected.FechaNacimiento = FechaNacimientoDatePicker.Date.LocalDateTime;
                        itemSelected.Email = EmailTextBox.Text;
                        itemSelected.Telefono = Int32.Parse(TelefonoTextBox.Text);

                        string apocope1 = itemSelected.Nombre;
                        string apocope2 = itemSelected.Apellido;

                        itemSelected.Apocope = apocope1.Substring(0, 1).ToUpper() + apocope2.Substring(0, 1).ToUpper();

                        db.Tecnicos.Update(itemSelected);

                        await db.SaveChangesAsync();

                        TecnicosList = await db.Tecnicos.OrderBy(x => x.Nombre).ToListAsync();
                        cvs.Source = GetGroupsByLetter();

                        InputModalDialog.IsModal = false;
                        MasterListView.SelectedItem = itemSelected;
                        MasterListView.ScrollIntoView(itemSelected);
                    }
                }
                var query = db.Tecnicos.Count();
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
            if (printHelper != null)
            {
                printHelper.UnregisterForPrinting();
                // Initalize common helper class and register for printing
                printHelper = new PrintHelper(this);
                printHelper.RegisterForPrinting();

                // Initialize print content for this scenario
                printHelper.PreparePrintContent(new TecnicosReport());
            }

            //Enable
            MasterListView.IsEnabled = true;
            ListViewCommandBar.IsEnabled = true;
            SearchAutoSuggestBox.IsEnabled = true;

        }
        #endregion

        #region Edit Nav
        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            using (var db = new ProjectContext())
            {
                flag = 1;

                var editItem = (Tecnicos)MasterListView.SelectedItem;

                if (MasterListView.SelectedItem != null)
                {
                    NombreTextBox.Text = editItem.Nombre;
                    ApellidoTextBox.Text = editItem.Apellido;
                    DniTextBox.Text = editItem.Dni;
                    DireccionTextBox.Text = editItem.Direccion;
                    FechaNacimientoDatePicker.Date = editItem.FechaNacimiento;
                    EmailTextBox.Text = editItem.Email;
                    TelefonoTextBox.Text = editItem.Telefono.ToString();

                    InputModalDialog.IsModal = true;

                    ValidateNombre();
                    ValidateApellido();
                    ValidateDni();
                    ValidateDireccion();
                    ValidateFechaNacimiento();
                    ValidateEmail();
                    ValidateTelefono();

                    ValidateSaveButton();
                }
            }
            //Disable
            MasterListView.IsEnabled = false;
            ListViewCommandBar.IsEnabled = false;
            SearchAutoSuggestBox.IsEnabled = false;
        }
        #endregion

        #region Delete
        private async void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            using (var db = new ProjectContext())
            {
                var itemToDelete = (Tecnicos)MasterListView.SelectedItem;
                if (MasterListView.SelectedItem != null)
                {
                    //MessageDialog
                    var dialog = new MessageDialog("¿Desea eliminar el técnico seleccionado?", "Advertencia");
                    dialog.Commands.Add(new UICommand("Si") { Id = 0 });
                    dialog.Commands.Add(new UICommand("No") { Id = 1 });

                    var result = await dialog.ShowAsync();

                    if (result != null && result.Label == "Si")
                    {
                        db.Tecnicos.Remove(itemToDelete);
                        await db.SaveChangesAsync();

                        TecnicosList = await db.Tecnicos.OrderBy(x => x.Nombre).ToListAsync();
                        cvs.Source = GetGroupsByLetter();
                        MasterListView.SelectedItem = await db.Tecnicos.OrderBy(x => x.Nombre).FirstOrDefaultAsync();

                    }
                }
                var query = db.Tecnicos.Count();
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
            if (printHelper != null)
            {
                printHelper.UnregisterForPrinting();
                // Initalize common helper class and register for printing
                printHelper = new PrintHelper(this);
                printHelper.RegisterForPrinting();

                // Initialize print content for this scenario
                printHelper.PreparePrintContent(new TecnicosReport());
            }
        }
        #endregion

        #endregion

        #region Search
        private async void SearchAutoSuggestBox_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            using (var db = new ProjectContext())
            {
                string search = SearchAutoSuggestBox.Text;
                var query = await db.Tecnicos
                    .Where(x => x.Nombre.Contains(search) || x.Apellido.Contains(search))
                    .OrderBy(x => x.Nombre)
                    .ToListAsync();
                var query2 = await db.Tecnicos
                    .Where(x => x.Nombre.Contains(search) || x.Apellido.Contains(search))
                    .OrderBy(x => x.Nombre)
                    .FirstOrDefaultAsync();

                TecnicosList = query;
                cvs.Source = GetGroupsByLetter();
                MasterListView.SelectedItem = query2;
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
            printHelper.PreparePrintContent(new TecnicosReport());
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (printHelper != null)
            {
                printHelper.UnregisterForPrinting();
            }
        }
        #endregion

        private Tecnicos _lastSelectedItem;

        //flag int
        private int flag = 0;

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
            // Assure we are displaying the correct item. This is necessary in certain adaptive cases.
            MasterListView.SelectedItem = _lastSelectedItem;
        }

        //Open ModalDialog
        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            flag = 0;
            CleanInputs();
            ValidateNombre();
            ValidateApellido();
            ValidateDni();
            ValidateDireccion();
            ValidateEmail();
            ValidateFechaNacimiento();
            ValidateTelefono();

            InputModalDialog.IsModal = true;

            //Disable
            MasterListView.IsEnabled = false;
            ListViewCommandBar.IsEnabled = false;
            SearchAutoSuggestBox.IsEnabled = false;
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

        //MasterListView SelectionChanged
        private void MasterListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var clickedItem = (Tecnicos) MasterListView.SelectedItem;
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

        //Load Page
        private async void TecnicosPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            using (var db = new ProjectContext())
            {
                TecnicosList = await db.Tecnicos.OrderBy(x => x.Nombre).ToListAsync();
                cvs.Source = GetGroupsByLetter();

                MasterListView.SelectedItem = await db.Tecnicos
                    .OrderBy(x => x.Nombre)
                    .FirstOrDefaultAsync();
                var query = db.Tecnicos.Count();
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
    }
}
