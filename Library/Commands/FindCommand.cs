using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Commands
{
    public class FindCommand : Command
    {
        /*
         * FIND ITEM [INT]
         * FIND USER [INT]
         * FIND ITEM [STRING]
         * FIND USER [STRING]
         */

        public FindCommand()
        {
        }

        public override string Name => "Find";

        public override string Usage => "FIND [ITEM | USER] [INT | STRING]";

        public override string Description => "To Find stuff in the library";

        public override List<string> Identifiers => new List<string>() { "FIND", "LOCATE" };

        public override (bool, string) CheckIfValid(ref string[] text)
        {
            var data = base.CheckIfValid(ref text);
            if (!data.Item1)
            {
                return data;
            }
            if (text.Length != 3)
            {
                return (false, Usage);
            }
            PriorityUpgrade(ref text, new int[] { 1 });
            if (text[1] != "USER" && text[1] != "ITEM") return (false, Usage);
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
            if (text[1] == "USER")
            {
                if (int.TryParse(text[2], out int id))
                {
                    return controller.FindEntityByID(Entities.User, id).Details;
                }
            }
            else
            {
                if (int.TryParse(text[2], out int id))
                {
                    return controller.FindEntityByID(Entities.Item, id).Details;
                }
            }
            StringBuilder builder = new StringBuilder();
            foreach (Entity entity in controller.FindEntityByName(text[2]))
            {
                builder.AppendLine(entity.Details);
            }
            return builder.ToString();
        }
    }
}