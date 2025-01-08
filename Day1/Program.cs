int answer1 = 0;
int answer2 = 0;

string[] numbers = ["zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"];

foreach (string line in File.ReadAllLines("puzzle.txt")) {

    string newline = line;
    for (int index = 0; index < numbers.Length; index++) {
        newline = newline.Replace(numbers[index], $"{numbers[index]}{index}{numbers[index]}");
    }
    answer1 += Convert.ToInt32($"{line.First(char.IsDigit)}{line.Last(char.IsDigit)}");
    answer2 += Convert.ToInt32($"{newline.First(char.IsDigit)}{newline.Last(char.IsDigit)}");
}
Console.WriteLine($"Part 1 answer : {answer1}");
Console.WriteLine($"Part 2 answer : {answer2}");