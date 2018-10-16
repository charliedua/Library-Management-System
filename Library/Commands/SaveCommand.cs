using Library.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Commands
{
    internal class SaveCommand : Command
    {
        /*
         * SAVE USER ALL
         * SAVE ITEM ALL
         * SAVE ALL
        */

        public SaveCommand()
        {
        }

        public override string Name => "SAVE";
        public override Permissions RequiredPermissions => Permissions.Create;
        public override string Description => "This Command lets you save stuff to the database";

        public override List<string> Identifiers => new List<string>() { "SAVE" };

        public override string Usage => "{SAVE [ITEM | USER] ALL} \n {SAVE ALL}";

        public override (bool, string) CheckIfValid(ref string[] text)
        {
            (bool, string) a = base.CheckIfValid(ref text);
            if (!a.Item1)
            {
                return a;
            }
            switch (text.Length)
            {
                case 2:
                    PriorityUpgrade(ref text, new int[] { 1 });
                    if (text[1] != "ALL") return (false, Usage);
                    break;

                case 3:
                    PriorityUpgrade(ref text, new int[] { 1, 2 });
                    if (text[1] != "USER" && text[1] != "ITEM") return (false, Usage);
                    if (text[2] != "ALL") return (false, Usage);
                    break;

                default:
                    return (false, Usage);
            }
            return (true, "");
        }

        public override string Execute(ref LibraryController controller, string[] text)
        {
            var data = CheckIfValid(ref text);
            bool valid = data.Item1;
            if (!valid)
            {
                return data.Item2;
            }
            switch (text.Length)
            {
                case 2 when valid:
                    Utility.SaveAllUsers(controller.Users);
                    Utility.SaveAllItems(controller.Items);
                    break;

                case 3 when valid:
                    if (text[1] == "USER")
                    {
                        Utility.SaveAllUsers(controller.Users);
                    }
                    else if (text[1] == "ITEM")
                    {
                        Utility.SaveAllItems(controller.Items);
                    }
                    break;

                default:
                    break;
            }
            return $"Saved Entities to the Database.";
        }
    }
}