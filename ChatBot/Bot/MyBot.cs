using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ChatBot.Model;
using System.Reflection;
using LibrarySDK;

namespace ChatBot.Bot
{
    static class MyBot
    {
        public static string HelpCommand()
        {
            string names = "Список команд\n";
            foreach (var item in commands)
            {
                names += item.Name + "\n";
            }
            return names;
        }
        private static List<Command> commands = new List<Command>();
        public static List<MethodInfo> helpFunctions = new List<MethodInfo>();
        public static void Start()
        {
            ParseCommandFileAsync();
            ReadHelpFunctionsAsync();
        }
        public static string ExecuteCommand(string message)
        {
            foreach (var item in commands)
            {
                if (item.Name == message)
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
                    } while (((char)sr.Peek()).ToString() != "*" && sr.EndOfStream != true);
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
                        commands.Add(new Command(name, answer, null));
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
                        while (((char)sr.Peek()).ToString() != "$" && ((char)sr.Peek()).ToString() != "\n")
                        {
                            sr.Read();
                        }

                    } while (((char)sr.Read()).ToString() != "\n");
                    commands.Add(new Command(name, answer, helpCommands));
                }

            }
        }
        private static async void ParseCommandFileAsync()
        {
            await Task.Run(() => ParseCommandFile());
        }
        private static void ReadHelpFunctions()
        {
            var helpDir = new DirectoryInfo(Directory.GetCurrentDirectory() + "/helper");
            var libraries = helpDir.GetFiles();

            foreach (var item in libraries)
            {
                Assembly asm = Assembly.LoadFrom(helpDir.Name + "/" + item);
                var typesFromDll = asm.GetTypes();
                foreach (var separType in typesFromDll)
                {
                    var methodsDll = separType.GetMethods()
                        .Where(m => m.GetCustomAttributes(false)
                        .Select(a => a.ToString() == typeof(Help).ToString()).FirstOrDefault()).ToArray();
                    foreach (var methodFromDll in methodsDll)
                    {
                        helpFunctions.Add(methodFromDll);
                    }
                }
            }
        }
        private static async void ReadHelpFunctionsAsync()
        {
            await Task.Run(() => ReadHelpFunctions());
        }

        public static string ExecuteHelpFunction(string FunctionName, object[] parameters)
        {
            string result = "";
            object instance = null;
            foreach (var item in helpFunctions)
            {
                if (item.Name == FunctionName)
                {
                    var currentType = item.DeclaringType;
                    if (!currentType.IsAbstract)
                    {
                        instance = Activator.CreateInstance(currentType);
                    }

                    try
                    {
                        result += (string)item.Invoke(instance, parameters);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message + "\nФункция " + item.Name + " не выполнена");
                    }
                    return result;
                }
            }
            return "Ошибка: метод отсутсвует";
        }
    }
}