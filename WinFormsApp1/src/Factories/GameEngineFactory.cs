using WinFormsApp1.Engin;
using WinFormsApp1.Entities.Chessboard;
using WinFormsApp1.FormLayout;
using WinFormsApp1.Interfaces;
using WinFormsApp1.Services;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Factories;

public class GameEngineFactory
{
    public static GameEngine Get(Chessboard chessboard, Dictionary<Position, ButtonCell> buttonCells)
    {
        IMovedService movedService = new MovedService();
        return new GameEngine(
            movedService,
            new StateService(),
            new ValidationMovedService(movedService),
            chessboard,
            buttonCells
        );
    }
}