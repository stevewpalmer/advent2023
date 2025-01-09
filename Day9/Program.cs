long answer1 = 0;
long answer2 = 0;
foreach (string input in File.ReadLines("puzzle.txt")) {
    long[] data = input.Split(' ').Select(long.Parse).ToArray();
    answer1 += Extrapolate(data);
    answer2 += Extrapolate(data, true);
}
Console.WriteLine($"Part 1 answer: {answer1}");
Console.WriteLine($"Part 2 answer: {answer2}");
return;

long Extrapolate(long[] data, bool part2 = false) {
    if (data.Any(d => d != 0)) {
        List<long> next = [];
        for (int d = 0; d < data.Length - 1; d++) {
            next.Add(data[d + 1] - data[d]);
        }
        long result = Extrapolate(next.ToArray(), part2);
        return part2 ? data[0] - result : data[^1] + result;
    }
    return 0L;
}