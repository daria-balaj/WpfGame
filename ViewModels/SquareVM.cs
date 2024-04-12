using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfGame.Commands;
using WpfGame.Models;
using WpfGame.Services;

namespace WpfGame.ViewModels
{
    public class SquareVM
    {
        public Square Cell { get; set; }
        public GameLogic logic;
        private ICommand startMoveCommand;
        private ICommand movePieceCommand;
        public SquareVM(Square square, GameLogic logic) //3
        {
            this.Cell = square;
            this.logic = logic;
        }
        public ICommand StartMoveCommand
        {
            get
            {
                if (startMoveCommand == null)
                {
                    startMoveCommand = new RelayCommand<Square>(logic.ClickPiece);
                }
                return startMoveCommand;
            }
        }

        public ICommand MovePieceCommand
        {
            get
            {
                if (movePieceCommand == null)
                {
                    movePieceCommand = new RelayCommand<Square>(logic.MovePiece);
                }
                return movePieceCommand;
            }
        }

    }
}
