using NUnit.Framework;

namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
	[TestFixture(Category = "Integration")]
	public class CalibrateCommandTestFixture : BaseTestFixture
	{
		[Test]
		public void Test_CalibrateDryToCurrentSoilMoistureValueCommand_20Percent()
		{
			using (var helper = new CalibrateCommandTestHelper())
			{
				helper.Label = "dry";
				helper.Letter = "D";
				helper.SimulatedSoilMoisturePercentage = 20;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestCalibrateCommand();
			}
		}

		[Test]
		public void Test_CalibrateDryToCurrentSoilMoistureValueCommand_30Percent()
		{
			using (var helper = new CalibrateCommandTestHelper())
			{
				helper.Label = "dry";
				helper.Letter = "D";
				helper.SimulatedSoilMoisturePercentage = 30;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestCalibrateCommand();
			}
		}

		[Test]
		public void Test_CalibrateDryToSpecifiedValueCommand_200()
		{
			using (var helper = new CalibrateCommandTestHelper())
			{
				helper.Label = "dry";
				helper.Letter = "D";
				helper.RawSoilMoistureValue = 200;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestCalibrateCommand();
			}
		}

		[Test]
		public void Test_CalibrateDryToSpecifiedValueCommand_220()
		{
			using (var helper = new CalibrateCommandTestHelper())
			{
				helper.Label = "dry";
				helper.Letter = "D";
				helper.RawSoilMoistureValue = 220;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestCalibrateCommand();
			}
		}

		[Test]
		public void Test_CalibrateWetToCurrentSoilMoistureValueCommand_80Percent()
		{
			using (var helper = new CalibrateCommandTestHelper())
			{
				helper.Label = "wet";
				helper.Letter = "W";
				helper.SimulatedSoilMoisturePercentage = 80;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestCalibrateCommand();
			}
		}

		[Test]
		public void Test_CalibrateWetToCurrentSoilMoistureValueCommand_90Percent()
		{
			using (var helper = new CalibrateCommandTestHelper())
			{
				helper.Label = "wet";
				helper.Letter = "W";
				helper.SimulatedSoilMoisturePercentage = 90;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestCalibrateCommand();
			}
		}

		[Test]
		public void Test_CalibrateWetToSpecifiedValueCommand_880()
		{
			using (var helper = new CalibrateCommandTestHelper())
			{
				helper.Label = "wet";
				helper.Letter = "W";
				helper.RawSoilMoistureValue = 880;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestCalibrateCommand();
			}
		}

		[Test]
		public void Test_CalibrateWetToSpecifiedValueCommand_900()
		{
			using (var helper = new CalibrateCommandTestHelper())
			{
				helper.Label = "wet";
				helper.Letter = "W";
				helper.RawSoilMoistureValue = 900;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestCalibrateCommand();
			}
		}
	}
}
