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
    public class ReservationViewModel : ViewModel<Reservation>
    {
        private readonly LibraryAdminService _adminService = App.Service;
        private readonly WebSocketClient _webSocketClient = App.WebSocketClient;
        public ObservableCollection<Reservation> CurrentReservations { get; set; } 
        public ReservationViewModel()
        {
            Collection = new ObservableCollection<Reservation>();
            CurrentReservations = new ObservableCollection<Reservation>();
            _webSocketClient.NotificationReceived += OnNotificateionRecieve;
            LoadCollection();
        }

        protected override sealed void LoadCollection()
        {
            Collection.Clear();
            _adminService.GetAllReservations().ForEach((r) => Collection.Add(r));
        }

        public void UpdateCurrentReservations(List<Reservation> reservations)
        {
            CurrentReservations.Clear();
            reservations.ForEach(r => CurrentReservations.Add(r));
        }
        public override bool Update(Reservation element)
        {
            var success = _adminService.UpdateReservation(element);
            if (success)
            {
                LoadCollection();
            }
            return success;
        }

        public override bool Add(Reservation element)
        {
            var success = _adminService.AddReservation(element);
            if (success)
            {
                Collection.Add(element);
            }
            return success;
        }

        public override bool Delete(Reservation element)
        {
            var success = _adminService.DeleteReservation(element);
            if (success)
            {
                Collection.Remove(element);
            }
            return success;
        }

        public override void OnNotificateionRecieve(object sender, WebSocketClientNotificationEventArgs e)
        {
            if (e.Notification.Target == typeof (Reservation).Name.ToLower())
            {
                Reservation reservation = e.Notification.DataAs<Reservation>();
                var temp = Collection.FirstOrDefault(r => r.Id.Equals(reservation.Id));
                if (temp != null)
                {
                    LoadCollection();
                }
            }
        }
    }
}
