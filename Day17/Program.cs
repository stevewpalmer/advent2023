using System.Drawing;
using PathStep = (System.Drawing.Point pt, int dt);

(int dx, int dy)[] directions = [(0, -1), (1, 0), (0, 1), (-1, 0)];

int[][] grid = File.ReadAllLines("puzzle.txt")
    .Select(l => l.ToCharArray()
        .Select(c => c - '0')
        .ToArray())
    .ToArray();
int h = grid.Length;
int w = grid[0].Length;

Point start = new(0, 0);
Point end = new(w - 1, h - 1);

Console.WriteLine($"Part 1 answer: {CalculatePath(0, 3)}");
Console.WriteLine($"Part 2 answer: {CalculatePath(3, 10)}");
return;

long CalculatePath(int minimum, int maximum) {
    PriorityQueue<PathStep, int> queue = new();
    HashSet<(Point, int)> visited = [];
    queue.Enqueue((start, 1), 0);

    int best = int.MaxValue;
    while (queue.TryDequeue(out PathStep element, out int cost)) {
        if (!visited.Add((element.pt, element.dt))) {
            continue;
        }
        if (element.pt == end) {
            best = cost;
            break;
        }
        foreach (int d in (int[]) [1, -1, 0]) {
            int nd = (element.dt + d % 4 + 4) % 4;
            int nextCost = cost;
            Point step = element.pt;
            if (nd == element.dt) {
                continue;
            }
            for (int i = 0; i < maximum; ++i) {
                step = new Point(step.X + directions[nd].dx, step.Y + directions[nd].dy);
                if (step is { X: >= 0, Y: >= 0 } && step.X < w && step.Y < h) {
                    nextCost += grid[step.Y][step.X];
                    if (i >= minimum) {
                        queue.Enqueue((step, nd), nextCost);
                    }
                }
            }
        }
    }
    return best;
}
