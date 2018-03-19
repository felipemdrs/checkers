using System;
using System.Linq;

namespace gabi.checkers
{
    public class Logic
    {
        public PieceColor Color { get; }

        private readonly Board _board;
        private readonly Random rnd = new Random();

        public Logic(PieceColor color, Board board)
        {
            Color = color;
            _board = board;
        }

        private void GetPlayerPieces()
        {
            _board.GetPieces(Color);
        }

        public void Go()
        {
            var isValid = false;

            var obrigatory = _board.Obrigatory(Color);

            if (obrigatory != null)
            {
                while (obrigatory != null)
                {
                    _board.Capture(obrigatory.Item1, obrigatory.Item2, obrigatory.Item3, obrigatory.Item4);
                    obrigatory = _board.Obrigatory(Color);
                }
            }
            else
            {
                while (!isValid)
                {
                    var rows = _board.GetPieces(Color);
                    var square = rows.OrderBy(x => Guid.NewGuid()).FirstOrDefault();

                    var newX = square.X + square.GetVerticalDirection();
                    var newY = square.Y + GetRandoHorizontalDirection();

                    isValid = _board.Move(square, newX, newY);
                }
            }

            _board.Print();
        }

        private int GetRandoHorizontalDirection()
        {
            var dir = rnd.Next(0, 2);

            return dir == 0 ? -1 : 1;
        }
    }
}