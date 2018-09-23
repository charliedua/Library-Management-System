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
         * SAVE USER ID [INT]
         * SAVE ITEM ALL
         * SAVE ITEM ID [INT]
         * SAVE ALL
         * SAVE ?
        */

        public SaveCommand()
        {
        }

        public override string Name => "SAVE";

        public override string Description => "This Command lets you save stuff to the database";

        public override List<string> Identifiers => new List<string>() { "SAVE" };

        public override string Usage => throw new NotImplementedException();

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

                case 4:
                    PriorityUpgrade(ref text, new int[] { 1, 2 });
                    if (text[1] != "USER" && text[1] != "ITEM") return (false, Usage);
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
            if (valid)
            {
                return data.Item2;
            }
            int numOfEntities = 0;
            switch (text.Length)
            {
                case 2 when valid:
                    numOfEntities += controller.SaveTheUnsaved(Entities.User);
                    numOfEntities += controller.SaveTheUnsaved(Entities.Item);
                    break;

                case 3 when valid:
                    if (text[1] == "USER")
                    {
                        numOfEntities += controller.SaveTheUnsaved(Entities.User);
                    }
                    else if (text[1] == "ITEM")
                    {
                        numOfEntities += controller.SaveTheUnsaved(Entities.Item);
                    }
                    break;

                case 4 when valid:
                    List<ISavable> savables = new List<ISavable>();
                    if (text[1] == "USER")
                    {
                        savables = controller.Users.ToList<ISavable>();
                    }
                    else if (text[1] == "ITEM")
                    {
                        savables = controller.Items.ToList<ISavable>();
                    }
                    foreach (ISavable savable in savables.FindAll(x => !x.Saved && (x as Entity).ID == int.Parse(text[3])))
                    {
                        savable.Save();
                        numOfEntities++;
                    }
                    break;

                default:
                    break;
            }
            return $"Saved {numOfEntities} Entities to the Database.";
        }
    }
}