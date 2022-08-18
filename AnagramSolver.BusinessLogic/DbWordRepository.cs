using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic
{
    public class DbWordRepository : IDbWordRepository
    {
        private readonly string dictionaryPath;
        private readonly IFileReader _fileReader;
        private readonly IConfiguration _config;
        SqlConnectionStringBuilder builder;

        public HashSet<Word> Words { get; set; }

        public DbWordRepository(IFileReader fileReader, IConfiguration config)
        {
            _fileReader = fileReader;
            _config = config;
            dictionaryPath = Path.Combine(Directory.GetCurrentDirectory(),
                _config.GetSection("DictionaryFilePath").Value);
            builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = _config.GetConnectionString("WordsDatabase");
            //SeedDatabase();
        }

        public void AddWord(Word word)
        {
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                if (word.BaseWord.Contains("'"))
                {
                    word.BaseWord = word.BaseWord.Replace("'", "''");
                }

                var sql = "INSERT INTO dbo.Word (Word, PartOfSpeech, Number) " +
                          $"VALUES (N'{word.BaseWord}', '{word.PartOfSpeech}', '{word.Number}')";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                    //Words.Add(word);
                }
            }
        }

        public HashSet<Word> LoadDictionary()
        {
            var wordSet = new HashSet<Word>();

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                var sql = "SELECT * FROM dbo.Word";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var word = new Word()
                                {
                                    BaseWord = reader.GetString(1),
                                    PartOfSpeech = reader.GetString(2),
                                    Number = reader.GetInt32(3)
                                };
                                wordSet.Add(word);
                            }
                        }
                    }
                }
            }

            return wordSet;
        }

        public bool WordExists(Word word)
        {
            return LoadDictionary().Contains(word);
        }
        
        public List<Word> SearchWord(string word)
        {
            var words = new List<Word>();

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                var sql = "SELECT * FROM dbo.Word WHERE Word LIKE @Word";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add(new SqlParameter("@Word", $"%{word}%"));
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var wordObj = new Word()
                                {
                                    BaseWord = reader.GetString(1),
                                    PartOfSpeech = reader.GetString(2),
                                    Number = reader.GetInt32(3)
                                };
                                words.Add(wordObj);
                            }
                        }
                    }
                }
            }
            return words;
        }

        public bool StoreToCachedTable(string inputWord, List<string> anagrams)
        {
            if (AnagramsFound(inputWord))
            {
                return false;
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    var sql = "INSERT INTO dbo.CachedWord (Word) " +
                          $"VALUES (N'{inputWord}')";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    foreach(var anagram in anagrams)
                    {
                        sql = "INSERT INTO dbo.Anagrams (Anagram, WordId) " +
                          $"VALUES (N'{anagram}', (SELECT Id FROM dbo.CachedWord WHERE Word = N'{inputWord}'))";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
                return true;
            }
        }

        public List<string> GetCachedAnagrams(string inputWord)
        {
            List<string> words = new List<string>();
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                var sql = "SELECT Anagram FROM dbo.Anagrams WHERE WordId = " +
                    $"(SELECT Id FROM dbo.CachedWord WHERE Word = N'{inputWord}')";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                words.Add(reader.GetString(0));
                            }
                        }
                    }
                }
            }
            return words;
        }

        public void StoreSearchData(string ipAddress, string inputWord, List<string> anagrams, int timeSpent)
        {
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                var sql = "INSERT INTO dbo.SearchHistory (IpAddress, TimeSpent, SearchWord, Anagrams) " +
                      $"VALUES ('{ipAddress}', '{timeSpent}', N'{inputWord}', '{string.Join(",", anagrams)}')";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool AnagramsFound(string word)
        {
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                var sql = $"SELECT * FROM dbo.CachedWord WHERE Word = '{word}'";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }

        public List<SearchHistory> GetSearchHistory()
        {
            var history = new List<SearchHistory>();
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                var sql = $"SELECT * FROM SearchHistory";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var search = new SearchHistory() { IpAddress = reader.GetString(1), TimeSpent = reader.GetInt32(2), SearchWord = reader.GetString(3), Anagrams = reader.GetString(4) };
                                history.Add(search);
                            }
                        }
                    }
                }
            }
            return history;
        }

        public void DeleteTableData(string tableName)
        {
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("DeleteTableData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@TableName", SqlDbType.VarChar).Value = tableName;

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void SeedDatabase()
        {
            var lines = _fileReader.ReadFile(dictionaryPath);

            Word? lastWord = null;
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                foreach (var line in lines)
                {
                    var wordArr = line.Split('\t');

                    Word word = new Word { BaseWord = wordArr[0], PartOfSpeech = wordArr[1], Number = int.Parse(wordArr[3]) };

                    if ((lastWord != null && lastWord.BaseWord == word.BaseWord && lastWord.PartOfSpeech != word.PartOfSpeech)
                        || (lastWord == null) || (lastWord != null && lastWord.BaseWord != word.BaseWord))
                    {
                        if (word.BaseWord.Contains("'"))
                        {
                            word.BaseWord = word.BaseWord.Replace("'", "''");
                        }

                        var sql = "INSERT INTO dbo.Word (Word, PartOfSpeech, Number) " +
                                  $"VALUES (N'{word.BaseWord}', '{word.PartOfSpeech}', '{word.Number}')";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                        lastWord = word;
                    }

                    Word word2 = new Word { BaseWord = wordArr[2], PartOfSpeech = wordArr[1], Number = int.Parse(wordArr[3]) };

                    if ((word2.BaseWord != word.BaseWord)
                        || (word2.BaseWord == word.BaseWord && word2.PartOfSpeech != word.PartOfSpeech))
                    {
                        if (word2.BaseWord.Contains("'"))
                        {
                            word2.BaseWord = word2.BaseWord.Replace("'", "''");
                        }
                        var sql = "INSERT INTO dbo.Word (Word, PartOfSpeech, Number) " +
                                  $"VALUES (N'{word2.BaseWord}', '{word2.PartOfSpeech}', '{word2.Number}')";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            
        }
    }
}
