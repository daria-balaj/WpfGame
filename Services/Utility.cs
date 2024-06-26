﻿using System;
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
using System.Data.Common;
using System.Windows;

namespace WpfGame.Services
{
    public class Utility
    {
        public static ImageSource beige_square = ConvertStringToImageSource(@"C:\Users\daria\source\repos\MAP\WpfGame\\Resources\BeigeSquare.jpg");
        public static ImageSource brown_square = ConvertStringToImageSource(@"C:\Users\daria\source\repos\MAP\WpfGame\Resources\BrownSquare.png");
        public static ImageSource light_piece = ConvertStringToImageSource(@"C:\Users\daria\source\repos\MAP\WpfGame\\Resources\RedCircle.png");
        public static ImageSource dark_piece = ConvertStringToImageSource(@"C:\Users\daria\source\repos\MAP\WpfGame\\Resources\BlackCircle.png");
        public static ImageSource light_king = ConvertStringToImageSource(@"C:\Users\daria\source\repos\MAP\WpfGame\Resources\CrownedRed.png");
        public static ImageSource dark_king = ConvertStringToImageSource(@"C:\Users\daria\source\repos\MAP\WpfGame\\Resources\CrownedBlack.png");
        public static ImageSource cell_highlight = ConvertStringToImageSource(@"C:\Users\daria\source\repos\MAP\WpfGame\\Resources\GreenSquare.png");

        public const int boardSize = 8; 
        public static int collectedRedPieces = 0;
        public static int collectedBlackPieces = 0;
        public static Piece currentTurn = new Piece(Color.Red, PieceType.Regular);
        public static List<Square> neighbors = new List<Square>();
        private static bool allowDoubleJump;
        private static bool isDoubleJumpPossible;
        public static Square selectedCell { get; set; }

        public static int maxRemainingPieces = 0, redWins = 0, blackWins = 0;

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

        public static bool AllowDoubleJump
        {
            get { return allowDoubleJump; }
            set { allowDoubleJump = value; }
        }
        public static bool IsDoubleJumpPossible
        {
            get { return isDoubleJumpPossible; }
            set { isDoubleJumpPossible = value; }
        }

        public static bool isInBounds(int row, int column)
        {
            return row >= 0 && column >= 0 && row < boardSize && column < boardSize;
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
            ObservableCollection<ObservableCollection<Square>> board = new ObservableCollection<ObservableCollection<Square>>();
            for (int i = 0; i < boardSize; i++)
            {
                board.Add(new ObservableCollection<Square>());
                for (int j = 0; j < boardSize; j++)
                {
                    if ((i + j) % 2 == 0)
                    {
                        board[i].Add(new Square(i, j, SquareColor.Brown, null));
                    }
                    else if (i < 3)
                    {
                        board[i].Add(new Square(i, j, SquareColor.Beige, new Piece(Color.Black)));
                    }
                    else if (i > 4)
                    {
                        board[i].Add(new Square(i, j, SquareColor.Beige, new Piece(Color.Red)));
                    }
                    else
                    {
                        board[i].Add(new Square(i, j, SquareColor.Beige, null));
                    }
                }
            }
            return board;
        }

        public const char NO_PIECE = 'N';
        public const char BLACK_PIECE = 'B';
        public const char RED_PIECE = 'R';
        public const char RED_KING = 'K';
        public const char BLACK_KING = 'L';
        public const char BLACK_TURN = '2';
        public const char RED_TURN = '1';
        public const char HAD_COMBO = 'H';
        public const char EXTRA_PATH = 'E';

        public static void SaveGame(ObservableCollection<ObservableCollection<Square>> board)
        {
            var path = @"C:\Users\daria\source\repos\MAP\WpfGame\Resources\save.txt";
            //var path = @"C:\Users\sorin\source\repos\WpfGame\Resources\save.txt";
            using (var writer = new StreamWriter(path))
            {
                if (allowDoubleJump)
                {
                    writer.Write(1);
                }
                else
                {
                    writer.Write(0);
                }
                writer.WriteLine();
                if (currentTurn.Color == Color.Red)
                {
                    writer.Write(RED_TURN);
                }
                else
                {
                    writer.Write(BLACK_TURN);
                }
                writer.WriteLine();
                //board
                foreach (var line in board)
                {
                    foreach (var square in line)
                    {
                        switch (square)
                        {
                            case { } when square.Piece == null:
                                writer.Write(NO_PIECE);
                                break;
                            case { } when square.Piece.Color == Color.Red && square.Piece.Type == PieceType.Regular:
                                writer.Write(RED_PIECE);
                                break;
                            case { } when square.Piece.Color == Color.Black && square.Piece.Type == PieceType.Regular:
                                writer.Write(BLACK_PIECE);
                                break;
                            case { } when square.Piece.Color == Color.Black && square.Piece.Type == PieceType.King:
                                writer.Write(BLACK_KING);
                                break;
                            case { } when square.Piece.Color == Color.Red && square.Piece.Type == PieceType.King:
                                writer.Write(RED_KING);
                                break;
                            default:
                                break;
                        }
                    }
                    writer.WriteLine();
                }
                writer.WriteLine(collectedRedPieces);
                writer.WriteLine(collectedBlackPieces);
                //writer.Write("-");
            }
        }

        public static (ObservableCollection<ObservableCollection<Square>>, Piece) LoadGame()
        {
            var path = @"C:\Users\daria\source\repos\MAP\WpfGame\Resources\save.txt";
            //var path = @"C:\Users\sorin\source\repos\WpfGame\Resources\save.txt";
            ObservableCollection<ObservableCollection<Square>> board = new ObservableCollection<ObservableCollection<Square>>();
            using (var reader = new StreamReader(path))
            {
                string text;
                text = reader.ReadLine();
                if (text == "1")
                {
                    allowDoubleJump = true;
                }
                else
                {
                    allowDoubleJump = false;
                }
                text = reader.ReadLine();
                if (text == RED_TURN.ToString())
                {
                    Utility.CurrentTurn.Color = Color.Red;
                    Utility.CurrentTurn.ImageSource = Utility.light_piece;
                }
                else
                {
                    Utility.CurrentTurn.Color = Color.Black;
                    Utility.CurrentTurn.ImageSource = dark_piece;
                }
                for (int i = 0; i < boardSize; i++)
                {
                    board.Add(new ObservableCollection<Square>());
                    text = reader.ReadLine();
                    for (int j = 0; j < boardSize; j++)
                    {
                        //board[i][j].Highlight = null;
                        switch (text[j])
                        {
                            case { } when text[j] == NO_PIECE:
                                if ((i + j) % 2 == 0)
                                {
                                    board[i].Add(new Square(i, j, SquareColor.Brown, null));
                                }
                                else board[i].Add(new Square(i, j, SquareColor.Beige, null));
                                break;
                            case { } when text[j] == RED_PIECE:
                                if ((i + j) % 2 == 0)
                                {
                                    board[i].Add(new Square(i, j, SquareColor.Brown, new Piece(Color.Red)));
                                }
                                else board[i].Add(new Square(i, j, SquareColor.Beige, new Piece(Color.Red)));
                                //board[i][j].Piece.Position = board[i][j];
                                break;
                            case { } when text[j] == RED_KING:
                                if ((i + j) % 2 == 0)
                                {
                                    board[i].Add(new Square(i, j, SquareColor.Brown, new Piece(Color.Red, PieceType.King)));
                                }
                                else
                                    board[i].Add(new Square(i, j, SquareColor.Beige, new Piece(Color.Red, PieceType.King)));
                                //board[i][j].Piece.Position = board[i][j];
                                break;
                            case { } when text[j] == BLACK_PIECE:
                                if ((i + j) % 2 == 0)
                                {
                                    board[i].Add(new Square(i, j, SquareColor.Brown, new Piece(Color.Black)));
                                }
                                else board[i].Add(new Square(i, j, SquareColor.Beige, new Piece(Color.Black)));
                                //board[i][j].Piece.Position = board[i][j];
                                break;
                            case { } when text[j] == BLACK_KING:
                                if ((i + j) % 2 == 0)
                                {
                                    board[i].Add(new Square(i, j, SquareColor.Brown, new Piece(Color.Black, PieceType.King)));
                                }
                                else
                                    board[i].Add(new Square(i, j, SquareColor.Beige, new Piece(Color.Black, PieceType.King)));
                                //board[i][j].Piece.Position = board[i][j];
                                break;
                        }
                    }
                }
                Utility.collectedRedPieces = int.Parse(reader.ReadLine());
                Utility.collectedBlackPieces = int.Parse(reader.ReadLine());

                neighbors.Clear();
                return (board, CurrentTurn);
            }
        }

        public static void Statistics(string s)
        {
            string PATH = @"C:\Users\daria\source\repos\MAP\WpfGame\Resources\stats.txt";

            using (var reader = new StreamReader(PATH))
            {
                Utility.redWins = int.Parse(reader.ReadLine());
                blackWins = int.Parse(reader.ReadLine());
                maxRemainingPieces = int.Parse(reader.ReadLine());
                string text = "Number of Player 1 wins: " + redWins + "\nNumber of Player 2 wins: " + blackWins + "\nRecord of remaining pieces on board: " + maxRemainingPieces;
                MessageBox.Show(text, "Statistics", MessageBoxButton.OK);
            }
        }

        public static void About(string s)
        {
            string PATH = @"C:\Users\daria\source\repos\MAP\WpfGame\Resources\about.txt";

            using (var reader = new StreamReader(PATH))
            {
                MessageBox.Show(reader.ReadToEnd(), "About", MessageBoxButton.OKCancel);
            }
        }

    }
}
