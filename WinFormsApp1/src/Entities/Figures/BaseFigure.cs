using WinFormsApp1.Enums;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Entities.Figures;

public abstract class BaseFigure
{
    private Position _position;
    private FigureColor _color;
    private bool _alive = true;

    protected BaseFigure(FigureColor color, Position position)
    {
        _position = position;
        _color = color;
    }

    public abstract List<Position> GetAvailableMoves();

    public Position GetPosition()
    {
        return _position;
    }

    public FigureColor GetColor()
    {
        return _color;
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