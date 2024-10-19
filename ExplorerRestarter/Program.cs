using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace ExplorerRestarter
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // Start the tray icon
            var trayIcon = new TrayIcon();
            
            // Start the key watcher
            var keyWatcher = new KeyWatcher();
            
            Application.Run();
        }
    }
}