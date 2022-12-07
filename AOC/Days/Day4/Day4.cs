using Days;

namespace Day4
{
    public class Executor : Day
    {
        public Executor()
        {
        }

        public override string part1(string input)
        {
            var lines = input.Split("\n");
            return lines
                .Select(line => line.Split(","))
                .Select(split => new { elf1 = GetRange(split.ElementAt(0)), elf2 = GetRange(split.ElementAt(1)) })
                .Select(assignment => {
                    return assignment.elf1.Intersect(assignment.elf2).Count() == assignment.elf1.Count() ||assignment.elf2.Intersect(assignment.elf1).Count() == assignment.elf2.Count();
                })
                .Where(value => value)
                .Count()
                .ToString();

        }

        public override string part2(string input)
        {
            var lines = input.Split("\n");
            return lines
                .Select(line => line.Split(","))
                .Select(split => new { elf1 = GetRange(split.ElementAt(0)), elf2 = GetRange(split.ElementAt(1)) })
                .Select(assignment => {
                    return assignment.elf1.Intersect(assignment.elf2).Count() > 0;
                })
                .Where(value => value)
                .Count()
                .ToString();

        }

        private int[] GetRange(String value)
        {
            var values = value.Split("-");
            var start = int.Parse(values.ElementAt(0));
            var end = int.Parse(values.ElementAt(1));
            return Enumerable.Range(start, (end - start) + 1).ToArray();
        }
    }
}

