using HailStone = (long x, long y, long z, long dx, long dy, long dz);
List<HailStone> stones = [];

const long MinSquare = 200000000000000;
const long MaxSquare = 400000000000000;

foreach (string input in File.ReadAllLines("puzzle.txt")) {
    long[] values = input.Replace('@', ',').Split(',').Select(long.Parse).ToArray();
    stones.Add((values[0], values[1], values[2], values[3], values[4], values[5]));
}

long answer1 = 0;
for (int i = 0; i < stones.Count; i++) {
    for (int j = i + 1; j < stones.Count; j++) {
        long determinant = stones[j].dx * stones[i].dy - stones[j].dy * stones[i].dx;
        if (determinant != 0) {
            long t1 = ((stones[j].y - stones[i].y) * stones[j].dx - (stones[j].x - stones[i].x) * stones[j].dy) / determinant;
            long t2 = ((stones[j].y - stones[i].y) * stones[i].dx - (stones[j].x - stones[i].x) * stones[i].dy) / determinant;
            if (t1 >= 0 && t2 >= 0) {
                long px = stones[j].x + stones[j].dx * t2;
                long py = stones[j].y + stones[j].dy * t2;
                if (px is >= MinSquare and <= MaxSquare && py is >= MinSquare and <= MaxSquare) {
                    ++answer1;
                }
            }
        }
    }
}
Console.WriteLine($"Part 1 answer {answer1}");
