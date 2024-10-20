using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace ExplorerRestarter
{
    public class TrayIcon
    {
        private readonly NotifyIcon _icon;
        private readonly CommandLoader _commandLoader;

        public TrayIcon()
        {
            this._commandLoader = new CommandLoader();
            
            using (Stream iconStream = Assembly
                       .GetExecutingAssembly()
                       .GetManifestResourceStream(
                           "ExplorerRestarter.Resources.icon" 
                                + (Utilities.SystemConfiguration.DarkTheme() ? "-light" : "")
                                + ".ico"
                       )
                  )
            {
                if (iconStream == null)
                {
                    MessageBox.Show(
                        "Icon resource not found. Exiting...",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );

                    Application.Exit();
                    return;
                }

                this._icon = new NotifyIcon
                {
                    Icon = new Icon(iconStream),
                    Visible = true
                };
            }
            
            this._icon.Text = "Explorer Restarter";
            this._icon.DoubleClick += (sender, args) => ExplorerHandler.Restart();
            
            this.CreateContextMenu();
            
            this._commandLoader.FileChanged += (sender, args) => this.CreateContextMenu();
        }

        private void CreateContextMenu()
        {
            var menu = new ContextMenu();

            // Restart Explorer
            var restartExplorerItem = new MenuItem("Restart Explorer");
            restartExplorerItem.Click += (sender, args) => ExplorerHandler.Restart();

            menu.MenuItems.Add(restartExplorerItem);
            
            if (this._commandLoader.Commands.Count > 0)
            {
                foreach (Data.Command command in this._commandLoader.Commands)
                {
                    var commandItem = new MenuItem(command.Name);
                    commandItem.Click += (sender, args) => ExplorerHandler.RunCommand(command.Instructions);
                    
                    foreach (Data.Instruction instruction in command.Instructions)
                    {
                        if (!instruction.Standalone)
                        {
                            continue;
                        }
                        
                        var instructionItem = new MenuItem(instruction.Name);
                        instructionItem.Click += (sender, args) => ExplorerHandler.RunInstruction(instruction);
                        
                        commandItem.MenuItems.Add(instructionItem);
                    }
                    
                    menu.MenuItems.Add(commandItem);
                }
            }
            
            // Separator
            menu.MenuItems.Add(new MenuItem("-"));
            
            // Reload Commands
            var reloadCommandsItem = new MenuItem("Reload Commands");
            reloadCommandsItem.Click += (sender, args) =>
            {
                this._commandLoader.LoadCommands();
                this._icon.ContextMenu = null;
                this.CreateContextMenu();
            };
            
            menu.MenuItems.Add(reloadCommandsItem);
            
            // Open Folder containing this executable
            var openFolderItem = new MenuItem("Open Folder");
            openFolderItem.Click += (sender, args) => Process.Start(Directory.GetCurrentDirectory());
            
            menu.MenuItems.Add(openFolderItem);
            
            // Separator
            menu.MenuItems.Add(new MenuItem("-"));
            
            // About
            var aboutItem = new MenuItem("About");
            aboutItem.Click += (sender, args) => Process.Start("https://github.com/jakeandreoli/ExplorerRestarter");

            menu.MenuItems.Add(aboutItem);

            // Exit
            var exitItem = new MenuItem("Exit");
            exitItem.Click += (sender, args) => Application.Exit();

            menu.MenuItems.Add(exitItem);

            // Set the context menu
            this._icon.ContextMenu = menu;
        }
    }
}