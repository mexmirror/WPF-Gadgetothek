using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ch.hsr.wpf.gadgeothek.service;
using ch.hsr.wpf.gadgeothek.websocket;

namespace ch.hsr.wpf.gadgeothek.ui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string _url = "http://mge3.dev.ifs.hsr.ch/";
        public static readonly LibraryAdminService Service = new LibraryAdminService(_url);
        public static readonly WebSocketClient WebSocketClient = new WebSocketClient(_url);
        public static string Url => _url;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            WebSocketClient.ListenAsync();
        }
    }
}
