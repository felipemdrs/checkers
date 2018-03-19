using System;
using System.Collections.Generic;
using System.Text;

namespace gabi.checkers
{
    public class Statistics
    {
        private int _matches;
        private int _p1Win;
        private int _p2Win;

        public void RegisterMatch()
        {
            _matches++;
        }

        public void Play1Win()
        {
            _p1Win++;
        }

        public void Play2Win()
        {
            _p2Win++;
        }

        public int GetPlay1Win()
        {
            return _p1Win;
        }

        public int GetPlay2Win()
        {
            return _p2Win;
        }
    }
}
