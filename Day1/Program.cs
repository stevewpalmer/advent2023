int total1 = 0;
int total2 = 0;

string [] numbers = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

foreach (string line in File.ReadAllLines("day1.txt")) {

    string newline = line;
    for (int index = 0; index < numbers.Length; index++) {
        newline = newline.Replace(numbers[index], $"{numbers[index]}{index}{numbers[index]}");
    }
    total1 += Convert.ToInt32($"{line.First(char.IsDigit)}{line.Last(char.IsDigit)}");
    total2 += Convert.ToInt32($"{newline.First(char.IsDigit)}{newline.Last(char.IsDigit)}");
}
Console.WriteLine($"Puzzle 1 answer : Total = {total1}");
Console.WriteLine($"Puzzle 2 answer : Total = {total2}");
