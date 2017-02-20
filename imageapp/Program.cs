using System;

namespace imageapp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1) {
                throw new ArgumentException("First argument must be the file path");
            }
            var path = args[0];
            Console.WriteLine($"Processing {path}");
        }
    }
}
