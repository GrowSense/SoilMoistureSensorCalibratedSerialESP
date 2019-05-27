using NUnit.Framework;

namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
    [TestFixture (Category = "Integration")]
    public class MqttOutputTimeTestFixture : BaseTestFixture
    {
        [Test]
        public void Test_MqttOutputTime_3Second ()
        {
            using (var helper = new MqttOutputTimeTestHelper ()) {
                helper.ReadInterval = 3;

                helper.DevicePort = GetDevicePort ();
                helper.DeviceBaudRate = GetDeviceSerialBaudRate ();

                helper.SimulatorPort = GetSimulatorPort ();
                helper.SimulatorBaudRate = GetSimulatorSerialBaudRate ();

                helper.TestMqttOutputTime ();
            }
        }

        [Test]
        public void Test_MqttOutputTime_4Seconds ()
        {
            using (var helper = new MqttOutputTimeTestHelper ()) {
                helper.ReadInterval = 4;

                helper.DevicePort = GetDevicePort ();
                helper.DeviceBaudRate = GetDeviceSerialBaudRate ();

                helper.SimulatorPort = GetSimulatorPort ();
                helper.SimulatorBaudRate = GetSimulatorSerialBaudRate ();

                helper.TestMqttOutputTime ();
            }
        }
    }
}
