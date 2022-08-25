using AnagramSolver.Contracts.Interfaces.Core;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Text;

namespace AnagramSolver.Cli
{
    public class UI
    {
        private readonly IAnagramSolver _anagramSolver;
        private readonly IConfiguration _config;
        private readonly IDisplay _display;

        public UI(IAnagramSolver anagramSolver, IConfiguration config)
        {
            _anagramSolver = anagramSolver;
            _config = config;
            Console.OutputEncoding = Console.InputEncoding = Encoding.Unicode;
            _display = new Display(WriteToDebug);

            StartApp();

        }

        public void StartApp()
        {
            _display.Write("Anagram Solver\n");

            _display.Write(" " +
                                "1 - Start looking for anagrams \n " +
                                "2 - Request anagrams \n " +
                                "3 - Exit \n ");

            _display.Write("Your choice: ");
            var selection = Console.ReadLine();

            switch (selection)
            {
                case "1":
                    Console.Clear();
                    StartLookingForAnagrams(1);
                    break;

                case "2":
                    Console.Clear();
                    StartLookingForAnagrams(2);
                    StartApp();
                    break;

                case "3":
                    Environment.Exit(0);
                    break;

                default:
                    Console.Clear();
                    _display.Write("Enter valid number 1-3\n");
                    StartApp();
                    break;
            }
        }

        private void StartLookingForAnagrams(int selection)
        {
            IEnumerable<string> anagrams;
            _display.Write("Your input: ");
            var input = Console.ReadLine();

            if (input == null)
                return;

            if (selection == 1)
            {
                var task =  _anagramSolver.GetAnagramsAsync(input);
                task.Wait();
                anagrams = task.Result;
            }
            else
            {
                var task = _anagramSolver.RequestAnagrams(input);
                task.Wait();
                anagrams = task.Result;
            }
            PrintAnagrams(anagrams);
            StartApp();
        }

        private void PrintAnagrams(IEnumerable<string> anagrams)
        {
            if (anagrams.Count() > 0)
            {
                _display.Write("\nAnagrams:");

                foreach (var anagram in anagrams)
                {
                    _display.Write(anagram);
                }
                _display.Write("");
            }
            else
            {
                _display.Write("Anagrams not found");
            }
        }

        public void WriteToConsole(string message)
        {
            Console.WriteLine(message);
        }

        public void WriteToDebug(string message)
        {
            Debug.WriteLine(message);
        }
        public void WriteToFile(string message)
        {

        }
    }
}
