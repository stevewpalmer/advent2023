using System.Drawing;

char[][] maze = File.ReadAllLines("puzzle.txt").Select(l => l.ToCharArray()).ToArray();
(int, int)[] directions = [(0, -1), (1, 0), (0, 1), (-1, 0)];
Point start = Point.Empty;

int h = maze.Length;
int w = maze[0].Length;

for (int r = 0; r < h; r++) {
    for (int c = 0; c < w; c++) {
        if (maze[r][c] == 'S') {
            start = new Point(c, r);
        }
    }
}

Console.WriteLine($"Part 1 answer: {Walk(64)}");
return;

long Walk(int maxSteps) {
    Queue<(Point, int)> queue = new();
    Dictionary<Point, int> visited = [];
    queue.Enqueue((start, 0));

    while (queue.TryDequeue(out (Point, int) element)) {
        (Point pt, int steps) = element;
        if (!visited.TryAdd(pt, steps)) {
            continue;
        }
        if (steps == maxSteps + 1) {
            break;
        }
        foreach ((int dx, int dy) d in directions) {
            Point step = new(pt.X + d.dx, pt.Y + d.dy);
            if (step is { X: >= 0, Y: >= 0 } && step.X < w && step.Y < h && "S.".Contains(maze[step.Y][step.X])) {
                queue.Enqueue((step, steps + 1));
            }
        }
    }
    return visited.Values.Count(v => v % 2 == 0);
}

