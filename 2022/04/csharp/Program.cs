
if (args is not [var path] || !Path.Exists(path))
{
    Console.WriteLine("Missing input file path. Either parameter was not correctly passed or the file does not exist.");
    return -1;
}

var filePath = args[0];

var pairAssignments = File.ReadAllLines(filePath);

var totalPuzzle1 = 0;
var totalPuzzle2 = 0;
foreach (var pairAssignment in pairAssignments)
{
    var assignments = pairAssignment.Split(',');
    var firstAssignment = assignments[0].Split('-');
    var secondAssignment = assignments[1].Split('-');

    (var firstMin, var firstMax) = GetValues(firstAssignment);
    (var secondMin, var secondMax) = GetValues(secondAssignment);

    if (IsFullyContained(firstMin, firstMax, secondMin, secondMax))
        totalPuzzle1++;

    if (Overlaps(firstMin, firstMax, secondMin, secondMax))
        totalPuzzle2++;
}

Console.WriteLine($"Total (puzzle 1): {totalPuzzle1}");
Console.WriteLine($"Total (puzzle 2): {totalPuzzle2}");

return 0;

static bool IsFullyContained(int firstMin, int firstMax, int secondMin, int secondMax)
{
    return (firstMin >= secondMin && firstMax <= secondMax) ||
        (secondMin >= firstMin && secondMax <= firstMax);
}

static bool Overlaps(int firstMin, int firstMax, int secondMin, int secondMax)
{
    return (firstMin >= secondMin && secondMax >= firstMin) ||
        (secondMin >= firstMin && firstMax >= secondMin);
}

static (int Min, int Max) GetValues(IList<string> assignment)
{
    return (Convert.ToInt32(assignment[0]), Convert.ToInt32(assignment[1]));
}
