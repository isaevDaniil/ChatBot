using ChatBot.Bot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot
{
    class Program
    {
        static void Main(string[] args)
        {
            MyBot.Start();

            Console.WriteLine(MyBot.HelpCommand());
            foreach (var item in MyBot.helpFunctions)
            {
                Console.WriteLine(item.Name);
            }
            
            while(true)
            {               
                Console.WriteLine(MyBot.ExecuteCommand(Console.ReadLine()));
            }
        }
    }
}
