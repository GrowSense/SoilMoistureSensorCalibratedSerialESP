using System;
using NUnit.Framework;

namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
	[TestFixture(Category = "Integration")]
	public class SoilMoistureSensorPowerTestFixture : BaseTestFixture
	{
		[Test]
		public void Test_SoilMoistureSensorPower_AlwaysOn_1SecondReadInterval()
		{
			using (var helper = new SoilMoistureSensorPowerTestHelper())
			{
				helper.ReadInterval = 1;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestSoilMoistureSensorPower();
			}
		}

		[Test]
		public void Test_SoilMoistureSensorPower_AlwaysOn_3SecondReadInterval()
		{
			using (var helper = new SoilMoistureSensorPowerTestHelper())
			{
				helper.ReadInterval = 3;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestSoilMoistureSensorPower();
			}
		}

		[Test]
		public void Test_SoilMoistureSensorPower_OnAndOff_4SecondReadInterval()
		{
			using (var helper = new SoilMoistureSensorPowerTestHelper())
			{
				helper.ReadInterval = 4;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestSoilMoistureSensorPower();
			}
		}
		[Test]
		public void Test_SoilMoistureSensorPower_OnAndOff_6SecondReadInterval()
		{
			using (var helper = new SoilMoistureSensorPowerTestHelper())
			{
				helper.ReadInterval = 6;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestSoilMoistureSensorPower();
			}
		}
	}
}
