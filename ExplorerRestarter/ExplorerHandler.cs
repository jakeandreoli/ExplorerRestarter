using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using ExplorerRestarter.Data;

namespace ExplorerRestarter
{
    public static class ExplorerHandler
    {
        /**
         * Reset the mouse cursor to the center of the screen.
         */
        public static void ResetMouseCursor()
        {
#if DEBUG
            Console.WriteLine("Resetting mouse cursor...");
#endif

            Cursor.Position = new System.Drawing.Point(
                Screen.PrimaryScreen.Bounds.Width / 2,
                Screen.PrimaryScreen.Bounds.Height / 2
            );
        }
        
        /**
         * Restart Windows Explorer.
         */
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
        
        public static void RunInstruction(Instruction instruction)
        {
            try
            {
                using var process = new Process();
                
                string command = instruction.Command;
                command = command.Replace("\n", " && ");
                command = command.Replace("{localappdata}", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
                command = command.Replace("{appdata}", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                        
#if DEBUG
                Console.WriteLine($"Running command: {command}");
#endif
                        
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c " + command,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                process.Start();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
        
        public static void RunCommand(List<Instruction> instructions)
        {
            foreach (Instruction i in instructions)
            {
                RunInstruction(i);
            }
        }
    }
}