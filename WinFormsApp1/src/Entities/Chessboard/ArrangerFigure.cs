using WinFormsApp1.Entities.Figures;
using WinFormsApp1.Enums;
using WinFormsApp1.StaticData.FigureLayout;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Entities.Chessboard;

public class ArrangerFigure
{
    private Chessboard _chessboard;

    public ArrangerFigure(Chessboard chessboard)
    {
        _chessboard = chessboard;
    }

    public Chessboard ClassicArrangement()
    {
        // разобраться с KeyValuePair
        foreach (KeyValuePair<Position, BaseFigure> item in DefaultFigureLayout.GetLayout())
        {
            _chessboard.GetCellByPosition(item.Key).SetFigure(item.Value);
        }
        return _chessboard;
    }
}