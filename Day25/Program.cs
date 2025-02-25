Dictionary<string, HashSet<string>> graph = [];

foreach (string line in File.ReadLines("puzzle.txt")) {
    string[] parts = line.Replace(":", "").Split(' ');
    for (int i = 1; i < parts.Length; i++) {
        if (!graph.ContainsKey(parts[0])) {
            graph.Add(parts[0], []);
        }
        if (!graph.ContainsKey(parts[i])) {
            graph.Add(parts[i], []);
        }
        graph[parts[0]].Add(parts[i]);
        graph[parts[i]].Add(parts[0]);
    }
}

HashSet<string> group1 = [];
HashSet<string> group2 = [];

foreach (HashSet<string> group in graph.Values) {
    group1.UnionWith(group);
}

while (group1.Sum(Counter) != 3) {
    string component = group1.OrderBy(Counter).Last();
    group2.Add(component);
    group1.Remove(component);
}

long answer1 = group1.Count * group2.Count;
Console.WriteLine($"Part 1 answer: {answer1}");
return;

long Counter(string component) => graph[component].Except(group1).Count();
