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
        //public Color color;
        //public ImageSource imageSource;

        public Piece Piece
        {
            get { return this.piece; }
            set { this.piece = value; NotifyPropertyChanged(); }
        }

        //public Color Color
        //{
        //    get { return this.color; }
        //    set { 
        //        this.color = value;
        //        if (color == Color.Black) { this.imageSource = Utility.dark_piece; }
        //        else this.imageSource = Utility.light_piece;
        //        NotifyPropertyChanged("Color");
        //        NotifyPropertyChanged("ImageSource");
        //    }
        //}

        //public ImageSource ImageSource
        //{  
        //    get { return this.imageSource; }
        //    set { imageSource = value; NotifyPropertyChanged("ImageSource"); }

        //}

        public PieceVM(Piece piece)
        {
            this.piece = piece;
            //this.Color = Color.Red;
            //this.imageSource = Utility.light_piece;
        }
    }
}
