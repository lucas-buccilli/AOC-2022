using System.Text.RegularExpressions;
using Days;

namespace Day9
{
    public class Executor : Day
    {
        public Executor()
        {
        }

        public override string part1(string input)
        {
            var numberOfKnots = 2;
            var maxDistanceBetweenKnots = 2;

            IEnumerable<Command> commands = input
            .Split("\n")
            .Select(x => ParseCommand(x));
            Knot[] knots = GetKnots(numberOfKnots);

            HashSet<string> tailCoordinates = new HashSet<string>();

            foreach (var command in commands)
            {
                for (var i = 0; i < command.distance; i++)
                {
                    MoveRope(knots, command, maxDistanceBetweenKnots);

                    tailCoordinates.Add($"{knots.Last().coordinate.x}, {knots.Last().coordinate.y}");
                }
            }


            return tailCoordinates.Count().ToString();
        }

        public override string part2(string input)
        {
            var numberOfKnots = 9;
            var maxDistanceBetweenKnots = 2;

            IEnumerable<Command> commands = input
                .Split("\n")
                .Select(x => ParseCommand(x));
            Knot[] knots = GetKnots(numberOfKnots);

            HashSet<string> tailCoordinates = new HashSet<string>();

            foreach (var command in commands)
            {
                for (var i = 0; i < command.distance; i++)
                {
                    MoveRope(knots, command, maxDistanceBetweenKnots);

                    tailCoordinates.Add($"{knots.Last().coordinate.x}, {knots.Last().coordinate.y}");
                }
            }


            return tailCoordinates.Count().ToString();
        }

        private static void MoveRope(Knot[] knots, Command command, int maxDistanceBetweenKnots)
        {
            knots.First().Move(command.direction);
            for (int j = 1; j < knots.Length; j++)
            {
                knots[j].MoveTowardsKnot(knots[j - 1], maxDistanceBetweenKnots);
            }
        }

        private static Knot[] GetKnots(int numberOfKnots)
        {
            var knots = new Knot[numberOfKnots];
            for (var i = 0; i < numberOfKnots; i++)
            {
                knots[i] = new Knot(0, 0);
            }

            return knots;
        }


        private Command ParseCommand(String line)
        {
            var match = Regex.Match(line, @"^([RLDU])\s([\d]+)");
            var direction = ParseDirection(match.Groups[1].Value);
            var distance = int.Parse(match.Groups[2].Value);
            return new Command(direction, distance);
        }

        private Direction ParseDirection(string value)
        {
            return value switch
            {
                "U" => Direction.Up,
                "D" => Direction.Down,
                "L" => Direction.Left,
                "R" => Direction.Right,
                _ => throw new ArgumentException()
            };
        }

        private class Knot
        {
            public Coordinate coordinate { get; }
            public Knot(int x, int y)
            {
                this.coordinate = new Coordinate(x, y);
            }

            public void Move(Direction direction)
            {
                switch (direction)
                {
                    case Direction.Up:
                        this.coordinate.y++;
                        break;
                    case Direction.Down:
                        this.coordinate.y--;
                        break;
                    case Direction.Right:
                        this.coordinate.x++;
                        break;
                    case Direction.Left:
                        this.coordinate.x--;
                        break;
                };
            }

            public void MoveTowardsKnot(Knot knot, int maxDistance)
            {
                var xDiff = knot.coordinate.x - coordinate.x;
                var yDiff = knot.coordinate.y - coordinate.y;
                Direction? xMove = null;
                Direction? yMove = null;

                //handle potential diagnal
                if (xDiff != 0 && yDiff != 0)
                {
                    if (int.Abs(xDiff) >= maxDistance)
                    {
                        if (xDiff > 0)
                        {
                            xMove = Direction.Right;
                        }
                        else
                        {
                            xMove = Direction.Left;
                        }

                        if (yDiff > 0)
                        {
                            yMove = Direction.Up;
                        } else 
                        {
                            yMove = Direction.Down;
                        }
                    } else if (int.Abs(yDiff) >= maxDistance)
                    {
                        if (yDiff > 0)
                        {
                            yMove = Direction.Up;
                        }
                        else
                        {
                            yMove = Direction.Down;
                        }

                        if (xDiff > 0)
                        {
                            xMove = Direction.Right;
                        }
                        else
                        {
                            xMove = Direction.Left;
                        }
                    }
                } else
                {
                    if (int.Abs(xDiff) >= maxDistance)
                    {
                        if (xDiff > 0)
                        {
                            xMove = Direction.Right;
                        }
                        else
                        {
                            xMove = Direction.Left;
                        }
                    }

                    if (int.Abs(yDiff) >= maxDistance)
                    {
                        if (yDiff > 0)
                        {
                            yMove = Direction.Up;
                        }
                        else
                        {
                            yMove = Direction.Down;
                        }
                    }
                }
                

                if (xMove.HasValue)
                {
                    Move(xMove.Value);
                }

                if (yMove.HasValue)
                {
                    Move(yMove.Value);
                }
            }
        }

        private class Coordinate {

            public int x { get; set;  }
            public int y { get; set;  }

            public Coordinate(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        private class Command
        {
            public Direction direction { get; }
            public int distance { get; }

            public Command(Direction direction, int distance)
            {
                this.direction = direction;
                this.distance = distance;
            }
        }

        private enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }
    }
}

