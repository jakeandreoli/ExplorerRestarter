using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Windows.Forms;
using ExplorerRestarter.Data;

namespace ExplorerRestarter
{
    public class CommandLoader
    {
        public List<Data.Command> Commands = new List<Data.Command>();

        private List<Data.Instruction> LoadInstructions(XmlElement node)
        {
            var instructions = new List<Data.Instruction>();
            
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode is not XmlElement element)
                {
                    continue;
                }

                switch (childNode.Name)
                {
                    case "Instruction":
                        var name = "Unknown Instruction";

                        if (element.HasAttribute("name"))
                        {
                            name = element.GetAttribute("name");
                        }

                        instructions.Add(new Data.Instruction
                        {
                            Name = name,
                            Command = childNode.InnerText
                        });
                        break;
                }
            }

            return instructions;
        }

        private Data.Command? LoadCommand(XmlNode node)
        {
            var element = node as XmlElement;
            
            if (element == null)
            {
                return null;
            }

            var name = "Unknown Command";
                    
            if (element.HasAttribute("Name"))
            {
                name = element.GetAttribute("Name");
            }

            if (!element.HasChildNodes)
            {
                MessageBox.Show(
                    $"Command \"{name}\" has no instructions. Skipping...",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                
                return null;
            }
            
            List<Data.Instruction> instructions = this.LoadInstructions(element);
            
            return new Data.Command
            {
                Name = name,
                KeyCombination = new HashSet<Keys>(),
                Instructions = instructions
            };
        }
        
        public void LoadCommands()
        {
            this.Commands.Clear();

            string path = Path.Combine(Directory.GetCurrentDirectory(), "Commands.xml");
            
            if (!File.Exists(path))
            {
                return;
            }

            var document = new XmlDocument();
            document.Load(path);

            if (document.DocumentElement == null)
            {
                return;
            }

            foreach (XmlNode node in document.DocumentElement.ChildNodes)
            {
                if (node.NodeType != XmlNodeType.Element)
                {
                    continue;
                }

                switch (node.Name)
                {
                    case "Command":
                        Data.Command? command = this.LoadCommand(node);
                    
                        if (command != null)
                        {
                            this.Commands.Add(command.Value);
                        }
                        break;
                }
            }
        }
    }
}