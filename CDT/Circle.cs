using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CDT_Week_3
{
    public class Circle
    {
        private float startX, startY, centerX, centerY, radius;

        public Circle(float x, float y, float width)
        {
            this.startX = x;
            this.startY = y;
            this.radius = width/2;            

            this.centerX = x + radius;
            this.centerY = y + radius;

            this.Times = 0;
        }

        public float Radius
        {
            get
            {
                return this.radius;
            }
        }

        public Point Center
        {
            get
            {
                return new Point((int)this.centerX, (int)this.centerY);
            }
        }

        public Point TopLeft
        {
            get
            {
                return new Point((int)this.startX, (int)this.startY);
            }
        }

        public double ScaleX { set; get; }
        public double ScaleY { set; get; }
        public double Times { set; get; }

    }
}
