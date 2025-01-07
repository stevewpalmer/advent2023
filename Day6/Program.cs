string[] input = File.ReadAllLines("puzzle.txt");
int[] time = input[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToArray();
int[] distance = input[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToArray();

long answer1 = 1;
for (int i = 0; i < time.Length; i++) {
    answer1 *= Solve(time[i], distance[i]);
}

long realTime = long.Parse(string.Join("", time));
long realDistance = long.Parse(string.Join("", distance));
long answer2 = Solve(realTime, realDistance);

Console.WriteLine($"Part 1 answer: {answer1}");
Console.WriteLine($"Part 2 answer: {answer2}");
return;

long Solve(long t, long d) {
    double discriminant = t * t - 4 * d;
    long min = (long)Math.Floor((t - Math.Sqrt(discriminant)) / 2) + 1;
    long max = (long)Math.Ceiling((t + Math.Sqrt(discriminant)) / 2) - 1;
    return max - min + 1;
}