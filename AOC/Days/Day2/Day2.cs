using System;
using Days;

namespace Day2
{
    public class Executor : Day
    {
        public override string execute(string input)
        {
            IEnumerable<String> hands = input.Split("\n");

            var sets = hands
               .Where(value => !String.IsNullOrEmpty(value))
               .Select(value =>
               {
                   var set = value.Split(" ");
                   var opponentsHand = GetHand(set[0].Trim());
                   var myHand = GetMove(opponentsHand, GetDesiredOutcome(set[1].Trim()));
                   return new { Opponent = opponentsHand, Mine = myHand };
               });
            var scores = sets
               .Select(set =>
                {
                    Outcome outcome = GetOutcome(set.Opponent, set.Mine);
                    return GetValue(outcome) + GetValue(set.Mine);
                }).ToList();

            return scores.Sum().ToString();


        }

        static RPS GetHand(String value) => value switch
        {
            "A" => RPS.Rock,
            "B" => RPS.Paper,
            "C" => RPS.Scissors,
            _ => throw new ArgumentOutOfRangeException(nameof(value), $"Not in range: {value}"),
        };


        static int GetValue(RPS value) => value switch
        {
            RPS.Rock => 1,
            RPS.Paper => 2,
            RPS.Scissors => 3,
            _ => throw new ArgumentOutOfRangeException(nameof(value), $"Not in range: {value}"),
        };


        static int GetValue(Outcome value) => value switch
        {
            Outcome.Loss => 0,
            Outcome.Draw => 3,
            Outcome.Win => 6,
            _ => throw new ArgumentOutOfRangeException(nameof(value), $"Not in range: {value}"),
        };


        static RPS GetMove(RPS opponent, Outcome outcome)
        {
            if (outcome == Outcome.Win)
                return beats.First(value => value.Value == opponent).Key;
            else if (outcome == Outcome.Loss)
                return beats[opponent];
            else
                return opponent;
        }


        static Outcome GetDesiredOutcome(String value) => value switch
        {
            "X" => Outcome.Loss,
            "Y" => Outcome.Draw,
            "Z" => Outcome.Win,
            _ => throw new ArgumentOutOfRangeException(nameof(value), $"Not in range: {value}"),
        };


        static Outcome GetOutcome(RPS opponent, RPS mine)
        {
            if (beats[opponent] == mine)
            {
                return Outcome.Loss;
            } else if (beats[mine] == opponent)
            {
                return Outcome.Win;
            } else
            {
                return Outcome.Draw;
            }
        }

        static readonly Dictionary<RPS, RPS> beats = new()
        {
            {RPS.Rock, RPS.Scissors},
            {RPS.Paper, RPS.Rock},
            {RPS.Scissors, RPS.Paper}
        };
    }
}

