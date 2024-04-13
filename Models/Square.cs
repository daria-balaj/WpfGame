using MVVMPairs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using WpfGame.Services;

namespace WpfGame.Models
{
    public enum SquareColor
    {
        Brown,
        Beige
    }
    public class Square : BaseNotification
    {
        private int row;
        public int Row
        {
            get { return row; }
            set { row = value; NotifyPropertyChanged("Row"); }
        }
        private int column;
        public int Column
        {
            get { return column; }
            set { column = value; NotifyPropertyChanged("Column"); }
        }
        private Piece piece;
        public Piece Piece
        {
            get { return piece; }
            set { piece = value; NotifyPropertyChanged("Piece"); }
        }
        private SquareColor color;
        public SquareColor Color
        {
            get { return color; }
            set { color = value; NotifyPropertyChanged("Color"); }
        }
        private ImageSource? pathToImage;
        public ImageSource? PathToImage
        {
            get { return pathToImage; }
            set { pathToImage = value; NotifyPropertyChanged("ImageSource"); }
        }

        private ImageSource? highlight;
        public ImageSource? Highlight
        {
            get { return highlight; }
            set { highlight = value; NotifyPropertyChanged("Highlight"); }
        }

        public Square(int row, int column, SquareColor color, Piece piece)
        {
            this.Row = row;
            this.Column = column;
            this.Piece = piece;
            this.Color = color;
            if (this.Color == SquareColor.Beige) this.pathToImage = Utility.beige_square;
            else this.pathToImage = Utility.brown_square;
        }

        public Square(int row, int column, SquareColor color)
        {
            this.Row = row;
            this.Column = column;
            this.Color = color;
            if (this.Color == SquareColor.Beige) this.pathToImage = Utility.beige_square;
            else this.pathToImage = Utility.brown_square;
            this.Piece = null;
        }
    }
}
