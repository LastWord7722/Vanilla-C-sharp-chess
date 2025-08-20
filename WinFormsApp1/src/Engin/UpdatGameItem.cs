using WinFormsApp1.Entities.Figures;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Engin;

public readonly record struct UpdateGameItem
{
    public Position? To { get; }
    public Position? From { get; }
    public BaseFigure Figure { get; }

    public UpdateGameItem(Position? to, BaseFigure figure, Position? from = null)
    {
        From = from;
        To = to;
        Figure = figure;
    }
}