using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Library.Commands
{
    public class CreateCommand : Command
    {
        /*
         * CREATE USER name
         * CREATE USER name -P [1-7]
         * CREATE ITEM name
         * CREATE ACCOUNT username password USER ID [INT]
         */

        public CreateCommand() : base()
        {
        }

        /// <summary>
        /// Checks if valid.
        /// </summary>
        /// <param name="Text">The text.</param>
        /// <returns>
        /// Validity
        /// </returns>
        /// <exception cref="InvalidCommandSyntaxException"></exception>
        public override bool CheckIfValid(string[] Text)
        {
            switch (Text.Length)
            {
                case 3:
                    if (Text[0] != "CREATE") throw new InvalidCommandSyntaxException("[CREATE]");
                    if (Text[1] != "USER" && Text[1] != "ITEM") throw new InvalidCommandSyntaxException("['USER' or 'ITEM']");
                    return true;

                case 7:
                    if (Text[0] != "CREATE") throw new InvalidCommandSyntaxException("[CREATE]");
                    if (Text[1] != "ACCOUNT") throw new InvalidCommandSyntaxException("[ACCOUNT]");
                    if (Text[4] != "USER") throw new InvalidCommandSyntaxException("[USER]");
                    if (Text[5] != "ID") throw new InvalidCommandSyntaxException("[ID]");
                    bool a = int.TryParse(Text[6], out int temp);
                    if (!a) throw new InvalidCommandSyntaxException("[INTEGER]");
                    if (!Regex.IsMatch(Text[2], @"^[A-Za-z]{0,25}$")) throw new InvalidCommandSyntaxException("[TEXT only, no whitespace or illegal characters]");
                    if (Text.Length > 25 || Text.Length < 3) throw new InvalidCommandSyntaxException("[USERNAME BETWEEN 3 AND 25 CHARACTERS]");
                    return true;

                case 5:
                    if (Text[0] != "CREATE") throw new InvalidCommandSyntaxException("[CREATE]");
                    if (Text[1] != "USER") throw new InvalidCommandSyntaxException("[USER]");
                    if (Text[3] != "-P") throw new InvalidCommandSyntaxException(expected: "-P");
                    else
                    {
                        var a = int.TryParse(Text[4], out int temp0);
                        if (!a) throw new InvalidCommandSyntaxException("[INTEGER]");
                        if (temp0 < 0 || temp0 > 7) throw new InvalidCommandSyntaxException("[INTEGER between 0 AND 7]");
                    }
                    return true;

                default:
                    throw new InvalidCommandSyntaxException("{CREATE [USER | ITEM] [Name] [-P [INTEGER] if User]} | {CREATE ACCOUNT [USERNAME] [PASSWORD]}");
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
                    entity = new User(text[2]);
                    if (text.Length > 3 && text[3] == "-P")
                    {
                        ((User)entity).Permissions = User.IntToPerm(int.Parse(text[4]));
                    }
                    break;

                case "ITEM" when valid:
                    entity = new LibraryItem(text[2]);
                    break;

                case "ACCOUNT" when valid:
                    if (entity == null) return "CREATE a User | Item first";
                    if (entity is User a) a.CreateAccount(text[2], text[3]);
                    else throw new InvalidCommandParamatersException(typeof(User), entity.GetType());
                    break;

                default:
                    entity = null;
                    break;
            }
            return "Entity Successfully created\n" + entity.Details;
        }
    }
}