string[] input = File.ReadAllLines("puzzle.txt");
int d = input.Length;

char[,] grid = new char[d, d];
for (int y = 0; y < d; y++) {
    for (int x = 0; x < d; x++) {
        grid[x, y] = input[y][x];
    }
}

Console.WriteLine($"Part 1 answer: {Total(Tilt((char[,])grid.Clone()))}");
Console.WriteLine($"Part 2 answer: {Total(Cycle((char[,])grid.Clone()))}");
return;

string Stringify(char[,] g) {
    char[] s = new char[d * d];
    for (int c = 0; c < d * d; c++) {
        s[c] = g[c % d, c / d];
    }
    return new string(s);
}

char[,] Cycle(char[,] g) {
    Dictionary<string, long> seen = [];
    long cycle = 0;
    bool hasCycle = false;
    while (cycle < 1000000000) {
        Rotate(Tilt(g));
        Rotate(Tilt(g));
        Rotate(Tilt(g));
        Rotate(Tilt(g));
        ++cycle;
        if (!hasCycle && seen.TryGetValue(Stringify(g), out long cycleStart)) {
            long cycleLength = cycle - cycleStart;
            cycle += cycleLength * ((1000000000 - cycle) / cycleLength);
            hasCycle = true;
            continue;
        }
        seen[Stringify(g)] = cycle;
    }
    return g;
}

void Rotate(char[,] g) {
    for (int x = 0; x < d / 2; x++) {
        for (int y = x; y < d - x - 1; y++) {
            (char temp, g[x, y]) = (g[x, y], g[y, d - x - 1]);
            (g[y, d - x - 1], g[d - x - 1, d - y - 1]) = (g[d - x - 1, d - y - 1], g[d - y - 1, x]);
            g[d - y - 1, x] = temp;
        }
    }
}

char[,] Tilt(char[,] g) {
    for (int y = 0; y < d; y++) {
        for (int x = 0; x < d; x++) {
            if (g[x, y] == 'O') {
                int m = y;
                while (m > 0 && g[x, m - 1] == '.') {
                    m--;
                }
                g[x, y] = '.';
                g[x, m] = 'O';
            }
        }
    }
    return g;
}

int Total(char[,] g) {
    int total = 0;
    for (int y = 0; y < d; y++) {
        for (int x = 0; x < d; x++) {
            if (g[x, y] == 'O') {
                total += d - y;
            }
        }
    }
    return total;
}