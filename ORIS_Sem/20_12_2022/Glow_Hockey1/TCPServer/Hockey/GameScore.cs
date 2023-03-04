using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer.Hockey
{
    public class GameScore
    {
        public int firstPLayer { get; private set; }
        public int secondPLayer { get; private set; }

        public GameScore()
        {
            firstPLayer = 0;
            secondPLayer = 0;
        }

        public int increaseFirstPlayerScore => firstPLayer++;
        public int increaseSecondPlayerScore => secondPLayer++;

    }
}
