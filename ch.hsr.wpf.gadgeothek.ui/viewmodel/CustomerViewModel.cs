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
    public class CustomerViewModel: ViewModel<Customer>
    {
        private LibraryAdminService _adminService = App.Service;
        public CustomerViewModel()
        {
            Collection = new ObservableCollection<Customer>();
            LoadCollection();
        }
        protected sealed override void LoadCollection()
        {
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
