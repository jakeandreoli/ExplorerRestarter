using System.Collections.Generic;
using System.Windows.Forms;

namespace ExplorerRestarter.Data
{
    public struct Command
    {
        public string Name { get; set; }
        
        public HashSet<Keys> KeyCombination { get; set; }

        public List<Instruction> Instructions { get; set; }
    }
}