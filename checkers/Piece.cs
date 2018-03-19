using System;
using System.Collections.Generic;
using System.Text;

namespace gabi.checkers
{
    public enum PieceColor { Empty, White, Black }

    public class Piece
    {
        public bool IsChecker { get; set; }
        public PieceColor Color { get; }

        public Piece(PieceColor color)
        {
            Color = color;
        }
    }
}
