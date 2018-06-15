using NUnit.Framework;

namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
	[TestFixture(Category = "Integration")]
	public class ReadIntervalMqttCommandTestFixture : BaseTestFixture
	{
		[Test]
		public void Test_SetReadIntervalCommand_1Second()
		{
			using (var helper = new ReadIntervalMqttCommandTestHelper())
			{
				helper.ReadInterval = 1;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestSetReadIntervalCommand();
			}
		}

		[Test]
		public void Test_SetReadIntervalCommand_5Seconds()
		{
			using (var helper = new ReadIntervalMqttCommandTestHelper())
			{
				helper.ReadInterval = 5;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestSetReadIntervalCommand();
			}
		}
	}
}
