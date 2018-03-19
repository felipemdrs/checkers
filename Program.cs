using System;
using gabi.checkers;

namespace gabi
{
    class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board();

            Logic p1 = new Logic(PieceColor.White, board);
            Logic p2 = new Logic(PieceColor.Black, board);
            Counter c = new Counter(p1, p2);

            int max = 10000;

            for (int i = 0; i < max; i++)
            {
                Console.WriteLine("Partida: " + (i + 1) + "/" + max);
                board.NewGame();
                c.Start();
                c.Reset();
            }

            Console.WriteLine();
            Console.WriteLine("---- Statistics ----");
            Console.WriteLine("P1: " + c.Statistics.GetPlay1Win());
            Console.WriteLine("P2: " + c.Statistics.GetPlay2Win());
            Console.WriteLine("--------------------");

        }
    }
}
