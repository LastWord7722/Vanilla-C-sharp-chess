using WinFormsApp1.Entities.Figures;

namespace WinFormsApp1.ValueObjects;

public readonly record struct HistoryMoveItem
{
    public BaseFigure? CapturedFigure { get;}
    public BaseFigure Figure { get; }
    public Position From { get; }
    public Position To { get; }
    public HistoryCastingMoveItem? CastingMoveItem { get; } = null;

    public HistoryMoveItem(BaseFigure figure, Position from, Position to, BaseFigure? capturedFigure = null)
    {
        CapturedFigure = capturedFigure;
        Figure = figure;
        From = from;
        To = to;
    }
    public HistoryMoveItem(BaseFigure figure, Position from, Position to, HistoryCastingMoveItem castingMoveItem)
    {
        CastingMoveItem = castingMoveItem;
        Figure = figure;
        From = from;
        To = to;
    }
    public bool IsCastling() => CastingMoveItem != null;
    public string GetCode()
    {
        string captureText = CapturedFigure == null
            ? ""
            : $" x{CapturedFigure.Color.ToString()[0]}:{CapturedFigure.GetTypeFigure()}";

        return
            $"{Figure.Color.ToString()[0]}:{Figure.GetTypeFigure()} " +
            $"{From.GetPositionCode()}->{To.GetPositionCode()}{captureText}";
    }
}