using Days;

namespace Day3
{
    public class Executor : Day
    {
        public Executor()
        {
        }

        public override string part1(string input)
        {
            var lines = input.Split("\n");
            return lines.Select(line =>
            {
                var sack1 = line.Substring(0, line.Length / 2).ToCharArray().ToHashSet();
                var sack2 = line.Substring(line.Length / 2).ToCharArray().ToHashSet();
                return GetValue(sack1.Intersect(sack2).ElementAt(0));
            }).Sum().ToString();
        }

        public override string part2(string input)
        {
            var lines = input.Split("\n");
            return lines
                .Select((line, index) =>
                {
                    return new { line = line, group = index / 3 };
                })
                .GroupBy(value => value.group)
                .Select(group =>
                {
                    return group
                        .Select(value => value.line.ToCharArray().ToList())
                        .Aggregate<IEnumerable<Char>>((previousList, nextList) => previousList.Intersect(nextList))
                        .ElementAt(0);
                })
                .Select(value => GetValue(value))
                .Sum()
            .ToString();
        }

        private int GetValue(Char value)
        {
            if (value >= 65 && value <= 90)
            {
                return value - 38;
            } else
            {
                return value - 96;
            }
        }
    }
}

