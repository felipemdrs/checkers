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

        public bool Go()
        {
            bool lose = _board.GetPieces(Color).Length == 0;

            if (!lose)
            {
                var isValid = false;

                var obrigatory = _board.Obrigatory(Color);

                if (obrigatory != null)
                {
                    string pieceId = obrigatory.Item1.Value.Id;

                    while (obrigatory != null)
                    {
                        _board.Capture(obrigatory.Item1, obrigatory.Item2, obrigatory.Item3, obrigatory.Item4);
                        obrigatory = _board.Obrigatory(Color);

                        if (obrigatory != null && pieceId != obrigatory.Item1.Value.Id) break;
                    }
                }
                else
                {
                    var timeOut = 1000;

                    while (!isValid)
                    {
                        var rows = _board.GetPieces(Color);
                        var square = rows.OrderBy(x => Guid.NewGuid()).FirstOrDefault();

                        var newX = square.X + square.GetVerticalDirection();
                        var newY = square.Y + GetRandoHorizontalDirection();

                        isValid = _board.Move(square, newX, newY);

                        if (--timeOut == 0)
                        {
                            return true;
                        }
                    }
                }
            }

            return lose;
        }

        private int GetRandoHorizontalDirection()
        {
            var dir = rnd.Next(0, 2);

            return dir == 0 ? -1 : 1;
        }
    }
}