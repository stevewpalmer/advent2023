int answer1 = 0;
int answer2 = 0;
int index = 1;
foreach (string line in File.ReadAllLines("puzzle.txt")) {

    string[] game = line.Split(':');
    string[] plays = game[1].Split(';');

    bool valid = true;
    int maxBlue = 0;
    int maxRed = 0;
    int maxGreen = 0;
    foreach (string play in plays) {
        IEnumerable<string> gems = play.Split(',').Select(g => g.Trim());
        foreach (string gem in gems) {
            string[] parts = gem.Split(' ');
            int value = Convert.ToInt32(parts[0]);
            switch (parts[1]) {
                case "blue": {
                    maxBlue = Math.Max(maxBlue, value);
                    if (value > 14) {
                        valid = false;
                    }
                    break;
                }
                case "red": {
                    maxRed = Math.Max(maxRed, value);
                    if (value > 12) {
                        valid = false;
                    }
                    break;
                }
                case "green": {
                    maxGreen = Math.Max(maxGreen, value);
                    if (value > 13) {
                        valid = false;
                    }
                    break;
                }
            }
        }
    }
    if (valid) {
        answer1 += index;
    }
    answer2 += maxBlue * maxRed * maxGreen;
    index++;
}

Console.WriteLine($"Part 1 answer : {answer1}");
Console.WriteLine($"Part 2 answer : {answer2}");