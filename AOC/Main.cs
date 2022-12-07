using Days;

namespace AOC
{
    internal class AOC
    {


        static readonly Dictionary<String, Day> dayMap = new Dictionary<String, Day>()
        {
            {"day2", new Day2.Executor()},
            {"day3", new Day3.Executor()},
            {"day4", new Day4.Executor()},
            {"day5", new Day5.Executor()}
        };

        static void Main(string[] args)
        {
            String input = File.ReadAllText($"{Environment.CurrentDirectory}/input/{args[0]}.txt");
            Console.WriteLine(dayMap[args[0]].part1(input));
            Console.WriteLine(dayMap[args[0]].part2(input));
        }

    }
}