string[] parts = File.ReadAllText("puzzle.txt").Split(',');

long answer1 = parts.Aggregate<string, long>(0, (current, part) => current + Hash(part));
Console.WriteLine($"Part 1 answer: {answer1}");
return;

int Hash(string s) => s.Aggregate(0, (current, ch) => (current + ch) * 17 % 256);