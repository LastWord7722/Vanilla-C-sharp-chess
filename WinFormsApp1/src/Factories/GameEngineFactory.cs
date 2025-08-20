using WinFormsApp1.DI;
using WinFormsApp1.Entities.Chessboard;
using WinFormsApp1.Interfaces;

namespace WinFormsApp1.Factories;

public class GameEngineFactory
{
    public static IGameEngine Get(Chessboard chessboard)
    {
        IGameEngine game = Creator.Create<IGameEngine>();
        game.Chessboard = chessboard;
        return game;
    }
}