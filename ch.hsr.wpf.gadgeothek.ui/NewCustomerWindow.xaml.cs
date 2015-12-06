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
    public partial class NewCustomerWindow : Window
    {
        private readonly CustomerViewModel _customerViewModel;
        public NewCustomerWindow()
        {
            InitializeComponent();
            _customerViewModel = new CustomerViewModel();
        }

        private void AddCustomerButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(NameBox.Text.Equals("") &&
                  MailBox.Text.Equals("") &&
                  NumberBox.Text.Equals("") &&
                  PasswordBox.Password.Equals("")))
            {
                if (NumberBox.Text.All(char.IsDigit))
                {
                    Customer customer = new Customer(
                        NameBox.Text,
                        PasswordBox.Password,
                        MailBox.Text,
                        NumberBox.Text
                        );
                    var success = _customerViewModel.Add(customer);
                    if (success)
                    {
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Cannot add customer");
                    }
                }
                else
                {
                    MessageBox.Show("Studentnumber must be a number");
                }
            }
            else
            {
                MessageBox.Show("Please enter all information");
            }
        }
    }
}
