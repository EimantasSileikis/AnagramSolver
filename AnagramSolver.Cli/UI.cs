﻿using AnagramSolver.BusinessLogic;
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
        private readonly Settings _settings;

        public UI(IAnagramSolver anagramSolver, IConfiguration config, Settings settings)
        {
            _anagramSolver = anagramSolver;
            _config = config;
            _settings = settings;
            Console.OutputEncoding = Console.InputEncoding = Encoding.Unicode;
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
                    Console.Write("Please enter number which indicates count of generated anagrams (1 - 10): ");
                    var countInput = Console.ReadLine();
                    if(countInput == null || !InputValidation(countInput, "Enter valid number\n", out int countOfAnagrams) || countOfAnagrams < 1 || countOfAnagrams > 10)
                    {
                        AppSettings();
                        break;
                    }

                    _config.GetSection("MaxAnagrams").Value = countOfAnagrams.ToString();
                    _settings.SaveSettings();

                    Console.Clear();
                    StartApp();
                    break;

                case "2":
                    Console.Clear();
                    Console.Write("Please enter minimum length of input word: ");
                    var wordLength = Console.ReadLine();
                    if (wordLength == null || !InputValidation(wordLength, "Enter valid number\n", out int anagramLength) || anagramLength < 1)
                    {
                        AppSettings();
                        break;
                    }
                    _config.GetSection("MinWordLength").Value = anagramLength.ToString();
                    _settings.SaveSettings();

                    Console.Clear();
                    StartApp();
                    break;

                case "3":
                    Console.Clear();
                    StartApp();
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("Enter valid number 1-3\n");
                    AppSettings();
                    break;
            }
        }

        private bool InputValidation(string input, string failMessage, out int outputNumber)
        {
            bool success = int.TryParse(input, out outputNumber);

            if (!success)
                Console.WriteLine(failMessage);

            return success;
        }
    }
}
