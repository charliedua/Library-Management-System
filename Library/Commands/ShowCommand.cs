using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Commands
{
    internal class ShowCommand : Command
    {
        /*
         * SHOW ITEM ID [INT]
         * SHOW USER ID [INT]
         * SHOW ALL ITEMS
         * SHOW ALL USERS
         */

        public ShowCommand()
        {
        }

        public override string Name => "SHOW";

        public override string Usage => "{[SHOW | DISPLAY | VIEW] [ITEM | USER] ID [INT]}\n{SHOW ALL [ITEMS | USERS]}";

        public override string Description => "Command for seeing stuff in the library";

        public override List<string> Identifiers => new List<string>() { "SHOW", "DISPLAY", "VIEW" };

        public override (bool, string) CheckIfValid(ref string[] text)
        {
            (bool, string) data = base.CheckIfValid(ref text);
            if (!data.Item1)
            {
                return data;
            }
            switch (text.Length)
            {
                case 4:
                    PriorityUpgrade(ref text, new int[] { 1, 2 });
                    if (text[1] != "ITEM" && text[1] != "USER") return (false, Usage);
                    if (text[2] != "ID") return (false, Usage);
                    if (!int.TryParse(text[3], out int temp)) return (false, Usage);
                    break;

                case 3:
                    PriorityUpgrade(ref text, new int[] { 1, 2 });
                    if (text[1] != "ALL") return (false, Usage);
                    if (text[2] != "ITEMS" && text[2] != "USERS") return (false, Usage);
                    break;

                default:
                    return (false, Usage);
            }
            return (true, "");
        }

        public override string Execute(ref LibraryController controller, string[] text)
        {
            (bool, string) data = CheckIfValid(ref text);
            bool valid = data.Item1;
            if (!valid)
            {
                return data.Item2;
            }
            StringBuilder builder = new StringBuilder();
            switch (text.Length)
            {
                case 4:
                    switch (text[1])
                    {
                        case "ITEM":
                            builder.Append(controller.FindEntityByID(Entities.Item, int.Parse(text[3])).Details);
                            builder.AppendLine();
                            return builder.ToString();

                        case "USER":
                            builder.Append(controller.FindEntityByID(Entities.User, int.Parse(text[3])).Details);
                            builder.AppendLine();
                            return builder.ToString();

                        default:
                            return "";
                    }

                case 3:
                    switch (text[2])
                    {
                        case "ITEMS":
                            foreach (var item in controller.Items)
                            {
                                builder.Append(item.Details);
                                builder.AppendLine();
                            }
                            return builder.ToString();

                        case "USERS":
                            foreach (var user in controller.Users)
                            {
                                builder.Append(user.Details);
                                builder.AppendLine();
                            }
                            return builder.ToString();

                        default:
                            return "";
                    }

                default:
                    return "";
            }
        }
    }
}