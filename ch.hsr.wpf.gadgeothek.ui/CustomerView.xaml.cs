using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
using ch.hsr.wpf.gadgeothek.ui.services;
using ch.hsr.wpf.gadgeothek.ui.viewmodel;

namespace ch.hsr.wpf.gadgeothek.ui
{
    /// <summary>
    /// Interaction logic for CustomerView.xaml
    /// </summary>
    public partial class CustomerView : UserControl
    {
        private readonly EditButton _editButton;
        public CustomerViewModel CustomerViewModel { get; }
        public ReservationViewModel ReservationViewModel { get; }
        public Customer CurrentCustomer { get; set; }
 
        public CustomerView()
        {
            InitializeComponent();
            DataContext = this;
            CustomerViewModel = new CustomerViewModel();
            //CustomerGrid.ItemsSource = CustomerViewModel.Collection;
            ReservationViewModel = new ReservationViewModel();
            //ReservationGrid.ItemsSource = ReservationViewModel.CurrentReservations;
            _editButton = new EditButton(EditButton);
            SetFormEditebility(false);
        }
        private void CustomerGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = CustomerGrid.SelectedItem;
            if (item != null)
            {
                Customer customer = (Customer) item;
                CurrentCustomer = customer;
                //NameTextBox.Text = customer.Name;
                //EmailTextBox.Text = customer.Email;
                //StudentIdTextBox.Text = customer.Studentnumber
                var reservations = ReservationViewModel.Find(r => r.Customer.Equals(customer));
                ReservationViewModel.UpdateCurrentReservations(reservations);
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
            Customer temp = (Customer) CustomerGrid.SelectedItem;
            var collection = CustomerViewModel.Collection;
            Customer customer = collection.First(c => c.Studentnumber == temp.Studentnumber);
            customer.Name = NameTextBox.Text;
            customer.Email = EmailTextBox.Text;
            CustomerViewModel.Update(customer);
            SetFormEditebility(false);
            _editButton.Editing = false;
            _editButton.UpdateEditButton();
        }

        private void AddCustomerButton_OnClick(object sender, RoutedEventArgs e)
        {
            NewCustomerWindow window = new NewCustomerWindow();
            window.ShowDialog();
        }

        private void RemoveCustomerButton_OnClick(object sender, RoutedEventArgs e)
        {
            Customer customer = (Customer) CustomerGrid.SelectedItem;
            var success = CustomerViewModel.Delete(customer);
            if (!success)
            {
                MessageBox.Show("Customer cannot be deleted");
            }
        }

        private void RemoveReservationButton_OnClick(object sender, RoutedEventArgs e)
        {
            Reservation reservation = (Reservation)ReservationGrid.SelectedItem;
            if (reservation != null)
            {
                var success = ReservationViewModel.Delete(reservation);
                if (!success)
                {
                    MessageBox.Show("Reservation cannot be deleted");
                }
            }
        }
    }
}
