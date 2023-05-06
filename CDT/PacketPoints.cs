using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CDT_Week_3
{
    class PacketPoint
    {
        private Point pkPts = new Point();      // Coordinate for each packet point
        private int[] hours = new int[1];       // hour time stamp for each packet
        private int[] mins = new int[1];        // minute time stamp for each packet
        private int[] secs = new int[1];        // second time stamp for each packet
        private int[] msecs = new int[1];       // millisecond time stamp for each packet

        // Preparing the space for packet points and time stamps
        public PacketPoint()
        {
            pkPts = new Point();
        }

        public void setPkPt(Point pkPt)
        {
            pkPts.X = pkPt.X;
            pkPts.Y = pkPt.Y;
        }

        public int getX()
        {
            return pkPts.X;
        }

        public int getY()
        {
            return pkPts.Y;
        }
    }
}
