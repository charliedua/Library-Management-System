using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Commands
{
    public class ReturnCommand : Command
    {
        /*
         * RETURN ITEM ID [INT]
         * RETURN ITEMS                      (Shows menu)
         *
         */

        public ReturnCommand()
        {
        }

        public override string Name => throw new NotImplementedException();

        public override string Usage => throw new NotImplementedException();

        public override string Description => throw new NotImplementedException();

        public override List<string> Identifiers => throw new NotImplementedException();

        public override string Execute(ref LibraryController controller, string[] text)
        {
            throw new NotImplementedException();
        }
    }
}