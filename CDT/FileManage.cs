using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.IO;
using System.Text;
using System.Drawing;
using System.Data.SQLite;
using Microsoft.Ink;

namespace CDT_Week_3
{
    class FileManage
    {
        InkPicture inkPicture1;
        FileStream file;
        StreamWriter sw;
        StreamReader sr;
        String sub_name;
        DBFormat[] dbFormat;
        List<PenStroke> penStrokes;
        String fileNameWOExt;
        private bool[] scoreboard = new bool[13];
        private Circle clock;

        public String getID()
        {
            String id;

            file = new FileStream(".\\id.txt", FileMode.OpenOrCreate, FileAccess.Read);
            sr = new StreamReader(file);

            id = sr.ReadToEnd();

            sr.Close();
            file.Close();
            return id;
        }

        // This function handles the Save As.. command. It allows the user
        // to specify a filename and location, as well as the type of format
        // to save in. It then calls the appropriate helper function.
        // The try...catch section in this function will handle all of the error
        // handling for the helper methods it calls.
        public void saveFile(InkPicture inkPicture1, String sub_name, DBFormat[] dbf /*,ScoringNumbers scNumbers*/)
        {
            this.inkPicture1 = inkPicture1;
            this.sub_name = sub_name;
            dbFormat = new DBFormat[dbf.Length];
            dbFormat = dbf;
            //scoreboard = scNumbers.getScoreboard();

            /// Create a stream which will be used to save data to the output file
            Stream myStream = null;

            /// Create the SaveFileDialog, which presents a standard Windows
            /// Save dialog to the user.
            SaveFileDialog saveDialog = new SaveFileDialog();

            saveDialog.FileName = sub_name;

            /// Set the filter to suggest our recommended extensions
            saveDialog.Filter = "Ink Serialized Format files (*.isf)|*.isf";

            /// If the dialog exits and the user didn't choose Cancel
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    /// Attempt to Open the file with read/write permission
                    myStream = saveDialog.OpenFile();
                    if (myStream != null)
                    {
                        // Put the filename in a more canonical format
                        String filename = saveDialog.FileName.ToLower();

                        // Get a version of the filename without an extension
                        // This will be used for saving the associated image
                        String extensionlessFilename = Path.GetFileNameWithoutExtension(filename);

                        // Get the extension of the file 
                        String extension = Path.GetExtension(filename);

                        String filePath = filename.Replace(extensionlessFilename + extension, "");

                        fileNameWOExt = filePath + sub_name;

                        saveISF(myStream);

                        saveID("id.txt");
                        //saveDBFormat(filePath + sub_name + "_dbf.csv");
                        //saveCusps(filePath + sub_name + "_cusp.csv");
                        //saveAirtime(filePath + sub_name + "_air.csv");
                        //saveBzPts(filePath + sub_name + "_bzpts.csv");
                        //saveRawStroke(filePath + sub_name + "_rawS.csv");
                        //saveScore(filePath + sub_name + "_score.csv");

                        saveToSQLite(filePath + sub_name + "_sqlite.db3");
                    }
                    else
                    {
                        // Throw an exception if a null pointer is returned for the stream
                        throw new IOException();
                    }
                }
                catch (IOException /*ioe*/)
                {
                    MessageBox.Show("File error");
                }
                finally
                {
                    // Close the stream in the finally clause so it
                    // is always reached, regardless of whether an 
                    // exception occurs.  SaveXML, SaveHTML, and
                    // SaveISF can throw, so this precaution is necessary.
                    if (null != myStream)
                    {
                        myStream.Close();
                    }
                }
            } // End if user chose OK from dialog 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inkPicture1"></param>
        /// <param name="sub_name"></param>
        /// <param name="penStrokes"></param>
        /// <param name="scNumbers"></param>
        public bool saveFile(InkPicture inkPicture1, String sub_name, List<PenStroke> penStrokes, Circle clock)
        {
            this.inkPicture1 = inkPicture1;
            this.sub_name = sub_name;
            this.penStrokes = new List<PenStroke>();
            this.penStrokes = penStrokes;
            this.clock = clock;
            //this.scoreboard = scoreboard;

            /// Create a stream which will be used to save data to the output file
            Stream myStream = null;

            /// Create the SaveFileDialog, which presents a standard Windows
            /// Save dialog to the user.
            SaveFileDialog saveDialog = new SaveFileDialog();

            saveDialog.FileName = sub_name;

            /// Set the filter to suggest our recommended extensions
            saveDialog.Filter = "Ink Serialized Format files (*.isf)|*.isf";

            /// If the dialog exits and the user didn't choose Cancel
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    /// Attempt to Open the file with read/write permission
                    myStream = saveDialog.OpenFile();
                    if (myStream != null)
                    {
                        // Put the filename in a more canonical format
                        String filename = saveDialog.FileName.ToLower();

                        // Get a version of the filename without an extension
                        // This will be used for saving the associated image
                        String extensionlessFilename = Path.GetFileNameWithoutExtension(filename);

                        // Get the extension of the file 
                        String extension = Path.GetExtension(filename);

                        String filePath = filename.Replace(extensionlessFilename + extension, "");

                        fileNameWOExt = filePath + sub_name;

                        saveISF(myStream);

                        saveID("id.txt");                        

                        saveToSQLiteNew(filePath + sub_name + "_sqlite.db3");
                    }
                    else
                    {
                        // Throw an exception if a null pointer is returned for the stream
                        throw new IOException();
                    }
                }
                catch (IOException /*ioe*/)
                {
                    MessageBox.Show("File error");
                }
                finally
                {
                    // Close the stream in the finally clause so it
                    // is always reached, regardless of whether an 
                    // exception occurs.  SaveXML, SaveHTML, and
                    // SaveISF can throw, so this precaution is necessary.
                    if (null != myStream)
                    {
                        myStream.Close();
                    }
                }
                return true;
            } // End if user chose OK from dialog 

            return false;
        }


        private void saveToSQLite(String fileName)
        {
            SQLiteConnection.CreateFile(fileName);
            string connectionString = @"Data Source=" + Path.GetFullPath(fileName);
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            
            SQLiteCommand command = connection.CreateCommand();
            connection.Open();

            

            saveDBFormat(connection);
            saveCusps(connection);
            saveAirtime(connection);
            saveBzPts(connection);
            saveRawStroke(connection);
            saveScore(connection);
        }

        private void saveToSQLiteNew(String fileName)
        {
            SQLiteConnection.CreateFile(fileName);
            string connectionString = @"Data Source=" + Path.GetFullPath(fileName);
            SQLiteConnection connection = new SQLiteConnection(connectionString);

            SQLiteCommand command = connection.CreateCommand();
            connection.Open();
            createTables(connection);
            saveAllinDB(connection);
            saveScore(connection);
            saveClock(connection);
        }

        private void createTables(SQLiteConnection connection)
        {
            SQLiteCommand command = connection.CreateCommand();
            // create a pen strokes table
            string commandText = @"CREATE TABLE pen_stroke(
                                    id           INTEGER PRIMARY KEY,
                                    bzpoints        TEXT,       -- reference to point table
                                    bzcusps         TEXT,       -- reference to cusp table
                                    polycusps       TEXT,       -- reference to cusp table
                                    boundingbox     TEXT,       -- reference to rect table
                                    combineto    INTEGER,
                                    mergeto      INTEGER,
                                    mergingRect     TEXT,       -- reference to rect table
                                    packetpoints    TEXT,       -- reference to packetpoint table
                                    stroke          TEXT,       -- store byte or reference to inkstroke table?
                                    pxlboundingbox  TEXT,       -- reference to rect table
                                    recostrokes     TEXT,       -- either number or clock hands in string
                                    ishand       INTEGER,
                                    timestamp       TEXT)";

            command.CommandText = commandText;
            command.ExecuteNonQuery();

            // create a table to describe a point for general usage
            commandText = @"CREATE TABLE point(
                                    id           INTEGER PRIMARY KEY,
                                    isempty      INTEGER,
                                    x            INTEGER,
                                    y            INTEGER,
                                    type         TEXT)";
            command.CommandText = commandText;
            command.ExecuteNonQuery();

            // create a cusp table for general usage
            commandText = @"CREATE TABLE bzcusp(
                                    id        INTEGER PRIMARY KEY,
                                    cusp      INTEGER,
                                    type      TEXT)";
            command.CommandText = commandText;
            command.ExecuteNonQuery();
          
            // create a rectagle table for general usage
            commandText = @"CREATE TABLE rect(
                                    id          INTEGER PRIMARY KEY,
                                    x           INTEGER,
                                    y           INTEGER,
                                    bottom      INTEGER,
                                    height      INTEGER,
                                    isempty     INTEGER,
                                    left        INTEGER,
                                    right       INTEGER,                                    
                                    top         INTEGER,
                                    width       INTEGER,
                                    type        TEXT)";
            command.CommandText = commandText;
            command.ExecuteNonQuery();

            // create a packet point table
            commandText = @"CREATE TABLE packetpoint(
                                    id              INTEGER PRIMARY KEY,
                                    pkpoint         TEXT,                   -- reference to point table
                                    pressure        INTEGER,
                                    size            INTEGER,
                                    relatedstroke   INTEGER,
                                    timestamp       TEXT)";
            command.CommandText = commandText;
            command.ExecuteNonQuery();

            // create a ms ink.stroke table -- use byte to store all the data?
            commandText = @"CREATE TABLE inkstroke(
                                    id              INTEGER PRIMARY KEY,
                                    inkstroke       TEXT)";
            command.CommandText = commandText;
            command.ExecuteNonQuery();

            // create a ms ink.stroke table -- use byte to store all the data?
            commandText = @"CREATE TABLE clock(
                                    x              INTEGER,
                                    y              INTEGER,
                                    r              INTEGER,
                                    scaleX         REAL,
                                    scaleY         REAL,
                                    times          INTEGER)";
            command.CommandText = commandText;
            command.ExecuteNonQuery();
            command.Dispose();
        }
              
        private void saveAllinDB(SQLiteConnection connection)
        {           
            //SQLiteTransaction transaction = connection.BeginTransaction();
            foreach (PenStroke penstroke in penStrokes)
            {
                SQLiteCommand command = connection.CreateCommand();
                string rowID;
                StringBuilder refBZPoints = new StringBuilder();
                StringBuilder refBoundingBox = new StringBuilder();
                StringBuilder refPxlBoundingBox = new StringBuilder();
                StringBuilder refPacketPoints = new StringBuilder();
                StringBuilder refMergingRect = new StringBuilder();

                foreach (Point point in penstroke.BezierPoints)
                {
                    rowID = savePoint(connection, "Bezier Point", point);
                    refBZPoints.Append(rowID + ",");
                }
                                
                rowID = saveRect(connection, "Bounding Box", penstroke.BoundingBox);
                refBoundingBox.Append(rowID);

                rowID = saveRect(connection, "Pixel Bounding Box", penstroke.PixelBoundingBox);
                refPxlBoundingBox.Append(rowID);

                rowID = saveRect(connection, "Merging Rectangle", penstroke.MergingRectangle);
                refMergingRect.Append(rowID);

                foreach (PacketPoint pkPoint in penstroke.PacketPoints)
                {
                    rowID = savePacketPoint(connection, pkPoint);
                    refPacketPoints.Append(rowID + ",");
                }

                string commandText = @"INSERT INTO pen_stroke
                              (bzpoints, boundingbox, combineto, mergeto, mergingRect, packetpoints, pxlboundingbox, recostrokes, ishand, timestamp)
                              VALUES
                              ('" + refBZPoints.ToString() + "','" + refBoundingBox.ToString() + "'," + penstroke.CombineTo +
                              "," + penstroke.MergeTo + ",'" + refMergingRect.ToString() + "','" + refPacketPoints.ToString() +
                              "','" + refPxlBoundingBox.ToString() + "','" + penstroke.RecoStrokes + "'," + penstroke.isHand + "," + penstroke.TimeStamp.ToString() + ")";
                command = new SQLiteCommand(commandText, connection);
                command.ExecuteNonQuery();
                command.Dispose();
            }            
        }

        private string savePoint(SQLiteConnection connection, string type, Point point)
        {
            SQLiteCommand command = connection.CreateCommand();
            int isempty = 0;
            if (point.IsEmpty) isempty = 1;

            string commandText = @"INSERT INTO point
                              (isempty, x, y, type)
                              VALUES
                              (" + isempty + "," + point.X + "," + point.Y + ",'" + type + "');SELECT last_insert_rowid() AS id;";
            command = new SQLiteCommand(commandText, connection);
            string rowid = command.ExecuteScalar().ToString();

            command.Dispose();
            return rowid;
        }

        private string saveRect(SQLiteConnection connection, string type, Rectangle rect)
        {
            SQLiteCommand command = connection.CreateCommand();
            int isempty = 0;
            if (rect.IsEmpty) isempty = 1;

            string commandText = @"INSERT INTO rect
                              (isempty, x, y, type, bottom, right, left, top, height, width)
                              VALUES
                              (" + isempty + "," + rect.X + "," + rect.Y + ",'" + type + 
                              "',"+rect.Bottom+","+rect.Right+","+rect.Left+","+rect.Top+","+
                              rect.Height+","+rect.Width+");SELECT last_insert_rowid() AS id;";
            command = new SQLiteCommand(commandText, connection);
            string rowid = command.ExecuteScalar().ToString();
            command.Dispose();
            return rowid;
        }
        
        private string savePacketPoint(SQLiteConnection connection, PacketPoint pkPoint)
        {
            SQLiteCommand command = connection.CreateCommand();
            string pkptRowID = savePoint(connection, "Packet Point", pkPoint.PkPt);            

            string commandText = @"INSERT INTO packetpoint
                              (pkpoint, pressure, size, relatedstroke, timestamp)
                              VALUES
                              ('" + pkptRowID + "'," + pkPoint.Pressure + "," + pkPoint.Size + "," + pkPoint.StrokeID +
                               "," + pkPoint.TimeStamp.ToString() + ");SELECT last_insert_rowid() AS id;";
            command = new SQLiteCommand(commandText, connection);
            string rowid = command.ExecuteScalar().ToString();
            command.Dispose();
            return rowid;
        }

        private void saveClock(SQLiteConnection connection)
        {
            SQLiteCommand command = connection.CreateCommand();
            string commandText = @"INSERT INTO clock
                              (x, y, r, scaleX, scaleY, times)
                              VALUES
                              (" + clock.TopLeft.X + "," + clock.TopLeft.Y + "," + clock.Radius + "," + clock.ScaleX + "," + clock.ScaleY + ","+clock.Times + ")";
            command = new SQLiteCommand(commandText, connection);
            command.ExecuteNonQuery();
            command.Dispose();
        }

        // This function saves the form in ISF format.
        // It uses ExtendedProperties to preserve the first and last names.
        // ExtendedProperties are an easy way to store non-ink data within an
        // ink object. In this case, there is no outer format which contains the
        // ink, so the only place to store the names is within the ink object itself.
        private void saveISF(Stream s)
        {
            byte[] isf;

            // Perform the serialization
            isf = inkPicture1.Ink.Save(PersistenceFormat.InkSerializedFormat);

            // Write the ISF to the stream
            s.Write(isf, 0, isf.Length);
        }

        private void saveDBFormat(SQLiteConnection connection)
        {
            SQLiteCommand command = connection.CreateCommand();
            string commandText = @"CREATE TABLE dbformat(
                                    recogNo INTEGER,
                                    ltx     INTEGER,
                                    lty     INTEGER,
                                    rbx     INTEGER,
                                    rby     INTEGER,
                                    cenx    INTEGER,
                                    ceny    INTEGER,
                                    numorder INTEGER)";
            
            command.CommandText = commandText;
            command.ExecuteNonQuery();
            command.Dispose();
            //SQLiteTransaction transaction = connection.BeginTransaction();
            foreach (DBFormat dbf in dbFormat)
            {
                SQLiteCommand command2 = connection.CreateCommand();
                commandText = @"INSERT INTO dbformat
                              (recogNo,ltx,lty,rbx,rby,cenx,ceny,numorder)
                              VALUES
                              (" + dbf.getRecogNumber()+","+dbf.getPt1().X+","+dbf.getPt1().Y+","+dbf.getPt2().X+","
                               +dbf.getPt2().Y+","+dbf.getCenter().X+","+dbf.getCenter().Y+","+dbf.getOrder()+")";
                command2 = new SQLiteCommand(commandText, connection);
                command2.ExecuteNonQuery();
                command2.Dispose();
            }
            
        }

        private void saveCusps(SQLiteConnection connection)
        {
            SQLiteCommand command = connection.CreateCommand();
            string commandText = @"CREATE TABLE cusps(
                                    recogNo INTEGER,
                                    cuspX     INTEGER,
                                    cuspY     INTEGER,
                                    cnt     INTEGER,                                    
                                    numorder INTEGER)";
            command.CommandText = commandText;
            command.ExecuteNonQuery();
            command.Dispose();
            foreach (DBFormat dbf in dbFormat)
            {
                int cnt = 0;

                Point[] cusps = dbf.getCusps();
                foreach (Point cusp in cusps)
                {
                    SQLiteCommand command2 = connection.CreateCommand();
                    if ((cusp.X != 0) && (cusp.Y != 0))
                    {
                        cnt++;
                        commandText = @"INSERT INTO cusps(recogNo,cuspX,cuspY,cnt,numorder) 
                                        VALUES (" +dbf.getRecogNumber() + "," + cusp.X + "," + cusp.Y + "," + cnt
                                                 + "," + dbf.getOrder()+")";
                        command2 = new SQLiteCommand(commandText, connection);
                        command2.ExecuteNonQuery();
                    }
                    command2.Dispose();
                }
            }            
        }

        private void saveAirtime(SQLiteConnection connection)
        {
            SQLiteCommand command = connection.CreateCommand();
            string commandText = @"CREATE TABLE airtime(
                                    recogNo     INTEGER,
                                    airtime     INTEGER,
                                    title       TEXT,                                                                  
                                    numorder    INTEGER)";
            command.CommandText = commandText;
            command.ExecuteNonQuery();
            
            foreach (DBFormat dbf in dbFormat)
            {
                SQLiteCommand command2 = connection.CreateCommand();
                commandText = @"INSERT INTO airtime(recogNo,airtime,title,numorder) 
                                VALUES (" + dbf.getRecogNumber() + "," + dbf.getAirtime() + ",'"
                                          + dbf.getTitle()[4] + "'," + dbf.getOrder() + ")";
                command2 = new SQLiteCommand(commandText, connection);
                command.ExecuteNonQuery();
                command2.Dispose();   
            }           
        }

        private void saveBzPts(SQLiteConnection connection)
        {
            SQLiteCommand command = connection.CreateCommand();
            string commandText = @"CREATE TABLE bzpts(
                                    recogNo     INTEGER,
                                    title       TEXT,  
                                    bzptX       INTEGER,
                                    bzptY       INTEGER,
                                    cnt         INTEGER,                                                          
                                    numorder    INTEGER)";
            command.CommandText = commandText;
            command.ExecuteNonQuery();
            command.Dispose();
            foreach (DBFormat dbf in dbFormat)
            {
                int cnt = 0;

                Point[] bzPts = dbf.getBzPts();
                foreach (Point bzpt in bzPts)
                {
                    if ((bzpt.X != 0) && (bzpt.Y != 0))
                    {
                        SQLiteCommand command2 = connection.CreateCommand();
                        cnt++;
                        commandText = @"INSERT INTO bzpts(recogNo,title,bzptX,bzptY,cnt,numorder) 
                                        VALUES (" + dbf.getRecogNumber() + ",'" + dbf.getTitle()[4] + "',"
                                                  + bzpt.X + "," + bzpt.Y + "," + cnt + "," + dbf.getOrder() + ")";
                        command2 = new SQLiteCommand(commandText, connection);
                        command2.ExecuteNonQuery();
                        command2.Dispose();                      
                    }
                }
            }            
        }

        private void saveRawStroke(SQLiteConnection connection)
        {
            SQLiteCommand command = connection.CreateCommand();
            string commandText = @"CREATE TABLE stroke(
                                    recogNo     INTEGER,
                                    stroke       TEXT,                                                                                              
                                    numorder    INTEGER)";
            command.CommandText = commandText;
            command.ExecuteNonQuery();
            command.Dispose();
            foreach (DBFormat dbf in dbFormat)
            {
                SQLiteCommand command2 = connection.CreateCommand();
                string rawStroke = dbf.getRawStroke();
                if (rawStroke == "'")
                {
                    rawStroke = "''";
                }
                commandText = @"INSERT INTO stroke(recogNo,stroke,numorder) 
                                        VALUES (" + dbf.getRecogNumber() + ",'" + rawStroke + "'," + dbf.getOrder() + ")";
                command2 = new SQLiteCommand(commandText, connection);
                command2.ExecuteNonQuery();
                command2.Dispose();                     
            }            
        }

        private void saveScore(SQLiteConnection connection)
        {
            SQLiteCommand command = connection.CreateCommand();
            string commandText = @"CREATE TABLE score(
                                    num     INTEGER PRIMARY KEY,
                                    score   INTEGER)";
            command.CommandText = commandText;
            command.ExecuteNonQuery();
            command.Dispose();
            foreach (bool score in scoreboard)
            {
                int scorenum = 0;
                if (score)
                    scorenum = 1;
                SQLiteCommand command2 = connection.CreateCommand();
                commandText = @"INSERT INTO score(score) 
                                        VALUES (" + scorenum + ")";
                command2 = new SQLiteCommand(commandText, connection);
                command2.ExecuteNonQuery();
                command2.Dispose();
            }            
        }

        private void saveID(String fileName)
        {
            file = new FileStream(".\\" + fileName, FileMode.OpenOrCreate, FileAccess.Write);
            sw = new StreamWriter(file);

            sw.Write(sub_name);

            sw.Close();
            file.Close();
        }        

        public String getFilenameWOExt()
        {
            return fileNameWOExt;
        }
    }
}


