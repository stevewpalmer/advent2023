using System.Drawing;
using Block = (int z, int height, System.Drawing.Rectangle rect);

List<Block> blocks = [];
Dictionary<int, HashSet<int>> supportedBy = [];
Dictionary<int, HashSet<int>> supports = [];

foreach (string input in File.ReadAllLines("puzzle.txt")) {
    string[] parts = input.Split('~');
    int[] c1 = parts[0].Split(',').Select(int.Parse).ToArray();
    int[] c2 = parts[1].Split(',').Select(int.Parse).ToArray();
    Rectangle rect = new(c1[0], c1[1], c2[0] - c1[0] + 1, c2[1] - c1[1] + 1);
    blocks.Add((c1[2], c2[2] - c1[2] + 1, rect));
}

blocks.Sort((a, b) => a.z.CompareTo(b.z));

for (int c = 0; c < blocks.Count; c++) {
    int maxZ = 1;
    HashSet<int> blockSupportedBy = [];
    for (int d = c - 1; d >= 0; d--) {
        if (blocks[d].rect.IntersectsWith(blocks[c].rect)) {
            int nbTop = blocks[d].z + blocks[d].height;
            if (nbTop > maxZ) {
                blockSupportedBy = [d];
                maxZ = nbTop;
            }
            if (nbTop == maxZ) {
                blockSupportedBy.Add(d);
            }
        }
    }
    foreach (int d in blockSupportedBy) {
        if (!supports.ContainsKey(d)) {
            supports[d] = [];
        }
        supports[d].Add(c);
        if (!supportedBy.ContainsKey(c)) {
            supportedBy[c] = [];
        }
        supportedBy[c].Add(d);
    }
    blocks[c] = (maxZ, blocks[c].height, blocks[c].rect);
}

long answer1 = 0;
long answer2 = 0;
for (int c = 0; c < blocks.Count; c++) {
    if (!(supports.ContainsKey(c) && supports[c].Any(d => supportedBy[d].Count == 1))) {
        answer1++;
    }
    HashSet<int> chain = [c];
    for (int d = c + 1; d < blocks.Count; d++) {
        if (supportedBy.ContainsKey(d) && supportedBy[d].IsSubsetOf(chain)) {
            chain.Add(d);
        }
    }
    answer2 += chain.Count - 1;
}

Console.WriteLine($"Part 1 answer: {answer1}");
Console.WriteLine($"Part 2 answer: {answer2}");