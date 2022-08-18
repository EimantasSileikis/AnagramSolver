using AnagramSolver.Contracts.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace AnagramSolver.Cli
{
    public class UI
    {
        private readonly IAnagramSolver _anagramSolver;
        private readonly IConfiguration _config;

        public UI(IAnagramSolver anagramSolver, IConfiguration config)
        {
            _anagramSolver = anagramSolver;
            _config = config;
            Console.OutputEncoding = Console.InputEncoding = Encoding.Unicode;
            StartApp();
        }

        public void StartApp()
        {
            Console.WriteLine("Anagram Solver\n");

            Console.WriteLine(" " +
                                "1 - Start looking for anagrams \n " +
                                "2 - Request anagrams \n " +
                                "3 - Exit \n ");

            Console.Write("Your choice: ");
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
                    Console.WriteLine("Enter valid number 1-3\n");
                    StartApp();
                    break;
            }
        }

        private void StartLookingForAnagrams(int selection)
        {
            IList<string> anagrams;
            Console.Write("Your input: ");
            var input = Console.ReadLine();

            if (input == null)
                return;

            if (selection == 1)
            {
                anagrams = _anagramSolver.GetAnagrams(input);
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

        private void PrintAnagrams(IList<string> anagrams)
        {
            if (anagrams.Count > 0)
            {
                Console.WriteLine("\nAnagrams:");

                foreach (var anagram in anagrams)
                {
                    Console.WriteLine(anagram);
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Anagrams not found");
            }
        }
    }
}
