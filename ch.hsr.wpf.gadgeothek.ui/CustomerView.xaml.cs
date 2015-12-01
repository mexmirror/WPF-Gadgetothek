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
using ch.hsr.wpf.gadgeothek.domain;
using ch.hsr.wpf.gadgeothek.service;
using ch.hsr.wpf.gadgeothek.ui.Controls;
using ch.hsr.wpf.gadgeothek.ui.viewmodel;

namespace ch.hsr.wpf.gadgeothek.ui
{
    /// <summary>
    /// Interaction logic for CustomerView.xaml
    /// </summary>
    public partial class CustomerView : UserControl
    {
        private LibraryAdminService service = App.Service;
        private List<Customer> customers;
        private EditButton _editButton;
        public ObservableCollection<CustomerViewModel> CustomerViewModels { get; set; } 
 
        public CustomerView()
        {
            InitializeComponent();
            customers = service.GetAllCustomers();
            DataContext = this;
            CustomerViewModels = new ObservableCollection<CustomerViewModel>();
            LoadCustomer();
            _editButton = new EditButton(EditButton);
            SetFormEditebility(false);
        }

        private void LoadCustomer()
        {
            CustomerViewModels.Clear();
            customers.ForEach(customer =>
            {
                CustomerViewModels.Add(new CustomerViewModel {Customer=customer});
            });
        }

        private void CustomerGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = CustomerGrid.SelectedItem;
            if (item != null)
            {
                Customer customer = ((CustomerViewModel) item).Customer;
                NameTextBox.Text = customer.Name;
                EmailTextBox.Text = customer.Email;
                StudentnummerTextBox.Text = customer.Studentnumber;
                _editButton.EditBoxFilled = true;
                _editButton.UpdateEditButton();
            }
        }

        private string SetFormEditebility(bool boolean)
        {
            NameTextBox.IsEnabled = boolean;
            EmailTextBox.IsEnabled = boolean;
            SaveButton.IsEnabled = boolean;
            EditButton.IsEnabled = boolean;
            return null;
        }

        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            _editButton.OnClick(ref sender, ref e, SetFormEditebility);
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            Customer temp = ((CustomerViewModel) CustomerGrid.SelectedItem).Customer;
            Customer customer = CustomerViewModels.First(c => (c.Customer).Studentnumber == temp.Studentnumber).Customer;
            customer.Name = NameTextBox.Text;
            customer.Email = EmailTextBox.Text;
            service.UpdateCustomer(customer);
            SetFormEditebility(false);
            _editButton.Editing = false;
            _editButton.UpdateEditButton();
        }
    }
}
