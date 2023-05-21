using Client.Helpers;
using Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Client
{
    public class Main
    {
        private readonly IDepthChart _depthChart;

        public Main(IDepthChart depthChart)
        {
            _depthChart = depthChart;
        }

        public async Task RunAsync()
        {
            Console.WriteLine("Code Challenge: Depth Chart");
            var option = GetOption();
            while(option > 0)
            {
                try
                {
                    await DoDepthChart(option);
                } catch (Exception ex)
                {
                    AppConsole.Error(ex);
                }
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
                option = GetOption();
            }
        }

        public async Task DoDepthChart(int option)
        {
            string position;
            Player player;
            int? depth = null;
            switch (option)
            {
                case 1:
                    Console.WriteLine("Get Full Depth Chart");
                    Console.WriteLine("getting...");
                    var fullChart = await _depthChart.GetFullDepthChart();
                    Console.WriteLine(fullChart);
                    break;

                case 2:
                    Console.WriteLine("Get Backup Depth Chart");
                    position = InputPosition();
                    player = InputPlayer(false);
                    Console.WriteLine("getting...");
                    var backup = await _depthChart.GetBackups(position, player);
                    Console.WriteLine(backup);
                    break;

                case 3:
                    Console.WriteLine("Add Player to chart");
                    position = InputPosition();
                    player = InputPlayer(true);
                    depth = InputDepth();
                    Console.WriteLine("adding...");
                    var added = await _depthChart.AddPlayerToDepthChart(position, player, depth);
                    if (added)
                        Console.WriteLine("Player has been added.");
                    else
                        Console.WriteLine("Something went wrong.");
                    break;

                case 4:
                    Console.WriteLine("Remove Player from the chart");
                    position = InputPosition();
                    player = InputPlayer(false);
                    Console.WriteLine("removing...");
                    var removed = await _depthChart.RemovePlayerFromDepthChart(position, player);
                    Console.WriteLine(removed);
                    break;

                case 99:
                    Console.Clear();
                    break;
            }
        }

        private string InputPosition()
        {
            string position;
            do
            {
                Console.Write("Enter Position: ");
                position = Console.ReadLine();
            } while (string.IsNullOrEmpty(position));
            return position;
        }

        private int? InputDepth()
        {
            var number = -1;
            var regex = new Regex("^\\-?[0-9]+$");
            do
            {
                Console.Write("Enter Depth (-1 is null): ");
                var ln = Console.ReadLine();
                var test = regex.IsMatch(ln);
                if (regex.IsMatch(ln) && int.TryParse(ln, out number))
                {
                    if (number == -1) return null;
                    return number;
                }
                Console.WriteLine("Enter Number only");
            } while (number <= -1);
            return number;
        }

        private Player InputPlayer(bool nameRequired)
        {
            int number;
            string name = "";
            do
            {
                Console.Write("Enter Player Number: ");
                number = InputNumber();

                if (nameRequired)
                {
                    Console.Write("Enter Player Name: ");
                    name = Console.ReadLine();
                    if (string.IsNullOrEmpty(name))
                        Console.WriteLine("Player Name is required!");
                }
            } while ((nameRequired && string.IsNullOrEmpty(name)) || number < 1);
            return new Player(number, name);
        }

        private int InputNumber()
        {
            var number = -1;
            var regex = new Regex("^[0-9]+$");
            do
            {
                var ln = Console.ReadLine();
                if (regex.IsMatch(ln) && int.TryParse(ln, out number))
                {
                    return number;
                }
                Console.WriteLine("Enter Number only");
            } while (number < 0);
            return number;
        }

        private int GetOption()
        {
            var options = new Dictionary<int, string>
            {
                {1, "1. Get Full Depth Chart"},
                {2, "2. Get Backup Player"},
                {3, "3. Add Player to chart"},
                {4, "4. Remove Player from chart"},
                {99, "99. Clear Console"},
                {0, "0. Exit"},
            };

            Console.WriteLine("");
            var text = "Choose option number: ";
            Console.WriteLine(text);
            foreach (var item in options)
            {
                Console.WriteLine(item.Value);
            }

            var regex = new Regex("^[0-9]+$");
            var option = -1;
            do
            {
                var ln = Console.ReadLine();
                if (ln.ToUpper() == "Q") return 0;

                if (regex.IsMatch(ln) && int.TryParse(ln, out option))
                {
                    if (options.Keys.Any(x => x == option))
                        return option;
                }
                Console.Write(text);
            } while (!options.Keys.Any(x => x == option));
            return 0;
        }
    }
}
