using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCreater
{
    class Program
    {
        const int sizeInMb = 1024;
        const int blockSize = 1024 * 8;
        const int blocksPerMb = (1024 * 1024) / blockSize;
        const string legalCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ123456789 \n";

        private static Random random = new Random();
        private static string fileName = System.IO.Directory.GetParent(@"../../ ").FullName + "/text.txt";

        public static string RandomString()
        {
            return new string(Enumerable.Repeat(legalCharacters, blockSize)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        static void Main(string[] args)
        {
            Stopwatch watch = Stopwatch.StartNew();
            string test = RandomString();
            //byte[] data = new byte[blockSize];
            byte[] data = Encoding.ASCII.GetBytes(RandomString());
            using (FileStream stream = File.OpenWrite(fileName))
            {
                for (int i = 0; i < sizeInMb * blocksPerMb; i++)
                {
                    //random.NextBytes(data);
                    stream.Write(data, 0, data.Length);
                }
            }
            watch.Stop();
            long timetest = watch.ElapsedMilliseconds;
            watch.Restart();
            IEnumerable<string> lines = File.ReadAllLines(fileName);
            //int numb = random.Next(lines.Count());
            string rndphrase = lines.ElementAt(random.Next(lines.Count()));
            Console.Write("Your phrase to search is {0}", rndphrase);
            //We identify the matches. If the input is empty, then we return no matches at all
            IEnumerable<string> matches = !String.IsNullOrEmpty(rndphrase)
                                          ? lines.Where(line => line.IndexOf(rndphrase, StringComparison.OrdinalIgnoreCase) >= 0)
                                          : Enumerable.Empty<string>();

            //Console.WriteLine(matches.Any()
            //                  ? String.Format("Matches:\n> {0}", String.Join("\n> ", matches))
            //                  : "There were no matches");
            watch.Stop();
            //timetest = watch.ElapsedMilliseconds;
            Console.WriteLine("Your time for creating file: {0}; {1} And for searching your phrase : {2}", timetest, System.Environment.NewLine, watch.ElapsedMilliseconds);
            //foreach (var line in lines)
            //{
            //   Console.WriteLine(line);
            //}
            Console.ReadLine();
        }
    }
}