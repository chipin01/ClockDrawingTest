using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CDT_Week_3
{
    class EndPoint
    {
        private Point end_pt;           // Storage for the X and Y coordinates of each end point
        private int prior;

        public EndPoint(int x, int y)
        {
            end_pt.X = x;
            end_pt.Y = y;
        }

        public void setEndPt(Point end_pt)
        {
            this.end_pt = end_pt;
        }

        public void setPrior(int prior)
        {
            this.prior = prior;
        }

        public Point getEndPt()
        {
            return end_pt;
        }

        public int getPrior()
        {
            return prior;
        }
    }
}
