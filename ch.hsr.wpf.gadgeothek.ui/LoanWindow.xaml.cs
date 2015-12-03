using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using ch.hsr.wpf.gadgeothek.domain;
using ch.hsr.wpf.gadgeothek.ui.viewmodel;

namespace ch.hsr.wpf.gadgeothek.ui
{
    /// <summary>
    /// Interaction logic for LoanWindow.xaml
    /// </summary>
    public partial class LoanWindow : Window
    {
        public LoanViewModel LoanViewModel;
        public CustomerViewModel CustomerViewModel;
        public Gadget Gadget;
        public LoanWindow(Gadget gadget)
        {
            InitializeComponent();
            LoanViewModel = new LoanViewModel();
            CustomerViewModel = new CustomerViewModel();
            CustomerGrid.ItemsSource = CustomerViewModel.Collection;
            Gadget = gadget;
            SetGadgetTextBlocks();
        }

        private void CustomerGrid_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            var item = CustomerGrid.SelectedItem;
            if (item != null)
            {
                Customer c = (Customer) item;
                NameBlock.Text = c.Name;
                MailBlock.Text = c.Email;
                IdBlock.Text = c.Studentnumber;
            }
        }
        private void SetGadgetTextBlocks()
        {
            ProductBlock.Text = Gadget.Name;
            ManufacturerBlock.Text = Gadget.Manufacturer;
            PriceBlock.Text = Gadget.Price.ToString(CultureInfo.CurrentCulture);
        }
        private Customer SelectedCustomer()
        {
            return (Customer) CustomerGrid.SelectedItem;
        }
        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            Customer c = SelectedCustomer();
            if (c != null)
            {
                bool success = LoanViewModel.Add(new Loan
                {
                    Customer = c,
                    CustomerId = c.Studentnumber,
                    Gadget = Gadget,
                    GadgetId = Gadget.InventoryNumber,
                    Id = Guid.NewGuid().ToString(),
                    PickupDate = DateTime.Now,
                    ReturnDate = DateTime.Now.AddDays(7),
                });
                if (success)
                {

                    Close();
                }
                else
                {
                    MessageBox.Show("Loan unsuccsessful");
                }
            }
            else
            {
                MessageBox.Show("Select a gadget first");
            }
        }
    }
}
