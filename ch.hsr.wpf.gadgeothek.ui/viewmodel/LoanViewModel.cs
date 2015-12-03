using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using ch.hsr.wpf.gadgeothek.domain;
using ch.hsr.wpf.gadgeothek.service;

namespace ch.hsr.wpf.gadgeothek.ui.viewmodel
{
    public class LoanViewModel: ViewModel<Loan>
    {
        private readonly LibraryAdminService _adminService = App.Service;

        public LoanViewModel()
        {
            Collection = new ObservableCollection<Loan>();
            LoadCollection();
        }

        protected override sealed void LoadCollection()
        {
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
        public void Notify()
        {
            LoadCollection();
        }
    }
}
