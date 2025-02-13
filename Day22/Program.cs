using System.Drawing;
using Block = (int index, int x, int y, int z, int height, System.Drawing.Rectangle rect);

List<Block> blocks = [];
List<Block> bases = [];
Dictionary<int, List<int>> supportedBy = [];
Dictionary<int, List<int>> supports = [];

int blockIndex = 0;
foreach (string input in File.ReadAllLines("puzzle.txt")) {
    string[] parts = input.Split('~');
    int[] c1 = parts[0].Split(',').Select(int.Parse).ToArray();
    int[] c2 = parts[1].Split(',').Select(int.Parse).ToArray();
    Rectangle rect = new(c1[0], c1[1], c2[0] - c1[0] + 1, c2[1] - c1[1] + 1);
    blocks.Add((blockIndex, c1[0], c1[1], c1[2], c2[2] - c1[2] + 1, rect));
    blockIndex++;
}

foreach (Block b in blocks.OrderBy(b => b.z)) {
    int maxZ = 1;
    foreach (Block nb in bases.OrderByDescending(nb => nb.z + nb.height)) {
        if (nb.rect.IntersectsWith(b.rect)) {
            int nbTop = nb.z + nb.height;
            if (nbTop < maxZ) {
                continue;
            }
            maxZ = nbTop;
            if (!supportedBy.ContainsKey(b.index)) {
                supportedBy[b.index] = [];
            }
            supportedBy[b.index].Add(nb.index);
            if (!supports.ContainsKey(nb.index)) {
                supports[nb.index] = [];
            }
            supports[nb.index].Add(b.index);
        }
    }
    bases.Add((b.index, b.x, b.y, maxZ, b.height, b.rect));
}
long answer1 = 0;
for (int c = 0; c < blockIndex; c++) {
    answer1 += supports.TryGetValue(c, out List<int>? support) && support.Any(d => supportedBy[d].Count == 1) ? 0 : 1;
}
Console.WriteLine($"Part 1 answer: {answer1}");
