long answer1 = 0;
long answer2 = 0;

foreach (string s in File.ReadAllText("puzzle.txt").Split("\n\n")) {
    string[] vgrid = s.Split('\n');
    long result1 = 100 * FindSplit(vgrid);
    long result2 = 100 * FindSplit(vgrid, true);
    string[] hgrid = Enumerable.Range(0, vgrid[0].Length)
        .Select(n => string.Concat(vgrid.Select(t1 => t1[n])))
        .ToArray();
    if (result1 == 0) {
        result1 = FindSplit(hgrid);
    }
    if (result2 == 0) {
        result2 = FindSplit(hgrid, true);
    }
    answer1 += result1;
    answer2 += result2;
}
Console.WriteLine($"Part 1 answer: {answer1}");
Console.WriteLine($"Part 2 answer: {answer2}");
return;

int Diff(string s1, string s2) => s1.Where((t, c) => t != s2[c]).Count();

long FindSplit(string[] grid, bool addSmudge = false) {
    int match = 0;
    int row = 0;
    while (match == 0 && row < grid.Length - 1) {
        int diff = Diff(grid[row], grid[row + 1]);
        if (diff == 0 || (diff == 1 && addSmudge)) {
            bool smudge = diff == 0 && addSmudge;
            match = 1;
            for (int p = 1; match > 0 && row - p >= 0 && row + p < grid.Length - 1; p++) {
                diff = Diff(grid[row - p], grid[row + p + 1]);
                if (diff == 0) {
                    continue;
                }
                if (diff == 1 && smudge) {
                    smudge = false;
                    continue;
                }
                match = 0;
            }
            if (match == 1 && addSmudge && smudge) {
                match = 0;
            }
        }
        row++;
    }
    return row * match;
}