using Coord = (int X, int Y);

string[] input = File.ReadAllLines("puzzle.txt");

Console.WriteLine($"Part 1 answer: {Solve(2)}");
Console.WriteLine($"Part 2 answer: {Solve(1000000)}");
return;

long Solve(int expansion) {
    List<Coord> markers = [];
    int y = 0;
    foreach (string line in input) {
        int x = 0;
        foreach (char ch in line) {
            if (ch == '#') {
                markers.Add((x, y));
            }
            x++;
        }
        y++;
    }

    int w = input[0].Length;
    int h = input.Length;

    for (y = h - 1; y > 0; y--) {
        if (markers.Any(m => m.Y == y) && markers.All(m => m.Y != y - 1)) {
            for (int n = 0; n < markers.Count; n++) {
                if (markers[n].Y >= y) {
                    markers[n] = (markers[n].X, markers[n].Y + (expansion - 1));
                }
            }
        }
    }
    for (int x = w - 1; x > 0; x--) {
        if (markers.Any(m => m.X == x) && markers.All(m => m.X != x - 1)) {
            for (int n = 0; n < markers.Count; n++) {
                if (markers[n].X >= x) {
                    markers[n] = (markers[n].X + (expansion - 1), markers[n].Y);
                }
            }
        }
    }
    return markers
        .Aggregate<Coord, long>(0, (current1, p1) => markers
        .Where(p => p != p1)
        .Aggregate(current1, (current, p2) => current + (Math.Abs(p2.Y - p1.Y) + Math.Abs(p2.X - p1.X)))) / 2;
}