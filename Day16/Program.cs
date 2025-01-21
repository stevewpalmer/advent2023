using System.Drawing;
using PointAndDirection = (int x, int y, int dx, int dy);

char[][] grid = File.ReadAllLines("puzzle.txt").Select(l => l.ToCharArray()).ToArray();
int w = grid[0].Length;
int h = grid.Length;

long answer1 = Traverse((-1, 0, 1, 0));
long answer2 = 0;
for (int x = 0; x < w; x++) {
    answer2 = Math.Max(answer2, Traverse((x, -1, 0, 1)));
    answer2 = Math.Max(answer2, Traverse((x, h, 0, -1)));
}
for (int y = 0; y < h; y++) {
    answer2 = Math.Max(answer2, Traverse((-1, y, 1, 0)));
    answer2 = Math.Max(answer2, Traverse((w, y, -1, 0)));
}
Console.WriteLine($"Part 1 answer: {answer1}");
Console.WriteLine($"Part 2 answer: {answer2}");
return;

long Traverse(PointAndDirection start) {
    HashSet<PointAndDirection> visited = [];
    Queue<PointAndDirection> queue = [];
    queue.Enqueue(start);
    while (queue.Count > 0) {
        PointAndDirection pt = queue.Dequeue();
        pt.x += pt.dx;
        pt.y += pt.dy;
        if (pt.x < 0 || pt.x >= w || pt.y < 0 || pt.y >= h) {
            continue;
        }
        switch (grid[pt.y][pt.x]) {
            case '|':
                if (pt.dx != 0) {
                    if (visited.Add((pt.x, pt.y, 0, -1))) {
                        queue.Enqueue((pt.x, pt.y, 0, -1));
                    }
                    pt.dx = 0;
                    pt.dy = 1;
                }
                break;

            case '-':
                if (pt.dy != 0) {
                    if (visited.Add((pt.x, pt.y, -1, 0))) {
                        queue.Enqueue((pt.x, pt.y, -1, 0));
                    }
                    pt.dx = 1;
                    pt.dy = 0;
                }
                break;

            case '/':
                (pt.dx, pt.dy) = (-pt.dy, -pt.dx);
                break;

            case '\\':
                (pt.dx, pt.dy) = (pt.dy, pt.dx);
                break;
        }
        if (visited.Add(pt)) {
            queue.Enqueue(pt);
        }
    }
    return visited.Select(p => (p.x, p.y)).Distinct().Count();
}