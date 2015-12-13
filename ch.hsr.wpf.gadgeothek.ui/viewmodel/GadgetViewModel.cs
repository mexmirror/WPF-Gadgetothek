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
    public class GadgetViewModel: ViewModel<Gadget>
    {
        private readonly LibraryAdminService _adminService = App.Service;
        private readonly WebSocketClient _webSocketClient = App.WebSocketClient;
        public GadgetViewModel()
        {
            Collection = new ObservableCollection<Gadget>();
            _webSocketClient.NotificationReceived += OnNotificateionRecieve;
            LoadCollection();
        }
        protected sealed override void LoadCollection()
        {
            Collection.Clear();
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

        public override void OnNotificateionRecieve(object sender, WebSocketClientNotificationEventArgs e)
        {
            if (e.Notification.Target == typeof (Gadget).Name.ToLower())
            {
                Gadget gadget = e.Notification.DataAs<Gadget>();
                switch (e.Notification.Type)
                {
                    case WebSocketClientNotificationTypeEnum.Add:
                        Collection.Add(gadget);
                        break;
                    case WebSocketClientNotificationTypeEnum.Update:
                        var temp = Collection.First(g => g.InventoryNumber == gadget.InventoryNumber);
                        temp.Update(gadget);
                        break;
                    case WebSocketClientNotificationTypeEnum.Delete:
                        Collection.Remove(gadget);
                        break;
                    default:
                        LoadCollection();
                        break;
                }
            }
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
    }
}
