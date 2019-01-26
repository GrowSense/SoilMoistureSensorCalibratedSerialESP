using System;

namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
	public class ReadIntervalEEPROMTestHelper : GreenSenseHardwareTestHelper
	{
		public int ReadInterval = 3;

		public void TestReadIntervalEEPROM()
		{
			WriteTitleText("Starting read interval EEPROM test");

			Console.WriteLine("Read interval: " + ReadInterval + "%");
			Console.WriteLine("");

			ConnectDevices();

			ResetDeviceSettings ();

			SendReadIntervalCommand();

			ResetDeviceViaPin ();

			// Skip the first data entry
			WaitForData (1);

			// Wait for the next entry
			var dataEntry = WaitForDataEntry ();

			AssertDataValueEquals(dataEntry, "I", ReadInterval);
		}

		public void SendReadIntervalCommand()
		{
			var command = "I" + ReadInterval;

			WriteParagraphTitleText("Sending read interval command...");

			SendDeviceCommand(command);

			var dataEntry = WaitForDataEntry();

			WriteParagraphTitleText("Checking read interval value...");

			AssertDataValueEquals(dataEntry, "I", ReadInterval);
		}
	}
}
