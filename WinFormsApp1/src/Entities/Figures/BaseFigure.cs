using WinFormsApp1.Enums;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Entities.Figures;

public abstract class BaseFigure
{
    public Position Position { get; set; }
    public FigureColor Color { get; }

    protected BaseFigure(FigureColor color, Position position)
    {
        Position = position;
        Color = color;
    }

    //todo: методы получения позиции от текущей позиции стоит пересмотреть
    //т.к. зависимость цвета есть только у пешки, для остольных цвет не важен
    // для всех остольных возможность хода не зависит от цвета
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
    public abstract BaseFigure Clone();
}