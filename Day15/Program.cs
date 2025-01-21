using Item = (string label, int focalLength);
string[] steps = File.ReadAllText("puzzle.txt").Split(',');
List<Item>[] a = new List<Item>[256];

long answer1 = steps.Aggregate<string, long>(0, (current, part) => current + Hash(part));

foreach (string step in steps) {
    string[] parts = step.Split('-', '=');
    int box = Hash(parts[0]);
    if (step.Contains('-')) {
        a[box]?.RemoveAll(l => l.label == parts[0]);
    }
    if (step.Contains('=')) {
        a[box] ??= [];
        int current = a[box].FindIndex(l => l.label == parts[0]);
        if (current >= 0) {
            a[box][current] = (parts[0], int.Parse(parts[1]));
        }
        else {
            a[box].Add((parts[0], int.Parse(parts[1])));
        }
    }
}
long answer2 = 0;
for (int i = 0; i < 256; i++) {
    for (int c = 0; c < a[i]?.Count; c++) {
        answer2 += (i + 1) * (c + 1) * a[i][c].focalLength;
    }
}

Console.WriteLine($"Part 1 answer: {answer1}");
Console.WriteLine($"Part 2 answer: {answer2}");
return;

int Hash(string s) => s.Aggregate(0, (current, ch) => (current + ch) * 17 % 256);