if (args is not [var path] || !Path.Exists(path))
{
    Console.WriteLine("Missing input file path. Either parameter was not correctly passed or the file does not exist.");
    return -1;
}

var filePath = args[0];
var lines = File.ReadAllLines(filePath);

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


int sumPart1 = 0, sumPart2 = 0;

foreach (var calibrationValue in File.ReadAllLines(filePath))
{
    var digits = calibrationValue.Where(c => char.IsDigit(c));
    
    var firstNumber = GetFirstNumber(calibrationValue);
    var lastNumber = GetLastNumber(calibrationValue);
    var num = $"{firstNumber}{lastNumber}";

    sumPart1 += Convert.ToInt32($"{digits.First()}{digits.Last()}");
    sumPart2 += Convert.ToInt32(num);
}

Console.WriteLine($"[PART 1] Sum of all calibration values: {sumPart1}");
Console.WriteLine($"[PART 2] Sum of all calibration values: {sumPart2}");

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
    int end = word.Length;
    int length = 1;
    
    while (true) {
        if (char.IsDigit(word[end - 1])) {
            return word[end - 1].ToString();
        }
        
        var candidate = word[(end - length)..end];

        if (mapTable.ContainsKey(candidate))
            return mapTable[candidate];
             
        if (length < 5 && (end - length > 0)) {
            length++;
        } else {
            length = 0;
            end--;
        }
    }
}