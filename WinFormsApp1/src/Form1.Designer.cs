using WinFormsApp1.Entities.Chessboard;
using WinFormsApp1.Entities.Figures;
using WinFormsApp1.Enums;
using WinFormsApp1.Factories;
using WinFormsApp1.FormLayout;
using WinFormsApp1.Helpers;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1;

partial class Form1
{
    private FigureColor _figureColor = FigureColor.White;
    private Chessboard _chessboard;
    private ButtonCell? _selectedBtnFigure;
    private Dictionary<Position, ButtonCell> _buttonCells = new Dictionary<Position, ButtonCell>();
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        initChessboard();
        rendreChessboard();

        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Text = "Chess";

    }

    private void initChessboard()
    {
        _chessboard = new ArrangerFigure(Chessboard.Make()).ClassicArrangement();
    }
    
    //todo: возможно вынести доску в отденьную сущность формы, нужно изучить 
    private void rendreChessboard()
    {
        int heightWidth = 50;
        int x = 0;
        int y = 0;
        bool isBlack = false;
        var sortedCells = _chessboard.GetCells().OrderByDescending(kvp => kvp.Key.GetRow());
        
        foreach (var (_, cell) in sortedCells)
        {
            ButtonCell currentCell = ButtonCell.Make(cell, isBlack, x,y).SetClickEvent(BtnCell_Click);
            Position position = cell.GetPosition(); 
            _buttonCells.Add(cell.GetPosition(), currentCell);
            if (position.IsColumn('a'))
                currentCell.SetTopLeft(position.GetRow().ToString());
            if (y == heightWidth * 7)
                currentCell.SetBottomRight(position.GetColumn().ToString());
            if (currentCell.GetCell().HasFigure())
            {
                currentCell.SetCenter(IconFigureFactory.Create(currentCell.GetCell().GetFigure()));
            }
            this.Controls.Add(currentCell);
            isBlack = !isBlack;

            if (position.IsColumn('h'))
            {
                x = 0;
                y += heightWidth;
                isBlack = !isBlack;
            }
            else x += heightWidth;
        }

    }
    //todo: можно строго указать тип расширеного класса в случае использывание кастомных делегатов(требуется изучение)
    // в будущем расширю 
    
    //todo: нужно создать сервис по типу gameService чтоб из формы вынести логику и реализовать движение
    //сейчас просто накинут каркас
    void BtnCell_Click(Object sender, EventArgs e)
    {
        ButtonCell btnCell = sender as ButtonCell;
        if (btnCell == null) return;


        if (btnCell.GetCell().HasFigure() && btnCell.GetCell().GetFigure().GetColor() == _figureColor)
        {
            if (_selectedBtnFigure != null)
            {
                foreach (var move in _selectedBtnFigure.GetCell().GetFigure().GetAvailableMoves(_chessboard))
                {
                    _buttonCells[move].ResetColorToDefault();
                }
            }
            
            _selectedBtnFigure = btnCell;
            List<Position> availableMoves =_selectedBtnFigure.GetCell().GetFigure().GetAvailableMoves(_chessboard);
            foreach (var move in availableMoves)
            {
                _buttonCells[move].SetBackGroundColor(ColorHelper.GetMoveColor());
            }
        }
        if (_selectedBtnFigure != null)
        {
            List<Position> availableMoves =_selectedBtnFigure.GetCell().GetFigure().GetAvailableMoves(_chessboard);
            //btnCell.GetCell().GetPosition()
            if (availableMoves.Contains(btnCell.GetCell().GetPosition()))
            {
               Cell cellToMove = btnCell.GetCell();
               
               BaseFigure currentFigure = _selectedBtnFigure.GetCell().GetFigure();
               currentFigure.SetPosition(btnCell.GetCell().GetPosition());
               cellToMove.SetFigure(currentFigure);
               _selectedBtnFigure.GetCell().SetFigure(null);
               _figureColor = _figureColor == FigureColor.White ?  FigureColor.Black : FigureColor.White;
               _selectedBtnFigure = null;
               foreach (var (position,cell) in _buttonCells)
               {
                   if (cell.GetCell().HasFigure())
                   {
                       cell.SetCenter(IconFigureFactory.Create(cell.GetCell().GetFigure()));
                   }
                   else
                   {
                       cell.SetCenter("");
                   }
                   cell.ResetColorToDefault();
               }
            }
        }
    }
    #endregion
}