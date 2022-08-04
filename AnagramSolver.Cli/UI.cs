using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                                "1 - Start looking for anagrams \n ");

            Console.Write("Your choice: ");
            var selection = Console.ReadLine();

            switch (selection)
            {
                case "1":
                    Console.Clear();
                    StartLookingForAnagrams();
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("Enter valid number 1\n");
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

            var words = input.Split(" ");

            var minLength = _config.GetValue<int>("MinWordLength");

            foreach (var word in words)
            {
                if(word.Length < minLength)
                {
                    Console.Clear();
                    Console.WriteLine($"Minimum length of each word is {minLength}");
                    StartLookingForAnagrams();
                    return;
                }
            }

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
    }
}
