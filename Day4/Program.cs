int points = 0;
foreach (string line in File.ReadAllLines("puzzle.txt")) {
    string [] parts = line.Substring(line.IndexOf(':') + 1).Split('|');
    int [] winningNumbers = Array.ConvertAll(parts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries), int.Parse);
    int [] cardNumbers = Array.ConvertAll(parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries), int.Parse);
    int count = cardNumbers.Count(c => winningNumbers.Contains(c));
    if (count > 0) {
        points += 1 << (count - 1);
    }
}

Console.WriteLine($"Part 1 answer: {points}");
