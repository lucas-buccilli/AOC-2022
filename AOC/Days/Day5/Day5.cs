using System.Text.RegularExpressions;
using Days;

namespace Day5
{
    public class Executor : Day
    {
        public Executor()
        {
        }

        public override string part1(string input)
        {
            var lines = input.Split("\n\n");
            var procedureSection = lines.ElementAt(1);
            IList<Stack<char>> stacks = GetStacks(lines.ElementAt(0));

            var procedures = procedureSection.Split("\n").ToArray();
            foreach(var procedure in procedures)
            {
                var numbers = Regex.Matches(procedure, @"\d+")
                   .Cast<Match>()
                   .Select(m => m.Value)
                   .ToList();
                var quantity = int.Parse(numbers.ElementAt(0));
                var from = int.Parse(numbers.ElementAt(1));
                var to = int.Parse(numbers.ElementAt(2));

                for (int i = quantity; i > 0; i--)
                {
                    char box = stacks.ElementAt(from - 1).Pop();
                    stacks.ElementAt(to - 1).Push(box);
                }
                

            }

            return String.Join("", stacks.Where(stack => stack.Count() > 0).Select(stack => stack.Peek()));

        }

        public override string part2(string input)
        {
            var lines = input.Split("\n\n");
            var procedureSection = lines.ElementAt(1);
            IList<Stack<char>> stacks = GetStacks(lines.ElementAt(0));

            var procedures = procedureSection.Split("\n").ToArray();
            foreach (var procedure in procedures)
            {
                var numbers = Regex.Matches(procedure, @"\d+")
                   .Cast<Match>()
                   .Select(m => m.Value)
                   .ToList();
                var quantity = int.Parse(numbers.ElementAt(0));
                var from = int.Parse(numbers.ElementAt(1));
                var to = int.Parse(numbers.ElementAt(2));

                var tempStack = new Stack<char>();
                for (int i = quantity; i > 0; i--)
                {
                    char box = stacks.ElementAt(from - 1).Pop();
                    tempStack.Push(box);
                }

                while (tempStack.Count() > 0)
                {
                    stacks.ElementAt(to - 1).Push(tempStack.Pop());
                }


            }

            return String.Join("", stacks.Where(stack => stack.Count() > 0).Select(stack => stack.Peek()));

        }

        private static IList<Stack<char>> GetStacks(string stacks)
        {
            var RowsAndNumbers = stacks.Split("\n");
            var rows = RowsAndNumbers.Take(RowsAndNumbers.Length - 1).ToArray();
            var numberOfStacks = rows.ElementAt(rows.Length - 1).ToCharArray().Where(c => c == '[').Count();
            IList<Stack<char>> stack = new List<Stack<char>>();
            for (int i = 0; i < numberOfStacks; i++)
            {
                stack.Add(new Stack<char>());
            }

            for (int i = rows.Length - 1; i >= 0; i--)
            {
                var row = rows.ElementAt(i);
                var currentStack = 0;

                for (int j = 0; j < row.Length - 1; j += 3)
                {
                    if (row.ElementAt(j) == ' ')
                    {
                        j++;
                    }

                    var box = row.ElementAt(j + 1);
                    if (box == ' ')
                    {
                        currentStack++;
                        continue;
                    }

                    stack.ElementAt(currentStack).Push(box);
                    currentStack++;

                }
            }
            return stack;
        }
    }
}

