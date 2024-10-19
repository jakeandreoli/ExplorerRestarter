using System;
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
            
            // Start the keyboard hook
            var hook = new GlobalKeyboardHook();
            
            bool control = false,
                enter = false,
                shift = false;
            
            hook.KeyDown += (sender, e) =>
            {
#if DEBUG
                Console.WriteLine("KeyDown: " + e.KeyCode.ToString());
#endif
                
                switch (e.KeyCode)
                {
                    case Keys.RControlKey:
                        control = true;
                        break;
                    case Keys.OemQuestion:
                        shift = true;
                        break;
                    case Keys.Enter:
                        enter = true;
                        break;
                }
                
                TryRestartExplorer(
                    control, 
                    enter, 
                    shift
                );
            };
            
            hook.KeyUp += (sender, e) =>
            {
#if DEBUG
                Console.WriteLine("KeyUp: " + e.KeyCode.ToString());
#endif
                
                switch (e.KeyCode)
                {
                    case Keys.RControlKey:
                        control = false;
                        break;
                    case Keys.Enter:
                        enter = false;
                        break;
                    case Keys.OemQuestion:
                        shift = false;
                        break;
                }
            };
            
            Application.Run();
        }
        
        private static void TryRestartExplorer(bool control, bool enter, bool shift)
        {
            if (!control || !enter || !shift)
            {
                return;
            }
            
            ExplorerHandler.Restart();
        }
    }
}