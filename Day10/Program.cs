using System.Drawing;

Dictionary<char, Size> directions = new() {
    { 'U', new Size(0, -1) },
    { 'D', new Size(0, 1) },
    { 'L', new Size(-1, 0) },
    { 'R', new Size(1, 0) }
};
Dictionary<string, char> directionToChar = new() {
    { "UD", '|' },
    { "LR", '-' },
    { "UL", 'J' },
    { "UR", 'L' },
    { "DL", '7' },
    { "DR", 'F' }
};
Dictionary<char, string> charToDirection = directionToChar.ToDictionary(x => x.Value, x => x.Key);

char[][] map = File.ReadAllLines("puzzle.txt").Select(l => l.ToCharArray()).ToArray();
int w = map[0].Length;
int h = map.Length;

Point pt = Point.Empty;
for (int y = 0; y < h; y++) {
    for (int x = 0; x < w; x++) {
        if (map[y][x] == 'S') {
            pt = new Point(x, y);
            break;
        }
    }
}

string results = "";
if (pt.Y > 0 && "|7F".Contains(map[pt.Y - 1][pt.X])) {
    results += 'U';
}
if (pt.Y < h - 1 && "|LJ".Contains(map[pt.Y + 1][pt.X])) {
    results += 'D';
}
if (pt.X > 0 && "-LF".Contains(map[pt.Y][pt.X - 1])) {
    results += 'L';
}
if (pt.X < w - 1 && "-7J".Contains(map[pt.Y][pt.X + 1])) {
    results += 'R';
}
map[pt.Y][pt.X] = directionToChar[results];

Point start = pt;
Point last = pt;
List<Point> path = [pt];
pt = Point.Add(pt, directions[results[0]]);
do {
    path.Add(pt);
    results = charToDirection[map[pt.Y][pt.X]];
    Point n1 = Point.Add(pt, directions[results[0]]);
    Point n2 = Point.Add(pt, directions[results[1]]);
    (pt, last) = n1 == last ? (n2, pt) : (n1, pt);
} while (pt != start);

HashSet<Point> outside = new(path);
for (int y = 0; y < h; y++) {
    (bool inside, bool up) state = (false, false);
    for (int x = 0; x < w; x++) {
        if (path.Contains(new Point(x, y))) {
            state = map[y][x] switch {
                '|' => (!state.inside, state.up),
                'L' => (state.inside, true),
                'F' => (state.inside, false),
                'J' => (!state.up ? !state.inside : state.inside, false),
                '7' => (state.up ? !state.inside : state.inside, false),
                _ => state
            };
        }
        if (!state.inside) {
            outside.Add(new Point(x, y));
        }
    }
}

int answer1 = path.Count / 2;
int answer2 = w * h - outside.Count;

Console.WriteLine($"Part 1 answer: {answer1}");
Console.WriteLine($"Part 2 answer: {answer2}");