
var totalScorePuzzle1 = 0;
var totalScorePuzzle2 = 0;
foreach (var strategy in File.ReadAllLines("input.txt"))
{
    if (strategy is [var opponentMove, _, var myMove])
    {
        var gameScore = GetGameScore(opponentMove, myMove);
        var myMoveScore = GetMoveScore(myMove);
        totalScorePuzzle1 += gameScore + myMoveScore;

        var resultOfGame = myMove;
        var moveToPlay = GetMoveToPlay(opponentMove, resultOfGame);
        gameScore = GetGameScore(opponentMove, moveToPlay);
        myMoveScore = GetMoveScore(moveToPlay);
        totalScorePuzzle2 += gameScore + myMoveScore;
    }
    else throw new InvalidOperationException();
}

Console.WriteLine($"Total score (puzzle 1): {totalScorePuzzle1}");
Console.WriteLine($"Total score (puzzle 2): {totalScorePuzzle2}");

int GetGameScore(int opponentMove, int myMove)
{
    return (opponentMove, myMove) switch
    {
        ('A', 'Y') or ('B', 'Z') or ('C', 'X') => 6,
        ('A', 'X') or ('B', 'Y') or ('C', 'Z') => 3,
        _ => 0
    };
}

int GetMoveScore(int myMove)
{
    return myMove switch
    {
        'X' => 1,
        'Y' => 2,
        'Z' => 3,
        _ => throw new ArgumentException(nameof(myMove))
    };
}

int GetMoveToPlay(int opponentMove, int resultOfGame)
{
    return (opponentMove, resultOfGame) switch
    {
        ('A', 'X') or ('C', 'Y') or ('B', 'Z') => 'Z',
        ('C', 'X') or ('B', 'Y') or ('A', 'Z') => 'Y',
        ('B', 'X') or ('A', 'Y') or ('C', 'Z') => 'X',
        _ => throw new Exception("Unexpected combination of opponent move and result of game.")
    };
}
