using System;
namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
	public class MqttOutputTimeTestHelper : GreenSenseMqttHardwareTestHelper
	{
		public int ReadInterval = 1;

		public void TestMqttOutputTime()
		{
			WriteTitleText("Starting MQTT output time test");

			Console.WriteLine("Read interval: " + ReadInterval);

			EnableDevices(false);

			EnableMqtt();

			SetDeviceReadInterval(ReadInterval);

			// Wait for the first data line before starting
			Mqtt.WaitForData(1);

			var secondsBetweenData = Mqtt.WaitUntilData(1);

			Console.WriteLine("Time between data lines: " + secondsBetweenData + " seconds");

			var expectedMqttOutputTime = ReadInterval;

			AssertIsWithinRange("mqtt output time", expectedMqttOutputTime, secondsBetweenData, TimeErrorMargin);
		}
	}
}
