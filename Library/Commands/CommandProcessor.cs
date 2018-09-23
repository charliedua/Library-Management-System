using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Commands
{
    public class CommandProcessor
    {
        /// <summary>
        /// The commands
        /// </summary>
        private List<Command> _commands;

        /// <summary>
        /// The controller
        /// </summary>
        private LibraryController Controller;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandProcessor"/> class.
        /// </summary>
        /// <param name="controller">The controller.</param>
        public CommandProcessor(LibraryController controller, Action quit)
        {
            Controller = controller;
            _commands = new List<Command>()
            {
                new CreateCommand(),
                new DeleteCommand(),
                new SaveCommand  (),
                new QuitCommand  (quit),
                new IssueCommand(),
                new ShowCommand()
            };
            _commands.Add(new HelpCommand(_commands));
        }

        /// <summary>
        /// Gets the index from command text.
        /// </summary>
        /// <returns>
        /// The Index
        /// </returns>
        private int GetIndexFromCommandText(List<string> Text)
        {
            string txtToCheck = Text[0].ToUpper();
            for (int i = 0; i < _commands.Count; i++)
            {
                if (_commands[i].ContainsIdent(txtToCheck))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// converts the text To the list of string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public List<string> ToList(string text)
        {
            return new List<string>(text.Split(' '));
        }

        /// <summary>
        /// Invokes the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public string Invoke(string text)
        {
            text = text.Trim();
            List<string> cmdTxtArr = ToList(text);
            int index = GetIndexFromCommandText(cmdTxtArr);
            if (index != -1)
            {
                return _commands[index].Execute(ref Controller, cmdTxtArr.ToArray());
            }
            return "There is something That this Library is not Capable of YET. \n";
        }
    }
}