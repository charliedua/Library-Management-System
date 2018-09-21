using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Commands
{
    internal class SaveCommand : Command
    {
        public SaveCommand()
        {
        }

        public override string Name => "SAVE";

        public override string Description => "This Command lets you save stuff to the database";

        public override List<string> Identifiers => new List<string>() { "SAVE" };

        public override string Usage => throw new NotImplementedException();

        public override (bool, string) CheckIfValid(string[] text)
        {
            switch (text.Length)
            {
                case 1:
                    if (!ContainsIdent(text[0])) return (false, );
                    break;

                default:
                    break;
            }
            return (true, "");
        }

        public override string Execute(ref LibraryController controller, string[] text)
        {
            throw new NotImplementedException();
        }
    }
}