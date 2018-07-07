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

			Console.WriteLine("Waiting for the first response...");

			var secondsUntilResponse = Mqtt.WaitUntilData(1);

			Console.WriteLine("Time to first response: " + secondsUntilResponse + " seconds");

			var expectedMqttResponseTime = 0.1;

			AssertIsWithinRange("mqtt response time", expectedMqttResponseTime, secondsUntilResponse, TimeErrorMargin);

			Console.WriteLine("Skipping next data entries...");

			Mqtt.WaitUntilData(3); // TODO: See if this can be reduced or removed

			Console.WriteLine("Waiting for the next data entry...");

			var secondsBetweenData = Mqtt.WaitUntilData(1);

			Console.WriteLine("Time between data entries: " + secondsBetweenData + " seconds");

			var expectedMqttOutputTime = ReadInterval;

			AssertIsWithinRange("mqtt output time", expectedMqttOutputTime, secondsBetweenData, TimeErrorMargin);
		}
	}
}
