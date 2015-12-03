using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ch.hsr.wpf.gadgeothek.domain;
using ch.hsr.wpf.gadgeothek.service;
using ch.hsr.wpf.gadgeothek.ui.Controls;
using ch.hsr.wpf.gadgeothek.ui.viewmodel;

namespace ch.hsr.wpf.gadgeothek.ui
{
    /// <summary>
    /// Interaction logic for GadgetView.xaml
    /// </summary>
    public partial class GadgetView : UserControl
    {
        private readonly EditButton _editButton;
        public GadgetViewModel GadgetViewModel;
        public ObservableCollection<Gadget> Gadgets { get; set; } 
        public GadgetView()
        {
            InitializeComponent();
            GadgetViewModel = new GadgetViewModel();
            Gadgets = GadgetViewModel.Collection;
            DataContext = this;
            SetFormEditability(false);
            _editButton = new EditButton(EditButton);
        }
        private void GadgetGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item = GadgetGrid.SelectedItem;
            if (item != null)
            {
                Gadget gadget = ((Gadget) item);
                Manufacturer.Text = gadget.Manufacturer;
                Product.Text = gadget.Name;
                Price.Text = gadget.Price.ToString(CultureInfo.InvariantCulture);
                _editButton.EditBoxFilled = true;
                _editButton.UpdateEditButton();
            }
        }

        private string SetFormEditability(bool boolean)
        {
            Manufacturer.IsEnabled = boolean;
            Product.IsEnabled = boolean;
            Price.IsEnabled = boolean;
            SaveButton.IsEnabled = boolean;
            EditButton.IsEnabled = boolean;
            return null;
        }

        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            _editButton.OnClick(ref sender, ref e, SetFormEditability);
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var collection = GadgetViewModel.Collection;
            Gadget temp = ((Gadget) GadgetGrid.SelectedItem);
            Gadget gadget = 
                collection.First(g => g.InventoryNumber == temp.InventoryNumber);
            gadget.Manufacturer = Manufacturer.Text;
            gadget.Name = Product.Text;
            double newprice;
            double.TryParse(Price.Text, out newprice);
            gadget.Price = newprice;
            GadgetViewModel.Update(gadget);
            SetFormEditability(false);
            _editButton.Editing = false;
            _editButton.UpdateEditButton();
        }
    }
}
