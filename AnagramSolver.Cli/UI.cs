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
                                "2 - Exit \n ");

            Console.Write("Your choice: ");
            var selection = Console.ReadLine();

            switch (selection)
            {
                case "1":
                    Console.Clear();
                    StartLookingForAnagrams();
                    break;

                case "2":
                    Environment.Exit(0);
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("Enter valid number 1 or 2\n");
                    StartApp();
                    break;
            }
        }

        private void StartLookingForAnagrams()
        {
            Console.Write("Your input: ");
            var input = Console.ReadLine();

            if (input == null)
                return;

            var anagrams = _anagramSolver.GetAnagrams(input);

            if(anagrams.Count > 0)
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
            
            StartApp();
        }
    }
}
