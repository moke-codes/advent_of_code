using System.Collections.ObjectModel;
using System.Reflection.Metadata;

if (args is not [var path] || !Path.Exists(path))
{
    Console.WriteLine("Missing input file path. Either parameter was not correctly passed or the file does not exist.");
    return -1;
}

var filePath = args[0];
var lines = File.ReadAllLines(filePath);

var gameConstraint = (red: 12, green: 13, blue: 14);
var sumIds = 0;
foreach (var game in lines) 
{
    var gameId = GetGameId(game);

    if (GameMetConstraints(gameConstraint, game)) {
        sumIds += gameId;
    }
}

Console.WriteLine($"The sum of possible games is {sumIds}.");

var powerSum = 0;
foreach (var game in lines) 
{
    int maxRed = 0, maxGreen = 0, maxBlue = 0;
    foreach((int red, int green, int blue) cubes in GetGamePlays(game)) {
        if (cubes.red > maxRed) maxRed = cubes.red;
        if (cubes.green > maxGreen) maxGreen = cubes.green;
        if (cubes.blue > maxBlue) maxBlue = cubes.blue;
    }

    var power = maxRed * maxGreen * maxBlue;
    powerSum += power;
}

Console.WriteLine($"The sum of game powers is {powerSum}.");

return 0;

bool GameMetConstraints((int red, int green, int blue) gameConstraint, string game)
{
    foreach((int red, int green, int blue) cubes in GetGamePlays(game)) {
        if (cubes.red > gameConstraint.red ||
            cubes.green > gameConstraint.green ||
            cubes.blue > gameConstraint.blue)
            return false;
    }

    return true;
}

IEnumerable<(int red, int green, int blue)> GetGamePlays(string game)
{
    var plays = game.Split(':')[1].Split(';');
    foreach (var play in plays) {
        yield return ParsePlay(play);
    }
}

(int red, int green, int blue) ParsePlay(string play)
{
    (int red, int green, int blue) result = (0, 0, 0);
    var cubes = play.Split(',', StringSplitOptions.TrimEntries);
    foreach(var cube in cubes) {
        var cubeValues = cube.Split(' ');
        var cubeNumber = Convert.ToInt32(cubeValues[0]);
        var cubeColor = cubeValues[1];
        switch (cubeColor) {
            case "red": result.red = cubeNumber; break;
            case "green": result.green = cubeNumber; break;
            case "blue": result.blue = cubeNumber; break;
        }
    }

    return result;
}

int GetGameId(string game) {
    return Convert.ToInt32(game.Split(':')[0].Split(' ')[1]);
}