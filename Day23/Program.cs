using System.Drawing;

char[][] maze = File.ReadAllLines("puzzle.txt").Select(l => l.ToCharArray()).ToArray();
(int dx, int dy)[] directions = [(0, -1), (1, 0), (0, 1), (-1, 0)];
int h = maze.Length;
int w = maze[0].Length;
Point start = new(1, 0);
Point end = new(w - 2, h - 1);

Dictionary<Point, HashSet<(Point, int)>> graph = new();

Console.WriteLine($"Part 1 answer: {Walk()}");
Console.WriteLine($"Part 2 answer: {ReduceAndWalk()}");
return;

long Walk() {
    Queue<(Point, List<Point>)> queue = new();
    queue.Enqueue((start, []));

    long longest = 0;
    while (queue.TryDequeue(out (Point, List<Point>) element)) {
        (Point pt, List<Point> path) = element;
        if (pt == end && path.Count > longest) {
            longest = path.Count;
        }
        foreach ((int dx, int dy) in directions) {
            Point step = new(pt.X + dx, pt.Y + dy);
            if (step.Y >= 0 && step.Y < h && maze[step.Y][step.X] != '#') {
                if (maze[pt.Y][pt.X] == '>' && dx != 1) {
                    continue;
                }
                if (maze[pt.Y][pt.X] == '<' && dx != -1) {
                    continue;
                }
                if (maze[pt.Y][pt.X] == 'v' && dy != 1) {
                    continue;
                }
                if (maze[pt.Y][pt.X] == '^' && dy != -1) {
                    continue;
                }
                if (!graph.ContainsKey(pt)) {
                    graph[pt] = [];
                }
                graph[pt].Add((step, 1));
                if (!graph.ContainsKey(step)) {
                    graph[step] = [];
                }
                graph[step].Add((pt, 1));
                if (path.Contains(step)) {
                    continue;
                }
                queue.Enqueue((step, [.. path, step]));
            }
        }
    }
    return longest;
}

long ReduceAndWalk() {
    bool reduced;
    do {
        reduced = false;
        foreach (Point node in graph.Keys) {
            if (graph[node].Count == 2) {
                (Point, int) connectionA = graph[node].First();
                (Point, int) connectionB = graph[node].Last();
                int totalLength = connectionA.Item2 + connectionB.Item2;
                graph[connectionA.Item1].Remove((node, connectionA.Item2));
                graph[connectionB.Item1].Remove((node, connectionB.Item2));
                graph[connectionA.Item1].Add((connectionB.Item1, totalLength));
                graph[connectionB.Item1].Add((connectionA.Item1, totalLength));
                graph.Remove(node);
                reduced = true;
                break;
            }
        }
    } while (reduced);

    Queue<(Point, List<Point>, long)> queue = new();
    queue.Enqueue((start, [], 0));

    long longest = 0;
    while (queue.TryDequeue(out (Point, List<Point>, long) element)) {
        (Point pt, List<Point> path, long distance) = element;
        if (pt == end && distance > longest) {
            longest = distance;
        }
        foreach ((Point step, long length) in graph[pt]) {
            if (!path.Contains(step)) {
                queue.Enqueue((step, [.. path, step], length + distance));
            }
        }
    }
    return longest;
}