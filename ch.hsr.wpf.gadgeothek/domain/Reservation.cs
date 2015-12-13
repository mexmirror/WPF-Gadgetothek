using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ch.hsr.wpf.gadgeothek.Annotations;

namespace ch.hsr.wpf.gadgeothek.domain
{
    public class Reservation: INotifyPropertyChanged
    {

        public string Id { get; set; }
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }
        public string GadgetId { get; set; }
        public Gadget Gadget { get; set; }
        public DateTime? ReservationDate { get; set; }
        public bool Finished { get; set; }
        public int WaitingPosition { get; set; }
        public bool IsReady { get; set; }

        public override int GetHashCode()
        {
            return Id?.GetHashCode() ?? 31;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            var other = obj as Reservation;
            if (other == null)
                return false;
            if (Id == null)
                return other.Id == null;
            return Id == other.Id;
        }

        public Reservation Update(Reservation other)
        {
            Id = other.Id;
            Gadget = other.Gadget;
            GadgetId = other.GadgetId;
            Customer = other.Customer;
            CustomerId = other.CustomerId;
            Finished = other.Finished;
            IsReady = other.IsReady;
            ReservationDate = other.ReservationDate;
            WaitingPosition = other.WaitingPosition;
            return this;
        }
        public override string ToString()
        {
            return $"Reservation {Id}: {Gadget} reserved on {ReservationDate:yyyy-MM-dd}, current waiting position: {this.WaitingPosition}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
