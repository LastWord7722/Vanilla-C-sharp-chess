using WinFormsApp1.Entities.Figures;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Entities.Chessboard;

public class Cell
{
    public Position Position { get; }
    public BaseFigure? Figure { get; set; }

    public Cell(Position position)
    {
        Position = position;
    }

    public bool HasFigure()
    {
        return Figure != null;
    }

    public Cell Clone()
    {
        Cell cloneCell = new Cell(new Position(Position.GetColumn(), Position.GetRow()));
        cloneCell.Figure = Figure?.Clone();
        return cloneCell;
    }
}