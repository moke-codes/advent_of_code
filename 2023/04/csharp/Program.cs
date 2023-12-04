using Card = (
    System.Collections.Generic.List<int> scratchcardNumbers, 
    System.Collections.Generic.List<int> elfNumbers,
    int cardMatches);

if (args is not [var path] || !Path.Exists(path))
{
    Console.WriteLine("Missing input file path. Either parameter was not correctly passed or the file does not exist.");
    return -1;
}

var lines = File.ReadAllLines(path);
var cards = new List<Card>();
var totalPoints = 0;
for (var i = 0; i < lines.Length; i++) {
    var line = lines[i];
    var cardInfo = line.Split(':')[1];
    var numbers = cardInfo.Split('|', StringSplitOptions.TrimEntries);
    var scratchcardNumbers = numbers[0]
                                .Split(
                                    ' ', 
                                    StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                                .Select(x => Convert.ToInt32(x))
                                .ToList();
    var elfNumbers = numbers[1]
                                .Split(
                                    ' ', 
                                    StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                                .Select(x => Convert.ToInt32(x))
                                .ToList();
    var multiplier = 1;
    var cardPoints = 0;
    var cardMatches = 0;
    foreach (var elfNumber in elfNumbers) {
        if (scratchcardNumbers.Contains(elfNumber)) {
            cardMatches++;
            cardPoints += 1 * multiplier;
            if (cardPoints >= 2)
                multiplier *= 2;
        }
    }

    cards.Add(
        new Card(scratchcardNumbers, elfNumbers, cardMatches)
    );

    totalPoints += cardPoints;
    //Console.WriteLine($"Card {i+1} points: {cardPoints}");
}

Console.WriteLine($"Elf's total points: {totalPoints}");

var totalCards = 0;
for(int i = 0; i < cards.Count; i++) {
    totalCards += TotalCards(i, cards);
}

Console.WriteLine($"Elf's total cards: {totalCards}");

return 0;

int TotalCards(int cardToProcess, List<Card> cards) {
    var totalCards = 1;
    var card = cards[cardToProcess];
    for (int i = cardToProcess + 1; i < cardToProcess + 1 + card.cardMatches; i++) {
        totalCards += TotalCards(i, cards);
    }
    return totalCards;
}