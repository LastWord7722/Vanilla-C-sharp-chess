using WinFormsApp1.DI;
using WinFormsApp1.Entities.Chessboard;
using WinFormsApp1.FormLayout;
using WinFormsApp1.Interfaces;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Factories;

public class GameEngineFactory
{
    public static IGameEngine Get(Chessboard chessboard, Dictionary<Position, ButtonCell> buttonCells)
    {
        IGameEngine game = Creator.Create<IGameEngine>();
        game.Chessboard = chessboard;
        game.ButtonCells = buttonCells;
        return game;
    }
}