using System;
namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
	public class SerialOutputTimeTestHelper : GreenSenseHardwareTestHelper
	{
		public int ReadInterval = 1;

		public void TestSerialOutputTime()
		{
			WriteTitleText("Starting serial output time test");

			Console.WriteLine("Read interval: " + ReadInterval);

			EnableDevices(false);

			SetDeviceReadInterval(ReadInterval);

			ReadFromDeviceAndOutputToConsole();

			// Wait for the first bunch of data lines to come out before timing tem
			WaitForData(4); // TODO: See if this can be reduced

			// Get the time until the next data line
			var secondsBetweenDataLines = WaitUntilDataLine();

			var expectedTimeBetweenDataLines = ReadInterval;

			Console.WriteLine("Time between data lines: " + secondsBetweenDataLines + " seconds");

			AssertIsWithinRange("serial output time", expectedTimeBetweenDataLines, secondsBetweenDataLines, TimeErrorMargin);
		}
	}
}
