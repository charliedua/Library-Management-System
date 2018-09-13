using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Commands
{
    /*
     * DELETE USER ID [INT]
     * DELETE ITEM ID [INT]
     * DELETE ITEM ID [INT] FROM USER ID [INT]
     */

    /// <summary>
    /// The Delete command.
    /// </summary>
    /// <seealso cref="Library.Commands.Command" />
    public class DeleteCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCommand"/> class.
        /// </summary>
        public DeleteCommand()
        {
        }

        /// <summary>
        /// Checks if valid.
        /// </summary>
        /// <param name="Text">The text.</param>
        /// <returns></returns>
        /// <exception cref="InvalidCommandSyntaxException"></exception>
        public override bool CheckIfValid(string[] Text)
        {
            switch (Text.Length)
            {
                case 3:
                    if (Text[0] != "DELETE") throw new InvalidCommandSyntaxException("[DELETE]");
                    if (Text[1] != "USER" && Text[1] != "ITEM") throw new InvalidCommandSyntaxException("'USER' or 'ITEM'");
                    if (Text[2] != "ID") throw new InvalidCommandSyntaxException("[ID]");
                    if (int.TryParse(Text[3], out int a)) throw new InvalidCommandSyntaxException("[INTEGER]");
                    return true;

                case 8:
                    if (Text[0] != "DELETE") throw new InvalidCommandSyntaxException("[DELETE]");
                    if (Text[1] != "ITEM") throw new InvalidCommandSyntaxException("'USER' or 'ITEM'");
                    if (int.TryParse(Text[3], out int b)) throw new InvalidCommandSyntaxException("[INTEGER]");
                    if (Text[4] != "FROM") throw new InvalidCommandSyntaxException("[FROM]");
                    if (Text[5] != "USER") throw new InvalidCommandSyntaxException("[USER]");
                    if (Text[6] != "ID") throw new InvalidCommandSyntaxException("[ID]");
                    if (int.TryParse(Text[7], out int c)) throw new InvalidCommandSyntaxException("[INTEGER]");
                    return true;

                default:
                    throw new InvalidCommandSyntaxException("DELETE [USER | ITEM] ID [INTEGER]");
            }
        }

        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public override string Execute(ref Entity entity, string[] text)
        {
            bool valid;
            try
            {
                valid = CheckIfValid(text);
            }
            catch (InvalidCommandSyntaxException ex)
            {
                return ex.Message;
            }
            Database database = new Database();
            if (valid)
            {
                database.Delete(entity.TABLE_NAME, entity.ID);
            }
            return entity.Details;
        }
    }
}