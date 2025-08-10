using WinFormsApp1.Entities.Figures;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Entities.Chessboard;

//лучше юзать копозицию а не наследование чтоб moveList не наследовал всё апи list
public class MoveList
{
    private readonly List<HistoryMoveItem> _items = new();

    public void Add(BaseFigure figure, Position from, Position to, BaseFigure? capturedFigure = null)
        => _items.Add(new HistoryMoveItem(figure!, from, to, capturedFigure));

    public override string ToString()
        => string.Join(';', _items.Select(m => m.GetCode()));

    public HistoryMoveItem Last()
        => _items.LastOrDefault();
}