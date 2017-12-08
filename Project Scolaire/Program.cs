using System;
using System.Diagnostics;
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
        const string searchPhrase = "gdfgdfHDF";
        private static Random random = new Random();
        private static string fileName = System.IO.Directory.GetParent(@"../../ ").FullName + "/text.txt";

        public static string RandomString()
        {
            return new string(Enumerable.Repeat(legalCharacters, blockSize)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        static void GenerateFile()
        {
            string test = RandomString();
            byte[] data = Encoding.ASCII.GetBytes(RandomString());
            using (FileStream stream = File.OpenWrite(fileName))
            {
                for (int i = 0; i < sizeInMb * blocksPerMb; i++)
                {
                    stream.Write(data, 0, data.Length);
                }
            }
        }

        static void Main(string[] args)
        {
            if (!File.Exists(fileName))
            {
                GenerateFile();
            }
            for (int i = 0; i < 25; i++)
            {
                Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
                bool found = false;
                
                watch.Start();
                Parallel.ForEach(File.ReadLines(fileName).AsParallel(), new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount }, (line, state) =>
                {
                    if (line.Contains(searchPhrase))
                    {
                        found = true;
                        state.Stop();
                        return;
                    }
                });

                if (found)
                {
                    Console.WriteLine("Your time for searching your phrase ({0}) : {1}", searchPhrase, watch.ElapsedMilliseconds);
                }
                else
                {
                    Console.WriteLine("There is no your phrase in file, total time is : {0}", watch.ElapsedMilliseconds);
                }
                watch.Stop();
            }

            for (int i = 0; i < 25; i++)
            {
                Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
                bool found = false;

                watch.Start();
                Parallel.ForEach(File.ReadLines(fileName), new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount }, (line, state) =>
                {
                    if (line.Contains(searchPhrase))
                    {
                        found = true;
                        state.Stop();
                        return;
                    }
                });

                if (found)
                {
                    Console.WriteLine("Your time for searching your phrase ({0}) : {1}", searchPhrase, watch.ElapsedMilliseconds);
                }
                else
                {
                    Console.WriteLine("There is no your phrase in file, total time is : {0}", watch.ElapsedMilliseconds);
                }
                watch.Stop();
            }
            Console.ReadLine();
        }
    }
}