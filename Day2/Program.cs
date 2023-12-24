int total1 = 0;
int total2 = 0;
int index = 1;
foreach (string line in File.ReadAllLines("day2.txt")) {

    string[] game = line.Split(':');
    string[] plays = game[1].Split(';');

    bool valid = true;
    int maxBlue = 0;
    int maxRed = 0;
    int maxGreen = 0;
    foreach (string play in plays) {
        IEnumerable<string> gems = play.Split(',').Select(g => g.Trim());
        foreach (string gem in gems) {
            string [] parts = gem.Split(' ');
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
        total1 += index;
    }
    total2 += maxBlue * maxRed * maxGreen;
    index++;
}

Console.WriteLine($"Puzzle 1 answer : Total = {total1}");
Console.WriteLine($"Puzzle 2 answer : Total = {total2}");