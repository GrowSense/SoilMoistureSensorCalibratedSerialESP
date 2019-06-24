using System;
using NUnit.Framework;

namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
    [TestFixture (Category = "Integration")]
    public class WiFiSettingsCommandTestFixture : BaseTestFixture
    {
        [Test]
        public void Test_SetWiFiNetworkCommand ()
        {
            using (var helper = new SerialCommandTestHelper ()) {
                helper.Label = "WiFi network";
                helper.Value = "Network" + new Random ().Next (100).ToString ();
                helper.Key = "WN";
                helper.ValueIsOutputAsData = false;
                helper.RequiresResetSettings = false;
                helper.SeparateKeyValueWithColon = true;
                helper.CheckExpectedSerialOutput = true;

                helper.DevicePort = GetDevicePort ();
                helper.DeviceBaudRate = GetDeviceSerialBaudRate ();

                helper.SimulatorPort = GetSimulatorPort ();
                helper.SimulatorBaudRate = GetSimulatorSerialBaudRate ();

                helper.TestCommand ();
            }
        }

        [Test]
        public void Test_SetWiFiPasswordCommand ()
        {
            using (var helper = new SerialCommandTestHelper ()) {
                helper.Label = "WiFi password";
                helper.Value = "password" + new Random ().Next (100).ToString ();
                helper.Key = "WPass";
                helper.ValueIsOutputAsData = false;
                helper.RequiresResetSettings = false;
                helper.SeparateKeyValueWithColon = true;
                helper.CheckExpectedSerialOutput = true;

                helper.DevicePort = GetDevicePort ();
                helper.DeviceBaudRate = GetDeviceSerialBaudRate ();

                helper.SimulatorPort = GetSimulatorPort ();
                helper.SimulatorBaudRate = GetSimulatorSerialBaudRate ();

                helper.TestCommand ();
            }
        }
    }
}

