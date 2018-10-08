using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Commands
{
    internal class SudoCommand : Command
    {
        private CommandProcessor Processor { get; }

        private readonly Func<string, string> AskFunc;

        public SudoCommand(CommandProcessor processor, Func<string, string> askfunc)
        {
            Processor = processor;
            AskFunc = askfunc;
        }

        public override string Name => "Sudo";

        public override string Usage => "[SUDO [COMMAND]]";

        public override string Description => "Gives you super user powers to execute commmands";

        public override List<string> Identifiers => new List<string>() { "SUDO" };

        public override string Execute(ref LibraryController controller, string[] text)
        {
            var data = CheckIfValid(ref text);
            bool valid = data.Item1;
            if (!valid)
            {
                return data.Item2;
            }
            if (controller.CurrentUser.Account.VerifyPassword(controller.CurrentUser.Account.Username, AskFunc($"Enter Password [{controller.CurrentUser.Account.Username}] : ")))
            {
                List<Permissions> OldPerms = controller.CurrentUser.Permissions;
                List<Permissions> newPermissions = Utils.Utility.IntToPerm(7);
                controller.CurrentUser.Permissions = newPermissions;
                string[] newTextArray = new string[text.Length - 1];
                Array.Copy(text, 1, newTextArray, 0, newTextArray.Length);
                string result = Processor.Invoke(newTextArray.Aggregate((s, n) => s + " " + n));
                controller.CurrentUser.Permissions = OldPerms;
                return result;
            }
            else
            {
                return "Incorrect Password";
            }
        }
    }
}