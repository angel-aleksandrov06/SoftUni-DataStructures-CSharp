namespace _02.WordCruncher
{
    using System;

    public class Program
    {
        public static void Main(string[] args)
        {
            var input = Console.ReadLine().Split(", ");
            var target = Console.ReadLine();

            WordCruncher wc = new WordCruncher(input, target);

            foreach (var path in wc.GetPaths())
            {
                Console.WriteLine(path);
            }
        }
    }
}
