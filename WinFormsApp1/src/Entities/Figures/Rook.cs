using WinFormsApp1.Enums;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Entities.Figures;

public class Rook : BaseFigure
{
    public Rook(FigureColor color, Position position) : base(color, position)
    {
    }

    public override List<Position> GetAvailableMoves(Chessboard.Chessboard chessboard)
    {
        throw new NotImplementedException();
    }
}