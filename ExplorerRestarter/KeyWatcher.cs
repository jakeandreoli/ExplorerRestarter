using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ExplorerRestarter.Utilities;

namespace ExplorerRestarter
{
    public class KeyWatcher
    {
        private Dictionary<HashSet<Keys>, Action> _keyCommands = new Dictionary<HashSet<Keys>, Action>(HashSetComparer<Keys>.Default);
        private HashSet<Keys> _heldKeys = new HashSet<Keys>();
        
        public KeyWatcher()
        {
            this.InstantiateDefaultKeyBindings();
            
            var hook = new GlobalKeyboardHook();
            
            hook.KeyDown += (sender, e) =>
            {
                this._heldKeys.Add(e.KeyCode);
                this.CheckAndInvokeCommand();
            };
            
            hook.KeyUp += (sender, e) => this._heldKeys.Remove(e.KeyCode);
        }
        
        private void CheckAndInvokeCommand()
        {
#if DEBUG
            Console.WriteLine("Held keys: " + string.Join(", ", this._heldKeys));
#endif
            
            // Check for matching key combination
            foreach (HashSet<Keys> keyCombo in this._keyCommands.Keys.Where(keyCombo => keyCombo.IsSubsetOf(this._heldKeys)))
            {
                this._keyCommands[keyCombo].Invoke();
                break;
            }
        }
        
        private void InstantiateDefaultKeyBindings()
        {
            this._keyCommands = new Dictionary<HashSet<Keys>, Action>(HashSetComparer<Keys>.Default)
            {
                {
                    // Restart Explorer - Right Control + Enter + ?
                    new HashSet<Keys>
                    {
                        Keys.RControlKey, 
                        Keys.Return, 
                        Keys.OemQuestion
                    },
                    ExplorerHandler.Restart
                },
                {
                    // Reset mouse cursor - Right Control + Enter + ;
                    new HashSet<Keys>
                    {
                        Keys.RControlKey, 
                        Keys.Return, 
                        Keys.OemSemicolon
                    },
                    ExplorerHandler.ResetMouseCursor
                }
            };
        } 
    }
}