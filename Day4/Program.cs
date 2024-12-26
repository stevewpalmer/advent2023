Dictionary<int, (int[], int[])> cards = [];
Dictionary<(int, (int[], int[])), int> cache = [];

int cardNumber = 1;
foreach (string line in File.ReadAllLines("puzzle.txt")) {
    string[] parts = line.Substring(line.IndexOf(':') + 1).Split('|');
    int[] winningNumbers = Array.ConvertAll(parts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries), int.Parse);
    int[] cardNumbers = Array.ConvertAll(parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries), int.Parse);
    cards.Add(cardNumber, (winningNumbers, cardNumbers));
    cardNumber++;
}

int answer1 = 0;
foreach ((int[] winningNumbers, int[] cardNumbers) in cards.Values) {
    int count = cardNumbers.Count(winningNumbers.Contains);
    if (count > 0) {
        answer1 += 1 << (count - 1);
    }
}

int answer2 = 0;
foreach (KeyValuePair<int, (int[], int[])> card in cards) {
    answer2 += 1 + CountMatches(card.Key, card.Value);
}

Console.WriteLine($"Part 1 answer: {answer1}");
Console.WriteLine($"Part 2 answer: {answer2}");
return;

int CountMatches(int card, (int[] winningNumbers, int[] cardNumbers) numbers) {
    if (cache.TryGetValue((card, numbers), out int result)) {
        return result;
    }
    int count = numbers.cardNumbers.Count(numbers.winningNumbers.Contains);
    int total = count;
    for (int c = 1; c <= count; c++) {
        total += CountMatches(card + c, cards[card + c]);
    }
    cache.Add((card, numbers), total);
    return total;
}