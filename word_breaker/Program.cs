using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace word_breaker
{
    class Program
    {
        static void Main(string[] args)
        {
            var dictionary = MakeDictionary();
            var wordsFound = WordsFromUrl(dictionary, "http://www.carphonewarehouse.com");
            var joinedWords = string.Join(",", wordsFound);
        }
        private static List<string> WordsFromUrl(Dictionary<string, string> dictionary, string urlValue)
        {
            var wordsFound = new List<string>();
            var cleanUrl = CleanUrl(urlValue);
            for (var outer = 0; outer < cleanUrl.Length; outer++)
            {
                for (var inner = outer; inner < cleanUrl.Length; inner++)
                {
                    var check = cleanUrl.Substring(outer, inner - outer);
                    if (dictionary.ContainsKey(check))
                    {
                        wordsFound.Add(check);
                    }
                }
            }
            return wordsFound;
        }
        private static string CleanUrl(string urlValue)
        {
            var url = new UriBuilder(urlValue);
            return url.Host.ToLowerInvariant();
        }
        private static Dictionary<string, string> MakeDictionary()
        {
            var dictionary = new Dictionary<string, string>();
            using (var r = new StreamReader(File.OpenRead("en_US.txt")))
            {
                while (!r.EndOfStream)
                {
                    var line = r.ReadLine();
                    var postfix = line.LastIndexOf("/");
                    var word = postfix != -1 ? line.Substring(0, postfix) : line;
                    var striped = new string(word.Where(c => !char.IsDigit(c)).ToArray());
                    if (striped.Length < 3) // ignore 1 and two letter words
                        continue;
                    dictionary[striped] = string.Empty;
                }
            }
            return dictionary;
        }
    }
}
