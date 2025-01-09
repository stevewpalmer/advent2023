using System.Text.RegularExpressions;

Dictionary<string, (string, string)> map = [];

string [] input = File.ReadAllLines("puzzle.txt");
char [] directions = input[0].ToCharArray();

for (int i = 2; i < input.Length; i++) {
    string[] items = Regex.Matches(input[i], "[A-Z][A-Z][A-Z]").Select(c => c.Value).ToArray();
    map.Add(items[0], (items[1], items[2]));
}

long answer1 = WalkFrom("AAA");

string [] nodes = map.Keys.Where(k => k[2] == 'A').ToArray();
long answer2 = nodes.Select(WalkFrom).ToArray().Aggregate((step, v) => step * v / GCD(step, v));

Console.WriteLine($"Part 1 answer: {answer1}");
Console.WriteLine($"Part 2 answer: {answer2}");
return;

long GCD(long n1, long n2) => n2 == 0 ? n1 : GCD(n2, n1 % n2);

long WalkFrom(string node) {
    long steps = 0;
    while (node[2] != 'Z') {
        char direction = directions[steps++ % directions.Length];
        node = direction == 'L' ? map[node].Item1 : map[node].Item2;
    }
    return steps;
}