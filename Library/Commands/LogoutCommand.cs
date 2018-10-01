using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Commands
{
    public class LogoutCommand : Command
    {
        public LogoutCommand()
        {
        }

        public override string Name => "Logout";

        public override string Usage => "LOGOUT";

        public override string Description => "Logs you out.";

        public override List<string> Identifiers => new List<string>() { "LOGOUT" };

        public override string Execute(ref LibraryController controller, string[] text)
        {
            var data = CheckIfValid(ref text);
            bool valid = data.Item1;
            if (!valid)
            {
                return data.Item2;
            }
            controller.Logout();
            return "Successfully logged you out.";
        }
    }
}