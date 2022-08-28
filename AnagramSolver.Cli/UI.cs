using AnagramSolver.Contracts.Interfaces.Core;
using AnagramSolver.Contracts.Interfaces.Files;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Text;

namespace AnagramSolver.Cli
{
    public class UI
    {
        private readonly IAnagramSolver _anagramSolver;
        private readonly IConfiguration _config;
        private readonly IFileManager _fileManager;
        private readonly IDisplay _display;
        private readonly DisplayWithEvents _displayWithEvents;
        private readonly string filePath = Path
            .Combine(Directory.GetCurrentDirectory(), "CliOutput.txt");

        public UI(IAnagramSolver anagramSolver, IConfiguration config,
            IFileManager fileManager)
        {
            _anagramSolver = anagramSolver;
            _config = config;
            _fileManager = fileManager;
            Console.OutputEncoding = Console.InputEncoding = Encoding.Unicode;

            _display = new Display(WriteToConsole);
            _displayWithEvents = new DisplayWithEvents();
            _displayWithEvents.Print += WriteToConsole;
            _displayWithEvents.Print += WriteToFile;

            StartApp();
        }

        public void StartApp()
        {
            _displayWithEvents.Write("Anagram Solver\n");

            _displayWithEvents.Write(" " +
                                "1 - Start looking for anagrams \n " +
                                "2 - Request anagrams \n " +
                                "3 - Exit \n ");

            _displayWithEvents.Write("Your choice: ");
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
                    _displayWithEvents.Write("Enter valid number 1-3\n");
                    StartApp();
                    break;
            }
        }

        private void StartLookingForAnagrams(int selection)
        {
            IEnumerable<string> anagrams;
            _displayWithEvents.Write("Your input: ");
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
                _displayWithEvents.Write("\nAnagrams:");

                foreach (var anagram in anagrams)
                {
                    //CapitalLetterHandler capitalLetterHandler = new CapitalLetterHandler(CapitalizeFirstLetter);
                    //_display.FormattedPrint(capitalLetterHandler, anagram);

                    _displayWithEvents.FormattedPrint(CapitalizeFirstLetter, anagram);
                }
                _displayWithEvents.Write("");
            }
            else
            {
                _displayWithEvents.Write("Anagrams not found");
            }
        }

        public void WriteToConsole(string message)
        {
            Console.WriteLine(message);
        }

        public void WriteToConsole(object? sender, DisplayEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        public void WriteToDebug(string message)
        {
            Debug.WriteLine(message);
        }

        public void WriteToFile(string message)
        {
            _fileManager.WriteLine(filePath, message);
        }

        public void WriteToFile(object? sender, DisplayEventArgs e)
        {
            _fileManager.WriteLine(filePath, e.Message);
        }

        public string CapitalizeFirstLetter(string input)
        {
            if (input.Length == 0)
                return "";
            else if(input.Length == 1)
                return input.ToUpper();
            else
                return char.ToUpper(input[0]) + input.Substring(1);
        }
    }
}
