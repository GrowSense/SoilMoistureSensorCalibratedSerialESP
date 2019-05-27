using NUnit.Framework;

namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
    [TestFixture (Category = "Integration")]
    public class SerialOutputTimeTestFixture : BaseTestFixture
    {
        [Test]
        public void Test_SerialOutputTime_4Seconds ()
        {
            using (var helper = new SerialOutputTimeTestHelper ()) {
                helper.ReadInterval = 4;

                helper.DevicePort = GetDevicePort ();
                helper.DeviceBaudRate = GetDeviceSerialBaudRate ();

                helper.SimulatorPort = GetSimulatorPort ();
                helper.SimulatorBaudRate = GetSimulatorSerialBaudRate ();

                helper.TestSerialOutputTime ();
            }
        }

        [Test]
        public void Test_SerialOutputTime_5Seconds ()
        {
            using (var helper = new SerialOutputTimeTestHelper ()) {
                helper.ReadInterval = 5;

                helper.DevicePort = GetDevicePort ();
                helper.DeviceBaudRate = GetDeviceSerialBaudRate ();

                helper.SimulatorPort = GetSimulatorPort ();
                helper.SimulatorBaudRate = GetSimulatorSerialBaudRate ();

                helper.TestSerialOutputTime ();
            }
        }
    }
}
