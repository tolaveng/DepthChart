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
        public static void Warns(IEnumerable<string> errors)
        {
            Console.WriteLine("");
            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (var error in errors)
            {
                Console.WriteLine(error);
            }
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
    }
}
