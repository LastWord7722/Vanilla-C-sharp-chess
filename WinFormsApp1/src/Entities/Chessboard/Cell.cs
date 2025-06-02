using WinFormsApp1.Entities.Figures;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Entities.Chessboard;

public class Cell
{
    private Position _position;
    private BaseFigure? figure;
    
    public Cell(Position position)
    {
        _position = position;
    }

    public bool HasFigure()
    {
        return figure != null;
    }
}