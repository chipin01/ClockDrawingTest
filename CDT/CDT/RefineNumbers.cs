using System;
using System.Collections.Generic;
using System.Text;

namespace CDT_Week_3
{
    /// <summary>
    /// Refining process to raise the accuracy of number recognition
    /// </summary>
    class RefineNumbers
    {
        public RefineNumbers()
        {
        }

        /// <summary>
        /// Take unexpected incorrect characters entered by user, replace it to an intended number 
        /// and, return the corresponding number which reflects the user's intention
        /// </summary>
        /// <param name="strokeString"></param>
        /// <returns>Refined number as String type</returns>
        public String runRefineNumbers(String strokeString)
        {
            if (strokeString.Equals("(") || strokeString.Equals("|") ||
                strokeString.Equals("I") || strokeString.Equals("Ⅰ") || strokeString.Equals("{") ||
                strokeString.Equals("l") || strokeString.Equals("｜") || strokeString.Equals("'"))
            {
                strokeString = "1";
            }
            else if (strokeString.Equals("~") || strokeString.Equals("L") || strokeString.Equals("Z") ||
                strokeString.Equals("z") || strokeString.Equals("h") || strokeString.Equals("R"))
            {
                strokeString = "2";
            }
            else if (strokeString.Equals("S") || strokeString.Equals("}") || strokeString.Equals("by") ||
                strokeString.Equals(">"))
            {
                strokeString = "3";
            }
            else if (strokeString.Equals("+") || strokeString.Equals("y"))
            {
                strokeString = "4";
            }
            else if (strokeString.Equals("t") || strokeString.Equals("[") || strokeString.Equals("E"))
            {
                strokeString = "5";
            }
            else if (strokeString.Equals("b") || strokeString.Equals("G"))
            {
                strokeString = "6";
            }
            else if (strokeString.Equals("T") || strokeString.Equals("Y") || strokeString.Equals("n") ||
                strokeString.Equals("17") || strokeString.Equals(",") || strokeString.Equals("]"))
            {
                strokeString = "7";
            }
            else if (strokeString.Equals("P") || strokeString.Equals("p") ||
                strokeString.Equals("go") || strokeString.Equals("B") || strokeString.Equals("to") ||
                strokeString.Equals("m") || strokeString.Equals("do") || strokeString.Equals("00") ||
                strokeString.Equals("oo") || strokeString.Equals("OO"))
            {
                strokeString = "8";
            }
            else if (strokeString.Equals("of") || strokeString.Equals("a") || strokeString.Equals("q") ||
                strokeString.Equals("e") || strokeString.Equals("^") || strokeString.Equals("g"))
            {
                strokeString = "9";
            }
            else if (strokeString.Equals("o") || strokeString.Equals("O") || strokeString.Equals("x") ||
                strokeString.Equals("X") || strokeString.Equals("·") || strokeString.Equals("•") ||
                strokeString.Equals("U") || strokeString.Equals("u") || strokeString.Equals(".") || strokeString.Equals("-"))
            {
                strokeString = "0";
            }
            else if (strokeString.ToLower().Equals("so"))
            {
                strokeString = "10";
            }
            else if (strokeString.ToLower().Equals("is"))
            {
                strokeString = "11";
            }
            return strokeString;
        }
    }
}
