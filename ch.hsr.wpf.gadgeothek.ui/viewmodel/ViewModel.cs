using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ch.hsr.wpf.gadgeothek.service;
using ch.hsr.wpf.gadgeothek.websocket;

namespace ch.hsr.wpf.gadgeothek.ui.viewmodel
{
    public abstract class ViewModel<T>: INotifyCollectionChanged
    {
        public ObservableCollection<T> Collection { get; set; }
        public ViewModel()
        {
            Collection = new ObservableCollection<T>();
            LoadCollection();
        }
        protected abstract void LoadCollection();

        public abstract bool Update(T element);
        public abstract void OnNotificateionRecieve(object sender, WebSocketClientNotificationEventArgs e);

        public virtual bool Add(T element)
        {
            Collection.Add(element);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add));
            return true;
        }

        public virtual bool Delete(T element)
        {
            Collection.Remove(element);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove));
            return true;
        }
        public virtual List<T> Find(Predicate<T> predicate)
        {
            return Collection.Where((T t) => predicate(t)).ToList();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
