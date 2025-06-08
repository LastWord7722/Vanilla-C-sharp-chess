using WinFormsApp1.Entities.Figures;
using WinFormsApp1.Enums;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.StaticData.FigureLayout;

public static class DefaultFigureLayout
{
    private static readonly Dictionary<Position, Func<BaseFigure>> Layout = new()
    {
        { new Position('a', 1), () => new Rook(FigureColor.White, new Position('a', 1)) },
        { new Position('b', 1), () => new Knight(FigureColor.White, new Position('b', 1)) },
        { new Position('c', 1), () => new Bishop(FigureColor.White, new Position('c', 1)) },
        { new Position('d', 1), () => new Queen(FigureColor.White, new Position('d', 1)) },
        { new Position('e', 1), () => new King(FigureColor.White, new Position('e', 1)) },
        { new Position('f', 1), () => new Bishop(FigureColor.White, new Position('f', 1)) },
        { new Position('g', 1), () => new Knight(FigureColor.White, new Position('g', 1)) },
        { new Position('h', 1), () => new Rook(FigureColor.White, new Position('h', 1)) },
        
        { new Position('a', 2), () => new Pawn(FigureColor.White, new Position('a', 2)) },
        { new Position('b', 2), () => new Pawn(FigureColor.White, new Position('b', 2)) },
        { new Position('c', 2), () => new Pawn(FigureColor.White, new Position('c', 2)) },
        { new Position('d', 2), () => new Pawn(FigureColor.White, new Position('d', 2)) },
        { new Position('e', 2), () => new Pawn(FigureColor.White, new Position('e', 2)) },
        { new Position('f', 2), () => new Pawn(FigureColor.White, new Position('f', 2)) },
        { new Position('g', 2), () => new Pawn(FigureColor.White, new Position('g', 2)) },
        { new Position('h', 2), () => new Pawn(FigureColor.White, new Position('h', 2)) },
        
        
        { new Position('a', 8), () => new Rook(FigureColor.Black, new Position('a', 8)) },
        { new Position('b', 8), () => new Knight(FigureColor.Black, new Position('b', 8)) },
        { new Position('c', 8), () => new Bishop(FigureColor.Black, new Position('c', 8)) },
        { new Position('d', 8), () => new Queen(FigureColor.Black, new Position('d', 8)) },
        { new Position('e', 8), () => new King(FigureColor.Black, new Position('e', 8)) },
        { new Position('f', 8), () => new Bishop(FigureColor.Black, new Position('f', 8)) },
        { new Position('g', 8), () => new Knight(FigureColor.Black, new Position('g', 8)) },
        { new Position('h', 8), () => new Rook(FigureColor.Black, new Position('h', 8)) },
        
        { new Position('a', 7), () => new Pawn(FigureColor.Black, new Position('a', 7)) },
        { new Position('b', 7), () => new Pawn(FigureColor.Black, new Position('b', 7)) },
        { new Position('c', 7), () => new Pawn(FigureColor.Black, new Position('c', 7)) },
        { new Position('d', 7), () => new Pawn(FigureColor.Black, new Position('d', 7)) },
        { new Position('e', 7), () => new Pawn(FigureColor.Black, new Position('e', 7)) },
        { new Position('f', 7), () => new Pawn(FigureColor.Black, new Position('f', 7)) },
        { new Position('g', 7), () => new Pawn(FigureColor.Black, new Position('g', 7)) },
        { new Position('h', 7), () => new Pawn(FigureColor.Black, new Position('h', 7)) },
    };
    public static Dictionary<Position, BaseFigure> GetLayout()
    {
        Dictionary<Position, BaseFigure> copyLayout = new();

        foreach (var figure in Layout)
        {
            copyLayout[figure.Key] = figure.Value();
        }
        return copyLayout;
    }
}