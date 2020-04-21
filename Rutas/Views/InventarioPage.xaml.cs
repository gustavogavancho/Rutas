using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
using Template10.Utils;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Rutas.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InventarioPage : Page
    {
        public InventarioPage()
        {
            InitializeComponent();
        }

        int flag = 0;

        private PrintHelper printHelper;

        private Inventario _lastSelectedItem;

        #region Helper Methods

        #region Validate SaveButton
        public void ValidateSaveButton()
        {
            if (PartNumberValidator.Text == "Valido" && DescripcionValidator.Text == "Valido" &&
                AlmacenValidator.Text == "Valido" && LocacionValidator.Text == "Valido" &&
                BalanceValidator.Text == "Valido")
            {
                SaveButton.IsEnabled = true;
            }
            else
            {
                SaveButton.IsEnabled = false;
            }
        }
        #endregion

        #region Clean Inputs

        public void CleanInputs()
        {
            PartNumberAutoSuggestBox.Text = string.Empty;
            DescripcionTextBox.Text = string.Empty;
            AlmacenComboBox.SelectedItem = null;
            LocacionComboBox.SelectedItem = null;
        }

        #endregion

        #endregion

        #region Validate Methods

        #region PartNumber Method
        public void ValidatePartNumber()
        {
            if (PartNumberAutoSuggestBox.Text == string.Empty)
            {
                PartNumberValidator.Text = "Campo requerido";
                PartNumberRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveButton.IsEnabled = false;
            }
            else
            {
                PartNumberValidator.Text = "Valido";
                PartNumberRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
            }  
        }
        #endregion

        #region Descripcion Method
        public void ValidateDescripcion()
        {
            if (DescripcionTextBox.Text == string.Empty)
            {
                DescripcionValidator.Text = "Campo requerido";
                DescripcionRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveButton.IsEnabled = false;
            }
            else
            {
                DescripcionValidator.Text = "Valido";
                DescripcionRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                ValidateSaveButton();
            }
        }
        #endregion

        #region Almacen Method

        public void ValidateAlmacen()
        {
            if (AlmacenComboBox.SelectedItem == null)
            {
                AlmacenValidator.Text = "Campo requerido";
                AlmacenRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveButton.IsEnabled = false;
            }
            else
            {
                AlmacenValidator.Text = "Valido";
                AlmacenRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                ValidateSaveButton();
            }
        }

        #endregion

        #region Locacion Method

        public void ValidateLocacion()
        {
            if (LocacionComboBox.SelectedItem == null)
            {
                LocacionValidator.Text = "Campo requerido";
                LocacionRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveButton.IsEnabled = false;
            }
            else
            {
                LocacionValidator.Text = "Valido";
                LocacionRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                ValidateSaveButton();
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
                SaveButton.IsEnabled = false;
            }

            else if (isNum && Convert.ToInt32(BalanceTextBox.Text) >= 0)
            {
                BalanceValidator.Text = "Valido";
                BalanceRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                ValidateSaveButton();
            }

            else
            {
                BalanceValidator.Text = "Campo requerido";
                BalanceRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveButton.IsEnabled = false;
            }
        }

        #endregion

        #endregion

        #region Validate Events

        //PartNumberAutoSuggestBox TextChanged
        private async void PartNumberAutoSuggestBox_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            using (var db = new ProjectContext())
            {
                if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
                {
                    sender.ItemsSource =
                        await
                            db.PartNumbers.OrderBy(x => x.PartNumber)
                                .Where(x => x.PartNumber.Contains(PartNumberAutoSuggestBox.Text))
                                .Select(x => x.PartNumber)
                                .ToListAsync();
                }

                var query =
                    await
                        db.PartNumbers
                            .OrderBy(x => x.PartNumber)
                            .Where(x => x.PartNumber == PartNumberAutoSuggestBox.Text)
                            .Select(x => x.Descripcion)
                            .SingleOrDefaultAsync();

                if (PartNumberAutoSuggestBox.Text == string.Empty || query == null)
                {
                    PartNumberValidator.Text = "Campo requerido";
                    PartNumberRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                    DescripcionTextBox.Text = string.Empty;
                }
                else if (query != null)
                {
                    PartNumberValidator.Text = "Valido";
                    PartNumberRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                    DescripcionTextBox.Text = query;
                }
            }
        }

        //DescripcionTextBox TextChanged
        private void DescripcionTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateDescripcion();
        }

        //AlmacenComboBox SelectionChanged
        private void AlmacenComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ValidateAlmacen();
        }

        //LocacionComboBox SelectionChanged
        private void LocacionComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ValidateLocacion();
        }

        //BalanceTextBox TextChanged
        private void BalanceTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateBalance();
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
                        var invetario = new Inventario
                        {
                            Balance = Convert.ToInt32(BalanceTextBox.Text),
                            AlmacenesId = (int) AlmacenComboBox.SelectedValue,
                            LocacionesId = (int) LocacionComboBox.SelectedValue,
                            PartNumberId = await
                                db.PartNumbers
                                    .Where(x => x.PartNumber == PartNumberAutoSuggestBox.Text)
                                    .Select(x => x.Id)
                                    .SingleOrDefaultAsync(),
                        };

                        var query = db.Inventario
                            .Include(x => x.Almacenes)
                            .Include(x => x.Locaciones)
                            .Include(x => x.PartNumbers)
                            .Count(x => x.AlmacenesId == invetario.AlmacenesId &&
                                        x.LocacionesId == invetario.LocacionesId &&
                                        x.PartNumberId == invetario.PartNumberId);

                        if (query >= 1)
                        {
                            var message = new MessageDialog("Ya existe el item que usted desea crear", "Advertencia");

                            await message.ShowAsync();
                            SaveButton.IsEnabled = false;
                        }
                        else
                        {
                            db.Inventario.Add(invetario);

                            await db.SaveChangesAsync();

                            cvs.Source = await db.Inventario
                                .Include(x => x.Almacenes)
                                .Include(x => x.Locaciones)
                                .Include(x => x.PartNumbers)
                                .OrderBy(x => x.Locaciones.Descripcion)
                                .GroupBy(x => x.Almacenes.Descripcion)
                                .ToListAsync();

                            InputModalDialog.IsModal = false;

                            MasterListView.SelectedItem = invetario;
                            MasterListView.ScrollIntoView(invetario);
                        }

                    }
                    //Edit
                    else if (flag == 1)
                    {
                        var itemSelected = (Inventario) MasterListView.SelectedItem;

                        if (MasterListView.SelectedItem != null)
                        {
                            itemSelected.PartNumberId =
                                await
                                    db.PartNumbers.Where(x => x.PartNumber == PartNumberAutoSuggestBox.Text)
                                        .Select(x => x.Id)
                                        .SingleOrDefaultAsync();
                            itemSelected.AlmacenesId = (int) AlmacenComboBox.SelectedValue;
                            itemSelected.LocacionesId = (int) LocacionComboBox.SelectedValue;
                            itemSelected.Balance = Convert.ToInt32(BalanceTextBox.Text);
                            {
                                db.Inventario.Update(itemSelected);
                                await db.SaveChangesAsync();

                                cvs.Source = await db.Inventario
                                    .Include(x => x.Almacenes)
                                    .Include(x => x.Locaciones)
                                    .Include(x => x.PartNumbers)
                                    .OrderBy(x => x.Locaciones.Descripcion)
                                    .GroupBy(x => x.Almacenes.Descripcion)
                                    .ToListAsync();

                                InputModalDialog.IsModal = false;

                                MasterListView.SelectedItem = itemSelected;
                                MasterListView.ScrollIntoView(itemSelected);
                            }
                        }
                    }
                    var query1 = db.Inventario.Count();
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
                printHelper.PreparePrintContent(new InventarioReport());
            }

            //Enable
            MasterListView.IsEnabled = true;
            ListViewCommandBar.IsEnabled = true;
            SearchAutoSuggestBox.IsEnabled = true;

        }
        #endregion

        #region Edit
        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            CleanInputs();
            using (var db = new ProjectContext())
            {
                flag = 1;

                var editStock = (Inventario)MasterListView.SelectedItem;

                if (MasterListView.SelectedItem != null)
                {
                    PartNumberAutoSuggestBox.Text = editStock.PartNumbers.PartNumber;
                    DescripcionTextBox.Text = editStock.PartNumbers.Descripcion;
                    AlmacenComboBox.SelectedValue = (int)editStock.AlmacenesId;
                    LocacionComboBox.SelectedValue = (int)editStock.LocacionesId;
                    BalanceTextBox.Text = editStock.Balance.ToString();

                    InputModalDialog.IsModal = true;

                    PartNumberValidator.Text = "Valido";
                    PartNumberRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                    DescripcionValidator.Text = "Valido";
                    DescripcionRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                    ValidateDescripcion();
                    ValidateAlmacen();
                    ValidateLocacion();
                    ValidateBalance();

                    SaveButton.IsEnabled = true;
                }
            }
            //Disable
            SearchAutoSuggestBox.IsEnabled = false;
            MasterListView.IsEnabled = false;
            ListViewCommandBar.IsEnabled = false;
        }
        #endregion

        #region Delete
        private async void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            using (var db = new ProjectContext())
            {
                var itemToDelete = (Inventario) MasterListView.SelectedItem;
                if (MasterListView.SelectedItem != null)
                {
                    //MessageDialog
                    var dialog = new MessageDialog("¿Desea eliminar el item seleccionado?", "Advertencia");
                    dialog.Commands.Add(new UICommand("Si") {Id = 0});
                    dialog.Commands.Add(new UICommand("No") {Id = 1});

                    var result = await dialog.ShowAsync();

                    if (result != null && result.Label == "Si")
                    {
                        db.Inventario.Remove(itemToDelete);
                        await db.SaveChangesAsync();

                        cvs.Source = await db.Inventario
                            .Include(x => x.Almacenes)
                            .Include(x => x.Locaciones)
                            .Include(x => x.PartNumbers)
                            .OrderBy(x => x.Locaciones.Descripcion)
                            .GroupBy(x => x.Almacenes.Descripcion)
                            .ToListAsync();

                        MasterListView.SelectedItem = await db.Inventario
                            .Include(x => x.Almacenes)
                            .Include(x => x.Locaciones)
                            .Include(x => x.PartNumbers)
                            .OrderBy(x => x.Locaciones.Descripcion)
                            .GroupBy(x => x.Almacenes.Descripcion)
                            .FirstOrDefaultAsync();
                    }
                }
                var query = db.Inventario.Count();
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
                printHelper.PreparePrintContent(new InventarioReport());
            }
        }
        #endregion

        #endregion

        #region Search
        private async void SearchAutoSuggestBox_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            using (var db = new ProjectContext())
            {
                cvs.Source = await db.Inventario
                    .Include(x => x.Almacenes)
                    .Include(x => x.Locaciones)
                    .Include(x => x.PartNumbers)
                    .Where(x => x.PartNumbers.PartNumber.Contains(SearchAutoSuggestBox.Text) || x.PartNumbers.Descripcion.Contains(SearchAutoSuggestBox.Text))
                    .OrderBy(x=> x.Locaciones.Descripcion)
                    .GroupBy(x => x.Almacenes.Descripcion)
                    .ToListAsync();

                MasterListView.SelectedItem = await db.Inventario
                    .Include(x => x.Almacenes)
                    .Include(x => x.Locaciones)
                    .Include(x => x.PartNumbers)
                    .Where(x => x.PartNumbers.PartNumber.Contains(SearchAutoSuggestBox.Text) || x.PartNumbers.Descripcion.Contains(SearchAutoSuggestBox.Text))
                    .OrderBy(x => x.Locaciones.Descripcion)
                    .GroupBy(x => x.Almacenes.Descripcion)
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
            printHelper.PreparePrintContent(new InventarioReport());
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (printHelper != null)
            {
                printHelper.UnregisterForPrinting();
            }
        }
        #endregion

        //Go To PartNumbersPage
        private async void GoToPartNumberButton_OnClick(object sender, RoutedEventArgs e)
        {
            var service = Frame.GetNavigationService();
            await service.NavigateAsync(typeof(Views.PartNumbersPage));
        }

        //Go To AlmacenesPage
        private async void GoToAlmacesButton_OnClick(object sender, RoutedEventArgs e)
        {
            var service = Frame.GetNavigationService();
            await service.NavigateAsync(typeof(Views.AlmacenesPage));
        }

        //Go To LocacionesPage
        private async void GoToLocacionesButton_OnClick(object sender, RoutedEventArgs e)
        {
            var service = Frame.GetNavigationService();
            await service.NavigateAsync(typeof(Views.LocacionesPage));
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
            ValidatePartNumber();
            ValidateDescripcion();
            ValidateAlmacen();
            ValidateLocacion();
            ValidateBalance();

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
        private async void InventarioPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            using (var db = new ProjectContext())
            {
                AlmacenComboBox.ItemsSource = await db.Almacenes.ToListAsync();
                AlmacenComboBox.SelectedValuePath = "Id";
                AlmacenComboBox.DisplayMemberPath = "Descripcion";

                LocacionComboBox.ItemsSource = await db.Locaciones.ToListAsync();
                LocacionComboBox.SelectedValuePath = "Id";
                LocacionComboBox.DisplayMemberPath = "Descripcion";

                cvs.Source = await db.Inventario
                    .Include(x => x.Almacenes)
                    .Include(x => x.Locaciones)
                    .Include(x => x.PartNumbers)
                    .OrderBy(x => x.Locaciones.Descripcion)
                    .GroupBy(x => x.Almacenes.Descripcion)
                    .ToListAsync();


                MasterListView.SelectedItem = await db.Inventario
                    .Include(x => x.Almacenes)
                    .Include(x => x.Locaciones)
                    .Include(x => x.PartNumbers)
                    .OrderBy(x => x.Locaciones.Descripcion)
                    .GroupBy(x => x.Almacenes.Descripcion)
                    .FirstOrDefaultAsync();

                var query = db.Inventario.AsNoTracking().Count();
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
            var clickedItem = (Inventario)MasterListView.SelectedItem;
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
    }
}