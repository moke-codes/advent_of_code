
using static System.Console;
using Node = (string Left, string Right);

if (args is not [var path] || !Path.Exists(path))
{
    WriteLine("Missing input file path. Either parameter was not correctly passed or the file does not exist.");
    return -1;
}

var text = File.ReadAllText(path);
var parts = text.Split($"\r\n\r\n");
var instructions = parts[0];

var nodes = parts[1]
                .Split(Environment.NewLine)
                .Select(l => {
                    var nodeDef = l.Split('=', StringSplitOptions.TrimEntries);
                    var nodeValues = nodeDef[1].Trim('(',')').Split(',', StringSplitOptions.TrimEntries);
                    var node = new Node(nodeValues[0], nodeValues[1]);
                    return new KeyValuePair<string, Node>(nodeDef[0], node);
                })
                .ToDictionary();

int steps = 0;
var goTo = "AAA";
char direction = '0';
if (nodes.ContainsKey(goTo)) {
    do {
        direction = instructions[steps % instructions.Length];
        goTo = direction switch {
            'R' => nodes[goTo].Right,
            'L' => nodes[goTo].Left,
            _ => throw new Exception("Unexpected direction.")
        };
        steps++;
    } while (goTo != "ZZZ");
} else WriteLine("Skipping part 1");

WriteLine($"Part 1 : Steps to got to the end: {steps}");

steps = 0;
var goTos = nodes.Keys.Where(k => k[2] == 'A').ToList();
List<long> completed = [];
while(true) 
{
    direction = instructions[steps % instructions.Length];
    steps++;
    for(int i = 0; i < goTos.Count; i++) {
        if (goTos[i] == "END")
            continue;

        goTos[i] = direction switch {
            'R' => nodes[goTos[i]].Right,
            'L' => nodes[goTos[i]].Left,
            _ => throw new Exception("Unexpected direction.")
        };

        if (goTos[i][2] == 'Z') {
            completed.Add(steps);
            goTos[i] = "END";
        }             
    }

    if (completed.Count == goTos.Count)
        break;
}

WriteLine($"Part 2 : Steps to got to the end: {
    completed.Aggregate(Lcm)}");

return 0;

// Great Common Factor
static long Gcf(long a, long b)
{
    while (b != 0)
    {
        long temp = b;
        b = a % b;
        a = temp;
    }
    return a;
}

// Least Common Multiplier
static long Lcm(long a, long b)
{
    return (a / Gcf(a, b)) * b;
}