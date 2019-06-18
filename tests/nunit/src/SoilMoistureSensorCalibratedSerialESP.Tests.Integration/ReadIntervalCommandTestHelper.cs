using System;

namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
    public class ReadIntervalCommandTestHelper : SerialCommandTestHelper
    {
        public int ReadingInterval = 3;

        public void TestSetReadIntervalCommand ()
        {
            Letter = "I";
            Value = ReadingInterval;
            Label = "reading interval";

            TestCommand ();
        }
    }
}
