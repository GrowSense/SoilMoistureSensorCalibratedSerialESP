using System;
using NUnit.Framework;

namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
    [TestFixture (Category = "Integration")]
    public class CalibrateToCurrentCommandTestFixture : BaseTestFixture
    {
        [Test]
        public void Test_CalibrateDryToCurrentSoilMoistureValueCommand_20Percent ()
        {
            using (var helper = new CalibrateToCurrentCommandTestHelper ()) {
                helper.Label = "dry";
                helper.Key = "D";
                helper.SimulatedSoilMoisturePercentage = 20;

                helper.DevicePort = GetDevicePort ();
                helper.DeviceBaudRate = GetDeviceSerialBaudRate ();

                helper.SimulatorPort = GetSimulatorPort ();
                helper.SimulatorBaudRate = GetSimulatorSerialBaudRate ();

                helper.TestCalibrateCommand ();
            }
        }

        [Test]
        public void Test_CalibrateDryToCurrentSoilMoistureValueCommand_30Percent ()
        {
            using (var helper = new CalibrateToCurrentCommandTestHelper ()) {
                helper.Label = "dry";
                helper.Key = "D";
                helper.SimulatedSoilMoisturePercentage = 30;

                helper.DevicePort = GetDevicePort ();
                helper.DeviceBaudRate = GetDeviceSerialBaudRate ();

                helper.SimulatorPort = GetSimulatorPort ();
                helper.SimulatorBaudRate = GetSimulatorSerialBaudRate ();

                helper.TestCalibrateCommand ();
            }
        }

        [Test]
        public void Test_CalibrateWetToCurrentSoilMoistureValueCommand_80Percent ()
        {
            using (var helper = new CalibrateToCurrentCommandTestHelper ()) {
                helper.Label = "wet";
                helper.Key = "W";
                helper.SimulatedSoilMoisturePercentage = 80;

                helper.DevicePort = GetDevicePort ();
                helper.DeviceBaudRate = GetDeviceSerialBaudRate ();

                helper.SimulatorPort = GetSimulatorPort ();
                helper.SimulatorBaudRate = GetSimulatorSerialBaudRate ();

                helper.TestCalibrateCommand ();
            }
        }

        [Test]
        public void Test_CalibrateWetToCurrentSoilMoistureValueCommand_90Percent ()
        {
            using (var helper = new CalibrateToCurrentCommandTestHelper ()) {
                helper.Label = "wet";
                helper.Key = "W";
                helper.SimulatedSoilMoisturePercentage = 90;

                helper.DevicePort = GetDevicePort ();
                helper.DeviceBaudRate = GetDeviceSerialBaudRate ();

                helper.SimulatorPort = GetSimulatorPort ();
                helper.SimulatorBaudRate = GetSimulatorSerialBaudRate ();

                helper.TestCalibrateCommand ();
            }
        }

    }
}

