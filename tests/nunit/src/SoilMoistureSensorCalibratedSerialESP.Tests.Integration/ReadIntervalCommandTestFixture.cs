using NUnit.Framework;

namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
    [TestFixture (Category = "Integration")]
    public class ReadIntervalCommandTestFixture : BaseTestFixture
    {
        [Test]
        public void Test_SetReadIntervalCommand_1Second ()
        {
            using (var helper = new ReadIntervalCommandTestHelper ()) {
                helper.ReadingInterval = 1;

                helper.DevicePort = GetDevicePort ();
                helper.DeviceBaudRate = GetDeviceSerialBaudRate ();

                helper.SimulatorPort = GetSimulatorPort ();
                helper.SimulatorBaudRate = GetSimulatorSerialBaudRate ();

                helper.TestSetReadIntervalCommand ();
            }
        }

        [Test]
        public void Test_SetReadIntervalCommand_3Seconds ()
        {
            using (var helper = new ReadIntervalCommandTestHelper ()) {
                helper.ReadingInterval = 3;

                helper.DevicePort = GetDevicePort ();
                helper.DeviceBaudRate = GetDeviceSerialBaudRate ();

                helper.SimulatorPort = GetSimulatorPort ();
                helper.SimulatorBaudRate = GetSimulatorSerialBaudRate ();

                helper.TestSetReadIntervalCommand ();
            }
        }
    }
}
