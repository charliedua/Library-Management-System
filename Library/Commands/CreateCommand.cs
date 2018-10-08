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

        public override Permissions RequiredPermissions => Permissions.Create;

        /// <summary>
        /// Checks if valid.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>
        /// Validity
        /// </returns>
        /// <exception cref="InvalidCommandSyntaxException"></exception>
        public override (bool, string) CheckIfValid(ref string[] text)
        {
            const string Expected = "Expected: \n\t";
            const string Help = "Usage: \n\t";
            string[] splitHelpText = Usage.Split('\n');
            (bool, string) errMsg = (false, Expected + splitHelpText[0]);
            base.CheckIfValid(ref text);
            switch (text.Length)
            {
                case 3:
                    PriorityUpgrade(ref text, new int[] { 1 });
                    if (text[1] != "USER" && text[1] != "ITEM") return errMsg;
                    break;

                case 7:
                    PriorityUpgrade(ref text, new int[] { 1, 4, 5 });
                    (bool, string) errMsg1 = (false, Expected + splitHelpText[1]);
                    if (text[1] != "ACCOUNT") return errMsg1;
                    if (text[4] != "USER") return errMsg1;
                    if (text[5] != "ID") return errMsg1;
                    bool a = int.TryParse(text[6], out int temp);
                    if (!a) return errMsg1;
                    if (!Regex.IsMatch(text[2], @"^[A-Za-z]{0,25}$")) return (false, Expected + "Text between 0 to 25" + splitHelpText[1]);
                    if (text.Length > 25 || text.Length < 3) return (false, Expected + "USERNAME BETWEEN 3 AND 25 CHARACTERS" + splitHelpText[0]);
                    break;

                case 5:
                    PriorityUpgrade(ref text, new int[] { 1, 3 });
                    if (text[1] != "USER") return errMsg;
                    if (text[3] != "-P") return (false, Help + Usage);
                    else
                    {
                        var b = int.TryParse(text[4], out int temp0);
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
            var data = CheckIfValid(ref text);
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
                        ((User)entity).Permissions = Utils.Utility.IntToPerm(int.Parse(text[4]));
                    }
                    break;

                case "ITEM" when valid:
                    entity = new LibraryItem(text[2], controller.GetNextNewEntityID(Entities.Item));
                    break;

                case "ACCOUNT" when valid:
                    entity = controller.FindEntityByID(Entities.User, ID: int.Parse(text[6]));
                    if (entity != null)
                    {
                        if ((entity as User).HasAccount)
                            return "Can't create Account. Already have one.";
                        else
                            (entity as User).CreateAccount(username: text[2], password: text[3]);
                    }
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