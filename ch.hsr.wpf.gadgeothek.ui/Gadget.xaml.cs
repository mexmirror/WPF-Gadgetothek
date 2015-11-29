using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ch.hsr.wpf.gadgeothek.service;
using ch.hsr.wpf.gadgeothek.ui.viewmodel;

namespace ch.hsr.wpf.gadgeothek.ui
{
    /// <summary>
    /// Interaction logic for Gadget.xaml
    /// </summary>
    public partial class Gadget : UserControl
    {
        private LibraryAdminService service;
        private readonly static string url = "http://mge1.dev.ifs.hsr.ch/";
        private List<domain.Gadget> gadgets;
        private bool isEditBoxFilled = false;
        public ObservableCollection<GadgetViewModel> GadgetViewModels { get; set; }
        public Gadget()
        {
            InitializeComponent();
            service = new LibraryAdminService(url);
            gadgets = service.GetAllGadgets();
            GadgetGrid.ItemsSource = gadgets;
            GadgetGrid.IsReadOnly = true;
            GadgetViewModels = new ObservableCollection<GadgetViewModel>();
            SetAllEditInputs(false);
            LoadModel();
        }

        public void LoadModel()
        {
            GadgetViewModels.Clear();
            gadgets.ForEach(gadget =>
            {
                GadgetViewModels.Add(new GadgetViewModel() {Gadget = gadget});
            });
        }

        private void GadgetGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GadgetGrid.SelectedItem!= null)
            {
                domain.Gadget gadget = (domain.Gadget) GadgetGrid.SelectedItem;
                Manufacturer.Text = gadget.Manufacturer;
                Product.Text = gadget.Name;
                Price.Text = gadget.Price.ToString();
                EditButton.IsEnabled = true;
            }
        }

        private void SetAllEditInputs(bool boolean)
        {
            Manufacturer.IsEnabled = boolean;
            Product.IsEnabled = boolean;
            Price.IsEnabled = boolean;
            SaveButton.IsEnabled = boolean;
            EditButton.IsEnabled = boolean;
        }

        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            SetAllEditInputs(true);
            SetEditButtonState(true);
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            domain.Gadget g = ((domain.Gadget) GadgetGrid.SelectedItem);
            g.Manufacturer = Manufacturer.Text;
            g.Name = Product.Text;
            double newprice;
            double.TryParse(Price.Text, out newprice);
            g.Price = newprice;
            //TODO: Update condition
            service.UpdateGadget(g);
            SetAllEditInputs(false);
            SetEditButtonState(false);
        }

        private void SetEditButtonState(bool state)
        {
            if (!state)
            {
                EditButton.IsEnabled = true;
                EditButton.Content = "Edit";
            }
            else
            {
                EditButton.IsEnabled = true;
                EditButton.Content = "Discard";
            }
        }
    }
}
