using System.Drawing;

char[][] maze = File.ReadAllLines("puzzle.txt").Select(l => l.ToCharArray()).ToArray();
(int dx, int dy)[] directions = [(0, -1), (1, 0), (0, 1), (-1, 0)];
int h = maze.Length;
int w = maze[0].Length;
Point start = new(1, 0);
Point end = new(w - 2, h - 1);

Queue<(Point, List<Point>)> queue = new();
queue.Enqueue((start, []));

int longest = 0;
while (queue.TryDequeue(out var element)) {
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
            if (path.Contains(step)) {
                continue;
            }
            queue.Enqueue((step, [.. path, step]));
        }
    }
}

Console.WriteLine($"Part 1 answer: {longest}");
