using System;
using NUnit.Framework;
using duinocom;
using System.Threading;
using ArduinoSerialControllerClient;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SoilMoistureSensorCalibratedSerial.Tests.Integration
{
	[TestFixture(Category="Integration")]
	public class ReadIntervalCommandTestFixture : BaseTestFixture
	{
		[Test]
		public void Test_SetReadIntervalCommand()
		{
			Console.WriteLine ("");
			Console.WriteLine ("==============================");
			Console.WriteLine ("Starting read interval command test");
			Console.WriteLine ("");

			SerialClient soilMoistureMonitor = null;

			try {
				soilMoistureMonitor = new SerialClient (GetDevicePort(), GetDeviceSerialBaudRate());

				Console.WriteLine("");
				Console.WriteLine("Connecting to serial devices...");
				Console.WriteLine("");

				soilMoistureMonitor.Open ();

				Thread.Sleep (DelayAfterConnecting);

				Console.WriteLine("");
				Console.WriteLine("Reading the output from the monitor device...");
				Console.WriteLine("");

				// Read the output
				var output = soilMoistureMonitor.Read ();

				Console.WriteLine (output);
				Console.WriteLine ("");

				Console.WriteLine("");
				Console.WriteLine("Sending 'X' command to device to reset to defaults...");
				Console.WriteLine("");

				// Reset defaults
				soilMoistureMonitor.WriteLine ("X");

				Thread.Sleep(3000);

				Console.WriteLine("");
				Console.WriteLine("Reading the output from the monitor device...");
				Console.WriteLine("");

				// Read the output
				output = soilMoistureMonitor.Read ();

				Console.WriteLine (output);
				Console.WriteLine ("");

				var readInterval = 1; // Seconds

				var command = "V" + readInterval;

				Console.WriteLine("");
				Console.WriteLine("Sending '" + command + "' command to monitor device...");
				Console.WriteLine("");

				// Send the command
				soilMoistureMonitor.WriteLine (command);

				Thread.Sleep(3000);

				Console.WriteLine("");
				Console.WriteLine("Reading the output from the monitor device...");
				Console.WriteLine("");

				// Read the output
				output = soilMoistureMonitor.Read ();

				Console.WriteLine (output);
				Console.WriteLine ("");

				Console.WriteLine("");
				Console.WriteLine("Checking the output...");
				Console.WriteLine("");

				var data = ParseOutputLine(GetLastDataLine(output));

				// Ensure the calibration value is in the valid range
				Assert.AreEqual(readInterval, data["V"], "Invalid read interval: " + data["V"]);

			} catch (IOException ex) {
				Console.WriteLine (ex.ToString ());
				Assert.Fail ();
			} finally {
				if (soilMoistureMonitor != null)
					soilMoistureMonitor.Close ();
			}
		}

	}
}
