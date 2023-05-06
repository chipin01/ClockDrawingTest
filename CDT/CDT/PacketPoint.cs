using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CDT_Week_3
{
    public class PacketPoint
    {
        private Point pkPt = new Point();      // Coordinate for each packet point        
        private long timestamp;
        private int preesure, size, strokeID;
        // Preparing the space for packet points and time stamps
        public PacketPoint()
        {
            this.pkPt = new Point();
            
        }

        /// <summary>
        /// get or set the location of this packet point
        /// </summary>
        public Point PkPt
        {
            set
            {
                pkPt.X = value.X;
                pkPt.Y = value.Y;
            }
            get
            {
                return this.pkPt;
            }
        }

        /// <summary>
        /// get or set the timestamp of this packet point
        /// </summary>
        public long TimeStamp
        {
            set
            {
                this.timestamp = value;
            }
            get
            {
                return this.timestamp;
            }
        }        

        public int Pressure
        {
            get
            {
                return this.preesure;
            }
            set
            {
                this.preesure = value;
            }
        }

        public int Size
        {
            get
            {
                return this.size;
            }
            set
            {
                this.size = value;
            }
        }

        public int StrokeID
        {
            get
            {
                return this.strokeID;
            }
            set
            {
                this.strokeID = value;
            }
        }
    }
}
