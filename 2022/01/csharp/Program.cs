var currentCalories = 0;
var elfsSnacksCalories = new List<int>();
foreach (var calorie in File.ReadAllLines("input.txt"))
{
    if (!string.IsNullOrEmpty(calorie))
    {
        currentCalories += Convert.ToInt32(calorie);
        continue;
    }

    elfsSnacksCalories.Add(currentCalories);
    currentCalories = 0;
}

Console.WriteLine($"Max calories is {elfsSnacksCalories.Max()}");

// Find top three elves carrying the most calories and give us their total
var topThreeCaloriesTotal = elfsSnacksCalories
    .OrderDescending()
    .Take(3)
    .Sum();

Console.WriteLine($"The total calories of the top three elves is {topThreeCaloriesTotal}");
