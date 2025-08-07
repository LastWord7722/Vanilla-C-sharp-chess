using WinFormsApp1.Entities.Chessboard;
using WinFormsApp1.Enums;
using WinFormsApp1.Factories;
using WinFormsApp1.FormLayout;
using WinFormsApp1.Helpers;
using WinFormsApp1.Interfaces;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Engin;

public class GameEngine
{
    //todo: нужно подумать как вынести ui из логики Dictionary<Position, ButtonCell> _buttonCells
    private IMovedService _movedService;
    private IValidationMovedService _validationMovedService;
    private IStateService _stateService;
    private Chessboard _chessboard;
    private ButtonCell? _selectedBtnFigure;
    private Dictionary<Position, ButtonCell> _buttonCells;
    private List<Position> _currentPossibleMoves = new();


    public GameEngine(
        IMovedService movedService,
        IStateService colorService,
        IValidationMovedService validationMovedService,
        Chessboard chessboard,
        Dictionary<Position, ButtonCell> buttonCells
    )
    {
        _movedService = movedService;
        _validationMovedService = validationMovedService;
        _stateService = colorService;
        _chessboard = chessboard;
        _buttonCells = buttonCells;
    }
    
    private void HandleClickFigure(ButtonCell btnCell)
    {
        if (_currentPossibleMoves.Count > 0)
        {
            foreach (var move in _currentPossibleMoves)
            {
                _buttonCells[move].ResetColorToDefault();
            }
        }

        _selectedBtnFigure = btnCell;
        _currentPossibleMoves = _validationMovedService.GetRealAvailableMoves(_selectedBtnFigure.GetCell().Figure!, _chessboard);
        foreach (var move in _currentPossibleMoves)
        {
            _buttonCells[move].SetBackGroundColor(ColorHelper.GetMoveColor());
        }
    }

    private bool SelectedBtnIsNull()
    {
        return _selectedBtnFigure == null;
    }

    private void HandleMoveFigure(ButtonCell btnMoveTo)
    {
        if (SelectedBtnIsNull() || !_currentPossibleMoves.Contains(btnMoveTo.GetCell().Position))
        {
            return;
        }
        
        if (_selectedBtnFigure!.GetCell().Figure!.GetTypeFigure() == FigureType.King)
        {
            _movedService.MoveKingFigure(btnMoveTo.GetCell(), _selectedBtnFigure.GetCell(), _chessboard);
        }
        else
        {
            _movedService.MoveFigure(btnMoveTo.GetCell(), _selectedBtnFigure.GetCell());
        }
        
        _selectedBtnFigure = null;

        FigureColor figureColor = _stateService.GetCurrentColor();
        //todo: можно проврять не всех а только ту пешку которая ходила, я думаю это норм оптимизация
        _stateService.CheckAndPromotePawns(_chessboard.GetCellByFigure(FigureType.Pawn, figureColor),figureColor);
        //todo: надо разобраться с эвентами и убрать отсюда форму вообще, обновлять состояние иконок только той фигуры которая ходила
        foreach (var (_, cell) in _buttonCells)
        {
            if (cell.GetCell().HasFigure())
            {
                cell.SetCenter(IconFigureFactory.Create(cell.GetCell().Figure!));
            }
            else
            {
                cell.SetCenter("");
            }

            cell.ResetColorToDefault();
        }
        _currentPossibleMoves.Clear();
        _stateService.ToogleColor();

        if (!_validationMovedService.DetectNotCheckMate(_chessboard, _stateService.GetCurrentColor()))
        {
            MessageBox.Show(
                $"{_stateService.GetOtherColor()} победили! Шах и мат.",
                "Игра окончена",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            return;
        }
    }

    public void HandleClick(ButtonCell btnCell)
    {
        if (!_validationMovedService.DetectNotCheckMate(_chessboard, _stateService.GetCurrentColor()))
        {
            MessageBox.Show(
                $"{_stateService.GetOtherColor()} победили! Шах и мат.",
                "Игра окончена",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            return;
        }

        if (btnCell.GetCell().HasFigure() &&
            btnCell.GetCell().Figure!.Color == _stateService.GetCurrentColor())
        {
            HandleClickFigure(btnCell);
        }

        if (!SelectedBtnIsNull())
        {
            HandleMoveFigure(btnCell);
        }
    }
}