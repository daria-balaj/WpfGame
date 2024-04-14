using MVVMPairs.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Navigation;
using WpfGame.Models;
using WpfGame.Services;
using Color = WpfGame.Models.Color;

namespace WpfGame.ViewModels
{
    public class PieceVM : BaseNotification
    {
        public Piece piece;

        public Piece Piece
        {
            get { return this.piece; }
            set { this.piece = value; NotifyPropertyChanged(); }
        }

        public PieceVM(Piece piece)
        {
            this.piece = piece;
            //this.Color = Color.Red;
            //this.imageSource = Utility.light_piece;
        }
    }
}
