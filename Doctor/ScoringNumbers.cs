using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Media.Media3D;

// TODO: Correct quadrant check

namespace ClockReader
{
    // The class for grading process and storing each score
    class ScoringNumbers
    {
        private bool[] scoreboard = new bool[13];   // Storage that every score based on each scoring criterion will be saved
        private const int CENTER_CIRCLE_X = 397;    // The X coordinate of the center of pre-drawn circle
        private const int CENTER_CIRCLE_Y = 262;    // The Y coordinate of the center of pre-drawn circle
        private const int TOP_CIRCLE = 16;          // The top Y coordinates of the pre-drawn circle
        private const int HALF_RADIUS = CENTER_CIRCLE_Y - TOP_CIRCLE;
        private const int RIGHT_MOST = 0, BOTTOM = 1, LEFT_MOST = 2, TOP = 3;
        
        private int left_dist = 0, right_dist = 0;
        private Point both_bottom_pt, m_bottom_pt, h_bottom_pt, m_right, h_left;
        private const double HandIndicationAngel = 15;

        //private DBFormat[] tempDBFormat;
        //private EndPoint[] endPoint = new EndPoint[4];
        private List<PenStroke> penstrokes;
        private PenStroke stroke; 

        private int numberThreshold = 20;       // The angle threshold between which numbers must be situated in order to be considered in the correct position
        private int minute = 50;                // default time is 11;50
        private int hour = 11;                  // default time is 11:50
        private int timeThreshold = 20;         //default time threshold is =- 20 degrees
        private int generalSensitivity = 0;     //switch that changes all thresholds in the arbitrary incrememnt of 5
        //cases low = 0, medium = 1, and high = 0 correspond to 30, 25, 20
        //doc can also set each value individually through the graphical interface which will send more exact values
        
        //arrays to hold the locations of all the minute and hour values on the face of the clock 
        private Point[] minCoords = new Point[60]; 
        private Point[] hourCoords = new Point[12];
        private Point jointPoint;               //point of the joint of hands on clck 
        //bring in the circle bitmap in order to draw things on it
        private Bitmap circleBitmap; 

        private bool numPosGreaterThanMin = false; 

        //list of tuples of all the recoRects and their bounding box centers for score3
        private List <Tuple<int, Point>> recoStrokeList = new List<Tuple<int, Point>>();  

        //Clock object to keep the data in 
        private Clock myClock;
        private Circle clock;
        private int[] intRecoStrokes;
        Point clockCenter;  



        public ScoringNumbers(Clock oClock, Circle clock)
        {
            for (int i = 0; i < scoreboard.Length; i++)
                scoreboard[i] = true;
            this.penstrokes = oClock.PenStrokes;
            this.clock = clock;
            this.myClock = oClock;
            clockCenter = new Point(myClock.X + myClock.R, myClock.Y + myClock.R);
        }

        public int[] scoringCPH()
        {
            //pre-processing to enable the correct calculations later
            calcHourCoords();
            calcMinuteCoords();
            calculateActualNumbers();
            fillRecoStrokeList();
            

            PenStroke longhand, shorthand;
            scoreboard[0] = newScore1();
            scoreboard[1] = newScore2();
            scoreboard[2] = newScore3(); 
            scoreboard[4] = newScore5();
            scoreboard[5] = newScore6();
            scoreboard[6] = newScore7(out longhand, out shorthand);
            if (scoreboard[6])
            {
                scoreboard[7] = newScore8(shorthand); //<= cause null reference problem
                scoreboard[8] = newScore9(longhand);
                scoreboard[9] = newScore10(shorthand, longhand);
                scoreboard[11] = newScore12(shorthand, longhand); 
            }
            else
            {
                scoreboard[7] = false;
                scoreboard[8] = false;
                scoreboard[9] = false;
                scoreboard[11] = false; 
            }
            if (scoreboard[11])
            {
                scoreboard[12] = newScore13(shorthand, longhand);
            }
            else
                scoreboard[12] = false;

            
            this.myClock.ScoreBoard = convertToIntScoreboard(scoreboard);
            return convertToIntScoreboard(scoreboard);
        }

        public int[] convertToIntScoreboard(bool[] board)
        {
            int[] intScoreBoard = new int[13];
            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] == true)
                    intScoreBoard[i] = 1;
                else intScoreBoard[i] = 0;

            }
            return intScoreBoard; 
        }

        /// <summary>
        /// Scoring Criteria No. 1 (Only numbers 1-12 are present)
        /// </summary>
        /// <returns></returns>
        private bool newScore1()
        {
            int cnt1 = 0, cnt2 = 0, cnt3 = 0, cnt4 = 0, cnt5 = 0, cnt6 = 0, cnt7 = 0, cnt8 = 0, cnt9 = 0, cnt10 = 0,
                cnt11 = 0, cnt12 = 0; 
            int index = 0; 
            // Counting the number of each number
            foreach (PenStroke penStroke in penstrokes)
            {
                try
                {
                    if (penStroke.ActualNumber == 1)
                    {
                            cnt1++;
                    }
                    else if (penStroke.ActualNumber == 2)
                    {
                        cnt2++;
                    }
                    else if (penStroke.ActualNumber == 3)
                    {
                        cnt3++;
                    }
                    else if (penStroke.ActualNumber == 4 &&
                        penStroke.MergeTo != index - 1)
                    //if mergeTo == penStroke.Count -1 don't add to the count
                    //4 is made of 2 strokes that both get counted as 4
                    //have to check if it is the first or second stroke. Second stroke will have a mergeTo of the count of the first stroke
                    {
                        int value = penStroke.MergeTo;
                        cnt4++;
                    }
                    else if (penStroke.ActualNumber == 5 &&
                        penStroke.MergeTo != index - 1)
                    {
                        int value = penStroke.MergeTo;
                        //getting counted twice due to the double stroke
                        cnt5++;
                    }
                    else if (penStroke.ActualNumber == 6)
                    {
                        cnt6++;
                    }
                    else if (penStroke.ActualNumber == 7)
                    {
                        cnt7++;
                    }
                    else if (penStroke.ActualNumber == 8)
                    {
                        cnt8++;
                    }
                    else if (penStroke.ActualNumber == 9)
                    {
                        cnt9++;
                    }
                    else if (penStroke.ActualNumber == 10 && penStroke.OrderID != 1)
                    {
                        cnt10++;
                    }
                    else if (penStroke.ActualNumber == 11 && penStroke.OrderID != 1)
                    {
                        cnt11++;
                    }
                    else if (penStroke.ActualNumber == 12 && penStroke.OrderID != 1)
                    {
                        cnt12++;
                    }
                    else if (penStroke.RecoStrokes != "H1" && penStroke.RecoStrokes != "H2"
                        && penStroke.RecoStrokes != "?" && penStroke.RecoStrokes != "H1")
                    {
                        if (int.Parse(penStroke.RecoStrokes) > 12)
                        {
                            return false;
                            break; 
                        }
                    }
                    index++;
                }
                catch (FormatException e)
                {
                    Console.WriteLine("The number is not a sequence of digits.");
                }
                
            }

            // If the numbers of every individual number are matched to pre-defined criteria return true, return false otherwise
            if ((cnt1 == 1) && (cnt2 >= 1) && (cnt3 == 1) && (cnt4 == 1) && (cnt5 == 1)
                && (cnt6 == 1) && (cnt7 == 1) && (cnt8 == 1) && (cnt9 == 1) && (cnt10 == 1) && (cnt11 == 1) && (cnt12 == 1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Only Arabic Number are used
        /// </summary>
        /// <returns></returns>
        private bool newScore2()
        {
            // Searching all strokes
            foreach (PenStroke stroke in penstrokes)
            {
                // If any stroke is not a number or an hour or minute hand, return false, return true otherwise.
                if (stroke.RecoStrokes == "?")
                {
                    return false;
                }
                if (stroke.RecoStrokes == "H1" || stroke.RecoStrokes == "H2"|| stroke.RecoStrokes == "")
                {
                    continue;
                }
                else if (int.Parse(stroke.RecoStrokes) < 0 && stroke.isHand < 0)
                {
                    if (stroke.MergeTo < penstrokes.Count && penstrokes[stroke.MergeTo].isHand < 0)
                        return false;
                }
            }
            return true;
        }        
        
        // Scoring Criteria No 3. (Numbers are in correct order)
        private bool newScore3()
        {
            bool in_order = false;

            for (int i = 0; i < recoStrokeList.Count; i++)
            {
                Point myPoint = recoStrokeList[i].Item2; 
                //have to watch out for array out of bounds
                if (i <= recoStrokeList.Count - 2 && 
                    recoStrokeList.Count > 2)
                {
                 if (calcDistance(recoStrokeList[i].Item2, recoStrokeList[i+2].Item2) > calcDistance(recoStrokeList[i].Item2, recoStrokeList[i+1].Item2) &&
                    (calcDistance(recoStrokeList[i].Item2, recoStrokeList[i+2].Item2) > calcDistance(recoStrokeList[i+1].Item2, recoStrokeList[i + 2].Item2)))
                 {
                     in_order = true; 
                 }
                 else
                 {
                     in_order = false; 
                     break; 
                 }
                }
            }
            return in_order;
        }

        //Scoring Criteria No. 5 (Numbers are in the correct position)
        private bool newScore5()
        {
            bool zone_1 = false, zone_2 = false, zone_3 = false, zone_4 = false, zone_5 = false, zone_6 = false, zone_7 = false, 
                zone_8 = false, zone_9 = false, zone_10 = false, zone_11 = false, zone_12 = false; 
            System.Windows.Vector inputVector;
            System.Windows.Vector comparisonVector;
            bool is_withinThreshold = false;
            bool is_withinCircle = false;


            foreach (PenStroke stroke in penstrokes)
            {
                inputVector = new System.Windows.Vector(stroke.CenterPoint.X - clockCenter.X, clockCenter.Y - stroke.CenterPoint.Y);
                //if any unknown strokes come in, just continue on
                //Decision point: do unknown strokes cause the score to be flase automatically? 
                //No for now.
                if (stroke.RecoStrokes == "H1" || stroke.RecoStrokes == "H2"|| stroke.RecoStrokes == "?")
                {
                    continue;
                }
                if (stroke.RecoStrokes == "1")
                {
                    if (doubleDigit(stroke) == 12)
                    {
                        comparisonVector = new System.Windows.Vector(0, ((clockCenter.X + myClock.R) - clockCenter.X));
                        if ((getAngleBetween(inputVector, comparisonVector)) > numberThreshold
                        || (getAngleBetween(inputVector, comparisonVector)) < -numberThreshold)
                            zone_12 = false;
                        else
                            zone_12 = true;
                    }
                    else if (doubleDigit(stroke) == 10)
                    {
                        comparisonVector = new System.Windows.Vector(-((Math.Sqrt(3) * myClock.R) / 2), myClock.R / 2);
                        if ((getAngleBetween(inputVector, comparisonVector)) > numberThreshold
                            || (getAngleBetween(inputVector, comparisonVector)) < -numberThreshold)
                            zone_10 = false;
                        else
                            zone_10 = true;
                    }
                    else if (doubleDigit(stroke) == 11)
                    {
                        comparisonVector = new System.Windows.Vector(-myClock.R / 2, ((Math.Sqrt(3) * myClock.R) / 2));
                        if ((getAngleBetween(inputVector, comparisonVector)) > numberThreshold
                        || (getAngleBetween(inputVector, comparisonVector)) < -numberThreshold)
                            zone_11 = false;
                        else
                            zone_11 = true;
                    }
                    else
                    {
                        comparisonVector = new System.Windows.Vector(myClock.R / 2, (Math.Sqrt(3) * myClock.R) / 2);
                        if ((getAngleBetween(inputVector, comparisonVector)) > numberThreshold
                            || (getAngleBetween(inputVector, comparisonVector)) < -numberThreshold)
                            zone_1 = false;
                        else
                            zone_1 = true;
                    }
                }
                else if (stroke.RecoStrokes == "2" && 
                         stroke.OrderID != 1)
                {
                    comparisonVector = new System.Windows.Vector(((Math.Sqrt(3) * myClock.R) / 2), myClock.R / 2);
                    if ((getAngleBetween(inputVector, comparisonVector)) > numberThreshold
                        || (getAngleBetween(inputVector, comparisonVector)) < -numberThreshold)
                        zone_2 = false;
                    else
                        zone_2 = true;
                }

                else if (stroke.RecoStrokes == "3")
                {
                    comparisonVector = new System.Windows.Vector(myClock.R, 0);
                    if ((getAngleBetween(inputVector, comparisonVector)) > numberThreshold
                        || (getAngleBetween(inputVector, comparisonVector)) < -numberThreshold)
                        zone_3 = false;
                    else
                        zone_3 = true;
                }
                else if (stroke.RecoStrokes == "4")
                {
                    comparisonVector = new System.Windows.Vector(((Math.Sqrt(3) * myClock.R) / 2), -myClock.R / 2);
                    if ((getAngleBetween(inputVector, comparisonVector)) > numberThreshold
                        || (getAngleBetween(inputVector, comparisonVector)) < -numberThreshold)
                        zone_4 = false;
                    else
                        zone_4 = true;
                }
                else if (stroke.RecoStrokes == "5")
                {
                    comparisonVector = new System.Windows.Vector(myClock.R / 2, -((Math.Sqrt(3) * myClock.R) / 2));
                    if ((getAngleBetween(inputVector, comparisonVector)) > numberThreshold
                        || (getAngleBetween(inputVector, comparisonVector)) < -numberThreshold)
                        zone_5 = false;
                    else
                        zone_5 = true;
                }

                else if (stroke.RecoStrokes == "6")
                {
                    comparisonVector = new System.Windows.Vector(0, -myClock.R);
                    if ((getAngleBetween(inputVector, comparisonVector)) > numberThreshold
                        || (getAngleBetween(inputVector, comparisonVector)) < -numberThreshold)
                        zone_6 = false;
                    else
                        zone_6 = true;
                }
                else if (stroke.RecoStrokes == "7")
                {
                    comparisonVector = new System.Windows.Vector(-myClock.R / 2, -((Math.Sqrt(3) * myClock.R) / 2));
                    if ((getAngleBetween(inputVector, comparisonVector)) > numberThreshold
                        || (getAngleBetween(inputVector, comparisonVector)) < -numberThreshold)
                        zone_7 = false;
                    else
                        zone_7 = true;
                }
                else if (stroke.RecoStrokes == "8")
                {
                    comparisonVector = new System.Windows.Vector(-((Math.Sqrt(3) * myClock.R) / 2), -myClock.R / 2);
                    if ((getAngleBetween(inputVector, comparisonVector)) > numberThreshold
                        || (getAngleBetween(inputVector, comparisonVector)) < -numberThreshold)
                        zone_8 = false;
                    else
                        zone_8 = true;
                }
                else if (stroke.RecoStrokes == "9")
                {
                    comparisonVector = new System.Windows.Vector(-myClock.R, 0);
                    if ((getAngleBetween(inputVector, comparisonVector)) > numberThreshold
                       || (getAngleBetween(inputVector, comparisonVector)) < -numberThreshold)
                        zone_9 = false;
                    else
                        zone_9 = true;
                }
            }
            if (zone_1 && zone_2 && zone_3 && zone_4 && zone_5 && zone_6 && zone_7 && zone_8 && zone_9 && zone_10 && zone_11 && zone_12)
            {
                is_withinThreshold = true;
            }
            else
            {
                is_withinThreshold = false;
            }
            if (testWithinCircle())
            {
                is_withinCircle = true;
            }
            if (is_withinThreshold && is_withinCircle)//numbers are in their approximate position and not so close to the center
                return true;
            else
                return false; 

        }


        /// <summary>
        /// Scoring criteria No. 6: Numbers are all inside circle
        /// </summary>
        /// <returns></returns>
        private bool newScore6()
        {
            if (testWithinCircle())
                return true;
            else
                return false;
        }
        /// <summary>
        ///  Scoring Criteria No. 7: Two hands are present
        /// </summary>
        /// <returns></returns>
        /// 

        private bool newScore7(out PenStroke longHand, out PenStroke shortHand)
        {                        
            shortHand = null;
            longHand = null;
            // Searching entire DB format area
            foreach (PenStroke stroke in penstrokes)
            {
                // Finding the stroke with the longest width
                if (stroke.isHand == 1)
                    longHand = stroke;
                if (stroke.isHand == 2)
                    shortHand = stroke;
            }

            if (shortHand == null || longHand == null) return false;            
            
            return true;
        }
        /// <summary>
        /// Scoring Criteria 8, the hour target number is indicated
        /// </summary>
        /// <returns></returns>
        private bool newScore8(PenStroke shortHand)
        {
            PenStroke currentHourStroke = null; 
            foreach (PenStroke stroke in penstrokes)
            {
                if (stroke.ActualNumber == hour)
                {
                    currentHourStroke = stroke; 
                    break; 
                }
            }

             //find the beginning ponit and end point of the short hand
            Point firstPt = shortHand.StartPoint; 
            Point lastPt = shortHand.EndPoint; 
     
            double dist1 = calcDistance(firstPt, clockCenter);
            double dist2 = calcDistance(lastPt, clockCenter);

            Vector3D vectorHand = new Vector3D(lastPt.X - firstPt.X, lastPt.Y - firstPt.Y, 0);
            if (dist1 > dist2)
                vectorHand = -vectorHand;
            
            
            //here is the vector for calculating the time using the absolute positions of the hours
            //Vector3D vectorHour = new Vector3D(hourCoords[hour - 1].X - clockCenter.X, hourCoords[hour - 1].Y - clockCenter.Y, 0);
            //this is the vector calculated for the relative position of the drawn hour
            Vector3D vectorHour = new Vector3D(0,0,0);
            if (currentHourStroke != null)
            {
                try
                {
                    vectorHour = new Vector3D(currentHourStroke.CenterPoint.X - clockCenter.X, currentHourStroke.CenterPoint.Y - clockCenter.Y, 0);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Current Hour has a null reference", e);
                    //cannot calculate if the hour is correct because the number for the hour is not present, return false; 
                    return false;
                }
            }
            //vector if its supposed to be calculated based on relative numbers
            //have to find what bucket or region the number should be in relative to being between two numbers, then calculate the general area based on that
            double angleBetween = Vector3D.AngleBetween(vectorHand, vectorHour);
            if (Vector3D.AngleBetween(vectorHand, vectorHour) < timeThreshold && Vector3D.AngleBetween(vectorHand, vectorHour) > -timeThreshold)
            {
                return true;
            }
            else
                return false; 
        }        

        /// <summary>
        ///  Scoring criteria 9, the minute target number is indicated
        /// </summary>
        /// <param name="shortHand"></param>
        /// <returns></returns>
        private bool newScore9(PenStroke longHand){

            //the problem is that this calculation assumes we are started at an angle of -90
            //all the calculation we do is from an assumption of -90 to 90
            //which is the reason he converts back and forth from radians using 180 instaed of 90
            double rad = Math.PI / 180;
            //find exact number that the minute represents in hours; 
            double exactHour = ((double)(minute))/60 * 12; 
            int roundedHour = (int)Math.Floor(exactHour); 
            double hourPercent = exactHour - (double)roundedHour; 

            //calculate angles of roundedHour and roundedHour + 1
            //'deleted an int before myClock.R
            Point p0 = new Point (clockCenter.X, clockCenter.Y - myClock.R);
            Point p1 = new Point(); 
            Point p2 = new Point(); 

            //find proper strokes in penstrokes for roundedhours
            foreach(PenStroke stroke in penstrokes)
            {
                if (stroke.ActualNumber == roundedHour)
                {
                    p1.X = stroke.CenterPoint.X;
                    p1.Y = stroke.CenterPoint.Y;
                }
                if (stroke.ActualNumber == roundedHour + 1)
                {
                    p2.X = stroke.CenterPoint.X;
                    p2.Y = stroke.CenterPoint.Y;
                }
            }
            if (p1 != null && p2 != null)
            {
                double angle1 = (2 * Math.Atan2(p1.Y - p0.Y, p1.X - p0.X)) * 180 / Math.PI;
                double angle2 = (2 * Math.Atan2(p2.Y - p0.Y, p2.X - p0.X)) * 180 / Math.PI;
                double percentAngle = (angle2 - angle1) * hourPercent;

                //stuff to calculate newCoord with percent angle and startAngle
                double endangle = angle1 + percentAngle;
                double x = clockCenter.X + myClock.R * Math.Cos((endangle - 90) * rad);
                double y = clockCenter.Y + myClock.R * Math.Sin((endangle - 90) * rad);
                Point exactMinutePoint = new Point((int)x, (int)y);

                //test to see where things go
                //Graphics g = Graphics.FromImage(circleBitmap);
                //Pen pen = new System.Drawing.Pen(Color.Yellow, 5);
                //find the beginning ponit and end point of the short hand
                Point firstPt = longHand.StartPoint;
                Point lastPt = longHand.EndPoint;

                double dist1 = calcDistance(firstPt, clockCenter);
                double dist2 = calcDistance(lastPt, clockCenter);

                Vector3D vectorHand = new Vector3D(lastPt.X - firstPt.X, lastPt.Y - firstPt.Y, 0);
                if (dist1 > dist2)
                    vectorHand = -vectorHand;

                //vector for absolute position of minute coords
                //Vector3D vectorMin = new Vector3D(minCoords[minute - 1].X - clockCenter.X, minCoords[minute - 1].Y - clockCenter.Y, 0);
                Vector3D vectorMin = new Vector3D(0, 0, 0);
                try
                {
                    vectorMin = new Vector3D(exactMinutePoint.X - clockCenter.X, exactMinutePoint.Y - clockCenter.Y, 0);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Minute note present", e);
                    //cannot calculate whether the minute is accurate because the number indicating that minute is not present, score is false; 
                    return false;
                }
                //g.DrawEllipse(pen, minCoords[minute - 1].X, minCoords[minute - 1].Y, 2, 2);
                //vector if its supposed to be calculated based on relative numbers
                //have to find what bucket or region the number should be in relative to being between two numbers, then calculate the general area based on that
                double angleBetween = Vector3D.AngleBetween(vectorHand, vectorMin);
                if (Vector3D.AngleBetween(vectorHand, vectorMin) < timeThreshold && Vector3D.AngleBetween(vectorHand, vectorMin) > -timeThreshold)
                {
                    return true;
                }
                else
                    return false; 
            }
                return false; 
            
        } 
        

        /// <summary>
        ///  Scoring criteria 10: The hands are in correct propotion
        /// </summary>
        /// <returns></returns>
        private bool newScore10(PenStroke shorthand, PenStroke longhand)
        {
            // Finding hour hand, minute hand, and both hands if exist.
            //determine if 
            double shortLength = calcDistance(shorthand.StartPoint, shorthand.EndPoint);
            double longLength = calcDistance(longhand.StartPoint, longhand.EndPoint);
            if (longLength > shortLength)
            {
                return true;
            }
            else
                return false; 
        }

        //Scoring Criteria No. 11 (no superfluous marks)
        private bool newScore11()
        {
            foreach (PenStroke stroke in penstrokes)
            {
                //if the stroke is null or unknown assume that it is superfluos, user can change strokes into numbers if they would like
                if (stroke.RecoStrokes == "" || stroke.RecoStrokes == "?")
                    return false;
            }
            return true;
        }

        //
        //hands are relatively joined (within 12mm)
        // Scoring criteria 12
        private bool newScore12(PenStroke shortHand, PenStroke longHand)
        {
            //find out which of the points are the actual center points by finding which are the closest
            //first find the min between start to start and end to end
            //then find min between min.startPoint to hour.EndPoint etc. 
            
            double startsToStarts = Math.Min(calcDistance(shortHand.StartPoint, longHand.StartPoint), calcDistance(shortHand.EndPoint, longHand.EndPoint));
            double startToEnd = Math.Min(calcDistance(shortHand.StartPoint, longHand.EndPoint), calcDistance(shortHand.EndPoint, longHand.StartPoint)); 
            //check if what is considered the joint (closest region together) is less than 10mm (37pixels)
            if (Math.Min(startsToStarts, startToEnd) < 37)
            {
                return true;
            }
            else
                return false; 
        }

        //A center is present at the joining of the hands. The joint is near the center
        //has to be a threshold of the center. Can only happen if Number12()
        // Scoring criteria No. 13
        private bool newScore13(PenStroke shortHand, PenStroke longHand)
        {
            Point centerPoint = new Point(0,0); 
            double startToStarts = calcDistance(shortHand.StartPoint, longHand.StartPoint); 
            double endToEnds = calcDistance(shortHand.EndPoint, longHand.EndPoint); 
            double startShortToLongEnd = calcDistance(shortHand.StartPoint, longHand.EndPoint); 
            double endShortToLongStart = calcDistance(shortHand.EndPoint, longHand.StartPoint); 
            double[] distances = {startToStarts, endToEnds, startShortToLongEnd, endShortToLongStart}; 
            double minimum = startToStarts; 
            for (int i = 0; i < distances.Length; i++)
            {
                if (distances[i] < minimum)
                    minimum = distances[i]; 
            }
            if (minimum == startToStarts)
            {
                centerPoint = calcMidpoint(shortHand.StartPoint, longHand.StartPoint); 
            }
            else if (minimum == endToEnds)
            {
                centerPoint = calcMidpoint(shortHand.EndPoint, longHand.EndPoint);
            }
            else if (minimum == startShortToLongEnd) 
            {
                centerPoint = calcMidpoint(shortHand.StartPoint, longHand.EndPoint);
            }
            else if(minimum == endShortToLongStart)
            {
                centerPoint = calcMidpoint(shortHand.EndPoint, longHand.StartPoint);
            }
            //check if the centerPoint of the joint is less than 10mm to the center of circle
            //***Have the change the clock being used. Have a clock object at my disposal, but not a circle anymore
             
            if (calcDistance(centerPoint, clockCenter) < 37)
                return true;
            else
                return false; 
        }

        public bool testWithinCircle()
        {
            //creates a GraphicsPath going all the way around the clock, then makes a region from that path, then tests if all bounding boxes are within region
            //set size of clockRect
            Size clockRectSize = new Size();
            clockRectSize.Width = (int)(2 * myClock.R);
            clockRectSize.Height = (int)(2 * myClock.R);
            //set upper left point of rectangle for clockRect Region
            //This is the problem. The clock is not the proper size. It has to be half the size of the inkPicture and height
            //use a circle

            Point clockRectPoint = new Point((int)(myClock.X), (int)(myClock.Y));
            //define a rectangle region that contains the circle in order to draw a GraphicsPath for circle
            Rectangle clockBounds = new Rectangle(clockRectPoint, clockRectSize);
            System.Drawing.Drawing2D.GraphicsPath circularPath = new System.Drawing.Drawing2D.GraphicsPath();
            circularPath.StartFigure();
            circularPath.AddArc(clockBounds, 0, 360);
            circularPath.CloseFigure();
            //create a region consisting of the circularPath GraphicsPath 
            System.Drawing.Region circleRegion = new System.Drawing.Region(circularPath);
            //can now test whether the rectangle of each number IsVisible(Rectangle)
            bool allWithin = false;
            foreach (PenStroke stroke in penstrokes)
            {
                //test if each of the four corners of the bounding box are visible in the circle region
                //define the four points of the stroke bounding box 
                Point topLeft = stroke.BoundingBox.Location;
                Point botLeft = new Point(topLeft.X, topLeft.Y + stroke.BoundingBox.Height);
                Point topRight = new Point(topLeft.X + stroke.BoundingBox.Width, topLeft.Y);
                Point botRight = new Point(topLeft.X + stroke.BoundingBox.Width, topLeft.Y + stroke.BoundingBox.Height);
                //test if all points are within the circleRegion
                if (circleRegion.IsVisible(topLeft) &&
                    circleRegion.IsVisible(botLeft) &&
                    circleRegion.IsVisible(topRight) &&
                    circleRegion.IsVisible(botRight))
                    allWithin = true;
                else
                {
                    allWithin = false;
                    break;
                }
            }
            if (allWithin)
            {
                return true;
            }
            else
                return false;
        }
        //set whether the number positions on the clock are greater than a specified threshold set in the newScore6
        private void setNumPosGreaterThanMin(bool isGreater)
        {
            numPosGreaterThanMin = isGreater;
        }
        private bool getNumPosGreaterThanMin()
        {
            return numPosGreaterThanMin; //return true if all numbers are greater than a minimun radius; 
        }
        //get the angle between two vectors
        public double getAngleBetween(System.Windows.Vector a, System.Windows.Vector b)
        {
            
            double angle = System.Windows.Vector.AngleBetween(a, b);
            return angle;
        }


        private void calcMinuteCoords()
        {
            double startangle = -90; 
            double angle = 6; 
            double rad = Math.PI/180; 
            for (int i = 1; i < 61; i++)
            {
                double endangle = startangle + angle; 
                double x = clockCenter.X + myClock.R * Math.Cos(endangle * rad); 
                double y = clockCenter.Y + myClock.R * Math.Sin(endangle * rad); 

                Point myPoint = new Point((int)x,(int)y); 
                minCoords[i - 1] = myPoint; 
                startangle = endangle; 
            }
        }
        private void calcHourCoords()
        {
            double startangle = -90; 
            double angle = 30; 
            double rad = Math.PI/180; 
            for (int i = 1; i < 13; i++)
            {
                double endangle = startangle + angle; 
                double x = clockCenter.X + myClock.R * Math.Cos(endangle * rad); 
                double y = clockCenter.Y + myClock.R * Math.Sin(endangle * rad); 

                Point myPoint = new Point((int)x,(int)y); 
                hourCoords[i - 1] = myPoint; 
                startangle = endangle; 
            }
        }
        

        private double calcDistance(Point pt1, Point pt2)
        {
            return Math.Sqrt(Math.Pow(pt1.X - pt2.X, 2) + Math.Pow(pt1.Y - pt2.Y, 2));
        }
        private Point calcMidpoint(Point point1, Point point2)
        {
            Point midPoint = new Point((point1.X + point2.X) / 2, (point1.Y + point2.Y) / 2);
            return midPoint;
        }

        private int doubleDigit(PenStroke stroke)
        {
            if(stroke.CombineTo == 0)
                    {
                        return 10; 
                    }
                    else if(stroke.CombineTo == 1) 
                    {
                        return 11;
                    }
                    else if(stroke.CombineTo == 2)
                    {
                        return 12;
                    }
                    
            else return 1;
        }
      // The angle threshold between which numbers must be situated in order to be considered in the correct position


//providing public access to all the settings that may be changes based on user preference
        private void calculateActualNumbers()
        {
            //if 1 or 2 then go through double procedure
            foreach (PenStroke stroke in penstrokes)
            {
                //test to see if mergeTo works
                int merge = stroke.MergeTo;
                if (stroke.RecoStrokes == "1")
                {
                    if (stroke.CombineTo == 0)
                    {
                        stroke.ActualNumber = 10;
                    }
                    else if (stroke.CombineTo == 1)
                    {
                        stroke.ActualNumber = 11;
                    }
                    else if (stroke.CombineTo == 2)
                    {
                        stroke.ActualNumber = 12;
                    }
                    else stroke.ActualNumber = 1;
                }
                else if (stroke.RecoStrokes == "2")
                {
                    if (stroke.CombineTo != 1 && stroke.OrderID != 1)
                    {
                        stroke.ActualNumber = 2;
                    }
                    else if (stroke.CombineTo == 1)
                    {
                        stroke.ActualNumber = 12;
                    }
                }
                else if (stroke.RecoStrokes == "H1" || stroke.RecoStrokes == "H2" || stroke.RecoStrokes == "?" || stroke.RecoStrokes == "")
                    {
                        continue;
                    }
                else stroke.ActualNumber = int.Parse(stroke.RecoStrokes);
                }
            }    
        
        private void fillRecoStrokeList()
        {
            foreach (PenStroke stroke in penstrokes)
            {
                for (int i = 0; i < 13; i++)
                {

                    if (stroke.ActualNumber == i)
                    {
                        //has the number been combined?
                        if (Math.Abs(stroke.CombineTo) < 50)
                        {
                            if (stroke.OrderID != 1)
                                recoStrokeList.Add(new Tuple<int, Point>(i, stroke.CenterPoint));
                            
                        }
                        else
                        {
                            //add the number because it has not been combined
                            recoStrokeList.Add(new Tuple<int, Point>(i, stroke.CenterPoint));
                        }
                       
                    }
                }
            }
        }
        
        public int NumberThreshold 
{
    get{ return numberThreshold; }
    set { numberThreshold = value; }
}
public int Minute
{
  get { return minute; }
  set { minute = value; }
}
public int Hour
{
  get { return hour; }
  set { hour = value; }
}
public int TimeThreshold
{
  get { return timeThreshold; }
  set { timeThreshold = value; }
}       
public int GeneralSensitivity
{
  get { return generalSensitivity; }
  set { generalSensitivity = value; }

}
 public bool[] getScoreboard() 
{
    return scoreboard;
}
    }
}


/*
 * ******************************
 * ******************************
 * OLD CODE
private bool newScore3()
{
    bool is_inorder = false;

    List<PenStroke> sortedStrokes = this.penstrokes;            

    //// Sort DB by its priority
    //sortedStrokes = sortDB(sortedStrokes);

    //// Comparing the location of each number
    //is_inorder = compLocation(sortedDB);

    return true;
}
       
// Scoring Criteria No 3. (Numbers are in correct order)
private bool score3()
{
    bool is_inorder = false;

    DBFormat[] sortedDB = new DBFormat[tempDBFormat.Length];
    sortedDB = tempDBFormat;

    // Sort DB by its priority
    sortedDB = sortDB(sortedDB);

    // Comparing the location of each number
    is_inorder = compLocation(sortedDB);

    return is_inorder;
}*/

// Scoring Criteria No 5 (Numbers are in the correct position 
// (fairly close to their-quadrants & with-in the pre-drawn circle)
// Find the end point for each direction.
/*
private void findEndPt()
{
    foreach (DBFormat df in tempDBFormat)
    {
        if (df.getCenter().Y < endPoint[TOP].getEndPt().Y)
        {
            endPoint[TOP].setEndPt(df.getCenter());
            endPoint[TOP].setPrior(df.getPriority());
        }

        if (df.getCenter().Y > endPoint[BOTTOM].getEndPt().Y)
        {
            endPoint[BOTTOM].setEndPt(df.getCenter());
            endPoint[BOTTOM].setPrior(df.getPriority());
        }

        if (df.getCenter().X < endPoint[LEFT_MOST].getEndPt().X)
        {
            endPoint[LEFT_MOST].setEndPt(df.getCenter());
            endPoint[LEFT_MOST].setPrior(df.getPriority());
        }

        if (df.getCenter().X > endPoint[RIGHT_MOST].getEndPt().X)
        {
            endPoint[RIGHT_MOST].setEndPt(df.getCenter());
            endPoint[RIGHT_MOST].setPrior(df.getPriority());
        }
    }
}
*/
/*
        private bool handIndication(List<PacketPoint> packPts)
        {
            if (packPts.Count > 1)
            {
                Point firstPt = packPts[0].PkPt;
                Point lastPt = packPts[packPts.Count-1].PkPt;

                double dist1 = calcDistance(firstPt, clockCenter);
                double dist2 = calcDistance(lastPt, clockCenter);

                Vector3D vectorHand = new Vector3D(lastPt.X - firstPt.X, lastPt.Y - firstPt.Y, 0);

                if (dist1 > dist2)
                    vectorHand = -vectorHand;

                foreach (PenStroke stroke in penstrokes)
                {
                    if (stroke.isHand > 0 ) continue;
                    if (stroke.MergeTo < penstrokes.Count && penstrokes[stroke.MergeTo].isHand > 0) continue;
                    Point ptNum = stroke.CenterPoint;
                    Vector3D vectorNum = new Vector3D(ptNum.X - clockCenter.X, ptNum.Y - clockCenter.Y, 0);

                    if (Vector3D.AngleBetween(vectorHand, vectorNum) < HandIndicationAngel) return true;
                }
            }

            return false;
        }
         */

// Sorting DB based on the priority of each DB element
/*
private List<PenStroke> sortDB(List<PenStroke> sortedDB)
{
    DBFormat tmpDB = new DBFormat();

    for (int i = 0; i < sortedDB.Length; i++)
    {
        int j = i;

        if (sortedDB[j].getPriority() >= 0)
        {
            while (j > 0 && sortedDB[j - 1].getPriority() > sortedDB[j].getPriority())
            {
                tmpDB = sortedDB[j];
                sortedDB[j] = sortedDB[j - 1];
                sortedDB[j - 1] = tmpDB;
                j--;
            }
        }
    }

    return sortedDB;
}*/
/*
// Sorting DB based on the priority of each DB element
private DBFormat[] sortDB(DBFormat[] sortedDB)
{
    DBFormat tmpDB = new DBFormat();

    for (int i = 0; i < sortedDB.Length; i++)
    {
        int j = i;

        if (sortedDB[j].getPriority() >= 0)
        {
            while (j > 0 && sortedDB[j - 1].getPriority() > sortedDB[j].getPriority())
            {
                tmpDB = sortedDB[j];
                sortedDB[j] = sortedDB[j - 1];
                sortedDB[j - 1] = tmpDB;
                j--;
            }
        }
    }

    return sortedDB;
}

// Compare the order of numbers by their locations.
private bool compLocation(DBFormat[] sortedDB)
{
    int s_index = 0, e_index = 1;

    // Find the end point for each direction.
    findEndPt();
            
    // Checking the quadarant No 4.
    for (int i = endPoint[s_index].getPrior(); i < endPoint[e_index].getPrior() - 1; i++)
    {
        if (i >= 0) 
        {
            MessageBox.Show(i.ToString());
            // If the next number locates more right or upper than previous number then false, 
            // otherwise true.
            if ((tempDBFormat[i + 1].getCenter().X >= tempDBFormat[i].getCenter().X)
                || (tempDBFormat[i + 1].getCenter().Y <= tempDBFormat[i].getCenter().Y))
            {
                return false;
            }
        }
    }

    s_index++;
    e_index++;

    // Checking the quadarant No. 3
    for (int i = endPoint[s_index].getPrior(); i < endPoint[e_index].getPrior() - 1; i++)
    {
        // If the next number locates more right or lower than previous number then false, 
        // otherwise true.
        if ((tempDBFormat[i + 1].getCenter().X >= tempDBFormat[i].getCenter().X)
            || (tempDBFormat[i + 1].getCenter().Y >= tempDBFormat[i].getCenter().Y))
        {
            return false;
        }
    }

    s_index++;
    e_index++;

    // Checking one quadarant
    for (int i = endPoint[s_index].getPrior(); i < endPoint[e_index].getPrior() - 1; i++)
    {
        if (i < tempDBFormat.Length - 1) 
        {
            // TODO: this will cause index out of range problem
            // If the next number locates more right or lower than previous number 
            // then potentially false, otherwise true.
            if ((tempDBFormat[i + 1].getCenter().X <= tempDBFormat[i].getCenter().X)
                || (tempDBFormat[i + 1].getCenter().Y >= tempDBFormat[i].getCenter().Y))
            {
                // Check the location of each number which consists 10
                if (tempDBFormat[i].getPriority() == 9)
                {
                    if (tempDBFormat[i + 1].getCenter().X <= tempDBFormat[i].getCenter().X)
                    {
                        return false;
                    }
                    else
                    {
                        continue;
                    }
                }

                // Check the location of each number which consists 11
                if (tempDBFormat[i].getPriority() == 11)
                {
                    if (tempDBFormat[i + 1].getCenter().X <= tempDBFormat[i].getCenter().X)
                    {
                        return false;
                    }
                    else
                    {
                        continue;
                    }
                }

                // Check the location of each number which consists 12
                if (tempDBFormat[i].getPriority() == 13)
                {
                    if (tempDBFormat[i + 1].getCenter().X <= tempDBFormat[i].getCenter().X)
                    {
                        return false;
                    }
                    else
                    {
                        continue;
                    }
                }
                return false;
            }
        }
    }

    e_index = RIGHT_MOST;

    // Checking the quadarant No 4.
    for (int i = 0; i < endPoint[e_index].getPrior(); i++)
    {
        // If the next number locates more right or upper than previous number then false, 
        // otherwise true.
        if (i < tempDBFormat.Length - 1)
        {
            if ((tempDBFormat[i + 1].getCenter().X <= tempDBFormat[i].getCenter().X)
                || (tempDBFormat[i + 1].getCenter().Y <= tempDBFormat[i].getCenter().Y))
            {
                return false;
            }
        }
    }
    return true;
}
*/
/*
// Calcuate left and right distance if the two hands are merged.
private void calcBothDist(DBFormat dbf)
{
    Point left_pt = new Point(CENTER_CIRCLE_X, CENTER_CIRCLE_Y);
    Point right_pt = new Point(CENTER_CIRCLE_X, CENTER_CIRCLE_Y);
    both_bottom_pt = new Point(0, 0);
    left_dist = 0;
    right_dist = 0;

    // Finding left most, right most, and bottom pt
    foreach (Point cusp in dbf.getCusps())
    {
        if ((cusp.X <= left_pt.X) && (cusp.X != 0) && (cusp.Y != 0))
        {
            left_pt = cusp;
        }
        if (cusp.X >= right_pt.X)
        {
            right_pt = cusp;
        }
        if (cusp.Y >= both_bottom_pt.Y)
        {
            both_bottom_pt = cusp;
        }
    }

    left_dist = (int)Math.Sqrt(Math.Pow((both_bottom_pt.X - left_pt.X), 2)
        + Math.Pow((both_bottom_pt.Y - left_pt.Y), 2));

    right_dist = (int)Math.Sqrt(Math.Pow((right_pt.X - both_bottom_pt.X), 2)
        + Math.Pow((both_bottom_pt.Y - right_pt.Y), 2));
}

// Calculate the length of hour hand
private void calcHourDist(DBFormat dbf)
{
    Point left_pt = new Point(CENTER_CIRCLE_X, CENTER_CIRCLE_Y);
    h_bottom_pt = new Point(0, 0);
    left_dist = 0;

    // Finding left most, and bottom pt
    foreach (Point cusp in dbf.getCusps())
    {
        if ((cusp.X <= left_pt.X) && (cusp.X != 0) && (cusp.Y != 0))
        {
            left_pt = cusp;
        }
        if (cusp.Y >= h_bottom_pt.Y)
        {
            h_bottom_pt = cusp;
        }
    }

    left_dist = (int)Math.Sqrt(Math.Pow((h_bottom_pt.X - left_pt.X), 2)
        + Math.Pow((h_bottom_pt.Y - left_pt.Y), 2));
}

// Calculate the length of minute hand
private void calcMinDist(DBFormat dbf)
{
    Point right_pt = new Point(CENTER_CIRCLE_X, CENTER_CIRCLE_Y);
    m_bottom_pt = new Point(0, 0);
    right_dist = 0;

    // Finding right most, and bottom pt
    foreach (Point cusp in dbf.getCusps())
    {
        if (cusp.X >= right_pt.X)
        {
            right_pt = cusp;
        }
        if (cusp.Y >= m_bottom_pt.Y)
        {
            m_bottom_pt = cusp;
        }
    }

    right_dist = (int)Math.Sqrt(Math.Pow((right_pt.X - m_bottom_pt.X), 2)
        + Math.Pow((m_bottom_pt.Y - right_pt.Y), 2));
}*/
/*
public ScoringNumbers(DBFormat[] dbFormat)
{
    tempDBFormat = new DBFormat[dbFormat.Length];
    tempDBFormat = dbFormat;
    Point center_pt = new Point(CENTER_CIRCLE_X, CENTER_CIRCLE_Y);

    for (int i = 0; i < endPoint.Length; i++)
    {
        endPoint[i] = new EndPoint(CENTER_CIRCLE_X, CENTER_CIRCLE_Y);
    }
}
*/
/*
private bool score12()
{
    int bottom_dist = 100;
    bool is_both_hands = false;

    // Find whether the two hands are merged. If so, this criteria is automatically determined as passed.
    // Otherwise, calculate the distance between the two bottom points.
    foreach (DBFormat dbf in tempDBFormat)
    {
        if (dbf.getIsBothHand())
        {
            is_both_hands = true;
            break;
        }
        else if ((dbf.getIsHhand() == true) || (dbf.getIsMhand() == true))
        {
            bottom_dist = (int)Math.Sqrt(Math.Pow((m_bottom_pt.X - h_bottom_pt.X), 2)
                + Math.Pow(Math.Abs(m_bottom_pt.Y - h_bottom_pt.Y), 2));
            break;
        }
    }
            
    // If bottom distance is shorter than 12mm or the two hands are merged then pass, otherwise fail.
    if ((bottom_dist <= 60) || (is_both_hands == true))
    {
        return true;
    }
    else
    {
        return false;
    }
}
 * */
/*
private bool score13()
{
    int bot_cen_dist = 0;
    int h_bot_dist = 0, m_bot_dist = 0;

    // Finding the two hands are merged or not
    foreach (DBFormat dbf in tempDBFormat)
    {
        // If the two hands are merged, checking the distance between bottom point and center point. 
        if (dbf.getIsBothHand())
        {
            bot_cen_dist = (int)Math.Sqrt(Math.Pow(Math.Abs(both_bottom_pt.X - CENTER_CIRCLE_X), 2)
                + Math.Pow(Math.Abs(both_bottom_pt.Y - CENTER_CIRCLE_Y), 2));

            // If the distance is shorter than 10mm then pass, otherwise fail.
            if (bot_cen_dist <= 70)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // If the two hands are separated, checking the distance between two bottom points and center point. 
        else if (dbf.getIsHhand() || dbf.getIsMhand())
        {
            h_bot_dist = (int)Math.Sqrt(Math.Pow(Math.Abs(h_bottom_pt.X - CENTER_CIRCLE_X), 2)
                + Math.Pow(Math.Abs(h_bottom_pt.Y - CENTER_CIRCLE_Y), 2));

            m_bot_dist = (int)Math.Sqrt(Math.Pow(Math.Abs(m_bottom_pt.X - CENTER_CIRCLE_X), 2)
                + Math.Pow(Math.Abs(m_bottom_pt.Y - CENTER_CIRCLE_Y), 2));

            // If the distance is shorter than 10mm then pass, otherwise fail.
            if ((h_bot_dist <= 50) && (m_bot_dist <= 50))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    return false;
}*/

//There are no superfluous marks 
// Scoring criteria 11
//TODO: Figure out this scoring parameter
/*
private bool score11()
{
    foreach (DBFormat dbf in tempDBFormat)
    {
        // If any stroke is a number or one of hands return false
        // Otherwise true
        if ((dbf.getRecogNumber() < 0) && (!dbf.getIsMhand()) && (!dbf.getIsHhand())
            && (!dbf.getIsBothHand()))
        {
            return false;
        }
    }
    return true;
}
*/