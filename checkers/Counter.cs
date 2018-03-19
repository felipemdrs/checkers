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

        public Statistics Statistics { get; }

        public Counter(Logic p1, Logic p2)
        {
            _p1 = p1;
            _p2 = p2;

            Statistics = new Statistics();
            Reset();
        }

        public void Start()
        {
            while (!_current.Go())
            {
                NextTurn();
            }

            if (_turn)
            {
                Statistics.Play2Win();
            }
            else
            {
                Statistics.Play1Win();
            }
        }

        private void NextTurn()
        {
            _turn = !_turn;
            _current = _turn ? _p2 : _p1;
        }

        public void Reset()
        {
            Statistics.RegisterMatch();

            _current = _p1.Color == PieceColor.White ? _p1 : _p2;
            _turn = false;
        }
    }
}
