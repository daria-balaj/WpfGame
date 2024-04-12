using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using WpfGame.Models;
using Color = WpfGame.Models.Color;
using System.Resources;
using WpfGame.ViewModels;

namespace WpfGame.Services
{
    class Utility
    {
        public static ImageSource beige_square = ConvertStringToImageSource(@"C:\Users\daria\source\repos\MAP\WpfGame\Resources\BeigeSquare.jpg");
        public static ImageSource brown_square = ConvertStringToImageSource(@"C:\Users\daria\source\repos\MAP\WpfGame\Resources\BrownSquare.png");
        public static ImageSource light_piece = ConvertStringToImageSource(@"C:\Users\daria\source\repos\MAP\WpfGame\Resources\RedCircle.png");
        public static ImageSource dark_piece = ConvertStringToImageSource(@"C:\Users\daria\source\repos\MAP\WpfGame\Resources\BlackCircle.png");
        public static ImageSource light_king = ConvertStringToImageSource(@"C:\Users\daria\source\repos\MAP\WpfGame\Resources\CrownedRed.png");
        public static ImageSource dark_king = ConvertStringToImageSource(@"C:\Users\daria\source\repos\MAP\WpfGame\Resources\CrownedBlack.png");
        public static ImageSource cell_highlight = ConvertStringToImageSource(@"C:\Users\daria\source\repos\MAP\WpfGame\Resources\GreenSquare.png");

        public const int boardSize = 8;
        public static int collectedRedPieces = 0;
        public static int collectedBlackPieces = 0;
        //public static Color currentTurn = Color.Red;
        public static Piece currentTurn = new Piece(Color.Red, PieceType.Regular);
        public static ObservableCollection<ObservableCollection<Square>> board = new ObservableCollection<ObservableCollection<Square>>();
        public static List<Square> neighbors = new List<Square>();
        public static Square? selectedCell;

        public static Piece CurrentTurn
        {
            get { return currentTurn; }
            set { currentTurn = value; }
        }

        public static int CollectedRedPieces 
        { 
            get { return collectedRedPieces; }
            set { collectedRedPieces = value; }
        }
        public static int CollectedBlackPieces
        {
            get { return collectedBlackPieces; }
            set { collectedBlackPieces = value; }
        }

        public static bool extraMove = false;
        public static bool ExtraMove
        {
            get { return extraMove; }
            set { extraMove = value; }
        }
        public static bool extraPath = false;
        public static bool ExtraPath
        {
            get { return extraPath; }
            set { extraPath = value; }
        }

        public static bool isInBounds(int row, int column)
        {
            return row >= 0 && column >= 0 && row < boardSize && column < boardSize;
        }

        public static void findPossibleMoves(Square cell)
        {
            neighbors.Clear();
            Utility.extraPath = false;
            int direction;
            if (cell.Piece != null && cell.Piece.Color == Color.Red) direction = -1;
            else direction = 1;
            if (isInBounds(cell.Row + direction, cell.Column + 1))
            {
                if (board[cell.Row + direction][cell.Column + 1].Piece == null)
                    neighbors.Add(board[cell.Row + direction][cell.Column + 1]);
                else if (isInBounds(cell.Row + 2 * direction, cell.Column + 2) && board[cell.Row + 2 * direction][cell.Column + 2].Piece == null && board[cell.Row + direction][cell.Column + 1].Piece.Color != cell.Piece.Color)
                {
                    neighbors.Add(board[cell.Row + 2 * direction][cell.Column + 2]);
                    Utility.extraPath = true;
                }

            }
            if (isInBounds(cell.Row + direction, cell.Column - 1))
                if (board[cell.Row + direction][cell.Column - 1].Piece == null)
                    neighbors.Add(board[cell.Row + direction][cell.Column - 1]);
                else if (isInBounds(cell.Row + 2 * direction, cell.Column - 2) && board[cell.Row + 2 * direction][cell.Column - 2].Piece == null && board[cell.Row + direction][cell.Column - 1].Piece.Color != cell.Piece.Color)
                { 
                    neighbors.Add(board[cell.Row + 2 * direction][cell.Column - 2]);
                    Utility.extraPath = true;
                }   
            if (cell.Piece != null && cell.Piece.Type == PieceType.King)
            {
                if (isInBounds(cell.Row - direction, cell.Column + 1))
                {
                    if (board[cell.Row - direction][cell.Column + 1].Piece == null)
                        neighbors.Add(board[cell.Row + 1][cell.Column + 1]);
                    else if (isInBounds(cell.Row - 2 * direction, cell.Column + 2) && board[cell.Row - 2 * direction][cell.Column + 2].Piece == null && board[cell.Row - direction][cell.Column + 1].Piece.Color != cell.Piece.Color)
                    {
                        neighbors.Add(board[cell.Row - 2 * direction][cell.Column + 2]);
                        Utility.extraPath = true;
                    }
                }
                if (isInBounds(cell.Row - direction, cell.Column - 1))
                {
                    if (board[cell.Row - direction][cell.Column - 1].Piece == null)
                        neighbors.Add(board[cell.Row - direction][cell.Column - 1]);
                    else if (isInBounds(cell.Row - 2 * direction, cell.Column - 2) && board[cell.Row - 2 * direction][cell.Column - 2].Piece == null && board[cell.Row - direction][cell.Column - 1].Piece.Color != cell.Piece.Color)
                    {
                        neighbors.Add(board[cell.Row - 2 * direction][cell.Column - 2]);
                        Utility.extraPath = true;
                    }
                }
            }
        }

        public static ImageSource ConvertStringToImageSource(string imagePath)
        {
            BitmapImage imageSource = new BitmapImage();

            imageSource.BeginInit();
            imageSource.UriSource = new Uri(imagePath, UriKind.RelativeOrAbsolute);
            imageSource.EndInit();

            return imageSource;
        }

        public static ObservableCollection<ObservableCollection<Square>> initializeBoard()
        {
            //ObservableCollection<ObservableCollection<Square>> board = new ObservableCollection<ObservableCollection<Square>>();
            for (int i = 0; i < boardSize; i++)
            {
                board.Add(new ObservableCollection<Square>());
                for (int j = 0; j < boardSize; j++)
                {
                    if (i == 0 || i == 2)
                    {
                        if (j % 2 == 1)
                        {
                            board[i].Add(new Square(i, j, SquareColor.Beige, new Piece(Color.Black, PieceType.Regular)));
                            board[i][j].Piece.Position = board[i][j];
                        }

                        else
                            board[i].Add(new Square(i, j, SquareColor.Brown));
                    } 
                    else if (i == 1)
                    {
                        if (j % 2 == 0)
                        {
                            board[i].Add(new Square(i, j, SquareColor.Beige, new Piece(Color.Black, PieceType.Regular)));
                            board[i][j].Piece.Position = board[i][j];
                        }
                        else
                            board[i].Add(new Square(i, j, SquareColor.Brown));
                    }

                    if (i == 5 || i == 7)
                    {
                        if (j % 2 == 0)
                        {
                            board[i].Add(new Square(i, j, SquareColor.Beige, new Piece(Color.Red, PieceType.Regular)));
                            board[i][j].Piece.Position = board[i][j];
                        }
                        else
                            board[i].Add(new Square(i, j, SquareColor.Brown));
                    }
                    else if (i == 6)
                    {
                        if (j % 2 == 1)
                        {
                            board[i].Add(new Square(i, j, SquareColor.Beige, new Piece(Color.Red, PieceType.Regular)));
                            board[i][j].Piece.Position = board[i][j];
                        }
                        else
                            board[i].Add(new Square(i, j, SquareColor.Brown));

                    }
                    else if (i==3 || i==4)
                    {
                        if (i % 2 == j % 2)
                            board[i].Add(new Square(i, j, SquareColor.Brown));
                        else
                            board[i].Add(new Square(i, j, SquareColor.Beige));
                    }
                }
            }
            return board;
        }

        public void SaveGame()
        {
            var path = @"C:\Users\daria\source\repos\MAP\WpfGame\Resources\save.txt";
            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine(extraMove);

            }

        }
    }
}
