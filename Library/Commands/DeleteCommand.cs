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
         * DELETE USER ID [INT]
         * DELETE ITEM ID [INT]
         * DELETE ITEM ID [INT] FROM USER ID [INT]
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
        private const string HELP_TEXT = "DELETE [USER | ITEM] ID [INTEGER]";

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

        /// <summary>
        /// Checks if valid.
        /// </summary>
        /// <param name="Text">The text.</param>
        /// <returns></returns>
        /// <exception cref="InvalidCommandSyntaxException"></exception>
        public override (bool, string) CheckIfValid(string[] Text)
        {
            (bool, string) errrmsg = (false, "Expected: \n\t" + HELP_TEXT);
            if (Text.Length > 0)
            {
                if (!ContainsIdent(Text[0])) return errrmsg;
                if (Text.Last() == "?") return (false, "Usage: \n\t" + HELP_TEXT);
            }
            switch (Text.Length)
            {
                case 4:
                    if (Text[1] != "USER" && Text[1] != "ITEM") return errrmsg;
                    if (Text[2] != "ID") return errrmsg;
                    if (!int.TryParse(Text[3], out int a)) return errrmsg;
                    break;

                case 8:
                    if (Text[0] != "DELETE") return errrmsg;
                    if (Text[1] != "ITEM") return errrmsg;
                    if (!int.TryParse(Text[3], out int b)) return errrmsg;
                    if (Text[4] != "FROM") return errrmsg;
                    if (Text[5] != "USER") return errrmsg;
                    if (Text[6] != "ID") return errrmsg;
                    if (!int.TryParse(Text[7], out int c)) return errrmsg;
                    break;

                default:
                    return errrmsg;
            }
            return (true, "");
        }

        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public override string Execute(ref LibraryController controller, string[] text)
        {
            Entity entity = null;
            var data = CheckIfValid(text);
            bool valid = data.Item1;
            if (!valid)
            {
                return data.Item2;
            }
            Database database = new Database();
            if (valid)
            {
                bool found;
                switch (text[1])
                {
                    case "ITEM":
                        entity = controller.DeleteEntityByID(Entities.User, int.Parse(text[3]));// REMOVES IT FROM THE CONTROLLER
                        found = entity != null;
                        break;

                    case "USER":
                        entity = controller.DeleteEntityByID(Entities.Item, int.Parse(text[3]));
                        found = entity != null;
                        break;

                    default:
                        found = false;
                        break;
                }
                if (found)
                {
                    switch (entity)
                    {
                        case User _:
                            database.Delete(User.TABLE_NAME, entity.ID); // REMOVES IT FROMT THE DATABASE
                            break;

                        case LibraryItem _:
                            database.Delete(LibraryItem.TABLE_NAME, entity.ID); // REMOVES IT FROMT THE DATABASE
                            break;

                        default:
                            break;
                    }
                }
            }
            return entity != null ? entity.Details : "Can't find the entity";
        }
    }
}