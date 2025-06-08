using WinFormsApp1.Enums;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Entities.Figures;

public class Queen : BaseFigure
{
    public Queen(FigureColor color, Position position) : base(color, position)
    {
    }

    public override List<Position> GetAvailableMoves()
    {
        // 
        throw new NotImplementedException();
    }
}