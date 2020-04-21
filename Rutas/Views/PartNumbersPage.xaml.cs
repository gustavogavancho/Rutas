using System;
using System.Linq;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Microsoft.EntityFrameworkCore;
using Rutas.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Rutas.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PartNumbersPage : Page
    {
        public PartNumbersPage()
        {
            InitializeComponent();
        }

        int flag = 0;

        private PartNumbers _lastSelectedItem;

        #region Helper Methods

        #region Validate SaveButton
        public void ValidateSaveButton()
        {
            if (CategoriaValidator.Text == "Valido" && PartNumberValidator.Text == "Valido" &&
                DescripcionValidator.Text == "Valido")
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
            CategoriaComboBox.SelectedItem = null;
            PartNumberTextBox.Text = string.Empty;
            DescripcionTextBox.Text = string.Empty;
        }
        #endregion

        #endregion

        #region Validate Methods

        #region Categoria Method
        public void ValidateCategoria()
        {
            if (CategoriaComboBox.SelectedItem == null)
            {
                CategoriaValidator.Text = "Campo requerido";
                CategoriaRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveButton.IsEnabled = false;
            }
            else
            {
                CategoriaValidator.Text = "Valido";
                CategoriaRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                ValidateSaveButton();
            }
        }
        #endregion

        #region PartNumber Method
        public void ValidatePartNumber()
        {
            if (PartNumberTextBox.Text == string.Empty)
            {
                PartNumberValidator.Text = "Campo requerido";
                PartNumberRectangle.Fill = new SolidColorBrush(Colors.DarkRed);
                SaveButton.IsEnabled = false;
            }
            else
            {
                PartNumberValidator.Text = "Valido";
                PartNumberRectangle.Fill = new SolidColorBrush(Colors.DarkGreen);
                ValidateSaveButton();
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

        #endregion

        #region Validate Events

        //CategoriaComboBox SelectionChanged
        private void CategoriaComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ValidateCategoria();
        }

        //PartNumberComboox TextChanged
        private void PartNumberTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidatePartNumber();
        }

        //PartNumberTextBox TextChanging
        private void PartNumberTextBox_OnTextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            var selectionStart = sender.SelectionStart;
            sender.Text = sender.Text.ToUpper();
            sender.SelectionStart = selectionStart;
            sender.SelectionLength = 0;
        }

        //DescripcionTextBox TextChanged
        private void DescripcionTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateDescripcion();
        }

        //DescripcionTextBox TextChanging
        private void DescripcionTextBox_OnTextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            var selectionStart = sender.SelectionStart;
            sender.Text = sender.Text.ToUpper();
            sender.SelectionStart = selectionStart;
            sender.SelectionLength = 0;
        }

        #endregion

        #region CRUD

        #region Save
        private async void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            using (var db = new ProjectContext())
            {
                //Save
                if (flag == 0)
                {
                    var partNumber = new PartNumbers
                    {
                        Categoria = (Categoria) CategoriaComboBox.SelectedItem,
                        PartNumber = PartNumberTextBox.Text,
                        Descripcion = DescripcionTextBox.Text
                    };
                    db.PartNumbers.Add(partNumber);
                    await db.SaveChangesAsync();

                    cvs.Source = await
                        db.PartNumbers
                        .GroupBy(x => x.Categoria)
                        .ToListAsync();

                    InputModalDialog.IsModal = false;
                    MasterListView.SelectedItem = partNumber;
                    MasterListView.ScrollIntoView(partNumber);
                }
                //Edit
                else if (flag == 1)
                {
                    var itemSelected = (PartNumbers)MasterListView.SelectedItem;

                    if (MasterListView.SelectedItem != null)
                    {
                        itemSelected.Categoria = (Categoria)CategoriaComboBox.SelectedItem;
                        itemSelected.PartNumber = PartNumberTextBox.Text;
                        itemSelected.Descripcion = DescripcionTextBox.Text;

                        db.PartNumbers.Update(itemSelected);
                        await db.SaveChangesAsync();

                        cvs.Source = await
                            db.PartNumbers
                            .GroupBy(x => x.Categoria)
                            .ToListAsync();

                        InputModalDialog.IsModal = false;
                        MasterListView.SelectedItem = itemSelected;
                        MasterListView.ScrollIntoView(itemSelected);
                    }
                }
                var query1 = db.PartNumbers.Count();
                if (query1 <= 0)
                {
                    EmptyItemsBorder.Visibility = Visibility.Visible;
                }
                else
                {
                    EmptyItemsBorder.Visibility = Visibility.Collapsed;
                }

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
            flag = 1;
            using (var db = new ProjectContext())
            {
                var selectedPartNumber = (PartNumbers)MasterListView.SelectedItem;

                if (selectedPartNumber != null)
                {
                    CategoriaComboBox.SelectedItem = selectedPartNumber.Categoria;
                    PartNumberTextBox.Text = selectedPartNumber.PartNumber;
                    DescripcionTextBox.Text = selectedPartNumber.Descripcion;

                    InputModalDialog.IsModal = true;

                    ValidateCategoria();
                    ValidatePartNumber();
                    ValidateDescripcion();
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
        private async void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            using (var db = new ProjectContext())
            {
                var itemToDelete = (PartNumbers)MasterListView.SelectedItem;
                if (MasterListView.SelectedItem != null)
                {
                    //MessageDialog
                    var dialog = new MessageDialog("¿Desea eliminar el Part Number seleccionado?", "Advertencia");
                    dialog.Commands.Add(new UICommand("Si") { Id = 0 });
                    dialog.Commands.Add(new UICommand("No") { Id = 1 });

                    var result = await dialog.ShowAsync();

                    if (result != null && result.Label == "Si")
                    {
                        db.PartNumbers.Remove(itemToDelete);
                        await db.SaveChangesAsync();

                        cvs.Source = await
                            db.PartNumbers
                            .GroupBy(x => x.Categoria)
                            .ToListAsync();

                        MasterListView.SelectedItem = await
                            db.PartNumbers
                            .FirstOrDefaultAsync();
                    }
                }
                var query1 = db.Almacenes.Count();
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
        #endregion

        #endregion

        #region Search
        private async void SearchAutoSuggestBox_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            using (var db = new ProjectContext())
            {
                cvs.Source = await
                    db.PartNumbers
                    .Where(x => x.PartNumber.Contains(SearchAutoSuggestBox.Text) || x.Descripcion.Contains(SearchAutoSuggestBox.Text))
                    .GroupBy(x => x.Categoria)
                    .ToListAsync();

                MasterListView.SelectedItem = await
                    db.PartNumbers
                    .Where(x => x.PartNumber.Contains(SearchAutoSuggestBox.Text) || x.Descripcion.Contains(SearchAutoSuggestBox.Text))
                    .GroupBy(x => x.Categoria)
                    .FirstOrDefaultAsync();
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

        //Open ModalDialog
        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            flag = 0;
            CleanInputs();
            ValidateCategoria();
            ValidateDescripcion();
            ValidatePartNumber();

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
        private async void PartNumbersPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            using (var db = new ProjectContext())
            {
                CategoriaComboBox.ItemsSource = Enum.GetValues(typeof(Categoria));

                cvs.Source = await
                    db.PartNumbers
                   .GroupBy(x => x.Categoria)
                   .ToListAsync();
                MasterListView.SelectedItem = await 
                    db.PartNumbers
                   .GroupBy(x => x.Categoria)
                   .FirstOrDefaultAsync();

                var query = db.PartNumbers.Count();
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
            var clickedItem = (PartNumbers)MasterListView.SelectedItem;
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
