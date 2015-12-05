using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ch.hsr.wpf.gadgeothek.domain;
using ch.hsr.wpf.gadgeothek.service;
using ch.hsr.wpf.gadgeothek.websocket;

namespace ch.hsr.wpf.gadgeothek.ui.viewmodel
{
    public class CustomerViewModel: ViewModel<Customer>
    {
        private LibraryAdminService _adminService = App.Service;
        private WebSocketClient _webSocketClient = App.WebSocketClient;
        public CustomerViewModel()
        {
            Collection = new ObservableCollection<Customer>();
            _webSocketClient.NotificationReceived += OnNotificateionRecieve;
            LoadCollection();
        }
        protected sealed override void LoadCollection()
        {
            Collection.Clear();
            _adminService.GetAllCustomers().ForEach(c => Collection.Add(c));
        }

        public override bool Update(Customer element)
        {
            var success = _adminService.UpdateCustomer(element);
            if (success)
            {
                LoadCollection();
            }
            return success;
        }

        public override void OnNotificateionRecieve(object sender, WebSocketClientNotificationEventArgs e)
        {
            if (e.Notification.Target == typeof (Customer).Name.ToLower())
            {
                Customer customer = e.Notification.DataAs<Customer>();
                var temp = Collection.FirstOrDefault(c => c.Studentnumber == customer.Studentnumber);
                if (temp != null)
                {
                    LoadCollection();
                }
            }
        }

        public override bool Add(Customer element)
        {
            var success = _adminService.AddCustomer(element);
            if (success)
            {
                Collection.Add(element);
            }
            return success;
        }

        public override bool Delete(Customer element)
        {
            var success = _adminService.DeleteCustomer(element);
            if (success)
            {
                Collection.Remove(element);
            }
            return success;
        }

    }
}
