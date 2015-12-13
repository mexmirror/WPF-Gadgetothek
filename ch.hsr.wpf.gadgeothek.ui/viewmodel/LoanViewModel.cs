using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using ch.hsr.wpf.gadgeothek.domain;
using ch.hsr.wpf.gadgeothek.service;
using ch.hsr.wpf.gadgeothek.websocket;

namespace ch.hsr.wpf.gadgeothek.ui.viewmodel
{
    public class LoanViewModel: ViewModel<Loan>
    {
        private readonly LibraryAdminService _adminService = App.Service;
        private readonly WebSocketClient _webSocketClient = App.WebSocketClient;

        public LoanViewModel()
        {
            Collection = new ObservableCollection<Loan>();
            _webSocketClient.NotificationReceived += OnNotificateionRecieve;
            LoadCollection();
        }

        protected override sealed void LoadCollection()
        {
            Collection.Clear();
            _adminService.GetAllLoans().ForEach(l => Collection.Add(l));
        }

        public override bool Update(Loan element)
        {
            var success = _adminService.UpdateLoan(element);
            if (success)
            {
                LoadCollection();
            }
            return success;
        }

        public override void OnNotificateionRecieve(object sender, WebSocketClientNotificationEventArgs e)
        {
            if (e.Notification.Target == typeof (Loan).Name.ToLower())
            {
                Loan loan = e.Notification.DataAs<Loan>();
                /*var temp = Collection.FirstOrDefault(l => l.Id == loan.Id);
                if (temp != null)
                {
                    LoadCollection();
                }*/
                LoadCollection();
            }
        }

        public override bool Delete(Loan element)
        {
            var success =_adminService.DeleteLoan(element);
            if (success)
            {
                Collection.Remove(element);
            }
            return success;
        }

        public override bool Add(Loan element)
        {
            var success = _adminService.AddLoan(element);
            if (success)
            {
                Collection.Add(element);
            }
            return success;
        }

        public Loan FindFirstLoan(Predicate<Loan> predicate)
        {
            return Collection.FirstOrDefault(l => predicate(l));
        }
        public void Notify()
        {
            LoadCollection();
        }
    }
}
