using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using ch.hsr.wpf.gadgeothek.domain;
using ch.hsr.wpf.gadgeothek.ui.viewmodel;

namespace ch.hsr.wpf.gadgeothek.ui
{
    /// <summary>
    /// Interaction logic for NewGadgetWindow.xaml
    /// </summary>
    public partial class NewGadgetWindow : Window
    {
        private readonly GadgetViewModel _gadgetViewModel;
        public NewGadgetWindow()
        {
            InitializeComponent();
            _gadgetViewModel = new GadgetViewModel();
            
        }

        private void AddGadgetButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(NameBox.Text.Equals("") &&
                  ManufacturerBox.Text.Equals("") &&
                  PriceBox.Text.Equals("") &&
                  ConditionComboBox.SelectedItem == null))
            {
                Gadget g = new Gadget(NameBox.Text);
                g.Manufacturer = ManufacturerBox.Text;
                double price;
                Double.TryParse(PriceBox.Text, out price);
                g.Price = price;
                g.Condition = (domain.Condition) ConditionComboBox.SelectionBoxItem;
                var success = _gadgetViewModel.Add(g);
                if (success)
                {
                    Close();
                }
                else
                {
                    MessageBox.Show("Gadget cannot be saved");
                }
            }
            else
            {
                MessageBox.Show("Please provide all required data");
            }
        }
    }
}
