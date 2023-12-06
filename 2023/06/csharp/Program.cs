


using System.Security.Cryptography;

if (args is not [var path] || !Path.Exists(path))
{
    Console.WriteLine("Missing input file path. Either parameter was not correctly passed or the file does not exist.");
    return -1;
}

var lines = File.ReadAllLines(path);

var times = lines[0]
                    .Split(':')[1]
                    .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => Convert.ToInt32(s))
                    .ToList();

var distRecords = lines[1]
                    .Split(':')[1]
                    .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => Convert.ToInt32(s))
                    .ToList();

List<int> winsPerRace = [];
for (var i = 0; i < times.Count; i++) 
{
    var D = distRecords[i];
    var Tr = times[i];

    // distance = Tb * (Tr - Tb)
    // wins = Tb(n) onde distance > Dtb - Tb(n) * distance <= Dtb
    int wins = Enumerable.Range(0, Tr).Where(Tn => Tn * (Tr - Tn) > D).Count();
    winsPerRace.Add(wins);
}

Console.WriteLine($"Part 1: {winsPerRace.Aggregate((a, b) => a * b)}");

var Tp2 = Convert.ToInt64(
                lines[0]
                    .Split(':')[1]
                    .Replace(" ", string.Empty));

var Dp2 = Convert.ToInt64(
                lines[1]
                    .Split(':')[1]
                    .Replace(" ", string.Empty));

int  winsP2 = 0;
for (long i = 0; i < Tp2; i++)
  winsP2 += i * (Tp2 - i) > Dp2 ? 1 : 0;

Console.WriteLine($"Part 2: {winsP2}");

return 0;