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
        private ObservableCollection<ObservableCollection<Square>> board;
        public GameLogic(ObservableCollection<ObservableCollection<Square>> cells, Piece turn) //last
        {
            this.board = cells;
            this.currentTurn = turn;
        }

        public void SwitchTurns()
        {
            if (this.CurrentTurn.Color == Color.Red)
            {
                Utility.CurrentTurn.Color = Color.Black;
                this.currentTurn.Color = Color.Black;
            }
            else
            {
                Utility.CurrentTurn.Color = Color.Red;
                this.currentTurn.Color = Color.Red;
            }
        }

        public void ClickPiece(Square cell)
        {
            if (cell.Piece != null && (Utility.CurrentTurn.Color == cell.Piece.Color) && !Utility.ExtraMove)
            {
                    //Utility.selectedCell = cell;
                DisplayPossibleMoves(cell);
            }
        }

        public void MovePiece(Square cell)
        {
            cell.Piece = Utility.selectedCell.Piece;
            cell.Piece.Position = cell;
            if (Utility.selectedCell != null && Utility.selectedCell.Piece != null)
            {
                Utility.selectedCell.Piece = null;
                Utility.selectedCell = null;

                if (board[(Utility.selectedCell.Row + cell.Row) / 2][(Utility.selectedCell.Column + cell.Column) / 2].Piece != null)
                {
                    board[(Utility.selectedCell.Row + cell.Row) / 2][(Utility.selectedCell.Column + cell.Column) / 2].Piece = null;
                    Utility.extraMove = true;
                    //if (currentTurn.Color == Color.Black)
                    //{
                    //    Utility.collectedRedPieces++;
                    //}
                    //else
                    //{
                    //    Utility.collectedBlackPieces++;
                    //}
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
                Utility.findPossibleMoves(cell);
                if (!Utility.extraPath)
                {
                    Utility.extraMove = false;
                    SwitchTurns();
                }

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

                if(Utility.ExtraMove)
                {
                    if (currentTurn.Color == Color.Black)
                    {
                        Utility.collectedRedPieces++;
                    }
                    else
                    {
                        Utility.collectedBlackPieces++;
                    }
                    //Utility.findPossibleMoves(cell);
                }
            }
        }


        private void DisplayPossibleMoves(Square cell)
        {
            if (Utility.selectedCell != cell)
            {
                if (Utility.selectedCell != null)
                {
                    foreach (var square in Utility.neighbors)
                    {
                        square.Highlight = null;
                    }
                    Utility.neighbors.Clear();
                }

                Utility.findPossibleMoves(cell);
                
                if (Utility.extraPath)
                {
                    //Utility.CurrentNeighbours[square].Piece = null;
                    foreach (var square in Utility.neighbors)
                    {
                        square.Highlight = Utility.cell_highlight;
                    }
                    Utility.selectedCell = cell;
                    Utility.extraPath = false;
                }
                else
                {
                    Utility.ExtraMove = false;
                    SwitchTurns();
                }
            }
            else
            {
                foreach (var square in Utility.neighbors)
                {
                    board[square.Row][square.Column].Highlight = Utility.cell_highlight;
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
