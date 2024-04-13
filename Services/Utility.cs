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
using System.Data.Common;
using System.Windows;

namespace WpfGame.Services
{
    public class Utility
    {
        public static ImageSource beige_square = ConvertStringToImageSource(@"C:\Users\sorin\source\repos\WpfGame\\Resources\BeigeSquare.jpg");
        public static ImageSource brown_square = ConvertStringToImageSource(@"C:\Users\sorin\source\repos\WpfGame\\Resources\BrownSquare.png");
        public static ImageSource light_piece = ConvertStringToImageSource(@"C:\Users\sorin\source\repos\WpfGame\\Resources\RedCircle.png");
        public static ImageSource dark_piece = ConvertStringToImageSource(@"C:\Users\sorin\source\repos\WpfGame\\Resources\BlackCircle.png");
        public static ImageSource light_king = ConvertStringToImageSource(@"C:\Users\sorin\source\repos\WpfGame\Resources\CrownedRed.png");
        public static ImageSource dark_king = ConvertStringToImageSource(@"C:\Users\sorin\source\repos\WpfGame\\Resources\CrownedBlack.png");
        public static ImageSource cell_highlight = ConvertStringToImageSource(@"C:\Users\sorin\source\repos\WpfGame\\Resources\GreenSquare.png");

        public const int boardSize = 8; 
        public static int collectedRedPieces = 0;
        public static int collectedBlackPieces = 0;
        //public static Color currentTurn = Color.Red;
        public static Piece currentTurn = new Piece(Color.Red, PieceType.Regular);
        //public static ObservableCollection<ObservableCollection<Square>> board = new ObservableCollection<ObservableCollection<Square>>();
        public static List<Square> neighbors = new List<Square>();
        public static Square selectedCell { get; set; }



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

        private static bool extraMove = false;
        public static bool ExtraMove
        {
            get { return extraMove; }
            set { extraMove = value; }
        }
        private static bool extraPath = false;
        public static bool ExtraPath
        {
            get { return extraPath; }
            set { extraPath = value; }
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
            //var path = @"C:\Users\daria\source\repos\MAP\WpfGame\Resources\save.txt";
            var path = @"C:\Users\sorin\source\repos\WpfGame\Resources\save.txt";
            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine(extraMove);
                writer.WriteLine();
                if (ExtraMove)
                {
                    writer.Write(HAD_COMBO);
                }
                else
                {
                    writer.Write(NO_PIECE);
                }
                writer.WriteLine();
                if (ExtraPath)
                {
                    writer.Write(ExtraPath);
                }
                else
                {
                    writer.Write(NO_PIECE);
                }
                writer.WriteLine();
                //TO_DO_MULTI_JUMP
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
                            case { } when currentTurn.Color == Color.Red && square.Piece.Type == PieceType.King:
                                writer.Write(RED_KING);
                                break;
                            default:
                                break;
                        }
                    }
                    writer.WriteLine();
                }
                //writer.Write("-");
            }
        }

        public static void LoadGame()
        {
            //var path = @"C:\Users\daria\source\repos\MAP\WpfGame\Resources\save.txt";
            var path = @"C:\Users\sorin\source\repos\WpfGame\Resources\save.txt";
            ObservableCollection<ObservableCollection<Square>> board = new ObservableCollection<ObservableCollection<Square>>();
            using (var reader = new StreamReader(path))
            {
                string text;
                text = reader.ReadLine();
                if (text != NO_PIECE.ToString())
                {
                    ExtraMove = true;
                }
                else
                {
                    ExtraMove = false;
                }
                text = reader.ReadLine();
                if (text != NO_PIECE.ToString())
                {
                    ExtraPath = true;
                }
                else
                {
                    ExtraPath = false;
                }
                //to_do_multi_JUMP
                text = reader.ReadLine();
                if (text == RED_TURN.ToString())
                {
                    currentTurn.Color = Color.Red;
                    currentTurn.ImageSource = light_piece;
                }
                else
                {
                    currentTurn.Color = Color.Black;
                    currentTurn.ImageSource = dark_piece;
                }
                //board
                for (int i = 0; i < boardSize; i++)
                {
                    text = reader.ReadLine();
                    for (int j = 0; j < boardSize; j++)
                    {
                        board[i][j].Highlight = null;
                        switch (text[j])
                        {
                            case { } when text[j] == NO_PIECE:
                                board[i][j].Piece = null;
                                break;
                            case { } when text[j] == RED_PIECE:
                                board[i][j].Piece = new Piece(Color.Red, PieceType.Regular);
                                board[i][j].Piece.Position = board[i][j];
                                //to_DO
                                break;
                            case { } when text[j] == RED_KING:
                                board[i][j].Piece = new Piece(Color.Red, PieceType.King);
                                board[i][j].Piece.Position = board[i][j];

                                //todo
                                break;
                            case { } when text[j] == BLACK_PIECE:
                                board[i][j].Piece = new Piece(Color.Black, PieceType.Regular);
                                board[i][j].Piece.Position = board[i][j];
                                break;
                            case { } when text[j] == BLACK_KING:
                                board[i][j].Piece = new Piece(Color.Black, PieceType.King);
                                board[i][j].Piece.Position = board[i][j];
                                break;
                        }
                    }
                }

                neighbors.Clear();

            }
        }

        public static void About(string s)
        {
            string PATH = @"C:\Users\sorin\source\repos\WpfGame\Resources\about.txt";

            using (var reader = new StreamReader(PATH))
            {
                MessageBox.Show(reader.ReadToEnd(), "About", MessageBoxButton.OKCancel);
            }
        }

        public static int maxRemainingPieces = 0, redWins = 0, blackWins = 0;

        public static void Statistics(string s)
        {
            string PATH = @"C:\Users\sorin\source\repos\WpfGame\Resources\stats.txt";

            using (var reader = new StreamReader(PATH))
            {
                redWins = int.Parse(reader.ReadLine());
                blackWins = int.Parse(reader.ReadLine());
                maxRemainingPieces = int.Parse(reader.ReadLine());
                string text = "Number of Player 1 wins: " + redWins + "\n Number of Player 2 wins: " + blackWins + "\n Record of remaining pieces on board: " + maxRemainingPieces;
                MessageBox.Show(text, "Statistics", MessageBoxButton.OK);
            }
        }

        //internal static Action<string> QuitGame()
        //{
        //    //return 
        //}
    }
}
