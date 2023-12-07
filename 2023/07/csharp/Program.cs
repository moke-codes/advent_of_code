
using Hand = (string cards, int bid);

if (args is not [var path] || !Path.Exists(path))
{
    Console.WriteLine("Missing input file path. Either parameter was not correctly passed or the file does not exist.");
    return -1;
}

var lines = File.ReadAllLines(path);

static Hand lineToHand (string line) {
    var handParts = line.Split(" ");
    return new Hand(handParts[0], Convert.ToInt32(handParts[1]));
};

var hands = lines
    .Select(lineToHand)
    .Order(new HandComparer(jokersEnabled: false));

var total = hands
                .Select((hand, idx) => (hand, idx))
                .Sum(x => (x.idx + 1) * x.hand.bid);

Console.WriteLine($"Part 1: {total}");

hands = hands.Order(new HandComparer(jokersEnabled: true));

total = hands
            .Select((hand, idx) => (hand, idx))
            .Sum(x => (x.idx + 1) * x.hand.bid);

Console.WriteLine($"Part 2: {total}");

return 0;

enum TypeHand {
    HighCard,
    OnePair,
    TwoPair,
    ThreeOfAKind,
    FullHouse,
    FourOfAKind,
    FiveOfAKind
}

class HandComparer(bool jokersEnabled) : IComparer<Hand>
{
    bool _jokersEnabled = jokersEnabled;
    string _typeCards = "23456789TJQKA";

    private TypeHand GetHandType(Hand hand) 
    {
        Dictionary<char, int> handCards = new();

        foreach(var card in hand.cards) 
        {
            if (_jokersEnabled && card == 'J') //ignore Joker when counting card types
                continue;

            if (!handCards.ContainsKey(card))
                handCards.Add(card, 0);

            handCards[card] += 1;
        }

        var maxCardCount = _jokersEnabled
            ? handCards.Count == 0 // When all jokers
                ? 0
                : handCards.OrderByDescending(x => x.Value).First().Value +
                  // consider Jokers as if the card with max number
                  hand.cards.Where(c => c == 'J').Count() 
            : handCards.OrderByDescending(x => x.Value).First().Value; 
        
        return (handCards.Count, maxCardCount) switch {
            (0, _) => TypeHand.FiveOfAKind, // All jokers
            (1, _) => TypeHand.FiveOfAKind,
            (2, 4) => TypeHand.FourOfAKind,
            (2, 3) => TypeHand.FullHouse,
            (3, 3) => TypeHand.ThreeOfAKind,
            (3, 2) => TypeHand.TwoPair,
            (4, _) => TypeHand.OnePair,
            (5, _) => TypeHand.HighCard,
            _ => throw new Exception($"Not identified hand {hand.cards}.")
        };
    }

    private int CardStrength(char card) {
        return _jokersEnabled && card == 'J'
            ? -1
            : _typeCards.IndexOf(card);
    }

    public int Compare(Hand x, Hand y)
    {
        var xType = GetHandType(x);
        var yType = GetHandType(y);
        var xStrength = (int)xType;
        var yStrength = (int)yType;

        if (xStrength > yStrength) return 1;
        if (xStrength < yStrength) return -1;

        for (int cardIdx = 0; cardIdx < 5; cardIdx++) 
        {
            var xCardStrength = CardStrength(x.cards[cardIdx]);
            var yCardStrength = CardStrength(y.cards[cardIdx]);

            if (xCardStrength > yCardStrength) return 1;
            if (xCardStrength < yCardStrength) return -1;
        } 
        
        return 0;
    }
}