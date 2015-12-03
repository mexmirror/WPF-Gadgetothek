using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using ch.hsr.wpf.gadgeothek.domain;
using ch.hsr.wpf.gadgeothek.service;
using ch.hsr.wpf.gadgeothek.ui.viewmodel;

namespace ch.hsr.wpf.gadgeothek.ui
{
    /// <summary>
    /// Interaction logic for ReservationWindow.xaml
    /// </summary>
    public partial class ReservationWindow : Window
    {

        public CustomerViewModel CustomerViewModel;
        public ReservationViewModel ReservationViewModel;
        public Gadget Gadget { get; set; }
        public ReservationWindow(Gadget gadget)
        {
            InitializeComponent();
            Gadget = gadget;
            CustomerViewModel = new CustomerViewModel();
            CustomerGrid.ItemsSource = CustomerViewModel.Collection;
            ReservationViewModel = new ReservationViewModel();
            SetGadgetTextBlocks();
        }

        private void CustomerGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
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
                bool success = ReservationViewModel.Add(new Reservation
                {
                    Customer = c,
                    CustomerId = c.Studentnumber,
                    Finished = false,
                    GadgetId = Gadget.InventoryNumber,
                    Gadget = Gadget,
                    Id = Guid.NewGuid().ToString(),
                    ReservationDate = DateTime.Now,
                    WaitingPosition = 0
                });
                if (success)
                {

                    Close();
                }
                else
                {
                    MessageBox.Show("Reservation unsuccsessful");
                }
            }
            else
            {
                MessageBox.Show("Select a gadget first");
            }
        }
    }
}
