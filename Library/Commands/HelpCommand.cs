using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Commands.Exceptions;

namespace Library.Commands
{
    public class HelpCommand : Command
    {
        /// <summary>
        /// The data
        /// </summary>
        private readonly string data;

        /// <summary>
        /// The help text
        /// </summary>
        public override string Usage => "[HELP | ?]";

        /// <summary>
        /// Initializes a new instance of the <see cref="HelpCommand"/> class.
        /// </summary>
        /// <param name="commands">The commands.</param>
        public HelpCommand(List<Command> commands)
        {
            foreach (Command command in commands)
            {
                data += command.Name + " : " + command.Description + ",\n";
            }
            data += Name + " : " + Description + '\n';
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public override string Description => "this command provides you with this help menu";

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public override string Name => "HELP";

        /// <summary>
        /// Gets the identifiers.
        /// </summary>
        /// <value>
        /// The identifiers.
        /// </value>
        public override List<string> Identifiers => new List<string>() { "HELP", "?" };

        /// <summary>
        /// Checks if valid.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public override (bool, string) CheckIfValid(ref string[] text)
        {
            base.CheckIfValid(ref text);
            return (true, "");
        }

        /// <summary>
        /// Executes the specified controller.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public override string Execute(ref LibraryController controller, string[] text)
        {
            var data = CheckIfValid(ref text);
            bool valid = data.Item1;
            if (!valid)
            {
                return data.Item2;
            }
            return this.data;
        }
    }
}