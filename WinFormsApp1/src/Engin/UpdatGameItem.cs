using WinFormsApp1.Entities.Figures;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Engin;

public readonly record struct UpdateGameItem
{
    public Position To { get; }
    public Position From { get; }
    public BaseFigure Figure { get; }

    public UpdateGameItem(Position to, Position from, BaseFigure figure)
    {
        From = from;
        To = to;
        Figure = figure;
    }
}