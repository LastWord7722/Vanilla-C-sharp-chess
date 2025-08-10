using WinFormsApp1.Engin;
using WinFormsApp1.Entities.Chessboard;
using WinFormsApp1.Entities.Figures;
using WinFormsApp1.Enums;
using WinFormsApp1.Factories;
using WinFormsApp1.FormLayout;
using WinFormsApp1.Helpers;
using WinFormsApp1.Interfaces;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1;

partial class Form1
{
    IGameEngine? _gameEngine = null;
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
        
        Chessboard chessboard = initChessboard();
        Dictionary<Position, ButtonCell> buttonCells = rendreChessboard(chessboard);
        
        Button back = new Button();
        back.Size = new Size(50, 50);
        back.Location = new Point(750, 50);
        back.Text = "Back";
        back.Click += BtnBack_Click;
        this.Controls.Add(back);

        _gameEngine = GameEngineFactory.Get(chessboard,buttonCells);
        
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Text = "Chess";
    }

    private Chessboard initChessboard()
    {
        return new ArrangerFigure(Chessboard.Make()).ClassicArrangement();
    }
    
    //todo: возможно вынести доску в отденьную сущность формы, нужно изучить 
    private Dictionary<Position, ButtonCell> rendreChessboard(Chessboard chessboard)
    {
        Dictionary<Position, ButtonCell> buttonCells = new Dictionary<Position, ButtonCell>();
        int heightWidth = 50;
        int x = 0;
        int y = 0;
        bool isBlack = false;
        var sortedCells = chessboard.Cells.OrderByDescending(kvp => kvp.Key.GetRow());

        foreach (var (_, cell) in sortedCells)
        {
            ButtonCell currentCell = ButtonCell.Make(cell, isBlack, x,y).SetClickEvent(BtnCell_Click);
            Position position = cell.Position; 
            buttonCells.Add(cell.Position, currentCell);
            if (position.IsColumn('a'))
                currentCell.SetTopLeft(position.GetRow().ToString());
            if (y == heightWidth * 7)
                currentCell.SetBottomRight(position.GetColumn().ToString());
            if (currentCell.GetCell().HasFigure())
            {
                currentCell.SetCenter(IconFigureFactory.Create(currentCell.GetCell().Figure));
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

        return buttonCells;
    }

    void BtnBack_Click(Object sender, EventArgs e)
    {
        _gameEngine.HandleBack();
    }
    void BtnCell_Click(Object sender, EventArgs e)
    {
        ButtonCell btnCell = sender as ButtonCell;
        if (btnCell == null) return;
        
        _gameEngine.HandleClick(btnCell);
    }
    #endregion
}