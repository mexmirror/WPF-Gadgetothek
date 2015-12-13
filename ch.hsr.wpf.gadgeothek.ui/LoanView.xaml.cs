using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using ch.hsr.wpf.gadgeothek.ui.services;
using ch.hsr.wpf.gadgeothek.ui.viewmodel;

namespace ch.hsr.wpf.gadgeothek.ui
{
    /// <summary>
    /// Interaction logic for LoanVIew.xaml
    /// </summary>
    public partial class LoanView : UserControl
    {
        public LoanViewModel LoanViewModel { get; }
        public GadgetViewModel GadgetViewModel { get; }
        public ReservationViewModel ReservationViewModel { get; }
        public FilterService<Reservation> ReservationFilterService { get; } 
        public Loan CurrentLoan { get; set; }
        public LoanView()
        {
            InitializeComponent();
            DataContext = this;
            GadgetViewModel = new GadgetViewModel();
            LoanViewModel = new LoanViewModel();
            ReservationViewModel = new ReservationViewModel();
            ReservationFilterService = new FilterService<Reservation>(ReservationViewModel.Collection);
        }
        private void LoanButton_OnClick(object sender, RoutedEventArgs e)
        {
            Gadget item = SelectedGadget();
            if (item != null)
            {
                LoanWindow window = new LoanWindow(item);
                window.ShowDialog();
            }
            else
            {
                MessageBox.Show("Choose a Gadget to loan");
            }
        }

        private void ReserveButton_OnClick(object sender, RoutedEventArgs e)
        {
            Gadget item = SelectedGadget();
            if (item != null)
            {
                ReservationWindow window = new ReservationWindow(item);
                window.ShowDialog();
            }
            else
            {
                MessageBox.Show("Choose a Gadget to reserve");
            }
        }

        private Gadget SelectedGadget()
        {
            var item = GadgetGrid.SelectedItem;
            return (Gadget) item;
        }

        private void GadgetGrid_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            var item = GadgetGrid.SelectedItem;
            if (item != null)
            {
                Gadget g = SelectedGadget();
                //var reservations = ReservationViewModel.Find(r => r.Gadget.Equals(g));
                //ReservationViewModel.UpdateCurrentReservations(reservations);
                ReservationFilterService.SetFilter(r => r.Gadget.Equals(g));
                ReservationFilterService.FilterCollection();
                Loan loan = LoanViewModel.FindFirstLoan(l => l.Gadget.Equals(g));
                //TODO: Bind in xaml & better sort
                if (loan != null)
                {
                    //NameTextBlock.Text = loan.Customer.Name;
                    ////"dd.MM.yyyy"
                    //PickupTextBlock.Text = loan.PickupDate.ToString();
                    //ReturnTextBlock.Text = loan.ReturnDate.ToString();
                    CurrentLoan = loan;
                }
                else
                {
                    //NameTextBlock.Text = "";
                    //PickupTextBlock.Text = "";
                    //ReturnTextBlock.Text = "";
                    CurrentLoan = null;
                }
            }
        }

        private void AddGadgetButton_OnClick(object sender, RoutedEventArgs e)
        {
            NewGadgetWindow window = new NewGadgetWindow();
            window.ShowDialog();
        }

        private void RemoveGadgetButton_OnClick(object sender, RoutedEventArgs e)
        {
            Gadget gadget = SelectedGadget();
            var success = GadgetViewModel.Delete(gadget);
            if (!success)
            {
                MessageBox.Show("Gadget could not be removed");
            }
        }

        private void ReturnGadgetButton_OnClick(object sender, RoutedEventArgs e)
        {
            Gadget gadget = SelectedGadget();
            Loan loan = LoanViewModel.FindFirstLoan(l => l.Gadget.Equals(gadget));
            loan.ReturnDate = DateTime.Now;
            var success = LoanViewModel.Update(loan);
            if (success)
            {
                MessageBox.Show("Loan returned");
            }
            else
            {
                MessageBox.Show("Loan cannot be returned");
            }

        }

        private void RemoveReservationButton_OnClick(object sender, RoutedEventArgs e)
        {
            Reservation reservation = (Reservation)ReservationGrid.SelectedItem;
            if (reservation != null)
            {
                var success = ReservationViewModel.Delete(reservation);
                if (!success) MessageBox.Show("Reservation cannot be removed");
            }
            else
            {
                MessageBox.Show("Please select a reservation to remove");
            }
        }
    }
}
