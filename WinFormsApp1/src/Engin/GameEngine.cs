using WinFormsApp1.Entities.Chessboard;
using WinFormsApp1.Entities.Figures;
using WinFormsApp1.Enums;
using WinFormsApp1.Factories;
using WinFormsApp1.FormLayout;
using WinFormsApp1.Interfaces;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Engin;

public class GameEngine : IGameEngine
{
    //todo: нужно подумать как вынести ui из логики Dictionary<Position, ButtonCell> _buttonCells
    private readonly IMovedService _movedService;
    private readonly IValidationMovedService _validationMovedService;
    private readonly IStateService _stateService;
    private Cell? _selectedCellFigure;
    private List<Position> _currentPossibleMoves = new();
    public Dictionary<Position, ButtonCell> ButtonCells { private get; set; } = new();
    public Chessboard? Chessboard { private get; set; }


    public GameEngine(
        IMovedService movedService,
        IStateService colorService,
        IValidationMovedService validationMovedService
    )
    {
        _movedService = movedService;
        _validationMovedService = validationMovedService;
        _stateService = colorService;
    }

    private void HandleClickFigure(Cell cell)
    {
        ResetPossibleMove();
        _selectedCellFigure = cell;
        _currentPossibleMoves = _validationMovedService.GetRealAvailableMoves(_selectedCellFigure.Figure!, Chessboard!);
        //при большой количестве ходов видна задержка отрисовки, именно отрисовки сам просчёт быстрый
        foreach (var move in _currentPossibleMoves)
        {
            if (ButtonCells[move].GetCell().HasFigure()) //возможность атткаовать, нужно добавить особую иконку
            {
                ButtonCells[move].SetPossibleMove("possible_move.png", 40);
            }
            else
            {
                ButtonCells[move].SetPossibleMove("possible_move.png");
            }
        }
    }

    private void HandleMoveFigure(Cell toCell)
    {
        if (SelectedCellIsNull() || !_currentPossibleMoves.Contains(toCell.Position))
        {
            return;
        }

        if (_selectedCellFigure!.Figure!.GetTypeFigure() == FigureType.King)
        {
            _movedService.MoveKingFigure(toCell, _selectedCellFigure, Chessboard!, _stateService);
        }
        else
        {
            _stateService.AddHistoryMove(_selectedCellFigure, toCell);
            _movedService.MoveFigure(toCell, _selectedCellFigure);
        }
        
        _selectedCellFigure = null;
        _stateService.CheckAndPromotePawn(toCell);

        RerenderBoard();
        _currentPossibleMoves.Clear();
        _stateService.ToogleColor();

        if (!_validationMovedService.DetectNotCheckMate(Chessboard!, _stateService.GetCurrentColor()))
        {
            ShowWinModal();
            return;
        }
    }

    public void HandleClick(Cell cell)
    {
        if (Chessboard == null || ButtonCells.Count <= 0)
        {
            throw new NullReferenceException("Chessboard is null or empty ButtonCells");
        }

        if (!_validationMovedService.DetectNotCheckMate(Chessboard, _stateService.GetCurrentColor()))
        {
            ShowWinModal();
            return;
        }

        if (cell.HasFigure() && cell.Figure!.Color == _stateService.GetCurrentColor())
        {
            HandleClickFigure(cell);
        }

        if (!SelectedCellIsNull())
        {
            HandleMoveFigure(cell);
        }
    }

    public void HandleBack()
    {
        if (_stateService.HistoryMoves.Count() <= 0)
        {
            return;
        }

        HistoryMoveItem lastMove = _stateService.HistoryMoves.Last();

        Cell toCell = ButtonCells[lastMove.To].GetCell();
        Cell fromCell = ButtonCells[lastMove.From].GetCell();
        _movedService.MoveFigure(fromCell, toCell);

        if (lastMove.IsPromote)
        {
            fromCell.Figure = new Pawn(lastMove.Figure.Color, fromCell.Position);
        }

        if (_stateService.HistoryMoves.CountMoveFigures(lastMove.Figure) <= 1)
        {
            fromCell.Figure!.IsFigureNotMoved = true;
        }

        if (lastMove.CapturedFigure != null)
        {
            toCell.Figure = lastMove.CapturedFigure;
        }
        else if (lastMove.IsCastling())
        {
            HistoryCastingMoveItem casting = lastMove.CastingMoveItem!.Value;
            _movedService.MoveFigure(ButtonCells[casting.From].GetCell(), ButtonCells[casting.To].GetCell());
            ButtonCells[casting.From].GetCell().Figure!.IsFigureNotMoved = true;
        }

        _stateService.HistoryMoves.RemoveLast();
        _stateService.ToogleColor();
        _selectedCellFigure = null;
        RerenderBoard();
    }

    //todo: надо разобраться с эвентами и убрать отсюда форму вообще, обновлять состояние иконок только той фигуры которая ходила
    private void RerenderBoard()
    {
        ResetPossibleMove();
        foreach (var (_, cell) in ButtonCells)
        {
            if (cell.GetCell().HasFigure())
            {
                cell.SetCenter(IconFigureFactory.Create(cell.GetCell().Figure!));
            }
            else
            {
                cell.SetCenter("");
            }
        }
    }

    private void ResetPossibleMove()
    {
        if (_currentPossibleMoves.Count > 0)
        {
            foreach (var move in _currentPossibleMoves)
            {
                ButtonCells[move].SetPossibleMove("");
            }
        }
    }

    private bool SelectedCellIsNull()
    {
        return _selectedCellFigure == null;
    }

    private void ShowWinModal()
    {
        MessageBox.Show(
            $"{_stateService.GetOtherColor()} победили! Шах и мат.",
            "Игра окончена",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information
        );
    }
}