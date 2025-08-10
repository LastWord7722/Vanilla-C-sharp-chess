using WinFormsApp1.Entities.Figures;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Entities.Chessboard;

//лучше юзать копозицию а не наследование чтоб moveList не наследовал всё апи list
public class MoveList
{
    private readonly List<HistoryMoveItem> _items = new();

    public void Add(BaseFigure figure, Position from, Position to, BaseFigure? capturedFigure = null)
        => _items.Add(new HistoryMoveItem(figure!, from, to, capturedFigure));

    public void AddCastling(King king, Position fromKing, Position toKing, Rook rook, Position fromRook,
        Position toRook)
        => _items.Add(new HistoryMoveItem(king, fromKing, toKing, new HistoryCastingMoveItem(rook, fromRook, toRook)));
    public int CountMoveFigures(BaseFigure figure)
    {
        return _items.Count(v => v.Figure == figure);
    }
    public override string ToString()
        => string.Join(';', _items.Select(m => m.GetCode()));

    public HistoryMoveItem Last()
        => _items.LastOrDefault();

    public int Count() => _items.Count;

    public void RemoveLast() => _items.RemoveAt(_items.Count - 1);
}