using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Commands
{
    public class ClearCommand : Command
    {
        private readonly Action Clear;

        public ClearCommand(Action Clear)
        {
            this.Clear = Clear;
        }

        public override string Name => "Clear";

        public override string Usage => "[Cls | Clear]";

        public override string Description => "Use this commmand to clear the screen";

        public override List<string> Identifiers => new List<string>() { "CLS", "CLEAR" };

        public override string Execute(ref LibraryController controller, string[] text)
        {
            var data = CheckIfValid(ref text);
            bool valid = data.Item1;
            if (!valid)
            {
                return data.Item2;
            }
            Clear();
            return "";
        }
    }
}