if (args is not [var path] || !Path.Exists(path))
{
    Console.WriteLine("Missing input file path. Either parameter was not correctly passed or the file does not exist.");
    return -1;
}

var filePath = args[0];

var sum = 0;

foreach (var calibrationValue in File.ReadAllLines(filePath))
{
    var digits = calibrationValue.Where(c => char.IsDigit(c)).ToList();
    

    if (digits is { Count: 0 })
        continue;

    //Console.WriteLine($"{digits.First()},{digits.Last()}");

    sum += Convert.ToInt32($"{digits.First()}{digits.Last()}");
}

Console.WriteLine($"[PART 1] Sum of all calibration values: {sum}");

// PART TWO
sum = 0;

var mapTable = new Dictionary<string,string> {
    {"one", "1"},
    {"two", "2"},
    {"three", "3"},
    {"four", "4"},
    {"five", "5"},
    {"six", "6"},
    {"seven", "7"},
    {"eight", "8"},
    {"nine", "9"}
};
foreach (var calibrationValue in File.ReadAllLines(filePath))
{
    try {
    var firstNumber = GetFirstNumber(calibrationValue);
    var lastNumber = GetLastNumber(calibrationValue);

    var num = $"{firstNumber}{lastNumber}";
    //Console.WriteLine($"{calibrationValue}: {num}");

    sum += Convert.ToInt32(num);
    }
    catch {
        Console.WriteLine(calibrationValue);
        throw;
    }
}

Console.WriteLine($"[PART 2] Sum of all calibration values: {sum}");

return 0;

string GetFirstNumber(string word) 
{    
    int start = 0;
    int length = 1;
    
    while (true) {
        if (char.IsDigit(word[start])) {
            return word[start].ToString();
        }
               
        var candidate = word.Substring(start, length);

        if (mapTable.ContainsKey(candidate))
            return mapTable[candidate];
        
        if (length < 5 && (start + length < word.Length)) {
            length++;
        } else {
            length = 0;
            start++;
        }
    }
}

string GetLastNumber(string word) 
{
    int end = word.Length - 1;
    int length = 1;
    
    while (true) {
        if (char.IsDigit(word[end])) {
            return word[end].ToString();
        }
        
        var candidate = word.Substring(word.Length - (word.Length - 1 - end) - length, length);

        if (mapTable.ContainsKey(candidate))
            return mapTable[candidate];
             
        if (length < 5 && (end + 1 - length > 0)) {
            length++;
        } else {
            length = 0;
            end--;
        }
    }
}