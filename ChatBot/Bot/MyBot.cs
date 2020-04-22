using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ChatBot.Model;

namespace ChatBot.Bot
{
    static class MyBot
    {
        public static string HelpCommand()
        {
            string names="Список команд\n";
            foreach (var item in commands)
            {
                names += item.Name + "\n";
            }
            return names;
        }
        private static List<Command> commands = new List<Command>();
        public static void Start()
        {
            ParseCommandFile();
        }
        public static string ExecuteCommand(string message)
        {
            foreach (var item in commands)
            {
                if (item.Name==message)
                {
                    return item.Execute();
                }
            }
            return "Я тебя не понимаю!";

        }

        private static void ParseCommandFile()
        {
            string name;
            string answer;
            string helpCommand;
            List<string> helpCommands;
            using (var sr = new StreamReader("Commands.txt"))
            {
                while (!sr.EndOfStream)
                {
                    name = "";
                    answer = "";
                    helpCommands = new List<string>();
                    do
                    {
                        sr.Read();
                    } while (((char)sr.Peek()).ToString() != "*" && sr.EndOfStream!=true);
                    sr.Read();
                    do
                    {
                        name += (char)sr.Read();
                    } while (((char)sr.Peek()).ToString() != "*");
                    do
                    {
                        sr.Read();
                    } while (((char)sr.Peek()).ToString() != ":");
                    sr.Read();
                    do
                    {
                        answer += (char)sr.Read();
                    } while (((char)sr.Peek()).ToString() != "$" && ((char)sr.Peek()).ToString() != "\n");
                    if (((char)sr.Read()).ToString() == "\n")
                    {
                        commands.Add(new Command(name,answer,null));
                        continue;
                    }
                    do
                    {
                        helpCommand = "";
                        sr.Read();
                        do
                        {
                            helpCommand += (char)sr.Read();
                        } while (((char)sr.Peek()).ToString() != "}");
                        helpCommands.Add(helpCommand);
                        while(((char)sr.Peek()).ToString() != "$" && ((char)sr.Peek()).ToString() != "\n")
                        {
                            sr.Read();
                        }

                    } while (((char)sr.Read()).ToString() != "\n");
                    commands.Add(new Command(name, answer, helpCommands));
                }

            }
        }

    }
}
