using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer.Hockey
{
    public class Puck
    {
        public Point position;
        private Point acceleration;
        public Puck()
        {
            position = new Point(50, 0);
            acceleration = new Point(0, 0);
        }

        public void HitWithPlayer(Player player, Point playerAcceleration)
        {
        }
    }
}
