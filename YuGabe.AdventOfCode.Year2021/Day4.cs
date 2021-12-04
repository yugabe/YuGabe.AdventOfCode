namespace YuGabe.AdventOfCode.Year2021;

public class Day4 : Day<(int[] Draws, Day4.BoardNumber[][][] Boards)>
{
    public record BoardNumber(int Value)
    {
        public bool Marked { get; set; }
    }

    public override (int[] Draws, BoardNumber[][][] Boards) ParseInput(string rawInput)
    {
        var lines = rawInput.SplitAtNewLines(splitOptions: StringSplitOptions.TrimEntries);
        return (lines[0].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse).ToArray(), lines[2..].Chunk(6).Select(board => board[..5].Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(n => new BoardNumber(int.Parse(n))).ToArray()).ToArray()).ToArray());
    }

    public override object ExecutePart1()
    {
        foreach (var draw in Input.Draws)
        {
            MarkBoards(draw, Input.Boards);

            if (Input.Boards.FirstOrDefault(IsWinner) is var winnerBoard && winnerBoard != null)
                return GetBoardValue(winnerBoard, draw);
        }

        throw null!;
    }

    public override object ExecutePart2()
    {
        var boards = Input.Boards;

        foreach (var draw in Input.Draws)
        {
            MarkBoards(draw, boards);

            if (boards.Length == 1 && IsWinner(boards[0]))
                return GetBoardValue(boards[0], draw);

            boards = boards.Where(board => !IsWinner(board)).ToArray();
        }

        throw null!;
    }

    private static void MarkBoards(int draw, IEnumerable<BoardNumber[][]> boards)
    {
        foreach (var number in boards.SelectMany(b => b.SelectMany(r => r)).Where(n => n.Value == draw))
            number.Marked = true;
    }

    private static bool IsWinner(BoardNumber[][] board) 
        => board.Any(row => row.All(number => number.Marked)) || Enumerable.Range(0, 4).Any(colIndex => board.All(row => row[colIndex].Marked));

    private static int GetBoardValue(BoardNumber[][] board, int draw)
        => board.Sum(r => r.Where(n => !n.Marked).Sum(n => n.Value)) * draw;

}
