using System.Drawing;

List<Point> points = [];

int x = 0;
int y = 0;

points.Add(new Point(x, y));
foreach (string line in File.ReadLines("puzzle.txt")) {
    string[] parts = line.Split(' ');
    int length = int.Parse(parts[1]);
    switch (parts[0][0]) {
        case 'R': x += length; break;
        case 'L': x -= length; break;
        case 'U': y -= length; break;
        case 'D': y += length; break;
    }
    points.Add(new Point(x, y));
}
long area = 0;
long perm = 0;
for (int i = 0; i < points.Count - 1; i++) {
    int px1 = points[i].X;
    int py1 = points[i].Y;
    int px2 = points[i + 1].X;
    int py2 = points[i + 1].Y;
    area += px1 * py2 - py1 * px2;
    perm += GCD(Math.Abs(px2 - px1), Math.Abs(py2 - py1));
}
long answer1 = area / 2 + perm / 2 + 1;
Console.WriteLine($"Part 1 answer: {answer1}");
return;

int GCD(int val1, int val2) {
    while (val2 != 0) {
        (val1, val2) = (val2, val1 % val2);
    }
    return val1;
}