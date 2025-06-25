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
    protected int GetPreviousRow(int currentRow)
    {
        int previousRow = Color == FigureColor.White ? currentRow - 1 : currentRow + 1;
        return previousRow < 1 || previousRow > 8 ? -1 : previousRow;
    }
    protected int GetNextRow(int currentRow)
    {
        int nextRow = Color == FigureColor.White ? currentRow + 1 : currentRow - 1;
        return nextRow < 1 || nextRow > 8 ? -1 : nextRow;
    }

    protected FigureColor GetEnemyColor() => Color == FigureColor.White ? FigureColor.Black : FigureColor.White;

    protected char? GetLeftColumn(char currentColumn, char[] columns)
    {
        int indexCurrentColumn = Array.IndexOf(columns, currentColumn);
        int leftColumnIndex = Color == FigureColor.White ? indexCurrentColumn + 1 : indexCurrentColumn - 1;
        return leftColumnIndex < 0 || leftColumnIndex >= columns.Length ? null : columns[leftColumnIndex];
    }

    protected char? GetRightColumn(char currentColumn, char[] columns)
    {
        int indexCurrentColumn = Array.IndexOf(columns, currentColumn);
        int rightColumnIndex = Color == FigureColor.White ? indexCurrentColumn - 1 : indexCurrentColumn + 1;
        
        return rightColumnIndex < 0 || rightColumnIndex >= columns.Length ? null : columns[rightColumnIndex];
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