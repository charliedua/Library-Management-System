using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Commands
{
    public class CreateCommand : Command
    {
        public CreateCommand() : base()
        {
        }

        /// <summary>
        /// Checks if valid.
        /// </summary>
        /// <returns>
        /// Validity
        /// </returns>
        /// <exception cref="InvalidCommandSyntaxException">CREATE</exception>
        public override bool CheckIfValid(string[] Text)
        {
            switch (Text.Length)
            {
                case 4:
                    if (Text[0] != "CREATE") throw new InvalidCommandSyntaxException("CREATE");
                    if (Text[1] != "USER" || Text[1] != "ITEM") throw new InvalidCommandSyntaxException("'USER' or 'ITEM'");
                    return true;

                case 5:
                    if (Text[0] != "CREATE") throw new InvalidCommandSyntaxException("CREATE");
                    if (Text[1] != "USER" && Text[1] != "ITEM") throw new InvalidCommandSyntaxException("'USER' or 'ITEM'");
                    if (Text[3] != "-P")
                    {
                        throw new InvalidCommandSyntaxException(expected: "-P");
                    }
                    else
                    {
                        var a = int.TryParse(Text[4], out int temp);
                        if (!a)
                            throw new InvalidCommandSyntaxException("[INTEGER]");
                        if (temp < 0 || temp > 7)
                        {
                            throw new InvalidCommandSyntaxException("[INTEGER between 0 AND 7]");
                        }
                    }
                    return true;

                default:
                    throw new InvalidCommandSyntaxException("CREATE [USER | ITEM] [Name] [-P [INTEGER] if User]");
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
            catch (InvalidCommandSyntaxException exception)
            {
                return exception.Message;
            }
            if (text[1] == "USER" && valid)
            {
                entity = new User(text[2]);
            }
            else if (text[1] == "ITEM" && valid)
            {
                entity = new LibraryItem(text[2]);
            }
            else
            {
                entity = null;
            }
            if (entity is User user)
            {
                user.Permissions = User.IntToPerm(int.Parse(text[4]));
            }
            return "Entity Successfully created\n" + entity.Details;
        }
    }
}