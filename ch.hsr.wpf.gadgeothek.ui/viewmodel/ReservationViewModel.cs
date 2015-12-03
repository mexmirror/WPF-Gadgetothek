using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ch.hsr.wpf.gadgeothek.domain;
using ch.hsr.wpf.gadgeothek.service;

namespace ch.hsr.wpf.gadgeothek.ui.viewmodel
{
    public class ReservationViewModel : ViewModel<Reservation>
    {
        private readonly LibraryAdminService _adminService = App.Service;

        public ReservationViewModel()
        {
            Collection = new ObservableCollection<Reservation>();
            LoadCollection();
        }

        protected override sealed void LoadCollection()
        {
            _adminService.GetAllReservations().ForEach((r) => Collection.Add(r));
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

        public void Notify()
        {
            LoadCollection();
        }
    }
}
