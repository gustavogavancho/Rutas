using System;
using System.Linq;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Microsoft.EntityFrameworkCore;
using Prueba2.Services;
using Rutas.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Rutas.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AlmacenesPage : Page
    {
        public AlmacenesPage()
        {
            InitializeComponent();
        }

        int flag = 0;

        private Almacenes _lastSelectedItem;

        #region Helper Methods

        #region Validate SaveButton
        public void ValidateSaveButton()
        {
            if (DescripcionValidator.Text == "Valido")
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
            DescripcionTextBox.Text = string.Empty;
        }
        #endregion

        #endregion

        #region Validate Methods

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

        //DescripcionTextBox TextChanged
        private void DescripcionTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateDescripcion();
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
                    var almacen = new Almacenes
                    {
                        Descripcion = DescripcionTextBox.Text,
                    };
                    string apocope1 = almacen.Descripcion;
                    almacen.Apocope = apocope1.Substring(0, 1).ToUpper() + apocope1.Substring(1, 2).ToUpper();

                    db.Almacenes.Add(almacen);
                    await db.SaveChangesAsync();

                    MasterListView.ItemsSource = await
                        db.Almacenes
                        .OrderBy(x=> x.Descripcion)
                       .ToListAsync();

                    InputModalDialog.IsModal = false;
                    MasterListView.SelectedItem = almacen;
                    MasterListView.ScrollIntoView(almacen);
                }
                //Edit
                else if (flag == 1)
                {
                    var itemSelected = (Almacenes)MasterListView.SelectedItem;

                    if (MasterListView.SelectedItem != null)
                    {
                        itemSelected.Descripcion = DescripcionTextBox.Text;
                        string apocope1 = itemSelected.Descripcion;
                        itemSelected.Apocope = apocope1.Substring(0, 1).ToUpper() + apocope1.Substring(1, 2).ToUpper();

                        db.Almacenes.Update(itemSelected);
                        await db.SaveChangesAsync();

                        MasterListView.ItemsSource = await
                            db.Almacenes
                           .OrderBy(x => x.Descripcion)
                           .ToListAsync();

                        InputModalDialog.IsModal = false;
                        MasterListView.SelectedItem = itemSelected;
                        MasterListView.ScrollIntoView(itemSelected);
                    }
                }
                //Enable
                MasterListView.IsEnabled = true;
                ListViewCommandBar.IsEnabled = true;
                SearchAutoSuggestBox.IsEnabled = true;

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

        #region Edit
        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            flag = 1;
            var selectedPartNumber = (Almacenes)MasterListView.SelectedItem;

            if (selectedPartNumber != null)
            {
                DescripcionTextBox.Text = selectedPartNumber.Descripcion;

                InputModalDialog.IsModal = true;

                ValidateDescripcion();
                ValidateSaveButton();
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
                var itemToDelete = (Almacenes)MasterListView.SelectedItem;
                if (MasterListView.SelectedItem != null)
                {
                    //MessageDialog
                    var dialog = new MessageDialog("¿Desea eliminar el almacén seleccionada?", "Advertencia");
                    dialog.Commands.Add(new UICommand("Si") { Id = 0 });
                    dialog.Commands.Add(new UICommand("No") { Id = 1 });

                    var result = await dialog.ShowAsync();

                    if (result != null && result.Label == "Si")
                    {
                        db.Almacenes.Remove(itemToDelete);
                        await db.SaveChangesAsync();

                        MasterListView.ItemsSource = await
                            db.Almacenes
                            .OrderBy(x => x.Descripcion)
                           .ToListAsync();

                        MasterListView.SelectedItem = await
                            db.Almacenes
                            .FirstOrDefaultAsync();
                    }
                }

                var query = db.Almacenes.Count();
                if (query <= 0)
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
                MasterListView.ItemsSource = await
                    db.Almacenes
                    .Where(x => x.Descripcion.Contains(SearchAutoSuggestBox.Text))
                    .OrderBy(x => x.Descripcion)
                    .ToListAsync();

                MasterListView.SelectedItem = await
                    db.Almacenes
                    .Where(x => x.Descripcion.Contains(SearchAutoSuggestBox.Text))
                    .OrderBy(x => x.Descripcion)
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
            ValidateDescripcion();

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
        private async void AlmacenesPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            using (var db = new ProjectContext())
            {
                MasterListView.ItemsSource = await
                    db.Almacenes
                   .OrderBy(x => x.Descripcion)
                   .ToListAsync();

                MasterListView.SelectedItem = await 
                    db.Almacenes
                   .OrderBy(x => x.Descripcion)
                   .FirstOrDefaultAsync();

                var query = db.Almacenes.Count();
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
            var clickedItem = (Almacenes)MasterListView.SelectedItem;
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