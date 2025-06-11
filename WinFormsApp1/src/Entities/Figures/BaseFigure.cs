using WinFormsApp1.Enums;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Entities.Figures;

public abstract class BaseFigure
{
    protected Position Position;
    protected FigureColor Color;
    private bool _alive = true;

    protected BaseFigure(FigureColor color, Position position)
    {
        Position = position;
        Color = color;
    }

    protected int GetNextRow(int currentRow)
    {
        int nextRow = Color == FigureColor.White ? currentRow + 1 : currentRow - 1;
        return nextRow < 1 || nextRow > 8 ? -1 : nextRow;
    }
    protected FigureColor GetEnemyColor() => Color == FigureColor.White ? FigureColor.Black : FigureColor.White;
    protected char? GetLeftColumn(char currentColumn, List<char> columns)
    {
        int indexCurrentColumn = columns.IndexOf(currentColumn);
        int leftColumnIndex = Color == FigureColor.White ? indexCurrentColumn + 1 : indexCurrentColumn - 1;

        return leftColumnIndex < 1 || leftColumnIndex > 7 ? null : columns[leftColumnIndex];
    }

    protected char? GetRightColumn(char currentColumn, List<char> columns)
    {
        int indexCurrentColumn = columns.IndexOf(currentColumn);
        int rightColumnIndex = Color == FigureColor.White ? indexCurrentColumn - 1 : indexCurrentColumn + 1;

        return rightColumnIndex < 1 || rightColumnIndex > 7 ? null : columns[rightColumnIndex];
    }

    public abstract List<Position> GetAvailableMoves(Chessboard.Chessboard chessboard);

    public Position GetPosition()
    {
        return Position;
    }

    public BaseFigure SetPosition(Position position)
    {
        Position = position;
        return this;
    }

    public FigureColor GetColor()
    {
        return Color;
    }

    public void ToNotAlive()
    {
        _alive = false;
    }

    public bool IsAlive()
    {
        return _alive;
    }
}