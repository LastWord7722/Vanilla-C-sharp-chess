using WinFormsApp1.Entities.Chessboard;
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
    private IColorService _colorService;
    private Chessboard _chessboard;
    private ButtonCell? _selectedBtnFigure;
    private Dictionary<Position, ButtonCell> _buttonCells;


    public GameEngine(
        IMovedService movedService,
        IColorService colorService,
        IValidationMovedService validationMovedService,
        Chessboard chessboard,
        Dictionary<Position, ButtonCell> buttonCells
    )
    {
        _movedService = movedService;
        _validationMovedService = validationMovedService;
        _colorService = colorService;
        _chessboard = chessboard;
        _buttonCells = buttonCells;
    }

    public GameEngine SetSelectedBtnFigure(ButtonCell? selectedBtnFigure)
    {
        _selectedBtnFigure = selectedBtnFigure;
        return this;
    }

    private void HandleClickFigure(ButtonCell btnCell)
    {
        if (_selectedBtnFigure != null)
        {
            foreach (var move in _selectedBtnFigure.GetCell().Figure!.GetAvailableMoves(_chessboard))
            {
                _buttonCells[move].ResetColorToDefault();
            }
        }

        _selectedBtnFigure = btnCell;
        List<Position> availableMoves =
            _validationMovedService.GetRealAvailableMoves(_selectedBtnFigure.GetCell().Figure!, _chessboard);
        // на клоне доски будем делать каждый возможный ход и если ход возмжный будет красить кнопки, скорее всего\
        // состояние будет у сервиса...
        foreach (var move in availableMoves)
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
        if (SelectedBtnIsNull())
        {
            return;
        }

        List<Position> availableMoves =
            _validationMovedService.GetRealAvailableMoves(_selectedBtnFigure!.GetCell().Figure!, _chessboard);
        // сосотояние возможных ходов делегируем отлеьному классу, как на отрисовке ходов(тот же класс) какой то валидатор
        if (!availableMoves.Contains(btnMoveTo.GetCell().Position))
        {
            return;
        }

        _movedService.MoveFigure(btnMoveTo.GetCell(), _selectedBtnFigure.GetCell());
        _selectedBtnFigure = null;
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

        _colorService.ToogleColor();
        if (!_validationMovedService.DetectNotCheckMate(_chessboard, _colorService.GetCurrentColor()))
        {
            MessageBox.Show(
                $"{_colorService.GetOtherColor()} победили! Шах и мат.",
                "Игра окончена",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            return;
        }
    }

    public void HandleClick(ButtonCell btnCell)
    {
        if (!_validationMovedService.DetectNotCheckMate(_chessboard, _colorService.GetCurrentColor()))
        {
            MessageBox.Show(
                $"{_colorService.GetOtherColor()} победили! Шах и мат.",
                "Игра окончена",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            return;
        }

        if (btnCell.GetCell().HasFigure() &&
            btnCell.GetCell().Figure!.Color == _colorService.GetCurrentColor())
        {
            HandleClickFigure(btnCell);
        }

        if (!SelectedBtnIsNull())
        {
            HandleMoveFigure(btnCell);
        }
    }
}