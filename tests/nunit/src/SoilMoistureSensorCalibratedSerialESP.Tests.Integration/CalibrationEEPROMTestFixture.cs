using System;
using NUnit.Framework;
using duinocom;
using System.Threading;
using ArduinoSerialControllerClient;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.IO.Ports;

namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
	[TestFixture(Category = "Integration")]
	public class CalibrationEEPROMTestFixture : BaseTestFixture
	{
		[Test]
		public void Test_CalibrateDryEEPROM_10()
		{
			using (var helper = new CalibrationEEPROMTestHelper())
			{
				helper.Label = "dry";
				helper.Letter = "D";
				helper.RawSoilMoistureValue = 10;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestCalibrationEEPROM();
			}
		}

		[Test]
		public void Test_CalibrateDryEEPROM_200()
		{
			using (var helper = new CalibrationEEPROMTestHelper())
			{
				helper.Label = "dry";
				helper.Letter = "D";
				helper.RawSoilMoistureValue = 200;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestCalibrationEEPROM();
			}
		}

		[Test]
		public void Test_CalibrateWetEEPROM_950()
		{
			using (var helper = new CalibrationEEPROMTestHelper())
			{
				helper.Label = "wet";
				helper.Letter = "W";
				helper.RawSoilMoistureValue = 950;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestCalibrationEEPROM();
			}
		}

		[Test]
		public void Test_CalibrateWetEEPROM_1020()
		{
			using (var helper = new CalibrationEEPROMTestHelper())
			{
				helper.Label = "wet";
				helper.Letter = "W";
				helper.RawSoilMoistureValue = 1020;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestCalibrationEEPROM();
			}
		}
	}
}
