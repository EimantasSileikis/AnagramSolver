using AnagramSolver.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Cli
{
    public class UI
    {
        private readonly IAnagramSolver _anagramSolver;
        public UI(IAnagramSolver anagramSolver)
        {
            _anagramSolver = anagramSolver;
            StartApp();
        }

        public void StartApp()
        {
            Console.WriteLine("Anagram Solver\n");

            Console.WriteLine(" " +
                                "1 - Start looking for anagrams \n " +
                                "2 - Settings \n");

            Console.Write("Your choice: ");
            var selection = Console.ReadLine();

            switch (selection)
            {
                case "1":
                    Console.Clear();
                    StartLookingForAnagrams();
                    break;

                case "2":
                    Console.Clear();
                    AppSettings();
                    break;

                default:
                    Console.WriteLine("Enter valid number 1 or 2\n");
                    break;
            }
        }

        private void StartLookingForAnagrams()
        {
            Console.Write("Your input: ");
            var input = Console.ReadLine();

            if(input != null)
            {
                var anagrams = _anagramSolver.GetAnagrams(input);

                Console.WriteLine("\nAnagrams:");

                foreach (var anagram in anagrams) 
                {
                    Console.WriteLine(anagram);
                }
                Console.WriteLine();
            }

            StartApp();
        }

        private void AppSettings()
        {
            Console.WriteLine("What would you like to change:");

            Console.WriteLine(" " +
                                "1 - Count of generated anagrams \n " +
                                "2 - Minimum length of input word \n " +
                                "3 - Go back");

            Console.Write("Your choice: ");
            var selection = Console.ReadLine();

            switch (selection)
            {
                case "1":
                    Console.Clear();
                    
                    break;

                case "2":
                    Console.Clear();
                    
                    break;

                case "3":
                    Console.Clear();
                    StartApp();
                    break;

                default:
                    Console.WriteLine("Enter valid number 1-3\n");
                    break;
            }
        }
    }
}
