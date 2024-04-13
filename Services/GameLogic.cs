using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfGame.Models;
using WpfGame.ViewModels;

namespace WpfGame.Services
{

    public class GameLogic
    {
        public Piece currentTurn;
        public Piece CurrentTurn
        {
            get { return this.currentTurn; }
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
                cell.Piece = Utility.selectedCell.Piece;
                cell.Piece.Position = cell;

                if (board[(Utility.selectedCell.Row + cell.Row) / 2][(Utility.selectedCell.Column + cell.Column) / 2].Piece != null)
                {
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
                //findPossibleMoves(cell);

                if (cell.Piece.Type == PieceType.Regular)
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

                //if(Utility.ExtraMove)
                //{
                //    //if (currentTurn.Color == Color.Black)
                //    //{
                //    //    Utility.collectedRedPieces++;
                //    //}
                //    //else
                //    //{
                //    //    Utility.collectedBlackPieces++;
                //    //}
                //    //Utility.selectedCell = cell;
                //    //DisplayPossibleMoves(cell);
                //}
                Utility.selectedCell.Piece = null;
                Utility.selectedCell = null;
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
                        Utility.neighbors.Add(board[cell.Row + 1][cell.Column + 1]);
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

        public void GameOver()
        {
            //todo
        }
    }
}
