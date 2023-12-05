// This optimization was based in the solution from @paulsonjonathan
// found here: https://github.com/jonathanpaulson/AdventOfCode/blob/master/2023/5.py

using System.Diagnostics;
using Map = (long destination, long source, long length);
using Range = (long start, long end);

if (args is not [var path] || !Path.Exists(path))
{
    Console.WriteLine("""
        Missing input file path. 
        Either parameter was not correctly passed or the file does not exist.
    """);

    return -1;
}

var file = File.ReadAllText(path);
var lines = file.Split('\n');
var seeds = lines[0].Split(':')[1]
                    .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Convert.ToInt64(x))
                    .ToList();

List<Mapping> mappings = file
    .Split(Environment.NewLine + Environment.NewLine)[1..] //Map blocks ignoring seeds part
    .Select(block => new Mapping(block.Split("\r\n")[1..])) // Split block in lines ignoring "..map:" part
    .ToList();

long minLocation = long.MaxValue;
foreach (var seed in seeds)
{
    long val = seed;
    foreach (var mapping in mappings)
    {
        val = mapping.Apply(val);
    }
    minLocation = val < minLocation ? val : minLocation;
}

Console.WriteLine($"Part 1 : the lowest location is {minLocation}");

Stopwatch sp = Stopwatch.StartNew();
minLocation = long.MaxValue;
var seedRanges = seeds.GetPairs();
foreach ((long start, long size) in seedRanges)
{
    IEnumerable<Range> ranges = [(start, start + size)];
    foreach (var mapping in mappings)
    {
        ranges = mapping.ApplyRanges(ranges);
    }
    var val = ranges.Min().start; // minimal value of minimal range
    minLocation = val < minLocation ? val : minLocation;
}

sp.Stop();
Console.WriteLine($"Part 2: the lowest location is {minLocation}");
Console.WriteLine($"{sp.Elapsed}");

return 0;

public static class Extensions
{
    public static Map GetMap(this string line) => line
                    .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Convert.ToInt64(x))
                    .ToList()
                    .CreateMap();
    public static Map CreateMap(this IList<long> source) => new Map(source[0], source[1], source[2]);

    public static IEnumerable<(long start, long size)> GetPairs(this IList<long> source)
    {
        for (int i = 0; i < source.Count - 1; i += 2)
        {
            yield return (source[i], source[i + 1]);
        }
    }
    // More succint but a little bit less efficient
    //    => source.Zip(source.Skip(1)).Where((tuple, index) => index % 2 == 0);

    public static Range Min(this IEnumerable<Range> source) =>
        source.MinBy(t => Math.Min(t.start, t.end));
}

class Mapping(IEnumerable<string> lines)
{
    List<Map> _maps = lines.Select(l => l.GetMap()).ToList();

    public long Apply(long seed)
    {
        foreach ((long dest, long src, long size) in _maps)
        {
            if (src <= seed && seed < src + size)
            {
                return dest + (seed - src);
            }
        };

        return seed;
    }

    public IEnumerable<Range> ApplyRanges(IEnumerable<Range> source)
    {
        Stack<Range> translated = new(), 
                     notTranslated = new(source),
                     aux;

        foreach ((long dest, long src, long size) in _maps)
        {
            aux = new();
            long src_end = src + size;

            while (notTranslated.Any())
            {
                (long start, long end) = notTranslated.Pop();

                Range before = (start, Math.Min(end, src)),
                      inter = (Math.Max(start, src), Math.Min(src_end, end)),
                      after = (Math.Max(src_end, start), end);

                if (before.end > before.start)
                    aux.Push(before);
                if (inter.end > inter.start)
                    translated.Push((dest + inter.start - src, dest + inter.end - src));
                if (after.end > after.start)
                    aux.Push(after);
            }
            notTranslated = aux;
        };

        return translated.Union(notTranslated);
    }
}