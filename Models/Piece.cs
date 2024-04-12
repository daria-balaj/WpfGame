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
    public class Piece
    {
        public Square? Position { get; set; }
        public ImageSource ImageSource { get; set; }
        public Color Color { get; set; }
        public PieceType Type { get; set; }

        public Piece(Color color, PieceType type)
        {
            this.Color = color;
            this.Type = type;
            if (this.Color == Color.Black) this.ImageSource = Utility.dark_piece;
            else this.ImageSource = Utility.light_piece;
        }
    }
}
