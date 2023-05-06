using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ClockReader
{
    public class Clock
    {
        private int[] scores;
        private float[] airtimes;
        private StringBuilder parInfo;
        private Microsoft.Ink.Ink ink;
        private List<PenStroke> penstrokes;

        public int numOfTrial { get; set; }
        public Clock()
        {
            scores = new int[13];
            parInfo = new StringBuilder("");
            penstrokes = new List<PenStroke>();
        }

        /// <summary>
        /// set or get the scores from the recorded data
        /// </summary>
        public int[] ScoreBoard
        {
            set
            {
                scores = value;
            }
            get
            {
                return this.scores;
            }
        }

        public float[] AirTime
        {
            set
            {
                this.airtimes = value;
            }
            get
            {
                return this.airtimes;
            }
        }

        public List<PenStroke> PenStrokes
        {
            get
            {
                return this.penstrokes;
            }
            set
            {
                this.penstrokes = value;
            }
        }

        public Microsoft.Ink.Ink Ink
        {
            set
            {
                this.ink = value;
            }
            get
            {
                return this.ink;
            }
        }

        public int X { set; get; }
        public int Y { set; get; }
        public int R { set; get; }
    }
}
