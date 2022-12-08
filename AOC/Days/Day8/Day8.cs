using System.Text.RegularExpressions;
using Days;

namespace Day8
{
    public class Executor : Day
    {
        public Executor()
        {
        }

        public override string part1(string input)
        {
            IEnumerable<IEnumerable<Tree>> trees = GetTrees(input);
            return trees
                .SelectMany(x => x)
                .Where(x => x.IsVisible())
                .Count()
                .ToString();  
        }

        public override string part2(string input)
        {
            IEnumerable<IEnumerable<Tree>> trees = GetTrees(input);
            return trees
                .SelectMany(x => x)
                .Select(x => x.GetScenicScore())
                .OrderDescending()
                .First()
                .ToString();
        }

        private IEnumerable<IEnumerable<Tree>> GetTrees(string input)
        {
            int[][] treeHeights = input
                .Split("\n")
                .Select(x => x.ToCharArray())
                .Select(x => x
                    .Select(y => int.Parse(y.ToString()))
                    .ToArray())
                .ToArray();

            var inputWidth = treeHeights.ElementAt(0).Length;
            var inputHeight = treeHeights.Length;

            Tree[][] trees = new Tree[inputHeight][];
            for (var i = 0; i < inputHeight; i++)
            {
                trees[i] = new Tree[inputWidth];
                for (var j = 0; j < inputWidth; j++)
                {
                    trees[i][j] = new Tree();
                }
            }


            for (var i = 0; i < inputHeight; i++)
            {
                for (var j = 0; j < inputWidth; j++)
                {
                    var treeAbove = i - 1 < 0 ? null : trees[i - 1][j];
                    var treeBelow = i + 1 == inputHeight ? null : trees[i + 1][j];
                    var treeRight = j + 1 == inputWidth ? null : trees[i][j + 1];
                    var treeLeft = j - 1 < 0 ? null : trees[i][j - 1];
                    trees[i][j].height = treeHeights[i][j];
                    trees[i][j].above = treeAbove;
                    trees[i][j].below = treeBelow;
                    trees[i][j].right = treeRight;
                    trees[i][j].left = treeLeft;
                }
            }
            return trees;
        }



        public class Tree
        {
            public int? height { get; set; }
            public Tree? above { get; set; }
            public Tree? below { get; set; }
            public Tree? left { get; set;  }
            public Tree? right { get; set;  }
            private Boolean? isVisibleFromBelow;
            private Boolean? isVisibleFromAbove;
            private Boolean? isVisibleFromRight;
            private Boolean? isVisibleFromLeft;

            public Tree()
            {
            }

            public Boolean IsVisible()
            {
                if (!height.HasValue)
                {
                    throw new ArgumentException();
                }
                return IsVisibleFromAbove(height.Value) || IsVisibleFromBelow(height.Value) || IsVisibleFromRight(height.Value) || IsVisibleFromLeft(height.Value);
            }

            public Boolean IsVisibleFromAbove(int height)
            {
               return above == null || (above.height < height && above.IsVisibleFromAbove(height));
            }

            public Boolean IsVisibleFromBelow(int height)
            {
                return below == null || (below.height < height && below.IsVisibleFromBelow(height));
            }

            public Boolean IsVisibleFromRight(int height)
            {
                return right == null || (right.height < height && right.IsVisibleFromRight(height));
            }

            public Boolean IsVisibleFromLeft(int height)
            {
                return left == null || (left.height < height && left.IsVisibleFromLeft(height));
            }


            public int GetScenicScore()
            {
                if (!height.HasValue)
                {
                    throw new ArgumentException();
                }
                return GetScenicScoreFromAbove(height.Value) * GetScenicScoreFromBelow(height.Value) * GetScenicScoreFromRight(height.Value) * GetScenicScoreFromLeft(height.Value);
            }

            public int GetScenicScoreFromAbove(int height)
            {
                return above == null ? 0 : height <= above.height ? 1 : 1 + above.GetScenicScoreFromAbove(height);
            }

            public int GetScenicScoreFromBelow(int height)
            {
                return below == null ? 0 : height <= below.height ? 1 : 1 + below.GetScenicScoreFromBelow(height);
            }
            
            public int GetScenicScoreFromRight(int height)
            {
                return right == null ? 0 : height <= right.height ? 1 : 1 + right.GetScenicScoreFromRight(height);
            }

            public int GetScenicScoreFromLeft(int height)
            {
                return left == null ? 0 : height <= left.height ? 1 : 1 + left.GetScenicScoreFromLeft(height);
            }
        }
    }
}

