using ChatBot.Bot;
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
        private string ExecuteHelpCommand(string command)
        {
            string result = "";
            if (!(command.Contains("(") && command.Contains(")")))
            {
                Console.WriteLine(command);
                result = Console.ReadLine();               
            }
            else
            {
                string[] str = command.Split(new char[] { '(' });
                string commandName = str[0];

                List<string> parameters = new List<string>();
                str[1] = str[1].Replace("  ", "");
                if (str[1].Length < 3)
                {
                    result = MyBot.ExecuteHelpFunction(commandName, parameters.ToArray());
                }
                else
                {
                    string[] parametersQuestion = str[1].Split(new char[] { ',' });

                    parametersQuestion[parametersQuestion.Length - 1] = parametersQuestion[parametersQuestion.Length - 1].Trim(new char[] { ')' });


                    foreach (var item in parametersQuestion)
                    {
                        Console.WriteLine(item);
                        parameters.Add(Console.ReadLine());
                    }

                    result = MyBot.ExecuteHelpFunction(commandName, parameters.ToArray());
                }              
            }
            return result;
        }
    }
}
