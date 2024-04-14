using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfGame.Models;
using WpfGame.ViewModels;
using static System.Net.Mime.MediaTypeNames;
using Application = System.Windows.Application;

namespace WpfGame.Services
{

    public class GameLogic
    {
        private Piece currentTurn;
        public Piece CurrentTurn
        {
            get { return this.currentTurn; }
            set { this.currentTurn = value; }
        }
        private ObservableCollection<ObservableCollection<Square>> board { get; set; }
        public GameLogic(ObservableCollection<ObservableCollection<Square>> board, Piece turn) //last
        {
            this.board = board;
            this.currentTurn = turn;
        }

        public void SwitchTurns()
        {
            if (Utility.CurrentTurn.Color == Color.Red)
            {
                Utility.CurrentTurn.Color = Color.Black;
                Utility.CurrentTurn.ImageSource = Utility.dark_piece;
                this.currentTurn.Color = Color.Black;
                this.currentTurn.ImageSource = Utility.dark_piece;
            }
            else
            {
                Utility.CurrentTurn.Color = Color.Red;
                Utility.CurrentTurn.ImageSource = Utility.light_piece;
                this.currentTurn.Color = Color.Red;
                this.currentTurn.ImageSource = Utility.light_piece;
            }
        }

        public void ClickPiece(Square cell)
        {
            if (cell != null && (Utility.CurrentTurn.Color == cell.Piece.Color) /*&& !Utility.ExtraMove*/)
            {
                Utility.selectedCell = cell;
                Utility.selectedCell.Piece = cell.Piece;
                DisplayPossibleMoves(cell);
            }
        }

        public void MovePiece(Square cell)
        {
            if (Utility.selectedCell != null && Utility.selectedCell.Piece != null)
            {
                cell.Piece = new Piece(Utility.selectedCell.Piece);
                cell.Piece.Position = cell;
                Utility.selectedCell.Piece = null;
                //cell.Piece = Utility.selectedCell.Piece;

                int capturedRow = (Utility.selectedCell.Row + cell.Row) / 2;
                int capturedColumn = (Utility.selectedCell.Column + cell.Column) / 2;
                int rowDifference = Math.Abs(cell.Row - Utility.selectedCell.Row);
                int colDifference = Math.Abs(cell.Column - Utility.selectedCell.Column);

                if (rowDifference == 2 && colDifference == 2 && board[capturedRow][capturedColumn].Piece != null && board[capturedRow][capturedColumn].Piece.Color != cell.Piece.Color)
                { 
                    //cell.Piece = Utility.selectedCell.Piece;
                    //cell.Piece.Position = cell;
                    board[(Utility.selectedCell.Row + cell.Row) / 2][(Utility.selectedCell.Column + cell.Column) / 2].Piece = null;
                    Utility.ExtraMove = true;
                    if (currentTurn.Color == Color.Black)
                    {
                        Utility.collectedRedPieces++;
                    }
                    else
                    {
                        Utility.collectedBlackPieces++;
                    }
                    Utility.selectedCell = cell;
                    Utility.selectedCell.Piece = new Piece(cell.Piece);
                    DisplayPossibleMoves(cell);
                }
                else
                {
                    Utility.ExtraMove = false;
                    SwitchTurns();
                }

                foreach (Square square in Utility.neighbors)
                {
                    square.Highlight = null;
                }

                Utility.neighbors.Clear();
                //Utility.selectedCell.Piece = null;
                Utility.selectedCell = null;
                //findPossibleMoves(cell);

                if (cell.Piece?.Type == PieceType.Regular)
                {
                    if (cell.Row == 0 && cell.Piece.Color == Color.Red)
                    {
                        cell.Piece.Type = PieceType.King;
                        cell.Piece.ImageSource = Utility.light_king;
                    }
                    else if (cell.Row == Utility.boardSize - 1 && cell.Piece.Color == Color.Black)
                    {
                        cell.Piece.Type = PieceType.King;
                        cell.Piece.ImageSource = Utility.dark_king;
                    }
                }

                if (Utility.collectedRedPieces == 12 || Utility.collectedBlackPieces == 12)
                {
                    GameOver();
                }
            }
        }

        public void findPossibleMoves(Square cell)
        {
            Utility.neighbors.Clear();
            Utility.ExtraPath = false;
            int direction;
            if (cell.Piece != null && cell.Piece.Color == Color.Red) direction = -1;
            else direction = 1;
            if (Utility.isInBounds(cell.Row + direction, cell.Column + 1))
            {
                if (board[cell.Row + direction][cell.Column + 1].Piece == null)
                {
                    Utility.neighbors.Add(board[cell.Row + direction][cell.Column + 1]);
                    board[cell.Row + direction][cell.Column + 1].Highlight = Utility.cell_highlight;
                }
                else if (Utility.isInBounds(cell.Row + 2 * direction, cell.Column + 2) && board[cell.Row + 2 * direction][cell.Column + 2].Piece == null && board[cell.Row + direction][cell.Column + 1].Piece.Color != cell.Piece.Color)
                {
                    Utility.neighbors.Add(board[cell.Row + 2 * direction][cell.Column + 2]);
                    board[cell.Row + 2 * direction][cell.Column + 2].Highlight = Utility.cell_highlight;
                    Utility.ExtraPath = true;
                }

            }
            if (Utility.isInBounds(cell.Row + direction, cell.Column - 1))
                if (board[cell.Row + direction][cell.Column - 1].Piece == null) { 
                    Utility.neighbors.Add(board[cell.Row + direction][cell.Column - 1]);
                    board[cell.Row + direction][cell.Column - 1].Highlight = Utility.cell_highlight;
                }
                else if (Utility.isInBounds(cell.Row + 2 * direction, cell.Column - 2) && board[cell.Row + 2 * direction][cell.Column - 2].Piece == null && board[cell.Row + direction][cell.Column - 1].Piece.Color != cell.Piece.Color)
                {
                    Utility.neighbors.Add(board[cell.Row + 2 * direction][cell.Column - 2]);
                    board[cell.Row + 2 * direction][cell.Column - 2].Highlight = Utility.cell_highlight;
                    Utility.ExtraPath = true;
                }
            if (cell.Piece != null && cell.Piece.Type == PieceType.King)
            {
                if (Utility.isInBounds(cell.Row - direction, cell.Column + 1))
                {
                    if (board[cell.Row - direction][cell.Column + 1].Piece == null) { 
                        Utility.neighbors.Add(board[cell.Row - direction][cell.Column + 1]);
                        board[cell.Row - direction][cell.Column + 1].Highlight = Utility.cell_highlight;
                    }
                else if (Utility.isInBounds(cell.Row - 2 * direction, cell.Column + 2) && board[cell.Row - 2 * direction][cell.Column + 2].Piece == null && board[cell.Row - direction][cell.Column + 1].Piece.Color != cell.Piece.Color)
                    {
                        Utility.neighbors.Add(board[cell.Row - 2 * direction][cell.Column + 2]);
                        board[cell.Row - 2 * direction][cell.Column + 2].Highlight = Utility.cell_highlight;
                    Utility.ExtraPath = true;
                    }
                }
                if (Utility.isInBounds(cell.Row - direction, cell.Column - 1))
                {
                    if (board[cell.Row - direction][cell.Column - 1].Piece == null) { 
                        Utility.neighbors.Add(board[cell.Row - direction][cell.Column - 1]);
                    board[cell.Row - direction][cell.Column - 1].Highlight = Utility.cell_highlight;
                }
                else if (Utility.isInBounds(cell.Row - 2 * direction, cell.Column - 2) && board[cell.Row - 2 * direction][cell.Column - 2].Piece == null && board[cell.Row - direction][cell.Column - 1].Piece.Color != cell.Piece.Color)
                    {
                        Utility.neighbors.Add(board[cell.Row - 2 * direction][cell.Column - 2]);
                        board[cell.Row - 2 * direction][cell.Column - 2].Highlight = Utility.cell_highlight;
                        Utility.ExtraPath = true;
                        }
                }
            }
        }


        private void DisplayPossibleMoves(Square cell)
        {
            if (Utility.selectedCell == cell)
            {
                //if (Utility.selectedCell != null)
                //{
                    foreach (var square in Utility.neighbors)
                    {
                        square.Highlight = null;
                    }
                    Utility.neighbors.Clear();
                //}

                findPossibleMoves(cell);
                
                if (Utility.ExtraMove && !Utility.ExtraPath)
                {
                    Utility.ExtraMove = false;
                    SwitchTurns();
                }
                else
                {
                    foreach (var square in Utility.neighbors)
                    {
                        square.Highlight = Utility.cell_highlight;
                    }

                    Utility.selectedCell = cell;
                    Utility.selectedCell.Piece.Position = cell;
                    Utility.ExtraPath = false;
                }
            }
            else
            {
                //findPossibleMoves(cell);
                foreach (var square in Utility.neighbors)
                {
                    //board[square.Row][square.Column].Highlight = Utility.cell_highlight;
                    board[square.Row][square.Column].Highlight = null;
                }
                Utility.neighbors.Clear();
                Utility.selectedCell = null;
            }
            
        }

        //public (ObservableCollection<ObservableCollection<Square>>, Piece) LoadGame()
        //{
        //    (ObservableCollection<ObservableCollection<Square>>, Piece) pair = Utility.LoadGame();
        //    this.currentTurn.ImageSource = Utility.currentTurn.ImageSource;
        //    this.currentTurn.Color = Utility.currentTurn.Color;
        //    return pair;
        //}

        public void GameOver()
        {
            string PATH = @"C:\Users\sorin\source\repos\WpfGame\Resources\stats.txt";
            string winner;
            using (var writer = new StreamWriter(PATH))
            {
                if (Utility.collectedRedPieces == 12)
                {
                    winner = "Player 2";
                    Utility.blackWins++;
                    if (12 - Utility.collectedBlackPieces > Utility.maxRemainingPieces)
                        Utility.maxRemainingPieces = 12 - Utility.collectedBlackPieces;
                }
                else
                {
                    winner = "Player 1";
                    Utility.redWins++;
                    if (12 - Utility.collectedRedPieces > Utility.maxRemainingPieces)
                        Utility.maxRemainingPieces = 12 - Utility.collectedRedPieces;
                }
                writer.WriteLine(Utility.redWins);
                writer.WriteLine(Utility.blackWins);
                writer.WriteLine(Utility.maxRemainingPieces);
            }
            MessageBoxResult result = MessageBox.Show(winner + " won!", "Game over", MessageBoxButton.OK);

            if (result == MessageBoxResult.OK)
            {
                //Window currentWindow = Application.Current.MainWindow;
                //currentWindow.Close();

                //MainWindow newMainWindow = new MainWindow();
                //newMainWindow.Show();
                Frame _frame = (Application.Current.MainWindow.Content as Frame);
                _frame.NavigationService.GoBack();
            }
        }
    }
}
