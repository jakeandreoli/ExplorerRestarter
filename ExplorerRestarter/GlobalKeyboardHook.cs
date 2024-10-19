using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ExplorerRestarter
{
    public class GlobalKeyboardHook
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int id, LowLevelKeyboardProc callback, IntPtr hInstance, uint threadId);
        
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr id, int nCode, int wParam, IntPtr lParam);

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private IntPtr hookId;
        private LowLevelKeyboardProc proc;

        public event EventHandler<KeyEventArgs> KeyDown;
        public event EventHandler<KeyEventArgs> KeyUp;

        public GlobalKeyboardHook()
        {
            this.proc = this.HookCallback;
            this.hookId = this.SetHook(proc);
        }

        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (var curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                if (curModule == null)
                {
                    return IntPtr.Zero;
                }
                
                return SetWindowsHookEx(
                    WH_KEYBOARD_LL, 
                    proc, 
                    GetModuleHandle(curModule.ModuleName), 
                    0
                );
            }
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                var key = (Keys)vkCode;
                var kea = new KeyEventArgs(key);
                
                if (wParam == (IntPtr)WM_KEYDOWN)
                {
                    this.KeyDown?.Invoke(this, kea);
                }
                else if (wParam == (IntPtr)WM_KEYUP)
                {
                    this.KeyUp?.Invoke(this, kea);
                }

                if (kea.Handled)
                {
                    return (IntPtr)1;
                }
            }
            
            return CallNextHookEx(
                this.hookId, 
                nCode, 
                wParam.ToInt32(), 
                lParam
            );
        }
    }
}