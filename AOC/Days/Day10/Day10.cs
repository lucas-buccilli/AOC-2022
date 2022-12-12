using System.Text.RegularExpressions;
using AOC.Extensions;
using Days;

namespace Day10
{
    public class Executor : Day
    {
        public Executor()
        {
        }

        private static int part1Sum = 0;
        private static String[] CrtOutput = new string[240];

        private static HashSet<int> part1Pcs = new HashSet<int> { 20, 60, 100, 140, 180, 220};


        public override string part1(string input)
        {
            var instructions = ParseInstructions(input).ToList();
            instructions.ForEach(instruction => Processor.ProcessInstruction(instruction));

            return part1Sum.ToString();
        }

        public override string part2(string input)
        {
            var instructions = ParseInstructions(input).ToList();
            instructions.ForEach(instruction => Processor.ProcessInstruction(instruction));
            String output = "";
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 40; j++)
                {
                    output += CrtOutput[(i * 40) + j];

                }
                output += "\n";
            }
            return output;
        }


        private static IEnumerable<Instruction> ParseInstructions(String input)
        {
            var lines = input.Split("\n");

            return lines.Select(line =>
            {
                Instruction instruction;
                switch (line)
                {
                    case var command when Regex.IsMatch(line, @"^noop$"):
                        {
                            instruction = new NoOp();
                            break;
                        }
                    case var command when Regex.IsMatch(line, @"^add([a-z])\s((-)?(\d)+)$"):
                        {
                            var match = Regex.Match(command, @"^add([a-z])\s((-)?(\d)+)$");
                            instruction = new Add(match.Groups[1].Value.ToEnum<Register>(), int.Parse(match.Groups[2].Value));
                            break;
                        };
                    default: throw new ArgumentException(line.ToString());

                }
                return instruction;
            });
        }

        public  interface Instruction
        {

        }

        public class Add : Instruction
        {
            public Register register { get; }
            public int amount;

            public Add(Register register, int amount)
            {
                this.register = register;
                this.amount = amount;
            }
        }

        public class NoOp : Instruction
        {

        }


        public enum Command
        {
            NoOp,
            Add
        }

        public enum Register
        {
            X
        }

        public static class Processor
        {
            public static int PC { get; set; } = 1;


            public static Dictionary<Register, int> REGISTERS { get; } = new Dictionary<Register, int>()
            {
                { Register.X, 1 }
            };

            public static void ProcessInstruction(Instruction instruction)
            {

                switch (instruction)
                {
                    case NoOp:
                        {
                            var NoOpInstruction = (NoOp)instruction;
                            IncrementPc();
                            break;
                        }
                    case Add:
                        {
                            var AddInstruction = (Add)instruction;
                            IncrementPc();
                            IncrementPc();
                            REGISTERS[AddInstruction.register] += AddInstruction.amount;
                            break;
                        }
                       
                }
            }

            public static void IncrementPc()
            {
                if (part1Pcs.Contains(PC))
                {
                    part1Sum += PC * REGISTERS[Register.X];
                }

                if (PC <= 240) {
                    if (int.Abs(((PC - 1) % 40) - REGISTERS[Register.X]) <= 1)
                    {
                        CrtOutput[PC - 1] = "#";
                    } else
                    {
                        CrtOutput[PC - 1] = " ";
                    }
                }
                PC++;
            }
        }
    }
}

