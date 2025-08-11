using WinFormsApp1.Entities.Chessboard;
using WinFormsApp1.Enums;
using WinFormsApp1.Factories;
using WinFormsApp1.FormLayout;
using WinFormsApp1.Interfaces;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Engin;

public class GameEngine : IGameEngine
{
    //todo: нужно подумать как вынести ui из логики Dictionary<Position, ButtonCell> _buttonCells
    private IMovedService _movedService;
    private IValidationMovedService _validationMovedService;
    private IStateService _stateService;
    private ButtonCell? _selectedBtnFigure;
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

    private void HandleClickFigure(ButtonCell btnCell)
    {
        ResetPossibleMove();
        _selectedBtnFigure = btnCell;
        _currentPossibleMoves =
            _validationMovedService.GetRealAvailableMoves(_selectedBtnFigure.GetCell().Figure!, Chessboard);
        //при большой количестве ходов видна задержка отрисовки, именно отрисовки сам просчёт быстрый
        foreach (var move in _currentPossibleMoves)
        {
            if (ButtonCells[move].GetCell().HasFigure()) //возможность атткаовать, нужно добавить особую иконку
            {
                ButtonCells[move].SetPossibleMove("possible_move.png",40);
            }
            else
            {
                ButtonCells[move].SetPossibleMove("possible_move.png");
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

        Cell fromMove = _selectedBtnFigure!.GetCell();
        Cell toMove = btnMoveTo.GetCell();
        
        if (_selectedBtnFigure!.GetCell().Figure!.GetTypeFigure() == FigureType.King)
        {
            _movedService.MoveKingFigure(toMove, fromMove, Chessboard!,_stateService);
        }
        else
        {
            _stateService.AddHistoryMove(fromMove, toMove);
            _movedService.MoveFigure(toMove, fromMove);
        }
        
        Console.WriteLine(_stateService.HistoryMoves.ToString());
        Console.WriteLine("----");
        _selectedBtnFigure = null;

        FigureColor figureColor = _stateService.GetCurrentColor();
        //todo: можно проврять не всех а только ту пешку которая ходила, я думаю это норм оптимизация
        _stateService.CheckAndPromotePawns(Chessboard!.GetCellByFigure(FigureType.Pawn, figureColor), figureColor);
        RerenderBoard();
        _currentPossibleMoves.Clear();
        _stateService.ToogleColor();

        if (!_validationMovedService.DetectNotCheckMate(Chessboard, _stateService.GetCurrentColor()))
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

    private void RerenderBoard()
    {
        ResetPossibleMove();
        //todo: надо разобраться с эвентами и убрать отсюда форму вообще, обновлять состояние иконок только той фигуры которая ходила
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
    public void HandleClick(ButtonCell btnCell)
    {
        if (Chessboard == null || ButtonCells.Count <= 0)
        {
            throw new NullReferenceException("Chessboard is null or empty ButtonCells");
        }

        if (!_validationMovedService.DetectNotCheckMate(Chessboard, _stateService.GetCurrentColor()))
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

    public void HandleBack()
    {
        //todo: не реализована возвращение превращение пешки обратно в пешку
        if (_stateService.HistoryMoves.Count() <= 0)
        {
            return;
        }
        HistoryMoveItem lastMove = _stateService.HistoryMoves.Last();
        
        Cell toCell = ButtonCells[lastMove.To].GetCell();
        Cell fromCell = ButtonCells[lastMove.From].GetCell();
        _movedService.MoveFigure(fromCell, toCell);
        
        if (_stateService.HistoryMoves.CountMoveFigures(lastMove.Figure) <= 1)
        {
            fromCell.Figure!.IsFigureNotMoved = true;
        }
        if (lastMove.CapturedFigure != null)
        {
            toCell.Figure = lastMove.CapturedFigure; 
        }else if (lastMove.IsCastling())
        {
            HistoryCastingMoveItem casting = lastMove.CastingMoveItem!.Value;
            _movedService.MoveFigure(ButtonCells[casting.From].GetCell(), ButtonCells[casting.To].GetCell());
            ButtonCells[casting.From].GetCell().Figure!.IsFigureNotMoved = true;
        }

        _stateService.HistoryMoves.RemoveLast();
        _stateService.ToogleColor();
        _selectedBtnFigure = null;
        RerenderBoard();
    }
}