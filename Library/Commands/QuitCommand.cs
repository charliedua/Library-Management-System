using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Commands
{
    internal class QuitCommand : Command
    {
        public Action Quit;

        public QuitCommand(Action quitFunction)
        {
            Quit = quitFunction;
        }

        public override string Name => "Quit";

        public override string Description => "This command is for Quitting the application";

        public override List<string> Identifiers => new List<string>() { "QUIT", "EXIT", "BYE" };

        public override string Usage => "[ QUIT | BYE | EXIT ]";

        public override string Execute(ref LibraryController controller, string[] text)
        {
            (bool, string) data = CheckIfValid(ref text);
            if (!data.Item1)
            {
                return data.Item2;
            }
            try
            {
                Quit();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return "";
        }
    }
}