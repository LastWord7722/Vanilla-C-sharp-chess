using WinFormsApp1.Entities.Figures;

namespace WinFormsApp1.ValueObjects;

public readonly record struct HistoryMoveItem
{
    public BaseFigure Figure { get; }
    public Position From { get; }
    public Position To { get; }

    public HistoryMoveItem(BaseFigure figure, Position from, Position to)
    {
        Figure = figure;
        From = from;
        To = to;
    }

    public string GetCode()
    {
        return $"{Figure.Color}-{Figure.GetTypeFigure()} {From.GetPositionCode()} -> {To.GetPositionCode()}";
    }
}