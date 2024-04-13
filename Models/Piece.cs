using MVVMPairs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WpfGame.Services;

namespace WpfGame.Models
{
    public enum Color
    {
        Red,
        Black
    }
    public enum PieceType
    {
        Regular,
        King
    }
    public class Piece : BaseNotification
    {
        private Square? position;
        public Square? Position
        {
            get { return this.position; }
            set { this.position = value; NotifyPropertyChanged(); }
        }
        private ImageSource imageSource;
        public ImageSource ImageSource
        {
            get { return this.imageSource; }
            set { this.imageSource = value; NotifyPropertyChanged(); }
        }
        private Color color;
        public Color Color
        {
            get { return this.color; }
            set 
            {
                this.color = value;
                if (color == Color.Black) { this.imageSource = Utility.dark_piece; }
                else this.imageSource = Utility.light_piece;
                NotifyPropertyChanged("Color");
                NotifyPropertyChanged("ImageSource");
            }
        }
        private PieceType type;
        public PieceType Type
        {
            get { return this.type; }
            set { this.type = value; NotifyPropertyChanged(); }
        }
        public Piece(Color color, PieceType type)
        {
            this.color = color;
            this.type = type;
            if (this.color == Color.Black) this.imageSource = Utility.dark_piece;
            else this.imageSource = Utility.light_piece;
        }
        public Piece(Color color)
        {
            this.color = color;
            this.type = PieceType.Regular;
            if (this.color == Color.Black) this.imageSource = Utility.dark_piece;
            else this.imageSource = Utility.light_piece;
        }

        public Piece(Piece other)
        {
            this.Position = other.Position;
            this.ImageSource = other.ImageSource;
            this.Color = other.Color;
            this.Type = other.Type;
        }
    }
}
