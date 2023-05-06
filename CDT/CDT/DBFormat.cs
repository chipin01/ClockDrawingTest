using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;


namespace CDT_Week_3
{
    class DBFormat
    {
        private int recogNumber;
        private int cnt_1s = 0, cnt_2s = 0;
        private Point pt1, pt2;
        private int[] coord = new int[4];
        private String[] title = new String[5];
        private int section;
        private float airtime;
        private Point[] cusps = new Point[1];
        private Point center;
        private int order;
        private Point[] bzPts = new Point[1];
        private Point[] pkPts = new Point[1];
        private String rawStroke;
        private int rect_width;
        private int rect_height;
        private bool is_mhand = false, is_hhand = false;
        private bool is_both_hand = false;
        private bool is_merged = false;
        private bool is_combined = false; 
        private int priority = -1;
        private int end_pt_id = -1;
        private float pendown_time = 0;
        private float penup_time = 0; 
        
        public void setCoordinates(Point pt1, Point pt2)
        {
            this.pt1 = pt1;
            this.pt2 = pt2;
            coord[0] = pt1.X;
            coord[1] = pt1.Y;
            coord[2] = pt2.X - pt1.X;
            coord[3] = pt2.Y - pt1.Y;
            cnt_1s = -1;
            cnt_2s = -1;
            recogNumber = 9999;
            airtime = 0;
            center = new Point(((pt1.X + pt2.X)/2), ((pt1.Y + pt2.Y)/2));
        }

        public void setTitle()
        {
            if (recogNumber == 1)
            {
                if (cnt_1s == 3)
                {
                    title[0] = "r121x";
                    title[1] = "r121y";
                    title[2] = "r121width";
                    title[3] = "r121height";
                    title[4] = "air121";
                    priority = 13;
                }
                else if (cnt_1s == 4)
                {
                    title[0] = "r1x";
                    title[1] = "r1y";
                    title[2] = "r1width";
                    title[3] = "r1height";
                    title[4] = "air1";
                    priority = 0;
                }
                else if (cnt_1s == 0)
                {
                    title[0] = "r101x";
                    title[1] = "r101y";
                    title[2] = "r101width";
                    title[3] = "r101height";
                    title[4] = "air101";
                    priority = 9;
                }
                else if (cnt_1s == 1)
                {
                    title[0] = "r1111x";
                    title[1] = "r1111y";
                    title[2] = "r1111width";
                    title[3] = "r1111height";
                    title[4] = "air1111";
                    priority = 11;
                }
                else if (cnt_1s == 2)
                {
                    title[0] = "r1121x";
                    title[1] = "r1121y";
                    title[2] = "r1121width";
                    title[3] = "r1121height";
                    title[4] = "air1121";
                    priority = 12;
                }
            }
            else if (recogNumber == 2)
            {
                if (cnt_2s == 0)
                {
                    title[0] = "r122x";
                    title[1] = "r122y";
                    title[2] = "r122width";
                    title[3] = "r122height";
                    title[4] = "air122";
                    priority = 14;
                }
                else if (cnt_2s == 1)
                {
                    title[0] = "r2x";
                    title[1] = "r2y";
                    title[2] = "r2width";
                    title[3] = "r2height";
                    title[4] = "air2";
                    priority = 1;
                }
            }
            else if (recogNumber == 3)
            {
                title[0] = "r3x";
                title[1] = "r3y";
                title[2] = "r3width";
                title[3] = "r3height";
                title[4] = "air3";
                priority = 2;
            }
            else if (recogNumber == 4)
            {
                title[0] = "r4x";
                title[1] = "r4y";
                title[2] = "r4width";
                title[3] = "r4height";
                title[4] = "air4";
                priority = 3;
            }
            else if (recogNumber == 5)
            {
                title[0] = "r5x";
                title[1] = "r5y";
                title[2] = "r5width";
                title[3] = "r5height";
                title[4] = "air5";
                priority = 4;
            }
            else if (recogNumber == 6)
            {
                title[0] = "r6x";
                title[1] = "r6y";
                title[2] = "r6width";
                title[3] = "r6height";
                title[4] = "air6";
                priority = 5;
            }
            else if (recogNumber == 7)
            {
                title[0] = "r7x";
                title[1] = "r7y";
                title[2] = "r7width";
                title[3] = "r7height";
                title[4] = "air7";
                priority = 6;
            }
            else if (recogNumber == 8)
            {
                title[0] = "r8x";
                title[1] = "r8y";
                title[2] = "r8width";
                title[3] = "r8height";
                title[4] = "air8";
                priority = 7;
            }
            else if (recogNumber == 9)
            {
                title[0] = "r9x";
                title[1] = "r9y";
                title[2] = "r9width";
                title[3] = "r9height";
                title[4] = "air9";
                priority = 8;
            }
            else if (recogNumber == 0)
            {
                title[0] = "r0x";
                title[1] = "r0y";
                title[2] = "r0width";
                title[3] = "r0height";
                title[4] = "air0";
                priority = 10;
            }
        }

        public void setCnt_1s(int cnt_1s)
        {
            this.cnt_1s = cnt_1s;
            setTitle();
        }

        public void setCnt_2s(int cnt_2s)
        {
            this.cnt_2s = cnt_2s;
            setTitle();
        }

        public void setRecogNumber(int recogNumber)
        {
            this.recogNumber = recogNumber;
            setTitle();
        }

        public void setSection()
        {
            if ((recogNumber >= 3) && (recogNumber <=5))
            {
                section = 1;
            }
            else if (((recogNumber >= 6) && (recogNumber <= 9)) || (recogNumber == 0))
            {
                section = 2;
            }
            else if (recogNumber == 1)
            {
                if (cnt_1s == 4)
                {
                    section = 1;
                }
                else
                {
                    section = 3;
                }
            }
            else if (recogNumber == 2)
            {
                if (cnt_2s == 1)
                {
                    section = 1;
                }
                else
                {
                    section = 3;
                }
            }
            else
            {
                section = 0;
            }
        }

        public void setAirtime(float temp_airtime)
        {
            this.airtime += temp_airtime;
        }

        // TO DO: when the two strokes are overlapped 
        public void setPendownTime(float t_pendown)
        {
            this.pendown_time = t_pendown;
        }

        // TO DO: when the two strokes are overlapped
        public void setPenupTime(float t_penup)
        {
            this.penup_time = t_penup;
        }

        public void setCusps(Point[] t_cusps)
        {
            int num_cusps = cusps.Length + t_cusps.Length;
            int new_st_index = 0, second_cnt = 0, offset = 0;

            Point[] prev_cusps = new Point[cusps.Length];

            prev_cusps = cusps;

            cusps = new Point[num_cusps];

            for (int i = 0; i < prev_cusps.Length; i++)
            {
                cusps[i - offset] = prev_cusps[i];
                new_st_index = i - offset;
            }

            new_st_index++;

            for (int i = new_st_index; i < new_st_index + t_cusps.Length; i++)
            {
                cusps[i - offset] = t_cusps[second_cnt++];
            }
        }

        public void setOrder(int order)
        {
            this.order = order;
        }

        // Assign Bezier points of ACFormat variables which reside inside of the rectangle area of 
        // a particular DBFormat variable into that DBFormat variable.
        public void setBzPts(Point[] t_bzpts)
        {
            int num_bzpts = bzPts.Length + t_bzpts.Length;
            int new_st_index = 0, second_cnt = 0, offset = 0;

            Point[] prev_bzpts = new Point[bzPts.Length];

            prev_bzpts = bzPts;

            bzPts = new Point[num_bzpts];

            for (int i = 0; i < prev_bzpts.Length; i++)
            {
                bzPts[i - offset] = prev_bzpts[i];
                new_st_index = i - offset;
            }

            new_st_index++;

            for (int i = new_st_index; i < new_st_index + t_bzpts.Length; i++)
            {
                bzPts[i - offset] = t_bzpts[second_cnt++];
            }
        }

        // Assign Packet points of ACFormat variables which reside inside of the rectangle area of 
        // a particular DBFormat variable into that DBFormat variable.
        public void setPkPts(Point[] t_pkpts)
        {
            int num_pkpts = pkPts.Length + t_pkpts.Length;
            int new_st_index = 0, second_cnt = 0, offset = 0;

            Point[] prev_pkpts = new Point[pkPts.Length];

            prev_pkpts = pkPts;

            pkPts = new Point[num_pkpts];

            for (int i = 0; i < prev_pkpts.Length; i++)
            {
                pkPts[i - offset] = prev_pkpts[i];
                new_st_index = i - offset;
            }

            new_st_index++;

            for (int i = new_st_index; i < new_st_index + t_pkpts.Length; i++)
            {
                pkPts[i - offset] = t_pkpts[second_cnt++];
            }
        }

        public void setRawStroke(String rawStroke)
        {
            this.rawStroke = rawStroke;
        }

        public void setWidth(int rect_width)
        {
            this.rect_width = rect_width;
        }

        public void setHeight(int rect_height)
        {
            this.rect_height = rect_height;
        }

        public void setIsMhand(bool is_mhand)
        {
            this.is_mhand = is_mhand;
        }

        public void setIsHhand(bool is_hhand)
        {
            this.is_hhand = is_hhand;
        }

        public void setIsBothHand(bool is_both_hand)
        {
            this.is_both_hand = is_both_hand;
        }

        public void setIsMerged(bool is_merged)
        {
            this.is_merged = is_merged;
        }

        public void setIsCombined(bool is_Combined)
        {
            this.is_combined = is_Combined;
        }

        public void setEndPtID(int id)
        {
            this.end_pt_id = id;
        }

        public Point getPt1()
        {
            return pt1;
        }

        public Point getPt2()
        {
            return pt2;
        }

        public int getRecogNumber()
        {
            return recogNumber;
        }

        public int getSection()
        {
            return section;
        }

        public int[] getCoord()
        {
            return coord;
        }

        public String[] getTitle()
        {
            return title;
        }

        public int getCnt1()
        {
            return cnt_1s;
        }

        public int getCnt2()
        {
            return cnt_2s;
        }

        public bool dbFormatScreening(String strokes)
        {
            if (strokes.Equals("0") || strokes.Equals("1") || strokes.Equals("2") || strokes.Equals("3") ||
                strokes.Equals("4") || strokes.Equals("5") || strokes.Equals("6") || strokes.Equals("7") ||
                strokes.Equals("8") || strokes.Equals("9"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public float getAirtime()
        {
            return airtime;
        }

        public float getPendowntime()
        {
            return pendown_time;
        }

        public float getPenuptime()
        {
            return penup_time;
        }

        public Point getCenter()
        {
            return center;
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

        public String getRawStroke()
        {
            return rawStroke;
        }

        public int getWidth()
        {
            return rect_width;
        }

        public int getHeight()
        {
            return rect_height;
        }

        public bool getIsMhand()
        {
            return is_mhand;
        }

        public bool getIsHhand()
        {
            return is_hhand;
        }

        public bool getIsBothHand()
        {
            return is_both_hand;
        }

        public bool getIsMerged()
        {
            return is_merged;
        }

        public bool getIsCombined()
        {
            return is_combined;
        }

        public int getPriority()
        {
            return priority;
        }

        public int getEndPtID()
        {
            return end_pt_id;
        }
    }
}
