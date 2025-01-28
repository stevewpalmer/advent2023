using ActionSequence = (string name, bool pulse);
using Module = (int type, bool state, System.Collections.Generic.List<string> inputs, System.Collections.Generic.List<string> outputs);

const int FLIPFLOP = 1;
const int CONJUNCTION = 2;

List<ActionSequence> broadcaster = [];
Dictionary<string, Module> modules = [];

foreach (string line in File.ReadAllLines("puzzle.txt")) {
    string[] parts = line.Split(" -> ");
    string name = parts[0];
    List<string> connections = parts[1].Split(',', StringSplitOptions.TrimEntries).ToList();
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

long lowPulses = 0;
long highPulses = 0;

for (int i = 0; i < 1000; i++) {
    Queue<ActionSequence> actions = new(broadcaster);
    lowPulses += 1 + broadcaster.Count;
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
Console.WriteLine($"Part 1 answer: {lowPulses * highPulses}");
