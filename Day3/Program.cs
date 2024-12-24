using System.Drawing;

string[] lines = File.ReadAllLines("puzzle.txt");
List<(int, Rectangle)> parts = [];
List<(char, Rectangle)> symbols = [];

int height = lines.Length;
int width = lines[0].Length;
for (int row = 0; row < height; row++) {
    for (int column = 0; column < width; column++) {
        char ch = lines[row][column];
        if (char.IsDigit(ch)) {
            int start = column;
            int number = 0;
            do {
                number = number * 10 + (ch - '0');
                if (++column == width) {
                    break;
                }
                ch = lines[row][column];
            } while (char.IsDigit(ch));
            parts.Add((number, new Rectangle(start, row, column - start, 1)));
            column--;
            continue;
        }
        if (ch != '.') {
            symbols.Add((ch, new Rectangle(column, row, 1, 1)));
        }
    }
}
int answer1 = 0;
int answer2 = 0;
foreach ((char ch, Rectangle rect) in symbols) {
    rect.Inflate(1, 1);
    List<int> numbers = [];
    foreach ((int pn, Rectangle pr) in parts) {
        if (rect.IntersectsWith(pr)) {
            numbers.Add(pn);
        }
    }
    answer1 += numbers.Sum();
    if (ch == '*' && numbers.Count == 2) {
        answer2 += numbers[0] * numbers[1];
    }
}
Console.WriteLine($"Part 1 answer: {answer1}");
Console.WriteLine($"Part 2 answer: {answer2}");