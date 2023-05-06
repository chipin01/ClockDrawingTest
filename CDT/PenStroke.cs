using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using Microsoft.Ink;
using System.Drawing;

namespace CDT_Week_3
{
    public class PenStroke
    {
        public List<PacketPoint> pkPoints;
        private List<Point> bzPoints;
        private Stroke stroke;
        private String recoStroke;
        private long timestamp;
        // The cusps mark the points where
        // the stroke changes direction abruptly. A segment is defined as the
        // points between two cusps.
        private int[] polycusps;
        private int[] bzcusps;
        private Rectangle boundingBox;
        private Rectangle pixelBoundingBox;
        private int inflate = 3;
        private int id;
        private int mergeId, combineID, orderID;
        private Rectangle mergingRectangle;
        private Rectangle combinedRectangle;
        private int strokeNum = Int16.MinValue;
        private int boundingArea;
        private Point startPoint;
        private Point endPoint;
        public int isHand; // 1 is the long hand, and 2 is the shorthand
        private int actualNumber;
        private bool hasBeenCombined = false;

        public PenStroke(Stroke stk, Graphics g, InkPicture inkPic)
        {
            this.stroke = stk;
            this.id = stk.Id;
            this.isHand = int.MinValue;
            this.polycusps = stroke.PolylineCusps;
            this.bzcusps = stroke.BezierCusps;
            this.mergeId = int.MaxValue;
            this.combineID = int.MaxValue;
            this.pkPoints = new List<PacketPoint>();
            this.bzPoints = new List<Point>();

            #region convert the existing bezier points into the window based location and store it to a pen stroke
            Point[] bzPts = stk.BezierPoints;

            // Assign each Bezier point to the corresponding ACFormat cell
            foreach (Point bzPt in bzPts)
            {
                Point temp_bz = bzPt;

                // Convert the X, Y position to Window based pixel coordinates
                inkPic.Renderer.InkSpaceToPixel(g, ref temp_bz);

                if (inkPic.EditingMode == InkOverlayEditingMode.Ink)
                {
                    this.bzPoints.Add(temp_bz);
                }
            }
            #endregion

            #region compute bounding box
            Point ptLocation = new Point(stroke.GetBoundingBox().Left, stroke.GetBoundingBox().Top);
            Point ptSize = new Point(stroke.GetBoundingBox().Width, stroke.GetBoundingBox().Height);
            inkPic.Renderer.InkSpaceToPixel(g, ref ptLocation);
            inkPic.Renderer.InkSpaceToPixel(g, ref ptSize);
            pixelBoundingBox = new Rectangle(ptLocation, new Size(ptSize.X, ptSize.Y));
            pixelBoundingBox.Inflate(inflate, inflate);
            boundingArea = pixelBoundingBox.Width * pixelBoundingBox.Height;
            #endregion

        }

        public PenStroke()
        {
            bzPoints = new List<Point>();
            pkPoints = new List<PacketPoint>();

        }

        /// <summary>
        /// add a packet point into the container
        /// </summary>
        /// <param name="pkpt"></param>
        public void Add_PacketPoint(PacketPoint pkpt)
        {
            this.pkPoints.Add(pkpt);
        }

        /// <summary>
        /// get or set packet points in one stroke
        /// </summary>
        public List<PacketPoint> PacketPoints
        {
            set
            {
                this.pkPoints = value;
            }
            get
            {
                return this.pkPoints;
            }
        }

        /// <summary>
        /// add a bezier point into the container
        /// </summary>
        /// <param name="bzpt"></param>
        public void Add_BezierPoint(Point bzpt)
        {
            this.bzPoints.Add(bzpt);
        }

        /// <summary>
        /// get or set bezier points in one stroke
        /// </summary>
        public List<Point> BezierPoints
        {
            set
            {
                this.bzPoints = value;
            }
            get
            {
                return this.bzPoints;
            }
        }

        /// <summary>
        /// get the stroke data in one pen stroke
        /// </summary>
        public Stroke PenStk
        {
            get
            {
                return this.stroke;
            }
        }

        /// <summary>
        /// get or set the timestamp when finishing a stroke
        /// </summary>
        public long TimeStamp
        {
            get
            {
                return this.timestamp;
            }
            set
            {
                this.timestamp = value;
            }
        }

        public Rectangle BoundingBox
        {
            get
            {
                return this.boundingBox;
            }
            set
            {
                this.boundingBox = value;
            }
        }

        public Rectangle PixelBoundingBox
        {
            get
            {
                return pixelBoundingBox;
            }
        }

        public int Id
        {
            get
            {
                return this.id;
            }
        }
        // decipher whether number is first or second in the combined number
        public int OrderID
        {
            get
            {
                return this.orderID;
            }
            set
            {
                this.orderID = value;
            }
        }
        /// <summary>
        /// if the value return -1, it doesn't merge to any rectangle
        /// </summary>
        public int MergeTo
        {
            get
            {
                return this.mergeId;
            }
            set
            {
                this.mergeId = value;
            }
        }

        public Rectangle MergingRectangle
        {
            get
            {
                if (mergingRectangle != null)
                    return this.mergingRectangle;
                else
                    return this.pixelBoundingBox;
            }
            set
            {
                this.mergingRectangle = value;
            }
        }

        /// <summary>
        /// when two recognized numbers can combine a combinition number, e.g. "1" and "2" -> 12
        /// it needs to set the combined stroke ID
        /// </summary>
        public int CombineTo
        {
            get
            {
                return this.combineID;
            }
            set
            {
                this.combineID = value;
            }
        }

        public Rectangle CombinedRectangle
        {
            get
            {
                return this.combinedRectangle;
            }
            set
            {
                this.combinedRectangle = value;
                pixelBoundingBox = combinedRectangle;
            }
        }
        public String RecoStrokes
        {
            get
            {
                return this.recoStroke;
            }
            set
            {
                this.recoStroke = value;
            }
        }

        public int ActualNumber
        {
            get
            {
                return this.actualNumber;
            }
            set
            {
                this.actualNumber = value;
            }
        }


        public Point CenterPoint
        {
            get
            {
                int x = this.pixelBoundingBox.X + (this.pixelBoundingBox.Width / 2);
                int y = this.pixelBoundingBox.Y + (this.pixelBoundingBox.Height / 2);
                return new Point(x, y);
            }
        }
        public Point StartPoint
        {
            get
            {
                Point myPoint = new Point(bzPoints[0].X, bzPoints[0].Y);
                return myPoint;
            }
        }
        public Point EndPoint
        {
            get
            {
                int count = bzPoints.Count - 1;
                Point myPoint = new Point(bzPoints[count].X, bzPoints[count].Y);
                return myPoint;
            }
        }

        public int BoundingArea
        {
            get
            {
                return boundingArea;
            }
        }
    }
}
