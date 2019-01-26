using System;
using System.Threading;

namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
	public class CalibrateMqttCommandTestHelper : GreenSenseMqttHardwareTestHelper
	{
		public string Label;
		public string Letter;
		public int RawSoilMoistureValue = 0;

		public void TestCalibrateCommand()
		{
			WriteTitleText("Starting calibrate " + Label + " command test");

			Console.WriteLine("Raw soil moisture value: " + RawSoilMoistureValue);
			Console.WriteLine("");

			EnableMqtt();

			ConnectDevices(false);

			SendMqttCalibrationCommand();
		}

		public void SendMqttCalibrationCommand()
		{

			Mqtt.SendCommand(Letter, RawSoilMoistureValue);

			var data = WaitForData(3); // Wait for 3 data entries to let the soil moisture simulator stabilise

			// If using the soil moisture simulator then the value needs to be within a specified range
			if (SimulatorIsEnabled)
				AssertDataValueIsWithinRange(data[data.Length - 1], Letter, RawSoilMoistureValue, RawValueMarginOfError);
			else // Otherwise it needs to be exact
				AssertDataValueEquals(data[data.Length - 1], Letter, RawSoilMoistureValue);
		}
	}
}