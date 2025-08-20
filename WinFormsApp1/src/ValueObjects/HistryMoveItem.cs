using WinFormsApp1.Engin;
using WinFormsApp1.Entities.Figures;

namespace WinFormsApp1.ValueObjects;

public record struct HistoryMoveItem
{
    public BaseFigure Figure { get; }
    public Position From { get; }
    public Position To { get; }
    public HistoryCastingMoveItem? CastingMoveItem { get; } = null;
    public bool IsPromote { get; set; } = false;
    public BaseFigure? CapturedFigure { get; } = null;

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

    public bool IsCastling() => CastingMoveItem.HasValue;

    public string GetCode()
    {
        if (IsCastling())
        {
            bool isLongCastling = CastingMoveItem!.Value.From.GetColumn() > CastingMoveItem!.Value.To.GetColumn();
            return isLongCastling ? "O-O-O" : "O-O";
        }

        string captureText = CapturedFigure == null
            ? ""
            : $" x{CapturedFigure.Color.ToString()[0]}:{CapturedFigure.GetTypeFigure()}";

        string baseMove =
            $"{Figure.Color.ToString()[0]}:{Figure.GetTypeFigure()} " +
            $"{From.GetPositionCode()}->{To.GetPositionCode()}{captureText}";

        if (IsPromote)
        {
            baseMove += "=Q";
        }

        return baseMove;
    }

    public UpdateGame CreateUpdateGame(bool isBack = false)
    {
        var update = new UpdateGame();

        Position to = !isBack ? To : From;
        Position from = !isBack ? From : To;
        
        if (IsCastling())
        {
            var castling = CastingMoveItem!.Value;
            Position toCastling = !isBack ? castling.To : castling.From;
            Position fromCastling = !isBack ? castling.From : castling.To;

            update.AddUpdated(to, Figure, from);
            update.AddUpdated(toCastling, castling.Figure, fromCastling);

            return update;
        }
        
        update.AddUpdated(to, Figure, from);

        if (CapturedFigure != null && isBack)
        {
            update.AddUpdated(from, CapturedFigure);
        }

        if (IsPromote)
        {
            update.AddUpdated(to, isBack ? Figure : new Queen(Figure!.Color, Figure.Position));
        }

        return update;
    }
}