﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ch.hsr.wpf.gadgeothek.domain;
using ch.hsr.wpf.gadgeothek.service;

namespace ch.hsr.wpf.gadgeothek.ui.viewmodel
{
    public class GadgetViewModel: ViewModel<Gadget>
    {
        private readonly LibraryAdminService _adminService = App.Service;
        public GadgetViewModel()
        {
            Collection = new ObservableCollection<Gadget>();
            LoadCollection();
        }
        protected sealed override void LoadCollection()
        {
            _adminService.GetAllGadgets().ForEach((g => Collection.Add(g)));
        }
        public override bool Update(Gadget element)
        {
            var success = _adminService.UpdateGadget(element);
            if (success)
            {
                LoadCollection();
            }
            return success;
        }

        public override bool Delete(Gadget gadget)
        {
            var success = _adminService.DeleteGadget(gadget);
            if (success)
            {
                Collection.Remove(gadget);
            }
            return success;
        }

        public override bool Add(Gadget gadget)
        {
            var success = _adminService.AddGadget(gadget);
            if (success)
            {
                Collection.Add(gadget);
            }
            return success;
        }
        public void Notify()
        {
            LoadCollection();
        }
    }
}
