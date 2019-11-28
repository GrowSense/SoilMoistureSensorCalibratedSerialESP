using System;
using NUnit.Framework;

namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
    [TestFixture (Category = "Integration")]
    public class MqttSettingsCommandTestFixture : BaseTestFixture
    {
        [Test]
        public void Test_SetMqttHostCommand ()
        {
            using (var helper = new SerialCommandTestHelper ()) {
                helper.Label = "MQTT host";
                helper.Value = "10.0.0." + new Random ().Next (100).ToString ();
                helper.Key = "MHost";
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
        public void Test_SetMqttUsernameCommand ()
        {
            using (var helper = new SerialCommandTestHelper ()) {
                helper.Label = "MQTT username";
                helper.Value = "username" + new Random ().Next (100).ToString ();
                helper.Key = "MUser";
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
        public void Test_SetMqttPasswordCommand ()
        {
            using (var helper = new SerialCommandTestHelper ()) {
                helper.Label = "MQTT password";
                helper.Value = "password" + new Random ().Next (100).ToString ();
                helper.Key = "MPass";
                helper.ValueIsOutputAsData = false;
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
        public void Test_SetMqttPortCommand ()
        {
            using (var helper = new SerialCommandTestHelper ()) {
                helper.Label = "MQTT port";
                helper.Value = new Random ().Next (1000, 2000).ToString ();
                helper.Key = "MPort";
                helper.ValueIsOutputAsData = false;
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

