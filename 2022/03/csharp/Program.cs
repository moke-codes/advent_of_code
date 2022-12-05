
if (args is not [var path] || !Path.Exists(path))
{
    Console.WriteLine("Missing input file path. Either parameter was not correctly passed or the file does not exist.");
    return -1;
}

var filePath = args[0];

const string priority = "_abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
var rucksacks = File.ReadAllLines(filePath);
var total = 0;
foreach (var rucksack in rucksacks)
{
    var compartmentSize = rucksack.Length / 2;

    var firstCompartment = rucksack.Take(compartmentSize);
    var secondCompartment = rucksack.TakeLast(compartmentSize);

    var sharedItem = firstCompartment.Intersect(secondCompartment).Single();

    total += priority.IndexOf(sharedItem);
}

Console.WriteLine($"Puzzle 1: Total {total}");

total = 0;
foreach (var group in rucksacks.Chunk(3))
{
    var sharedItems = group[0].Intersect(group[1]);
    var sharedItem = group[2].Intersect(sharedItems).Single();

    total += priority.IndexOf(sharedItem);
}

Console.WriteLine($"Puzzle 2: Total {total}");

return 0;
