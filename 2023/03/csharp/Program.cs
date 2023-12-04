using Part = (int number, int line, int start, int end);
using Gear = (int line, int column);

if (args is not [var path] || !Path.Exists(path))
{
    Console.WriteLine("Missing input file path. Either parameter was not correctly passed or the file does not exist.");
    return -1;
}

var lines = File.ReadAllLines(path);

// Find numbers in line and their bounds (first digit position, last digit position)
// Verify surroundings for symbols (any value that is not a digit or '.')
// If is there a symbol around, sum it up
List<Part> parts = new();
Dictionary<Gear, List<Part>> gears = new();

for (int lineNbr = 0; lineNbr < lines.Length; lineNbr++)
{
    string number = string.Empty;
    int start = -1;
    for (int columnNbr = 0; columnNbr < lines[lineNbr].Length; columnNbr++)
    {
        var character = lines[lineNbr][columnNbr];
        if (char.IsDigit(character))
        {
            if (number == string.Empty)
                start = columnNbr;

            number += character;

            if (columnNbr == lines[lineNbr].Length - 1) {
                parts.Add(
                    new(Convert.ToInt32(number), lineNbr, start, columnNbr)
                );

                start = -1;
                number = string.Empty;
            }
        }
        else
        {
            if (number != string.Empty)
            {
                parts.Add(
                    new(Convert.ToInt32(number), lineNbr, start, columnNbr - 1)
                );

                start = -1;
                number = string.Empty;
            }
        }
    }
}

int sum = 0;
foreach (var part in parts)
{
    bool symbolFound = false;
    var leftBoundary = part.start == 0 ? -1 : part.start - 1;
    var rightBoundary = part.end == lines[0].Length - 1 ? -1 : part.end + 1;
    var upperBoundary = part.line == 0 ? -1 : part.line - 1;
    var bottomBoundary = part.line == lines.Length - 1 ? -1 : part.line + 1;

    // Look above
    if (upperBoundary != -1)
    {
        int lookStart = leftBoundary == -1 ? part.start : leftBoundary;
        int lookEnd = rightBoundary == -1 ? part.end : rightBoundary;
        int column = lookStart;
        do
        {
            var character = lines[part.line - 1][column];
            if (!char.IsDigit(character) && character != '.') {
                symbolFound = true;

                if (character == '*') {
                    Gear gear = new(part.line - 1, column);
                    if (gears.TryGetValue(gear, out var gearParts)) {
                        gearParts.Add(part);
                    } else {
                        gears.Add(gear, [ part ]);
                    }
                }
            }
            column++;
        } while (column <= lookEnd);
    }

    // Look left
    if (leftBoundary != -1)
    {
        int column = leftBoundary == -1 ? part.start : leftBoundary;
        var character = lines[part.line][column];
        if (!char.IsDigit(character) && character != '.') {
            symbolFound = true;
                
            if (character == '*') {
                Gear gear = new(part.line, column);
                if (gears.TryGetValue(gear, out var gearParts)) {
                    gearParts.Add(part);
                } else {
                    gears.Add(gear, [ part ]);
                }
            }
        }
    }

    // Look right
    if (!symbolFound && rightBoundary != -1)
    {
        int column = rightBoundary == -1 ? part.end : rightBoundary;
        var character = lines[part.line][column];
        if (!char.IsDigit(character) && character != '.') {
            symbolFound = true;
                
            if (character == '*') {
                Gear gear = new(part.line, column);
                if (gears.TryGetValue(gear, out var gearParts)) {
                    gearParts.Add(part);
                } else {
                    gears.Add(gear, [ part ]);
                }
            }
        }
    }

    // Look below
    if (!symbolFound && bottomBoundary != -1)
    {
        int lookStart = leftBoundary == -1 ? part.start : leftBoundary;
        int lookEnd = rightBoundary == -1 ? part.end : rightBoundary;
        int column = lookStart;

        do
        {
            var character = lines[part.line + 1][column];
            if (!char.IsDigit(character) && character != '.') {
                symbolFound = true;
                    
                if (character == '*') {
                    Gear gear = new(part.line + 1, column);
                    if (gears.TryGetValue(gear, out var gearParts)) {
                        gearParts.Add(part);
                    } else {
                        gears.Add(gear, [ part ]);
                    }
                }
            }
            column++;
        } while (column <= lookEnd);
    }

    if (symbolFound) {
        sum += part.number;
    }
}

Console.WriteLine($"The schematic's parts summed is {sum}");

double sumGearRatios = 0;
foreach (var (gear, gearParts) in gears) {
    if (gearParts.Count > 1)
        sumGearRatios += gearParts.Select(p => p.number).Aggregate((a, b) => a * b);
}

Console.WriteLine($"The sum of gear ratios is {sumGearRatios}");

return 0;