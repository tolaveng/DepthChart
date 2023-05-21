using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client.Helpers
{
    public static class AppConsole
    {
        public static void Warns(string message)
        {
            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ForegroundColor = defaultColor;
        }

        public static void Errors(IEnumerable<string> errors)
        {
            Console.WriteLine("");
            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var error in errors)
            {
                Console.WriteLine(error);
            }
            Console.ForegroundColor = defaultColor;
        }

        public static void Error(Exception ex)
        {
            if (ex is HttpRequestException)
            {
                Errors(new[] { "Have you run the Server Web API?" });
            }
            Errors(new[] {ex.Message});
        }

        public static bool Confirm(string message)
        {
            Console.Write(message);
            var ln = Console.ReadLine();
            if (ln.ToUpper() == "Y" || ln.ToUpper() == "YES") return true;
            return false;
        }
    }
}
