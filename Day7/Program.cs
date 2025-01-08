using CardSet = (string value, long bid);

long answer1 = Solve(false);
long answer2 = Solve(true);

Console.WriteLine($"Part 1 answer: {answer1}");
Console.WriteLine($"Part 2 answer: {answer2}");
return;

long Solve(bool hasJoker) {
    string values = hasJoker ? "J23456789TQKA" : "23456789TJQKA";
    const string hex = "0123456789ABCDEF";

    List<CardSet> allCards = [];
    foreach (string hand in File.ReadLines("puzzle.txt")) {
        string[] parts = hand.Split(' ');

        int[] totals = new int [16];
        string rawValue = "";
        foreach (int ch in parts[0].Select(card => values.IndexOf(card))) {
            rawValue += hex[ch];
            ++totals[ch];
        }
        int numberOfJokers = hasJoker ? hand.Count(c => c == 'J') : 0;
        totals[0] -= numberOfJokers;
        int[] orderedTotals = totals.Where(c => c > 0).OrderByDescending(c => c).ToArray();
        if (orderedTotals.Length == 0) {
            orderedTotals = [5];
        }
        else {
            orderedTotals[0] += numberOfJokers;
        }
        string value = string.Join("", orderedTotals).PadRight(5, '0') + rawValue;
        allCards.Add((value, long.Parse(parts[1])));
    }
    int rank = 1;
    return allCards.OrderBy(c => c.value).Select(c => c.bid).Sum(bid => rank++ * bid);
}