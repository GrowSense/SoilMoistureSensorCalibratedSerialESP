using System;

namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
    public class SerialOutputTimeTestHelper : GreenSenseHardwareTestHelper
    {
        public int ReadInterval = 1;

        public void TestSerialOutputTime ()
        {
            WriteTitleText ("Starting serial output time test");

            Console.WriteLine ("Read interval: " + ReadInterval);

            ConnectDevices (false);

            SetDeviceReadInterval (ReadInterval);

            ReadFromDeviceAndOutputToConsole ();

            // Skip some data before checking the output time
            WaitForData (3);

            // Get the time until the next data line
            var secondsBetweenDataLines = WaitUntilDataLine ();

            var expectedTimeBetweenDataLines = ReadInterval;

            Console.WriteLine ("Time between data lines: " + secondsBetweenDataLines + " seconds");

            AssertIsWithinRange ("serial output time", expectedTimeBetweenDataLines, secondsBetweenDataLines, TimeErrorMargin);
        }
    }
}
