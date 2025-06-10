using WinFormsApp1.Entities.Figures;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Entities.Chessboard;

public class Cell
{
    private Position _position;
    private BaseFigure? _figure;
    
    public Cell(Position position)
    {
        _position = position;
    }

    public BaseFigure? GetFigure()
    {
        return _figure;
    }
    public bool HasFigure()
    {
        return _figure != null;
    }

    public void SetFigure(BaseFigure figure)
    {
        _figure = figure;
    }

    public Position GetPosition()
    {
        return _position;
    }
}