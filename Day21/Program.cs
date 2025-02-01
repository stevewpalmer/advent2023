using System.Drawing;

char[][] maze = File.ReadAllLines("puzzle.txt").Select(l => l.ToCharArray()).ToArray();
(int, int)[] directions = [(0, -1), (1, 0), (0, 1), (-1, 0)];
Dictionary<Point, int> visited = [];
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

long answer1 = Walk();

long totalSquares = (26501365 - w / 2) / w;

long even = visited.Values.Count(v => v % 2 == 0);
long odd = visited.Values.Count(v => v % 2 == 1);

long evenCorners = visited.Values.Count(v => v % 2 == 0 && v > 65);
long oddCorners = visited.Values.Count(v => v % 2 == 1 && v > 65);

long answer2 = totalSquares * totalSquares * (odd + even) + totalSquares * (2 * odd - oddCorners + evenCorners) + (odd - oddCorners);

Console.WriteLine($"Part 1 answer: {answer1}");
Console.WriteLine($"Part 2 answer: {answer2}");
return;

long Walk() {
    Queue<(Point, int)> queue = new();
    queue.Enqueue((start, 0));

    while (queue.TryDequeue(out (Point, int) element)) {
        (Point pt, int steps) = element;
        if (!visited.TryAdd(pt, steps)) {
            continue;
        }
        foreach ((int dx, int dy) d in directions) {
            Point step = new(pt.X + d.dx, pt.Y + d.dy);
            if (step is { X: >= 0, Y: >= 0 } && step.X < w && step.Y < h && "S.".Contains(maze[step.Y][step.X])) {
                queue.Enqueue((step, steps + 1));
            }
        }
    }
    return visited.Values.Count(v => v < 65 && v % 2 == 0);
}