using ActionSequence = (string name, bool pulse);
using Module = (int type, bool state, System.Collections.Generic.List<string> inputs, System.Collections.Generic.List<string> outputs);

const int FLIPFLOP = 1;
const int CONJUNCTION = 2;

List<ActionSequence> broadcaster = [];
Dictionary<string, Module> modules = [];
Dictionary<string, long> rxCounter = [];

long lowPulses = 0;
long highPulses = 0;
long totalPresses = 0;
string rxInput = "";

foreach (string line in File.ReadAllLines("puzzle.txt")) {
    string[] parts = line.Split(" -> ");
    string name = parts[0];
    List<string> connections = parts[1].Split(',', StringSplitOptions.TrimEntries).ToList();
    if (connections.Contains("rx")) {
        rxInput = name[1..];
    }
    if (name[0] == '%') {
        modules[name[1..]] = (FLIPFLOP, false, [], connections);
        continue;
    }
    if (name[0] == '&') {
        modules[name[1..]] = (CONJUNCTION, false, [], connections);
        continue;
    }
    broadcaster = connections.Select(c => (c, false)).ToList();
}
foreach (KeyValuePair<string, Module> module in modules) {
    foreach (string connection in module.Value.outputs) {
        if (modules.TryGetValue(connection, out Module conjunction)) {
            conjunction.inputs.Add(module.Key);
        }
    }
}

for (int i = 0; i < 1000; i++) {
    PressButton();
}
ResetState();
while (rxCounter.Count < 4) {
    PressButton();
}
Console.WriteLine($"Part 1 answer: {lowPulses * highPulses}");
Console.WriteLine($"Part 1 answer: {rxCounter.Values.Aggregate(LCM)}");
return;

static long LCM(long a, long b) => Math.Abs(a * b) / GCD(a, b);
static long GCD(long a, long b) => b == 0 ? a : GCD(b, a % b);

void ResetState() {
    foreach (KeyValuePair<string, Module> module in modules) {
        modules[module.Key] = (module.Value.type, false, module.Value.inputs, module.Value.outputs);
    }
    totalPresses = 0;
}

void PressButton() {
    Queue<ActionSequence> actions = new(broadcaster);
    lowPulses += 1 + broadcaster.Count;
    ++totalPresses;
    while (actions.TryDequeue(out ActionSequence action)) {
        if (!modules.TryGetValue(action.name, out Module module)) {
            continue;
        }
        if (module.type == FLIPFLOP) {
            if (action.pulse) {
                continue;
            }
            module.state = !module.state;
            modules[action.name] = (FLIPFLOP, module.state, module.inputs, module.outputs);
        }
        if (module.type == CONJUNCTION) {
            List<bool> states = module.inputs.Select(connection => modules[connection].state).ToList();
            module.state = !states.All(s => s);
            modules[action.name] = (CONJUNCTION, module.state, module.inputs, module.outputs);
        }
        foreach (KeyValuePair<string, Module> mod in modules.Where(m => m.Value.outputs.Contains(rxInput))) {
            if (mod.Value.state) {
                rxCounter.TryAdd(mod.Key, totalPresses);
            }
        }
        foreach (string c in module.outputs) {
            actions.Enqueue((c, module.state));
            if (module.state) {
                ++highPulses;
            }
            else {
                ++lowPulses;
            }
        }
    }
}