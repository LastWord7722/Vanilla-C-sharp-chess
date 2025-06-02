using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Entities.Chessboard;

public class Chessboard
{
    private static readonly byte Size = 8;

    private static readonly List<char> Rows = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H'];

    //char is [A-H],
    private Dictionary<char, List<Cell>> _cells;

    private Chessboard(Dictionary<char, List<Cell>> cells)
    {
        _cells = cells;
    }

    public static Chessboard Make()
    {
        var cells = new Dictionary<char, List<Cell>>();
        foreach (char row in Rows)
        {
            var cellInRow = new List<Cell>();
            for (int i = 0; i < Size; i++)
            {
                cellInRow.Add(new Cell(new Position(row,i +1)));
            }

            cells[row] = cellInRow;
        }

        return new Chessboard(cells);
    }
}