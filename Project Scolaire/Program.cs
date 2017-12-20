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
        private static string fileName = Directory.GetParent(@"../../ ").FullName + "/text.txt";

        public static string RandomString()
        {
            return new string(Enumerable.Repeat(legalCharacters, blockSize)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        static void GenerateFile()
        {
            using (FileStream stream = File.OpenWrite(fileName))
            {
                for (int i = 0; i < sizeInMb * blocksPerMb; i++)
                {
                    byte[] data = Encoding.ASCII.GetBytes(RandomString());
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
           
            Console.WriteLine("5 tests for multithread methode : Parallel.ForEach(File.ReadLines(fileName).AsParallel()");
            for (int i = 0; i < 5; i++)
            {
                Stopwatch watch = Stopwatch.StartNew();
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
                    Console.WriteLine("Your time for searching your phrase ({0}) : {1}, ticks : {2}", searchPhrase, watch.Elapsed.Seconds, watch.Elapsed.Ticks);
                }
                else
                {
                    Console.WriteLine("There is no your phrase in file, total time is : {0}, ticks : {1}", watch.Elapsed.Seconds, watch.Elapsed.Ticks);
                }
                watch.Stop();
            }

            Console.WriteLine("5 tests for multithread methode : Parallel.ForEach(File.ReadLines(fileName)");
            for (int i = 0; i < 5; i++)
            {
                Stopwatch watch = Stopwatch.StartNew();
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
                    Console.WriteLine("Your time for searching your phrase ({0}) : {1}, ticks : {2}", searchPhrase, watch.Elapsed.Seconds, watch.Elapsed.Ticks);
                }
                else
                {
                    Console.WriteLine("There is no your phrase in file, total time is : {0}, ticks : {1}", watch.Elapsed.Seconds, watch.Elapsed.Ticks);
                }
                watch.Stop();
            }

            Console.WriteLine("Test for one-thread methode : File.ReadLines(fileName)");
            using (MemoryStream stream = new MemoryStream())
            {
                Stopwatch watch = Stopwatch.StartNew();
                watch.Start();
                var lines = File.ReadAllLines(fileName);
                if (lines.Contains(searchPhrase))
                {
                    Console.WriteLine("Your time for searching your phrase ({0}) : {1}, ticks : {2}", searchPhrase, watch.Elapsed.Seconds, watch.Elapsed.Ticks);
                }
                else
                {
                    Console.WriteLine("There is no your phrase in file, total time is : {0}, ticks : {1}", watch.Elapsed.Seconds, watch.Elapsed.Ticks);
                }
                lines = null;
                watch.Stop();
            }
            Console.ReadLine();
        }
    }
}