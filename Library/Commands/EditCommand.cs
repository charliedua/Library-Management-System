using System;
using System.Collections.Generic;

namespace Library.Commands
{
    internal class EditCommand : Command
    {
        /*
         * EDIT ITEM ID [INT]
         * EDIT USER ID [INT]
         * EDIT USER [INT]
         */

        private readonly Action<Entity, Entities> EditFunc;

        // TODO: ask for implemetation
        public EditCommand(Action<Entity, Entities> editFunc)
        {
            EditFunc = editFunc;
        }

        /*
         * public void EditUser(ref UserAccount account)
         * public void EditEntity(ref Entity, Entities type)
         */
        public override string Name => "Edit";

        public override (bool, string) CheckIfValid(ref string[] text)
        {
            var data = base.CheckIfValid(ref text);
            if (!data.Item1)
            {
                return data;
            }
            int temp;
            switch (text.Length)
            {
                case 4:
                    PriorityUpgrade(ref text, new int[] { 1, 2 });
                    if (text[1] != "USER" && text[1] != "ITEM") return (false, Usage);
                    if (text[2] != "ID") return (false, Usage);
                    if (!int.TryParse(text[3], out temp)) return (false, Usage);
                    break;

                case 3:
                    PriorityUpgrade(ref text, new int[] { 1 });
                    if (text[1] != "USER" && text[1] != "ITEM") return (false, Usage);
                    if (!int.TryParse(text[2], out temp)) return (false, Usage);
                    break;

                default:
                    return (false, Usage);
            }
            return (true, "");
        }

        public override Permissions RequiredPermissions => Permissions.Create;

        public override string Usage => "[EDIT | CHANGE | UPDATE] [USER | ITEM] ID [INT] \n[EDIT | CHANGE | UPDATE] [USER | ITEM] [INT]";

        public override string Description => "This command is used to change stuff in the library.";

        public override List<string> Identifiers => new List<string>() { "EDIT", "CHANGE", "UPDATE" };

        public override string Execute(ref LibraryController controller, string[] text)
        {
            var data = CheckIfValid(ref text);
            bool valid = data.Item1;
            if (!valid)
            {
                return data.Item2;
            }
            string id = null;
            switch (text.Length)
            {
                case 4:
                    if (text[2] != "ID")
                    {
                        return Usage;
                    }
                    PriorityUpgrade(ref text, new int[] { 1, 2 });
                    id = text[3];
                    break;

                case 3:
                    PriorityUpgrade(ref text, new int[] { 1 });
                    id = text[2];
                    break;

                default:
                    return Usage;
            }
            Entity EntityToEdit;
            if (text[1] == "USER")
            {
                EntityToEdit = controller.FindEntityByID(Entities.User, int.Parse(id));
                EditFunc(EntityToEdit, Entities.User);
                (EntityToEdit as ISavable).Changed = true;
            }
            else if (text[1] == "ITEM")
            {
                EntityToEdit = controller.FindEntityByID(Entities.Item, int.Parse(id));

                EditFunc(EntityToEdit, Entities.User);
            }
            else
            {
                EntityToEdit = null;
                return Usage;
            }
            return "New Details: \n" + EntityToEdit.Details;
        }
    }
}