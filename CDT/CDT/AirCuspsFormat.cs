using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;


namespace CDT_Week_3
{
    // Temporary storage for the information related each stroke 
    // (Holding collected information about the strokes until multi-stroke merging process is finished)
    class AirCuspsFormat
    {
        private Point[] cusps = new Point[1];   // Storage for the X and Y coordinates of endpoints in each stroke
        private float airtime = 0;              // Storage for airtime 
        private int order = 0;                  // Storage for the order of generation for each stroke
        private Point[] bzPts = new Point[1];   // Storage for Bezier points
        private Point[] pkPts = new Point[1];   // Storage for packet points
        
        private int first_hour = 0;     // Storage for the hour timestamp when the user starts drawing a stroke
        private int first_min = 0;      // Storage for the minute timestamp when the user starts drawing a stroke
        private int first_sec = 0;      // Storage for the second timestamp when the user starts drawing a stroke
        private int first_msec = 0;     // Storage for the millisecond timestamp when the user starts drawing a stroke

        private int last_hour = 0;      // Storage for the hour timestamp when the user finished drawing a stroke
        private int last_min = 0;       // Storage for the minute timestamp when the user finished drawing a stroke
        private int last_sec = 0;       // Storage for the second timestamp when the user finished drawing a stroke
        private int last_msec = 0;      // Storage for the millisecond timestamp when the user finished drawing a stroke

        private float penup_time = 0;       // Storage for penup time
        private float pendown_time = 0;     // Storage for pendown time

        // Store the cusp coordinates of strokes
        public void setCusps(Point cusp, int i)
        {
            this.cusps[i].X = cusp.X;
            this.cusps[i].Y = cusp.Y;
        }

        // Store the airtime between strokes
        public void setAirtime(float airtime)
        {
            this.airtime = airtime;
        }

        // Store all the coordinates of endpoints
        public void setCuspArea(int num_cusps)
        {
            cusps = new Point[num_cusps];
        }

        // Store the order of strokes
        public void setOrder(int order)
        {
            this.order = order;
        }

        // Setting the space for bezier points
        public void setBzPtArea(int num_bzpts)
        {
            bzPts = new Point[num_bzpts];
        }

        // Setting the space for packet points
        public void setPkPtArea(int num_pkpts)
        {
            pkPts = new Point[num_pkpts];
        }

        // Assign each incoming Bezier point in the array
        public void setBzPts(Point bzPt, int i)
        {
            this.bzPts[i].X = bzPt.X;
            this.bzPts[i].Y = bzPt.Y;
        }

        // Assign each incoming packet point in the array
        public void setPkPts(Point t_pkPt, int i)
        {
            this.pkPts[i] = t_pkPt;
        }
        
        public void setFirstHour(int first_hour)
        {
            this.first_hour = first_hour;
        }

        public void setFirstMin(int first_min)
        {
            this.first_min = first_min;
        }

        public void setFirstSec(int first_sec)
        {
            this.first_sec = first_sec;
        }

        public void setFirstMsec(int first_msec)
        {
            this.first_msec = first_msec;
        }

        public void setLastHour(int last_hour)
        {
            this.last_hour = last_hour;
        }

        public void setLastMin(int last_min)
        {
            this.last_min = last_min;
        }

        public void setLastSec(int last_sec)
        {
            this.last_sec = last_sec;
        }

        public void setLastMsec(int last_msec)
        {
            this.last_msec = last_msec;
        }

        public void setPendownTime(float pendown_time)
        {
            this.pendown_time = pendown_time;
        }

        public void setPenupTime(float penup_time)
        {
            this.penup_time = penup_time;
        }

        // Return the saved airtime
        public float getAirtime()
        {
            return airtime;
        }

        public Point[] getCusps()
        {
            return cusps;
        }

        public int getOrder()
        {
            return order;
        }

        public Point[] getBzPts()
        {
            return bzPts;
        }

        public Point[] getPkPts()
        {
            return pkPts;
        }

        public int getFirstHour()
        {
            return first_hour;
        }

        public int getFirstMin()
        {
            return first_min;
        }

        public int getFirstSec()
        {
            return first_sec;
        }

        public int getFirstMsec()
        {
            return first_msec;
        }

        public int getLastHour()
        {
            return last_hour;
        }

        public int getLastMin()
        {
            return last_min;
        }

        public int getLastSec()
        {
            return last_sec;
        }

        public int getLastMsec()
        {
            return last_msec;
        }

        public float getPendownTime()
        {
            return pendown_time;
        }

        public float getPenupTime()
        {
            return penup_time;
        }
    }
}
