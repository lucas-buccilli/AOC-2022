using System.Text.RegularExpressions;
using Days;
using static Day12.Executor;

namespace Day12
{
    public class Executor : Day
    {
        public Executor()
        {
        }

        public override string part1(string input)
        {
            Vertex[][] verticies = GetVerticies(input);
            List<Tuple<Vertex, Vertex>> edges = GetEdges(verticies);

            var allVerticies = verticies.SelectMany(x => x);

            Graph graph = new Graph(allVerticies, edges);

            var shortestPath = ShortestPathFunction(graph, allVerticies.Single(x => x.isStart));
            return (shortestPath(allVerticies.Single(x => x.isEnd)).Count() - 1).ToString();
        }

        public override string part2(string input)
        {
            Vertex[][] verticies = GetVerticies(input);
            List<Tuple<Vertex, Vertex>> edges = GetEdges(verticies);

            var allVerticies = verticies.SelectMany(x => x);

            Graph graph = new Graph(allVerticies, edges);
            var paths = allVerticies.Where(x => x.height == 1).Select(x =>
            {
                try
                {
                    var shortestPath = ShortestPathFunction(graph, x);
                    return (shortestPath(allVerticies.Single(x => x.isEnd)).Count());
                }
                catch (KeyNotFoundException)
                {
                    return int.MaxValue;
                }
            });

            return (paths.Min() - 1).ToString();
        }

        private static List<Tuple<Vertex, Vertex>> GetEdges(Vertex[][] verticies)
        {
            var width = verticies[0].Length;
            var height = verticies.Length;
            List<Tuple<Vertex, Vertex>> edges = new List<Tuple<Vertex, Vertex>>();
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    var currentVertex = verticies[i][j];
                    Vertex? above = null;
                    Vertex? below = null;
                    Vertex? left = null;
                    Vertex? right = null;

                    if (j - 1 >= 0)
                    {
                        left = verticies[i][j - 1];
                    }
                    if (j + 1 < width)
                    {
                        right = verticies[i][j + 1];
                    }
                    if (i - 1 >= 0)
                    {
                        above = verticies[i - 1][j];
                    }
                    if (i + 1 < height)
                    {
                        below = verticies[i + 1][j];
                    }

                    if (above != null && (above.height - currentVertex.height <= 1))
                    {
                        edges.Add(Tuple.Create(currentVertex, above));
                    }
                    if (below != null && (below.height - currentVertex.height <= 1))
                    {
                        edges.Add(Tuple.Create(currentVertex, below));
                    }
                    if (left != null && (left.height - currentVertex.height <= 1))
                    {
                        edges.Add(Tuple.Create(currentVertex, left));
                    }
                    if (right != null && (right.height - currentVertex.height <= 1))
                    {
                        edges.Add(Tuple.Create(currentVertex, right));
                    }

                }
            }

            return edges;
        }

        private static Vertex[][] GetVerticies(string input)
        {
            return input.Split("\n")
                .Select(x => x.ToCharArray()
                    .Select(y =>
                    {
                        Vertex vertex;
                        if (y == 'S')
                        {
                            vertex = new Vertex('a');
                            vertex.isStart = true;
                        }
                        else if (y == 'E')
                        {
                            vertex = new Vertex('z');
                            vertex.isEnd = true;
                        }
                        else
                        {
                            vertex = new Vertex(y);
                        }

                        return vertex;
                    }).ToArray())
                 .ToArray();
        }

        public Func<Vertex, IEnumerable<Vertex>> ShortestPathFunction(Graph graph, Vertex start)
        {
            var previous = new Dictionary<Vertex, Vertex>();

            var queue = new Queue<Vertex>();
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                var vertex = queue.Dequeue();
                foreach (var neighbor in graph.AdjacencyList[vertex])
                {
                    if (previous.ContainsKey(neighbor))
                        continue;

                    previous[neighbor] = vertex;
                    queue.Enqueue(neighbor);
                }
            }

            Func<Vertex, IEnumerable<Vertex>> shortestPath = v => {
                var path = new List<Vertex> { };
                var current = v;
                var temp = previous.Where(x => x.Key.isEnd);
                while (!current.Equals(start))
                {
                    path.Add(current);
                    current = previous[current];
                };

                path.Add(start);
                path.Reverse();

                return path;
            };

            return shortestPath;
        }

        public class Vertex
        {
            public int height { get; }
            public char value { get; }
            public bool isStart { get; set; }
            public bool isEnd { get; set; }
            public Vertex(char value)
            {
                this.height = (int)value - 96;
                this.value = value;
            }
        }

        public class Graph
        {
            public Graph() { }
            public Graph(IEnumerable<Vertex> vertices, IEnumerable<Tuple<Vertex, Vertex>> edges)
            {
                foreach (var vertex in vertices)
                    AddVertex(vertex);

                foreach (var edge in edges)
                    AddEdge(edge);
            }

            public Dictionary<Vertex, HashSet<Vertex>> AdjacencyList { get; } = new Dictionary<Vertex, HashSet<Vertex>>();

            public void AddVertex(Vertex vertex)
            {
                AdjacencyList[vertex] = new HashSet<Vertex>();
            }

            public void AddEdge(Tuple<Vertex, Vertex> edge)
            {
                if (AdjacencyList.ContainsKey(edge.Item1) && AdjacencyList.ContainsKey(edge.Item2))
                {
                    AdjacencyList[edge.Item1].Add(edge.Item2);
                }
            }
        }
    }
    
}

