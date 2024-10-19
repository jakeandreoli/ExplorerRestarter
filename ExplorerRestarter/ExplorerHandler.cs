using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace ExplorerRestarter
{
    public class ExplorerHandler
    {
        public static void Restart()
        {
#if DEBUG
            Console.WriteLine("Restarting Explorer...");
#endif
            try
            {
                foreach (Process proc in Process.GetProcessesByName("explorer"))
                {
                    proc.Kill();
                }

                // Pause for a moment
                System.Threading.Thread.Sleep(1000);
                
                // Restart Explorer
                Process.Start("explorer.exe", "/select,Shell:::{3080F90D-D7AD-11D9-BD98-0000947B0257}"); 
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}