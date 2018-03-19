using System;
using System.Collections.Generic;
using System.Text;

namespace gabi.checkers
{
    class Counter
    {
        private readonly Logic _p1;
        private readonly Logic _p2;

        private bool _turn = false;

        private Logic _current;

        public Counter(Logic p1, Logic p2)
        {
            _p1 = p1;
            _p2 = p2;

            _current = p1.Color == PieceColor.White ? p1 : p2;
        }

        public void Start()
        {
            while (true)
            {
                _current.Go();
                NextTurn();
                Console.ReadKey();
            }
        }

        private void NextTurn()
        {
            _turn = !_turn;
            _current = _turn ? _p2 : _p1;
        }
    }
}
