using Map = (long destRangeStart, long sourceRangeStart, long rangeLength);

List<long> listOfSeeds = [];
List<(string sourceCategory, string destCategory, List<Map>)> maps = [];

string sourceMapName = "";
string destinationMapName = "";
List<(long, long, long)> mappings = [];

foreach (string line in File.ReadAllLines("puzzle.txt")) {
    string[] parts = line.Split(' ');
    if (parts[0] == "") {
        if (mappings.Count > 0) {
            maps.Add((sourceMapName, destinationMapName, mappings));
        }
        sourceMapName = "";
        destinationMapName = "";
        mappings = [];
        continue;
    }
    if (parts[0] == "seeds:") {
        listOfSeeds = parts.Skip(1).Select(long.Parse).ToList();
        continue;
    }
    if (parts[1] == "map:") {
        string[] targets = parts[0].Split("-to-");
        sourceMapName = targets[0];
        destinationMapName = targets[1];
        continue;
    }
    if (sourceMapName != "" && destinationMapName != "") {
        long[] entry = parts.Select(long.Parse).ToArray();
        mappings.Add((entry[0], entry[1], entry[2]));
    }
}
if (mappings.Count > 0) {
    maps.Add((sourceMapName, destinationMapName, mappings));
}

long answer1 = long.MaxValue;
long answer2 = long.MaxValue;

for (int c = 0; c < listOfSeeds.Count; c += 2) {
    answer1 = MapSourceTarget(answer1, "seed", listOfSeeds[c], 1);
    answer1 = MapSourceTarget(answer1, "seed", listOfSeeds[c + 1], 1);
    answer2 = MapSourceTarget(answer2, "seed", listOfSeeds[c], listOfSeeds[c + 1]);
}

Console.WriteLine($"Part 1 answer: {answer1}");
Console.WriteLine($"Part 2 answer: {answer2}");
return;

long MapSourceTarget(long min, string sourceCategory, long seed, long range) {
    if (sourceCategory != "location") {
        (string sourceCategory, string destCategory, List<Map> maps) mapper = maps.First(m => m.sourceCategory == sourceCategory);
        foreach (Map map in mapper.maps) {
            long sourceRangeEnd = map.sourceRangeStart + map.rangeLength;
            long diff = map.destRangeStart - map.sourceRangeStart;
            long seedEnd = seed + range;
            if (seedEnd > map.sourceRangeStart && sourceRangeEnd > seed) {
                if (seed < map.sourceRangeStart) {
                    min = MapSourceTarget(min, mapper.sourceCategory, seed, map.sourceRangeStart - seed);
                    seed = map.sourceRangeStart;
                }
                if (sourceRangeEnd < seedEnd) {
                    min = MapSourceTarget(min, mapper.sourceCategory, sourceRangeEnd, seedEnd - sourceRangeEnd);
                    seedEnd = sourceRangeEnd;
                }
                seed += diff;
                seedEnd += diff;
                range = seedEnd - seed;
                break;
            }
        }
        return MapSourceTarget(min, mapper.destCategory, seed, range);
    }
    return Math.Min(min, seed);
}