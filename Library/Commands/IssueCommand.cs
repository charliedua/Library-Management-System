using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Commands
{
    internal class IssueCommand : Command
    {
        /*
         * ISSUE ITEM ID [INT]
         */

        public IssueCommand()
        {
        }

        public override string Name => "ISSUE";

        public override string Usage => "[ISSUE | TAKE | BORROW] ITEM ID [INT]";

        public override string Description => "Command to issue items";

        public override Permissions RequiredPermissions => Permissions.Read;

        public override List<string> Identifiers => new List<string>() { "ISSUE", "TAKE", "BORROW" };

        public override (bool, string) CheckIfValid(ref string[] text)
        {
            var data = base.CheckIfValid(ref text);
            if (!data.Item1)
            {
                return data;
            }
            switch (text.Length)
            {
                case 4:
                    PriorityUpgrade(ref text, new int[] { 1, 2 });
                    if (text[1] != "ITEM") return (false, Usage);
                    if (text[2] != "ID") return (false, Usage);
                    if (!int.TryParse(text[3], out int temp)) return (false, Usage);
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
            LibraryItem item = (LibraryItem)controller.FindEntityByID(Entities.Item, int.Parse(text[3]));
            if (item != null && item.Available)
            {
                controller.CurrentUser.Issue(item);
                return $"Item Issued Successfully.\nDetails: \n{item.Details}";
            }
            return "Can't issue item, ID not found OR unavailable";
        }
    }
}