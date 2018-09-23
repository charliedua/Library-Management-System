using System.Collections.Generic;
using System.Linq;

namespace Library.Commands
{
    public abstract class Command
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the usage.
        /// </summary>
        /// <value>
        /// The usage.
        /// </value>
        public abstract string Usage { get; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public abstract string Description { get; }

        /// <summary>
        /// Gets the identifiers.
        /// </summary>
        /// <value>
        /// The identifiers.
        /// </value>
        public abstract List<string> Identifiers { get; }

        /// <summary>
        /// Determines whether this instance contains ident.
        /// </summary>
        /// <returns></returns>
        public bool ContainsIdent(string ident)
        {
            return Identifiers.Contains(ident);
        }

        /// <summary>
        /// Checks if valid.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public virtual (bool, string) CheckIfValid(ref string[] text)
        {
            PriorityUpgrade(ref text, new int[] { 0 });
            if (!(text.Length > 0)) return (false, Usage);
            if (!ContainsIdent(text[0])) return (false, Usage);
            if (text.Last() == "?") return (false, Usage);
            return (true, "");
        }

        public virtual void PriorityUpgrade(ref string[] text, int[] vs)
        {
            foreach (var item in vs)
            {
                text[item] = text[item].ToUpper();
            }
        }

        /// <summary>
        /// Executes the specified controller.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public abstract string Execute(ref LibraryController controller, string[] text);
    }
}