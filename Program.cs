using System;
using gabi.checkers;

namespace gabi
{
    class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board();
            board.NewGame();

            Logic p1 = new Logic(PieceColor.White, board);
            Logic p2 = new Logic(PieceColor.Black, board);

            Counter c = new Counter(p1, p2);
            c.Start();
            
            board.Print();
        }
    }
}
