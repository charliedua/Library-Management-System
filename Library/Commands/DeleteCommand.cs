using Library.Commands.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Commands
{
    /// <summary>
    /// The Delete command.
    /// </summary>
    /// <seealso cref="Library.Commands.Command" />
    public class DeleteCommand : Command
    {
        /*
         * private DELETE USER ID[INT]
         * private DELETE ITEM ID[IN
         * DELETE ITEM ID[INT] FROM USER ID[INT]
         */

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCommand"/> class.
        /// </summary>
        public DeleteCommand()
        {
        }

        /// <summary>
        /// The help text
        /// </summary>
        public override string Usage => "DELETE [USER | ITEM] ID [INTEGER]";

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public override string Name => "DELETE";

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public override string Description => "This is the command to delete entities.";

        public override List<string> Identifiers => new List<string>() { "DELETE", "REMOVE" };

        public override Permissions RequiredPermissions => Permissions.Delete;

        /// <summary>
        /// Checks if valid.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        /// <exception cref="InvalidCommandSyntaxException"></exception>
        public override (bool, string) CheckIfValid(ref string[] text)
        {
            (bool, string) errrmsg = (false, "Expected: \n\t" + Usage);
            (bool, string) data = base.CheckIfValid(ref text);
            if (!data.Item1)
            {
                return data;
            }
            switch (text.Length)
            {
                case 4:
                    PriorityUpgrade(ref text, new int[] { 1, 2 });
                    if (text[1] != "USER" && text[1] != "ITEM") return errrmsg;
                    if (text[2] != "ID") return errrmsg;
                    if (!int.TryParse(text[3], out int a)) return errrmsg;
                    break;

                case 8:
                    PriorityUpgrade(ref text, new int[] { 0, 1, 4, 5, 6 });
                    if (text[0] != "DELETE") return errrmsg;
                    if (text[1] != "ITEM") return errrmsg;
                    if (!int.TryParse(text[3], out int b)) return errrmsg;
                    if (text[4] != "FROM") return errrmsg;
                    if (text[5] != "USER") return errrmsg;
                    if (text[6] != "ID") return errrmsg;
                    if (!int.TryParse(text[7], out int c)) return errrmsg;
                    break;

                default:
                    return errrmsg;
            }
            return (true, "");
        }

        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="text">The text.</param>
        /// <returns>the Result to be printed after the command has been executed.</returns>
        public override string Execute(ref LibraryController controller, string[] text)
        {
            Entity entity = null;
            var data = CheckIfValid(ref text);
            bool valid = data.Item1;
            if (!valid)
            {
                return data.Item2;
            }
            if (valid)
            {
                switch (text[1])
                {
                    case "ITEM":
                        entity = controller.DeleteEntityByID(Entities.Item, int.Parse(text[3]));// REMOVES IT FROM THE CONTROLLER
                        break;

                    case "USER":
                        entity = controller.DeleteEntityByID(Entities.User, int.Parse(text[3]));
                        break;

                    default:
                        break;
                }
            }
            return entity != null ? entity.Details : "Can't find the entity";
        }
    }
}