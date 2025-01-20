long answer1 = 0;
long answer2 = 0;

Dictionary<(int, int), long> cache;

foreach (string input in File.ReadLines("puzzle.txt")) {
    cache = [];
    string[] parts = input.Split(' ');
    int[] counts = parts[1].Split(',').Select(int.Parse).ToArray();
    answer1 += TryArrange(parts[0] + '?', counts);

    cache = [];
    string record5 = string.Concat(Enumerable.Repeat(parts[0] + '?', 5));
    int[] counts5 = Enumerable.Repeat(counts, 5).SelectMany(c => c).ToArray();
    answer2 += TryArrange(record5, counts5);
}

Console.WriteLine($"Part 1 answer: {answer1}");
Console.WriteLine($"Part 2 answer: {answer2}");
return;

long TryArrange(string record, int[] counts, int recordPos = 0, int countPos = 0) {
    if (cache.ContainsKey((recordPos, countPos))) {
        return cache[(recordPos, countPos)];
    }
    if (recordPos == record.Length) {
        return countPos == counts.Length ? 1 : 0;
    }
    long result = 0;
    if (record[recordPos] == '.' || record[recordPos] == '?') {
        result += TryArrange(record, counts, recordPos + 1, countPos);
    }
    if (countPos < counts.Length) {
        int end = recordPos + counts[countPos];
        if (end < record.Length && record[end] != '#' && !record.Substring(recordPos, counts[countPos]).Contains('.')) {
            result += TryArrange(record, counts, end + 1, countPos + 1);
        }
    }
    cache[(recordPos, countPos)] = result;
    return result;
}