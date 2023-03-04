using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer.Hockey
{
    public class Player
    {
        public readonly string Name;
        public Point Position;

        public Player(string name, Point position)
        {
            Name = name;
            Position = position;
        }
    }
}
