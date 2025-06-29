using WinFormsApp1.Enums;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Entities.Chessboard;

public class Chessboard
{
    private static readonly byte Size = 8;

    private static readonly char[] Columns = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H'];

    //char is [A-H],
    private Dictionary<Position, Cell> _cells;

    private Chessboard(Dictionary<Position, Cell> cells)
    {
        _cells = cells;
    }

    public static Chessboard Make()
    {
        var cells = new Dictionary<Position, Cell>();

        for (int i = 0; i < Size; i++)
        {
            foreach (char column in Columns)
            {
                Position currentPosition = new Position(column, i + 1);
                cells[currentPosition] = new Cell(currentPosition);
            }
        }

        return new Chessboard(cells);
    }

    public Dictionary<Position, Cell> GetCells()
    {
        return _cells;
    }

    public Cell GetCellByPosition(Position position) =>
        _cells[position];

    public bool HasFigureByPosition(Position position) =>
        GetCellByPosition(position).HasFigure();
    public bool HasUnionFigureByPosition(Position position, FigureColor figureColor) =>
        GetCellByPosition(position).HasFigure() && GetCellByPosition(position).GetFigure()!.GetColor() == figureColor;
    public bool HasEnemyFigureByPosition(Position position, FigureColor enemyFigureColor) =>
        HasFigureByPosition(position) && GetCellByPosition(position).GetFigure()!.GetColor() == enemyFigureColor;

    public char[] GetListColumns()
    {
        return Columns;
    }
}