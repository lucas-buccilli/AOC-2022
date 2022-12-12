using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Days;
using static Day11.Executor;

namespace Day11
{
    public class Executor : Day
    {
        public Executor()
        {
        }

        public override string part1(string input)
        {
            var activeMonkies = new Dictionary<int, int>();

            var monkies = ParseInput(input).OrderBy(x => x.MonkeyNumber).ToDictionary(x => x.MonkeyNumber);

            var divisors = monkies.Select(x => x.Value.Test.divisibleNumber).ToHashSet();

            for (int round = 0; round < 20; round++)
            {
                for (int monkeyNumber = 0; monkeyNumber < monkies.Count(); monkeyNumber++)
                {
                    Monkey monkey = monkies[monkeyNumber];
                    while(monkey.StartingItems.Count() > 0)
                    {
                        if (activeMonkies.ContainsKey(monkeyNumber))
                        {
                            activeMonkies[monkeyNumber]++;
                        } else
                        {
                            activeMonkies.Add(monkeyNumber, 1);
                        }
                        var item = monkey.StartingItems.Dequeue();
                        var inspection = monkey.Operation.Apply(item);
                        inspection = inspection / 3;
                        var throwTo = monkey.Test.Apply(inspection);
                        monkies[throwTo].StartingItems.Enqueue(inspection);
                    }
                }
            }


            var topMonkies = activeMonkies.OrderByDescending(x => x.Value).Take(2).Select(x => x.Value).ToArray();
            return (topMonkies[0] * topMonkies[1]).ToString();
        }

        public override string part2(string input)
        {
            var activeMonkies = new Dictionary<int, UInt64>();

            var monkies = ParseInput(input).OrderBy(x => x.MonkeyNumber).ToDictionary(x => x.MonkeyNumber);
            var mod = monkies.Select(x => x.Value.Test.divisibleNumber).Aggregate(1UL, (x, y) => y * x);

            for (int round = 0; round < 10000; round++)
            {
                for (int monkeyNumber = 0; monkeyNumber < monkies.Count(); monkeyNumber++)
                {
                    Monkey monkey = monkies[monkeyNumber];
                    while (monkey.StartingItems.Count() > 0)
                    {
                        if (activeMonkies.ContainsKey(monkeyNumber))
                        {
                            activeMonkies[monkeyNumber]++;
                        }
                        else
                        {
                            activeMonkies.Add(monkeyNumber, 1);
                        }
                        var item = monkey.StartingItems.Dequeue() % mod;
                        var inspection = monkey.Operation.Apply(item) % mod;
                        var throwTo = monkey.Test.Apply(inspection);
                        monkies[throwTo].StartingItems.Enqueue(inspection);
                    }
                }
            }


            var topMonkies = activeMonkies.OrderByDescending(x => x.Value).Take(2).Select(x => x.Value).ToArray();
            return (topMonkies[0] * topMonkies[1]).ToString();
        }

        public IEnumerable<Monkey> ParseInput(String input)
        {
            var monkiesText = input.Split("\n\n");
            return monkiesText.Select(monkeyText =>
            {
                var regex = Regex.Match(monkeyText, @"^(\n)?Monkey\s(\d+):\n.*Starting items: (.+)\n.*Operation:(.+)\n.*Test: divisible by (\d+)\n.*If true: throw to monkey (\d+)\n.*If false: throw to monkey (\d+)");
                var monkeyNumber = int.Parse(regex.Groups[2].Value);
                var startingItems = regex.Groups[3].Value.Split(",").Select(x => UInt64.Parse(x)).ToList();
                var operationRegex = Regex.Match(regex.Groups[4].Value, @"^.*new = old ([\*\+]) (.+)$");
                Operation operation = operationRegex.Groups[1].Value switch
                {
                    "*" => new MultiplyOperation(UInt64.TryParse(operationRegex.Groups[2].Value, out UInt64 value) ? value : null),
                    "+" => new AddOperation(UInt64.TryParse(operationRegex.Groups[2].Value, out UInt64 value) ? value : null)
                };
                var test = new Test(int.Parse(regex.Groups[7].Value), int.Parse(regex.Groups[6].Value), UInt64.Parse(regex.Groups[5].Value));

                return new Monkey(monkeyNumber, new Queue<UInt64>(startingItems), operation, test);
            }).ToList();
        }

        public class Monkey
        {
            public int MonkeyNumber { get; }
            public Queue<UInt64> StartingItems { get; } = new Queue<UInt64>();
            public Operation Operation { get; }
            public Test Test { get; }

            public Monkey(int monkeyNumber, Queue<UInt64> startingItems, Operation operation, Test test)
            {
                this.MonkeyNumber = monkeyNumber;
                this.StartingItems = startingItems;
                this.Operation = operation;
                this.Test = test;
            }
        }

        public class MultiplyOperation : Operation
        {
            public MultiplyOperation(UInt64? value) : base(value) { }
            public override UInt64 Apply(UInt64 worryLevel)
            {

                return worryLevel * value.GetValueOrDefault(worryLevel);
            }
        }

        public class AddOperation : Operation
        {
            public AddOperation(UInt64? value) : base(value) { }
            public override UInt64 Apply(UInt64 worryLevel)
            {
                return worryLevel + value.GetValueOrDefault(worryLevel);
            }
        }

        public abstract class Operation
        {
            public UInt64? value { get; }
            public abstract UInt64 Apply(UInt64 worryLevel);

            public Operation(UInt64? value)
            {
                this.value = value;
            }
        }

        public class Test
        {
            public int falseMonkeyNumber { get; }
            public int trueMonkeyNumber { get; }
            public UInt64 divisibleNumber { get; }

            public Test(int falseNumber, int trueNumber, UInt64 number)
            {
                this.falseMonkeyNumber = falseNumber;
                this.trueMonkeyNumber = trueNumber;
                this.divisibleNumber = number;
            }

            public int Apply(UInt64 worryNumber)
            {
                if (worryNumber % divisibleNumber == 0)
                {
                    return trueMonkeyNumber;
                }
                return falseMonkeyNumber;
            }
        }
    }
}

