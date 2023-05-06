using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace CDT_Week_3
{
    class RecoRect
    {
        public Rectangle[] copyRect(Rectangle[] divRect, int count)
        {
            Rectangle[] tempRect = new Rectangle[count];

            for (int i = 0; i < count; i++)
            {
                tempRect[i] = divRect[i];
            }

            return tempRect;
        }

        /// <summary>
        /// Check whether the two rectangles are overlapping.
        /// If they are overlapping, the distance of the center points 
        /// will be shorter than the sum of the half width/height of the rectangles
        /// </summary>
        /// <param name="prevRect"></param>
        /// <param name="nextRect"></param>
        /// <returns></returns>
        public bool checkOverlap(Rectangle prevRect, Rectangle nextRect)
        {
            int halfWidthPrev = (prevRect.Right - prevRect.Left) / 2;
            int halfHeightPrev = (prevRect.Bottom - prevRect.Top) / 2;

            int midHorCoordPrev = prevRect.Left + halfWidthPrev;
            int midVerCoordPrev = prevRect.Top + halfHeightPrev;


            int halfWidthNext = (nextRect.Right - nextRect.Left) / 2;
            int halfHeightNext = (nextRect.Bottom - nextRect.Top) / 2;

            int midHorCoordNext = nextRect.Left + halfWidthNext;
            int midVerCoordNext = nextRect.Top + halfHeightNext;

            int interHorMidLength = Math.Abs(midHorCoordNext - midHorCoordPrev);
            int interVerMidLength = Math.Abs(midVerCoordNext - midVerCoordPrev);            

            if ((interHorMidLength < (halfWidthNext + halfWidthPrev)) &&
                (interVerMidLength < (halfHeightNext + halfHeightPrev)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Rectangle updatedRect(Rectangle prevRect, Rectangle nextRect)
        {
            Rectangle newRectangle = new Rectangle();

            Point ptLoc = new Point();
            int ptWidth = 0, ptHeight = 0;

            ptLoc.X = prevRect.Left < nextRect.Left ? prevRect.Left : nextRect.Left;
            ptLoc.Y = prevRect.Top < nextRect.Top ? prevRect.Top : nextRect.Top;
            ptWidth = (prevRect.Right < nextRect.Right ? nextRect.Right : prevRect.Right) - ptLoc.X;
            ptHeight = (prevRect.Bottom < nextRect.Bottom ? nextRect.Bottom : prevRect.Bottom) - ptLoc.Y;

            newRectangle.Location = ptLoc;
            newRectangle.Width = ptWidth;
            newRectangle.Height = ptHeight;

            return newRectangle;
        }

        // Sort the collection of 1s and 2s by its X coordinate from lowest to highest
        public DBFormat[] sortDBFormat(DBFormat[] dbFor)
        {
            DBFormat[] tempDBFormat = new DBFormat[dbFor.Length];
            tempDBFormat = dbFor;

            DBFormat tmpDB = new DBFormat();

            int temp_DB_count = 0;

            for (int i = 0; i < tempDBFormat.Length; i++)
            {
                if ((tempDBFormat[i].getRecogNumber() == 1) || (tempDBFormat[i].getRecogNumber() == 2))
                {
                    temp_DB_count++;
                }
                else
                {
                    break;
                }
            }
            
            for (int i = 0 ; i < temp_DB_count ; i++)
            {
                int j = i;
                while (j > 0 && tempDBFormat[j - 1].getPt1().X > tempDBFormat[j].getPt1().X)
                {
                    tmpDB = tempDBFormat[j];
                    tempDBFormat[j] = tempDBFormat[j - 1];
                    tempDBFormat[j - 1] = tmpDB;
                    j--;
                }
            }

            return tempDBFormat;
        }

        // Assign each sorted 1s and 2s in the storage of corresponding number.
        public DBFormat[] assignDBFormat(DBFormat[] dbFormat, DBFormat[] dbFor1, DBFormat[] dbFor2)
        {
            DBFormat[] tempDBFormat = new DBFormat[dbFormat.Length];
            tempDBFormat = dbFormat;

            DBFormat[] tempDBFor1 = new DBFormat[dbFor1.Length];
            tempDBFor1 = dbFor1;
            DBFormat[] tempDBFor2 = new DBFormat[dbFor2.Length];
            tempDBFor2 = dbFor2;

            int cnt1 = 0, cnt2 = 0;

            for (int i = 0; i < dbFormat.Length; i++)
            {
                if (tempDBFormat[i].getRecogNumber() == 1)
                {
                    if (tempDBFormat[i].getPt2().Y <= 262)
                    {
                        tempDBFormat[i] = dbFor1[cnt1];
                        tempDBFormat[i].setCnt_1s(cnt1);
                        cnt1++;
                    }
                }
                else if (tempDBFormat[i].getRecogNumber() == 2)
                {
                    if (tempDBFormat[i].getPt2().Y <= 262)
                    {
                        tempDBFormat[i] = dbFor2[cnt2];
                        tempDBFormat[i].setCnt_2s(cnt2);
                        cnt2++;
                    }
                }
            }

            return tempDBFormat;
        }

        // Assign Width and Height of each rectangle area
        public DBFormat[] assignWidthHeight(DBFormat[] dbFormat)
        {
            DBFormat[] tempDBFormat = dbFormat;

            foreach (DBFormat dbf in tempDBFormat)
            {
                dbf.setWidth(dbf.getPt2().X - dbf.getPt1().X);
                dbf.setHeight(dbf.getPt2().Y - dbf.getPt1().Y);
 /*               MessageBox.Show("pt2: (" + dbf.getPt2().X + ", " + dbf.getPt2().Y + "), pt1: (" 
                    + dbf.getPt1().X + ", " + dbf.getPt1().Y + "), "
                    + "width: " + dbf.getWidth() + ", height: " + dbf.getHeight());*/
            }

            return tempDBFormat;
        }

        // Move airtime, Cusp points, Bezier points, and Packet points in each ACFormat to DBFormat variables
        public DBFormat[] assignAirtime(List<AirCuspsFormat> acFormat, DBFormat[] dbFormat)
        {
            AirCuspsFormat[] tempACFormat = new AirCuspsFormat[acFormat.Count];
            acFormat.CopyTo(tempACFormat);

            AirCuspsFormat tempACF = new AirCuspsFormat();

            DBFormat[] tempDBFormat = new DBFormat[dbFormat.Length];
            tempDBFormat = dbFormat;

            // TO DO: Only the first pendown and penup time are calculated and it seems wrong.

            foreach (DBFormat dbf in tempDBFormat)
            {
                int dup_count = 0;
                float temp_pendown = 0;
                float last_penup = 0;

                foreach (AirCuspsFormat acf in tempACFormat)
                {
                    tempACF = acf;
                    last_penup = acf.getPenupTime();

                    // Check whether the first cusp point of each ACFormat variable is inside of
                    // the rectangle recognition area of any DBFormat variable or not.
                    // If anyone of the cusp point is inside of that rectangle area, 
                    // assign airtime, cusp points, Bezier points, and packet points 
                    // into the corresponding DBF variable.
                    if(isInside(tempACF, dbf))
                    {
                        dup_count++;
                        dbf.setAirtime(acf.getAirtime());
                        dbf.setCusps(acf.getCusps());
                        dbf.setBzPts(acf.getBzPts());
                        dbf.setPkPts(acf.getPkPts());
                        temp_pendown += acf.getPendownTime();
                        temp_pendown += acf.getPenupTime();
                        break;
                    }
                }

                temp_pendown -= last_penup;
                dbf.setPendownTime(temp_pendown);
                dbf.setPenupTime(last_penup);
            }

            foreach (DBFormat dbf in dbFormat)
            {
            //    MessageBox.Show("Number: " + dbf.getRecogNumber() + ", Airtime: " + dbf.getAirtime());
            }

            return tempDBFormat;
        }

        // Check whether the first cusp point of each ACFormat variable is inside of
        // the rectangle recognition area of any DBFormat variable or not.
        // If anyone of the cusp point is inside of that rectangle area return true,
        // return false otherwise.
        public bool isInside(AirCuspsFormat acf, DBFormat dbf)
        {
            AirCuspsFormat tempACF = new AirCuspsFormat();
            tempACF = acf;

            if ((tempACF.getCusps()[0].X < dbf.getPt2().X)
                && (tempACF.getCusps()[0].X > dbf.getPt1().X)
                && (tempACF.getCusps()[0].Y < dbf.getPt2().Y)
                && (tempACF.getCusps()[0].Y > dbf.getPt1().Y))
            {
          //      MessageBox.Show("Number: " + dbf.getRecogNumber() + ", airtime: " + tempACF.getAirtime());

                if (tempACF.getOrder() > dbf.getOrder())
                {
                    dbf.setOrder(tempACF.getOrder());
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
