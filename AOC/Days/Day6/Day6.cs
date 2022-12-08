using System.Text.RegularExpressions;
using Days;

namespace Day6
{
    public class Executor : Day
    {
        public Executor()
        {
        }

        public override string part1(string input)
        {
            var chars = input.ToCharArray();
            for (var i = 3; i < chars.Length; i++)
            {
                var code = new HashSet<char>()
                {
                    chars[i],
                    chars[i - 1],
                    chars[i - 2],
                    chars[i - 3]
                };
                if (code.Count == 4)
                {
                    return (i + 1) .ToString();
                }
                
            }
            

            return chars.Length.ToString();
        }

        public override string part2(string input)
        {
            var chars = input.ToCharArray();
            for (var i = 13; i < chars.Length; i++)
            {
                var code = new HashSet<char>();

                for (var j = 0; j < 14; j++)
                {
                    code.Add(chars[i - j]);
                }
                if (code.Count == 14)
                {
                    return (i + 1).ToString();
                }

            }


            return chars.Length.ToString();
        }
    }
}

