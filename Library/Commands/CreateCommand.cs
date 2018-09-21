using Library.Commands.Exceptions;
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
         * CREATE USER ?
         * CREATE ?
         * CREATE USER name -P [1-7]
         * CREATE ITEM name
         * CREATE ACCOUNT username password USER ID [INT]
         */
        public override string Usage => "{CREATE [USER | ITEM] [Name] [-P [INTEGER] if User]}\n{CREATE ACCOUNT [USERNAME] [PASSWORD] USER ID [INT]}";

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommand"/> class.
        /// </summary>
        public CreateCommand() : base()
        {
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public override string Name => "CREATE";

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public override string Description => "This is the command used to create entities and accounts for users.";

        public override List<string> Identifiers => new List<string>() { "CREATE", "MAKE", "INTITIALIZE" };

        /// <summary>
        /// Checks if valid.
        /// </summary>
        /// <param name="Text">The text.</param>
        /// <returns>
        /// Validity
        /// </returns>
        /// <exception cref="InvalidCommandSyntaxException"></exception>
        public override (bool, string) CheckIfValid(string[] Text)
        {
            const string Expected = "Expected: \n\t";
            const string Help = "Usage: \n\t";
            string[] splitHelpText = Usage.Split('\n');
            (bool, string) errMsg = (false, Expected + splitHelpText[0]);
            if (Text.Length > 0)
            {
                if (!ContainsIdent(Text[0])) return errMsg;
                if (Text.Last() == "?") return (false, Help + Usage);
            }
            switch (Text.Length)
            {
                case 3:
                    if (!ContainsIdent(Text[0])) return errMsg;
                    if (Text[1] != "USER" && Text[1] != "ITEM") return errMsg;
                    break;

                case 7:
                    (bool, string) errMsg1 = (false, Expected + splitHelpText[1]);
                    if (Text[1] != "ACCOUNT") return errMsg1;
                    if (Text[4] != "USER") return errMsg1;
                    if (Text[5] != "ID") return errMsg1;
                    bool a = int.TryParse(Text[6], out int temp);
                    if (!a) return errMsg1;
                    if (!Regex.IsMatch(Text[2], @"^[A-Za-z]{0,25}$")) return (false, Expected + "Text between 0 to 25" + splitHelpText[1]);
                    if (Text.Length > 25 || Text.Length < 3) return (false, Expected + "USERNAME BETWEEN 3 AND 25 CHARACTERS" + splitHelpText[0]);
                    break;

                case 5:
                    if (Text[1] != "USER") return errMsg;
                    if (Text[3] != "-P") return (false, Help + Usage);
                    else
                    {
                        var b = int.TryParse(Text[4], out int temp0);
                        if (!b) return errMsg;
                        if (temp0 < 0 || temp0 > 7) return (false, Expected + "[INTEGER between 0 AND 7]" + splitHelpText[0]);
                    }
                    break;

                default:
                    return (false, Help + Usage);
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
            switch (text[1])
            {
                case "USER" when valid:
                    entity = new User(text[2], controller.GetNextNewEntityID(Entities.User));
                    if (text.Length > 3 && text[3] == "-P")
                    {
                        ((User)entity).Permissions = User.IntToPerm(int.Parse(text[4]));
                    }
                    break;

                case "ITEM" when valid:
                    entity = new LibraryItem(text[2], controller.GetNextNewEntityID(Entities.Item));
                    break;

                case "ACCOUNT" when valid:
                    entity = controller.FindEntityByID(Entities.User, ID: int.Parse(text[6]));
                    if ((entity as User)._hasAccount)
                        return "Can't create Account. Already have one.";
                    else
                        (entity as User).CreateAccount(username: text[2], password: text[3]);
                    break;

                default:
                    entity = null;
                    break;
            }
            if (entity != null)
            {
                if (entity is User user)
                {
                    controller.Users.Add(user);
                }
                else if (entity is LibraryItem item)
                {
                    controller.Items.Add(item);
                }
                return "Entity Successfully created\n" + entity.Details;
            }
            else
                return "Couldn't create What you wanted.";
        }
    }
}