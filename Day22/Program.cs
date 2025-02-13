using System.Drawing;
using System.Numerics;
using Block = (int index, System.Numerics.Vector3 pos, System.Numerics.Vector3 size);

List<Block> blocks = [];
List<Block> bases = [];
Dictionary<int, List<int>> supportedBy = [];
Dictionary<int, List<int>> supports = [];

int blockIndex = 0;
foreach (string input in File.ReadAllLines("puzzle.txt")) {
    string[] parts = input.Split('~');
    int[] corner1 = parts[0].Split(',').Select(int.Parse).ToArray();
    int[] corner2 = parts[1].Split(',').Select(int.Parse).ToArray();
    Vector3 p = new(corner1[0], corner1[1], corner1[2]);
    Vector3 r = new(corner2[0] - corner1[0] + 1, corner2[1] - corner1[1] + 1, corner2[2] - corner1[2] + 1);
    blocks.Add((blockIndex, p, r));
    blockIndex++;
}

foreach (Block b in blocks.OrderBy(b => b.pos.Z)) {
    Vector3 restingPosition = new(b.pos.X, b.pos.Y, 1);
    Rectangle bRect = new((int)b.pos.X, (int)b.pos.Y, (int)b.size.X, (int)b.size.Y);

    foreach (Block nb in bases.OrderByDescending(nb => nb.pos.Z + nb.size.Z)) {
        Rectangle nbRect = new((int)nb.pos.X, (int)nb.pos.Y, (int)nb.size.X, (int)nb.size.Y);
        if (nbRect.IntersectsWith(bRect)) {
            float nbTop = nb.pos.Z + nb.size.Z;
            if (nbTop < restingPosition.Z) {
                continue;
            }
            restingPosition.Z = nbTop;
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
    bases.Add((b.index, restingPosition, b.size));
}
long answer1 = 0;
for (int c = 0; c < blockIndex; c++) {
    answer1 += supports.TryGetValue(c, out List<int>? support) && support.Any(d => supportedBy[d].Count == 1) ? 0 : 1;
}
Console.WriteLine($"Part 1 answer: {answer1}");
