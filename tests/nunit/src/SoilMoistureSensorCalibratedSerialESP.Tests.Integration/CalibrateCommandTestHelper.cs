using System;
using System.Threading;

namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
    public class CalibrateCommandTestHelper : SerialCommandTestHelper
    {
        public int RawSoilMoistureValue = 0;

        public void TestCalibrateCommand ()
        {
            Value = RawSoilMoistureValue;
            TestCommand ();
        }
    }
}