using WinFormsApp1.Entities.Figures;

namespace WinFormsApp1.ValueObjects;

public readonly record struct HistoryCastingMoveItem
{
    public Rook Figure { get; }
    public Position From { get; }
    public Position To { get; }

    public HistoryCastingMoveItem(Rook figure, Position from, Position to)
    {
        Figure = figure;
        From = from;
        To = to;
    }

    // public string GetCode()
    // {
    //     string captureText = CapturedFigure == null
    //         ? ""
    //         : $" x{CapturedFigure.Color.ToString()[0]}:{CapturedFigure.GetTypeFigure()}";
    //
    //     return
    //         $"{Figure.Color.ToString()[0]}:{Figure.GetTypeFigure()} " +
    //         $"{From.GetPositionCode()}->{To.GetPositionCode()}{captureText}";
    // }
}