using System;

namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
	public class CalibrationEEPROMTestHelper : GreenSenseHardwareTestHelper
	{
		public string Label;
		public string Letter;
		public int RawSoilMoistureValue = 0;

		public void TestCalibrationEEPROM()
		{
			WriteTitleText("Starting calibration command test");

			Console.WriteLine("Label: " + Label);
			Console.WriteLine("Letter: " + Letter);
			Console.WriteLine("");

			ConnectDevices();

			ResetDeviceSettings ();

			SendCalibrationCommand();

			ResetDeviceViaPin ();

			var dataEntry = WaitForDataEntry ();

			AssertDataValueEquals(dataEntry, Letter, RawSoilMoistureValue);
		}

		public void SendCalibrationCommand()
		{
			var command = Letter + RawSoilMoistureValue;

			WriteParagraphTitleText("Sending calibration command...");

			SendDeviceCommand(command);

			var dataEntry = WaitForDataEntry();

			WriteParagraphTitleText("Checking calibration value...");

			AssertDataValueEquals(dataEntry, Letter, RawSoilMoistureValue);
		}
	}
}
