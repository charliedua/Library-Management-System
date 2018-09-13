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
            //if (Text.Length > 1)
            //{
            //    if (Text[0] != "CREATE") throw new InvalidCommandSyntaxException("CREATE");
            //    if (Text[1] != "USER" && Text[1] != "ITEM") throw new InvalidCommandSyntaxException("'USER' or 'ITEM'");
            //    if (Text.Length == 3)
            //    {
            //        return true;
            //    }
            //    else if (Text.Length == 5)
            //    {
            //        if (Text[3] != "-P") throw new InvalidCommandSyntaxException(expected: "-P");
            //        else
            //        {
            //            var a = int.TryParse(Text[4], out int temp);
            //            if (!a)
            //                throw new InvalidCommandSyntaxException("[INTEGER]");
            //            if (temp < 0 || temp > 7)
            //                throw new InvalidCommandSyntaxException("[INTEGER between 0 AND 7]");
            //        }
            //    }
            //    else
            //        throw new InvalidCommandSyntaxException("CREATE [USER | ITEM] [Name] [-P [INTEGER] if User]");
            //}
            //else
            //    throw new InvalidCommandSyntaxException("CREATE [USER | ITEM] [Name] [-P [INTEGER] if User]");
            //return true;
            switch (Text.Length)
            {
                case 3:
                    if (Text[0] != "CREATE") throw new InvalidCommandSyntaxException("CREATE");
                    if (Text[1] != "USER" && Text[1] != "ITEM") throw new InvalidCommandSyntaxException("'USER' or 'ITEM'");
                    return true;

                case 5:
                    if (Text[0] != "CREATE") throw new InvalidCommandSyntaxException("CREATE");
                    if (Text[1] != "USER" && Text[1] != "ITEM") throw new InvalidCommandSyntaxException("'USER' or 'ITEM'");
                    if (Text[3] != "-P") throw new InvalidCommandSyntaxException(expected: "-P");
                    else
                    {
                        var a = int.TryParse(Text[4], out int temp);
                        if (!a) throw new InvalidCommandSyntaxException("[INTEGER]");
                        if (temp < 0 || temp > 7) throw new InvalidCommandSyntaxException("[INTEGER between 0 AND 7]");
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

            switch (text[1])
            {
                case "USER" when valid:
                    entity = new User(text[2])
                    {
                        Permissions = User.IntToPerm(int.Parse(text[4]))
                    };
                    break;

                case "ITEM" when valid:
                    entity = new LibraryItem(text[2]);
                    break;

                default:
                    entity = null;
                    break;
            }

            if (entity is User user)
            {
            }

            return "Entity Successfully created\n" + entity.Details;
        }
    }
}