using System;

namespace gabi.checkers
{
    public class Square
    {
        public enum SquareColor { White, Black }

        public Piece Value { get; private set; }

        public SquareColor Color { get; set; }

        public int X { get; }
        public int Y { get; }

        public Square(int x, int y, SquareColor color)
        {
            Color = color;
            X = x;
            Y = y;
        }

        public bool SetValue(PieceColor pieceColor)
        {
            if (Value != null || Color == SquareColor.White) return false;

            Value = pieceColor == PieceColor.Empty ? null : new Piece(pieceColor);

            return true;
        }

        public void SetValue(Piece piece)
        {
            Value = piece;
        }

        public override string ToString()
        {
            if (Color == SquareColor.White)
            {
                return " |";
            }

            if (Value == null)
            {
                return "_|";
            }

            switch (Value.Color)
            {
                case PieceColor.White:
                    return "O|";
                case PieceColor.Black:
                    return "X|";
                default:
                    return "_|";
            }
        }

        public int GetVerticalDirection()
        {
            return Value.Color == PieceColor.White ? 1 : -1;
        }
    }
}