Console.WriteLine($"Part 1 answer: {SolveArea(1)}");
Console.WriteLine($"Part 1 answer: {SolveArea(2)}");
return;

long SolveArea(int part) {
    long x = 0;
    long y = 0;

    long area = 0;
    long perimeter = 0;

    long px = 0;
    long py = 0;
    foreach (string line in File.ReadLines("puzzle.txt")) {
        string[] parts = line.Split(' ');
        long hexNumber = Convert.ToInt64(parts[2].Substring(2, parts[2].Length - 3), 16);
        long length = part == 1 ? Convert.ToInt32(parts[1]) : hexNumber / 16;
        char direction = part == 1 ? parts[0][0] : "RDLU"[(int)hexNumber % 16];
        switch (direction) {
            case 'R': x += length; break;
            case 'L': x -= length; break;
            case 'U': y -= length; break;
            case 'D': y += length; break;
        }
        area += px * y - py * x;
        perimeter += length;
        px = x;
        py = y;
    }
    return area / 2 + perimeter / 2 + 1;
}