using System.Diagnostics;
using Map = (long destination, long source, long length);

if (args is not [var path] || !Path.Exists(path))
{
    Console.WriteLine("Missing input file path. Either parameter was not correctly passed or the file does not exist.");
    return -1;
}

var lines = File.ReadAllLines(path);
var parseState = "";
List<long> seeds = new();
List<long> seedsPart2 = new();
List<Map> seedToSoil = new();
List<Map> soilToFertilizer = new();
List<Map> fertilizerToWater = new();
List<Map> waterToLight = new();
List<Map> lightToTemperature = new();
List<Map> temperatureToHumidity = new();
List<Map> humidityToLocation = new();

foreach(var line in lines) 
{
    if (line == string.Empty) {
        parseState = string.Empty;
        continue;
    }

    if (line.StartsWith("seeds:")) {
        seeds = line
                    .Split(':')[1]
                    .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Convert.ToInt64(x))
                    .ToList();

        continue;
    }

    if (line.Contains(":")) {
        parseState = line.Split(":")[0];
        continue;
    }

    switch (parseState) {
        case "seed-to-soil map": seedToSoil.Add(line.GetMap()); break;
        case "soil-to-fertilizer map": soilToFertilizer.Add(line.GetMap()); break;
        case "fertilizer-to-water map": fertilizerToWater.Add(line.GetMap()); break;
        case "water-to-light map": waterToLight.Add(line.GetMap()); break;
        case "light-to-temperature map": lightToTemperature.Add(line.GetMap()); break;
        case "temperature-to-humidity map": temperatureToHumidity.Add(line.GetMap()); break;
        case "humidity-to-location map": humidityToLocation.Add(line.GetMap()); break;
    }
}

Dictionary<long,long> locationToSeed = new();
foreach(var seed in seeds!) {
    var location = 
        GetMapValue(humidityToLocation,
            GetMapValue(temperatureToHumidity, 
                GetMapValue(lightToTemperature,
                    GetMapValue(waterToLight,
                        GetMapValue(fertilizerToWater,
                            GetMapValue(soilToFertilizer,
                                GetMapValue(seedToSoil, seed)
                            )
                        )
                    )
                )
            )
        );

    locationToSeed.Add(location, seed);
}

var lowestLocation = locationToSeed.OrderBy(x => x.Key).First();

Console.WriteLine($"Part 1 : the lowest location is {lowestLocation.Key} from seed {lowestLocation.Value}");

Stopwatch sp = Stopwatch.StartNew();
long minLocation = long.MaxValue;
var seedRanges = seeds.GetPairs();
Parallel.ForEach(seedToSoil.GetPossibleSeeds(seedRanges), seed => {
    var location = 
        GetMapValue(humidityToLocation,
            GetMapValue(temperatureToHumidity, 
                GetMapValue(lightToTemperature,
                    GetMapValue(waterToLight,
                        GetMapValue(fertilizerToWater,
                            GetMapValue(soilToFertilizer,
                                GetMapValue(seedToSoil, seed)
                            )
                        )
                    )
                )
            )
        );
    //Console.Write($" {location} MinLocation: {minLocation}\n");
    Interlocked.CompareExchange(ref minLocation, location < minLocation? location : minLocation, minLocation);

    //Console.WriteLine($"MinLocation value after {minLocation}");
});
sp.Stop();
Console.WriteLine($"Part 2: the lowest location is {minLocation}");
Console.WriteLine($"{sp.Elapsed}");
return 0;

long GetMapValue(List<Map> maps, long value) {
    foreach(var map in maps) {
        if (value >= map.source && value <= (map.source + map.length - 1))
            return map.destination + (value - map.source);
    }

    return value;
}

public static class Extensions {

    public static Map GetMap(this string line) =>  line
                    .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Convert.ToInt64(x))
                    .ToList()
                    .CreateMap();
    public static Map CreateMap(this IList<long> source) {
        return new Map(source[0], source[1], source[2]);
    }

    public static (long start, long end) GetSourceRange(this Map map) {
        return (map.source, map.source + map.length - 1);
    }

    public static IEnumerable<(long start, long end)> GetPairs(
        this IList<long> source)
    {
        for (int i = 0; i < source.Count - 1; i += 2)
        {
            yield return (source[i], source[i] + source[i + 1] - 1);
        }
    }

    public static IEnumerable<long> GetPossibleSeeds(this IList<Map> source, IEnumerable<(long start, long end)> seedRanges) {
        foreach(var map in source) {
            foreach(var seedRange in seedRanges) {
                var mapSeedRange = map.GetSourceRange();
                if (mapSeedRange.start > seedRange.end ||
                    mapSeedRange.end < seedRange.start)
                    continue;

                long start = mapSeedRange.start > seedRange.start ? mapSeedRange.start : seedRange.start;
                long end = mapSeedRange.end < seedRange.end ? mapSeedRange.end : seedRange.end;

                for (long i = start; i <= end; i++)
                    yield return i;
            } 
        }
    }
}