using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ch.hsr.wpf.gadgeothek.ui.viewmodel;

namespace ch.hsr.wpf.gadgeothek.ui.services
{
    public class FilterService<T> : INotifyCollectionChanged
    {
        public Predicate<T> Predicate { get; set; }
        public ObservableCollection<T> Collection { get; set; }
        public ObservableCollection<T> Filtered { get; set; } 

        public FilterService(ObservableCollection<T> collection)
        {
            Collection = collection;
            Filtered = new ObservableCollection<T>();
            Collection.CollectionChanged += CollectionOnCollectionChanged;
        }

        private void CollectionOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            FilterCollection();
        }

        public void SetFilter(Predicate<T> predicate)
        {
            Predicate = predicate;
        }

        public void FilterCollection()
        {
            if (Predicate != null)
            {
                Filtered.Clear();
                List<T> list = Collection.Where(t => Predicate(t)).ToList();
                list.ForEach(t => Filtered.Add(t));
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace));
            }
        }
        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
