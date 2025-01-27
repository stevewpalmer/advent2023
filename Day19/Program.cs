using System.Text.RegularExpressions;

string input = File.ReadAllText("puzzle.txt");
string[][] parts = input
    .Replace('{', ' ')
    .Replace('}', ' ')
    .Split("\n\n")
    .Select(l => l.Split('\n', StringSplitOptions.TrimEntries))
    .ToArray();

Dictionary<string, string[]> rules = [];
foreach (string line in parts[0]) {
    string[] ruleParts = line.Split(' ');
    rules[ruleParts[0]] = ruleParts[1].Split(',');
}

long answer1 = parts[1]
    .Select(line => line.Split(',')
        .Select(varPart => varPart.Split('='))
        .ToDictionary(var => var[0], var => long.Parse(var[1])))
    .Where(vars => Eval("in", vars))
    .Sum(vars => vars.Keys.Select(v => vars[v]).Sum());

long answer2 = Eval2("in", new Dictionary<string, (int, int)> {
    { "s", (1, 4000) },
    { "m", (1, 4000) },
    { "a", (1, 4000) },
    { "x", (1, 4000) }
}).Sum(result => result.Aggregate<(int, int), long>(1, (current, set) => current * (set.Item2 - set.Item1 + 1)));

Console.WriteLine($"Part 1 answer: {answer1}");
Console.WriteLine($"Part 2 answer: {answer2}");
return;

bool Eval(string workflow, Dictionary<string, long> vars) {
    if (workflow is "A" or "R") {
        return workflow == "A";
    }
    foreach (string rule in rules[workflow]) {
        GroupCollection p = Regex.Matches(rule, @"([a-zAR]+)(([<|>])(\d+):([a-zAR]+))*")[0].Groups;
        if (p[2].Value == "") {
            return Eval(rule, vars);
        }
        long value = long.Parse(p[4].Value);
        if (p[3].Value == "<" && vars[p[1].Value] < value) {
            return Eval(p[5].Value, vars);
        }
        if (p[3].Value == ">" && vars[p[1].Value] > value) {
            return Eval(p[5].Value, vars);
        }
    }
    return false;
}

List<List<(int, int)>> Eval2(string workflow, Dictionary<string, (int, int)> ranges) {
    if (workflow is "A" or "R") {
        return workflow == "A" ? [ranges.Values.ToList()] : [];
    }
    List<List<(int, int)>> results = [];
    foreach (string rule in rules[workflow]) {
        GroupCollection p = Regex.Matches(rule, @"([a-zAR]+)(([<|>])(\d+):([a-zAR]+))*")[0].Groups;
        if (p[2].Value == "") {
            results.AddRange(Eval2(rule, ranges));
            return results;
        }
        int value = int.Parse(p[4].Value);
        (int start, int end) = ranges[p[1].Value];
        (int a1, int a2, int r1, int r2) = p[3].Value == "<" ? (1, value - 1, value, 4000) : (value + 1, 4000, 1, value);
        results.AddRange(Eval2(p[5].Value, new Dictionary<string, (int, int)>(ranges) {
            [p[1].Value] = (Math.Max(a1, start), Math.Min(a2, end))
        }));
        ranges[p[1].Value] = (Math.Max(r1, start), Math.Min(r2, end));
    }
    return results;
}