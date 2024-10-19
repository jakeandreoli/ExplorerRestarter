using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace ExplorerRestarter
{
    public class TrayIcon
    {
        public TrayIcon()
        {
            NotifyIcon icon;
            
            using (Stream iconStream = Assembly
                       .GetExecutingAssembly()
                       .GetManifestResourceStream("ExplorerRestarter.Resources.icon.ico")
            )
            {
                if (iconStream == null)
                {
                    MessageBox.Show("Icon resource not found. Exiting...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    return;
                }

                icon = new NotifyIcon
                {
                    Icon = new Icon(iconStream),
                    Visible = true
                };
            }

            var menu = new ContextMenu();

            // Restart Explorer
            var restartExplorerItem = new MenuItem("Restart Explorer");
            restartExplorerItem.Click += (sender, args) => ExplorerHandler.Restart();
            
            menu.MenuItems.Add(restartExplorerItem);

            // About
            var aboutItem = new MenuItem("About");
            aboutItem.Click += (sender, args) => Process.Start("https://github.com/jakeandreoli/ExplorerRestarter");
            
            menu.MenuItems.Add(aboutItem);
            
            // Separator
            menu.MenuItems.Add(new MenuItem("-"));
            
            // Exit
            var exitItem = new MenuItem("Exit");
            exitItem.Click += (sender, args) => Application.Exit();
            
            menu.MenuItems.Add(exitItem);

            // Set the context menu
            icon.ContextMenu = menu;
        }
    }
}