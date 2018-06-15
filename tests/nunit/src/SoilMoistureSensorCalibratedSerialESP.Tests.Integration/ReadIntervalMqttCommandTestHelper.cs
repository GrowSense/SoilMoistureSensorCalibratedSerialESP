using System;
namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
	public class ReadIntervalMqttCommandTestHelper : GreenSenseMqttHardwareTestHelper
	{
		public int ReadInterval = 1;

		public void TestSetReadIntervalCommand()
		{
			WriteTitleText("Starting read interval command test");

			Console.WriteLine("Read interval: " + ReadInterval);

			EnableDevices(false);

			EnableMqtt();

			// MQTT version
			Mqtt.SendCommand("V", ReadInterval);

			var dataEntry = WaitForDataEntry();

			AssertDataValueEquals(dataEntry, "V", ReadInterval);
		}
	}
}
