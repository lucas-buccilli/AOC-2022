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
            IEnumerable<Command> commands = input
                .Split("\n")
                .Select(x => parseCommand(x));

            var head = new Knot(0, 0);
            var tail = new Knot(0, 0);

            HashSet<string> tailCoordinates = new HashSet<string>();

            foreach(var command in commands)
            {
                for(var i = 0; i < command.distance; i++)
                {
                    head.Move(command.direction);
                    tail.MoveToKnot(head, 2);
                    tailCoordinates.Add($"{tail.coordinate.x}, {tail.coordinate.y}");
                }
            }


            return tailCoordinates.Count().ToString();
        }

        public override string part2(string input)
        {
            IEnumerable<Command> commands = input
            .Split("\n")
            .Select(x => parseCommand(x));

            var head = new Knot(0, 0);
            var one = new Knot(0, 0);
            var two = new Knot(0, 0);
            var three = new Knot(0, 0);
            var four = new Knot(0, 0);
            var five = new Knot(0, 0);
            var six = new Knot(0, 0);
            var seven = new Knot(0, 0);
            var eight = new Knot(0, 0);
            var tail = new Knot(0, 0);

            HashSet<string> tailCoordinates = new HashSet<string>();

            foreach (var command in commands)
            {
                for (var i = 0; i < command.distance; i++)
                {
                    head.Move(command.direction);
                    one.MoveToKnot(head, 2);
                    two.MoveToKnot(one, 2);
                    three.MoveToKnot(two, 2);
                    four.MoveToKnot(three, 2);
                    five.MoveToKnot(four, 2);
                    six.MoveToKnot(five, 2);
                    seven.MoveToKnot(six, 2);
                    eight.MoveToKnot(seven, 2);
                    tail.MoveToKnot(eight, 2);
                    tailCoordinates.Add($"{tail.coordinate.x}, {tail.coordinate.y}");
                }
            }


            return tailCoordinates.Count().ToString();
        }

        private Command parseCommand(String line)
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

            public void MoveToKnot(Knot knot, int maxDistance)
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

