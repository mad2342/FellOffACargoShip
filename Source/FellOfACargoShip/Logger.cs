using System;
using System.IO;

namespace FellOfACargoShip
{
    public class Logger
    {
        static string filePath = $"{FellOfACargoShip.ModDirectory}/FellOfACargoShip.log";
        public static void LogError(Exception ex)
        {
            if (FellOfACargoShip.DebugLevel >= 1)
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    var prefix = "[FellOfACargoShip @ " + DateTime.Now.ToString() + "]";
                    writer.WriteLine("Message: " + ex.Message + "<br/>" + Environment.NewLine + "StackTrace: " + ex.StackTrace + "" + Environment.NewLine);
                    writer.WriteLine("----------------------------------------------------------------------------------------------------" + Environment.NewLine);
                }
            }
        }

        public static void LogLine(String line, bool showPrefix = true)
        {
            if (FellOfACargoShip.DebugLevel >= 2)
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    string prefix = "";
                    if (showPrefix)
                    {
                        prefix = "[FellOfACargoShip @ " + DateTime.Now.ToString() + "]";
                    }
                    writer.WriteLine(prefix + line);
                }
            }
        }
    }
}
