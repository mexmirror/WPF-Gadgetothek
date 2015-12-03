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
using ch.hsr.wpf.gadgeothek.ui.viewmodel;

namespace ch.hsr.wpf.gadgeothek.ui
{
    /// <summary>
    /// Interaction logic for LoanVIew.xaml
    /// </summary>
    public partial class LoanView : UserControl
    {
        private LibraryAdminService service = App.Service;
        public LoanViewModel LoanViewModel;
        public GadgetViewModel GadgetViewModel;
        public ReservationViewModel ReservationViewModel;
        public LoanView()
        {
            InitializeComponent();
            DataContext = this;
            GadgetViewModel = new GadgetViewModel();
            LoanViewModel = new LoanViewModel();
            ReservationViewModel = new ReservationViewModel();
            GadgetGrid.ItemsSource = GadgetViewModel.Collection;

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
            if (item != null)
            {
                return ((Gadget) item);
            }
            return null;
        }

        private void GadgetGrid_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            var item = GadgetGrid.SelectedItem;
            if (item != null)
            {
                Gadget g = SelectedGadget();
                List<Reservation> reservations = ReservationViewModel.Find(r => r.Gadget.Equals(g));
                List<Loan> loans = LoanViewModel.Find(l => l.Gadget.Equals(g));
                //TODO: Bind in xaml
                LoanGrid.ItemsSource = loans;
                ReservationGrid.ItemsSource = reservations;
            }
            else
            {
                Console.WriteLine(@"Selected item = null");   
            }
        }
    }
}
