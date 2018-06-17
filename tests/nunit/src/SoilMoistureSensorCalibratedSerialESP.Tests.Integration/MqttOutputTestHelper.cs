using System;
using NUnit.Framework;
namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
	public class MqttOutputTestHelper : GreenSenseMqttHardwareTestHelper
	{
		public int ReadInterval = 1;
		public int SimulatedSoilMoistureSensorValue = -1;

		public void TestMqttOutput()
		{
			WriteTitleText("Starting MQTT output test");

			Console.WriteLine("Read interval: " + ReadInterval);

			EnableDevices(SimulatedSoilMoistureSensorValue > -1);

			EnableMqtt();

			SetDeviceReadInterval(ReadInterval);

			if (SimulatedSoilMoistureSensorValue > -1)
				SimulateSoilMoisture(SimulatedSoilMoistureSensorValue);

			Console.WriteLine("Waiting for MQTT data...");

			Mqtt.WaitForData(3);

			var latestEntry = Mqtt.Data[Mqtt.Data.Count - 1];

			Mqtt.PrintDataEntry(latestEntry);

			var valueString = latestEntry["C"];

			Console.WriteLine("Calibrated value string: \"" + valueString + "\"");

			var containsWhitespace = valueString.Trim().Length != valueString.Length;

			Assert.IsFalse(containsWhitespace, "The calibrated value contains whitespace: \"" + valueString + "\"");

			var isDecimal = valueString.Contains(".");

			Assert.IsFalse(isDecimal, "The calibrated value contains a decimal point when it shouldn't.");

			Console.WriteLine("MQTT calibrated value string: " + valueString);

			var value = Convert.ToInt32(valueString);

			AssertIsWithinRange("MQTT calibrated soil moisture", SimulatedSoilMoistureSensorValue, value, CalibratedValueMarginOfError);
		}
	}
}
