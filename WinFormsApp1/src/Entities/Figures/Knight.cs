using WinFormsApp1.Enums;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Entities.Figures;

public class Knight : BaseFigure
{
    public Knight(FigureColor color, Position position) : base(color, position)
    {
    }

    public override List<Position> GetAvailableMoves()
    {
        // 
        throw new NotImplementedException();
    }
}