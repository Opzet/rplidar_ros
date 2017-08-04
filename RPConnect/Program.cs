using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using RPLidarSerial.RPLidar;
using RPLidarSerial;

namespace RPLidarSerialSimpleConnect
{
    public class Program
    {
        static RPLidarSerialDevice RPLidar;
        public static void Main(string[] args)
        {
            //New RPLidar object
            RPLidar = new RPLidarSerialDevice();
            //Set output parameters
            RPLidar.Verbose = false;
            RPLidar.WriteOutCoordinates = false;
            try
            {
                //Connect RPLidar
                RPLidar.Connect();
                //Reset - Not really sure how this is supposed to work, reconnecting USB works too
                //RPLidar.Reset();
                //Stop motor
                RPLidar.StopMotor();
                //Get Device Information
                RPLidar.GetDeviceInfo();
                //Get Device Health
                RPLidar.GetDeviceHealth();
                //Get Data Event
                RPLidar.Data += RPLidar_Data;
                //Start Scan Thread
                RPLidar.StartScan();
            }
            catch (System.IO.IOException ex)
            {
                HandleError("Serial connection failed: " + ex.Message);
            }
            catch(Exception ex)
            {
                HandleError("Something bad happend \n\t:" + ex.Source + "\n\t:" + ex.Message);
            }

            Console.WriteLine("Press any key to exit");         
            Console.ReadKey();

            //Stop Scanning
            RPLidar.StopScan();
            //Disconnect
            RPLidar.Disconnect();
            //Dispose Object
            RPLidar.Dispose();
        }

        static void RPLidar_Data(List<Response_PointFormat> Frames)
        {
            //Handle data here
            foreach(Response_PointFormat _frame in Frames)
            {
                Console.WriteLine("Distance: " + _frame.Distance.ToString() + " Angle: " + _frame.AngleDegrees.ToString());
                Console.WriteLine("X: " + _frame.X.ToString() + " Y: " + _frame.Y.ToString());
            }
        }

        private static void HandleError(string Message)
        {
            Console.WriteLine(Message);
            Console.WriteLine("\nPress any key to exit");
            Console.ReadKey();
            Environment.Exit(1);
        }
    }
}
