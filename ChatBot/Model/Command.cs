using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot.Model
{
    class Command
    {
        public Command(string name,string answer,List<string> helpCommands)
        {
            Name = name;
            Answer = answer;
            HelpCommands = helpCommands;
        }
        public string Name { get; set; }

        public string Answer { get; set; }
        public List<string> HelpCommands { get; set; }
        public bool Contains(string command)
        {
            return command == Name;
        }

        public string Execute()
        {
            string commandResult="";
            commandResult += Answer;
            if (HelpCommands!=null)
            {
                foreach (var item in HelpCommands)
                {
                    commandResult += " " + ExecuteHelpCommand(item);
                }
            }
            return commandResult;
        }
        private string ExecuteHelpCommand(string message)
        {
            Console.WriteLine(message);
            string result = Console.ReadLine();
            return result;
        }
    }
}
