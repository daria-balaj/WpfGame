using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using WpfGame.Commands;
using WpfGame.Models;
using WpfGame.Services;

namespace WpfGame.ViewModels
{
    public class GameVM
    {
        public GameLogic logic;
        public GameLogic Logic
        {
            get { return logic; }
            set { logic = value; }
        }
        //private readonly NavigationService ns;
        //private Frame _frame;
        public ObservableCollection<ObservableCollection<SquareVM>> Board { get; set; }
        public ObservableCollection<ObservableCollection<Square>> modelboard { get; set; }

        public PieceVM CurrentTurn { get; set; }

        public GameVM() //1
        {
            this.modelboard = Utility.initializeBoard();
            Piece piece = new Piece(Color.Red, PieceType.Regular);
            this.logic = new GameLogic(modelboard, piece);
            this.Board = BoardVM(modelboard);
            this.CurrentTurn = new PieceVM(piece);
            //_navigationService = ns;
            //_frame = frame;
        }

        private ObservableCollection<ObservableCollection<SquareVM>> BoardVM(ObservableCollection<ObservableCollection<Square>> board) //2
        {
            ObservableCollection<ObservableCollection<SquareVM>> result = new ObservableCollection<ObservableCollection<SquareVM>>();
            for (int i = 0; i < 8; i++)
            {
                ObservableCollection<SquareVM> line = new ObservableCollection<SquareVM>();
                for (int j = 0; j < 8; j++)
                {
                    SquareVM cellVM = new SquareVM(board[i][j], logic);
                    line.Add(cellVM);
                }
                result.Add(line);
            }
            return result;
        }

        //public ICommand QuitGameCommand => new RelayCommand<string>(Utility.QuitGame());
        public ICommand SaveGameCommand => new RelayCommand<ObservableCollection<ObservableCollection<Square>>>(Utility.SaveGame);
        public ICommand HelpCommand => new RelayCommand<string>(Utility.About);
    }


}
