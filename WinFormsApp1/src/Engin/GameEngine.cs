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
    private readonly IMovedService _movedService;
    private readonly IValidationMovedService _validationMovedService;
    private readonly IStateService _stateService;
    private Cell? _selectedCellFigure;
    private List<Position> _currentPossibleMoves = new();
    public Chessboard? Chessboard { private get; set; }
    private readonly UpdateGame _updateGame = new();


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
        _selectedCellFigure = cell;
        _currentPossibleMoves = _validationMovedService.GetRealAvailableMoves(_selectedCellFigure.Figure!, Chessboard!);
        _updateGame.AddRangeMoved(_currentPossibleMoves);
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

        _updateGame.AddRangeAll(_stateService.HistoryMoves.Last().CreateUpdateGame());

        _currentPossibleMoves.Clear();
        _stateService.ToogleColor();

        if (!_validationMovedService.DetectNotCheckMate(Chessboard!, _stateService.GetCurrentColor()))
        {
            ShowWinModal();
            return;
        }
    }

    public UpdateGame HandleClick(Cell cell)
    {
        if (Chessboard == null)
        {
            throw new NullReferenceException("Chessboard is null");
        }
        
        if (!_validationMovedService.DetectNotCheckMate(Chessboard, _stateService.GetCurrentColor()))
        {
            ShowWinModal();
            return _updateGame;
        }
        
        _updateGame.Clear();
        _updateGame.AddRangeCleared(_currentPossibleMoves);
        
        if (cell.HasFigure() && cell.Figure!.Color == _stateService.GetCurrentColor())
        {

            HandleClickFigure(cell);
        }
        else if (!SelectedCellIsNull() )
        {
            HandleMoveFigure(cell);
        }
        if (cell.HasFigure() && cell.Figure!.Color != _stateService.GetCurrentColor())
        {
            _selectedCellFigure = null;
        }
        
        return _updateGame;
    }

    public UpdateGame HandleBack()
    {
        if (_stateService.HistoryMoves.Count() <= 0)
        {
            return new UpdateGame();
        }

        HistoryMoveItem lastMove = _stateService.HistoryMoves.Last();
        Chessboard chessboard = Chessboard!; 
        Cell toCell = chessboard.GetCellByPosition(lastMove.To);
        Cell fromCell = chessboard.GetCellByPosition(lastMove.From);
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
            _movedService.MoveFigure(chessboard.GetCellByPosition(casting.From), chessboard.GetCellByPosition(casting.To));
            chessboard.GetCellByPosition(casting.From).Figure!.IsFigureNotMoved = true;
        }
        
        UpdateGame updated = _stateService.HistoryMoves.Last().CreateUpdateGame(true);
        _stateService.HistoryMoves.RemoveLast();
        _stateService.ToogleColor();
        _selectedCellFigure = null;
        return updated;
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