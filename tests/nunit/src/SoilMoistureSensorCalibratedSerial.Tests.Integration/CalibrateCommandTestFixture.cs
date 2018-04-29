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
	public class CalibrateCommandTestFixture : BaseTestFixture
	{
		[Test]
		public void Test_CalibrateDryToCurrentValueCommand()
		{
			var percentage = 20;

			var raw = 219;

			TestCalibrateToCurrentCommand ("dry", "D", percentage, raw);
		}

		[Test]
		public void Test_CalibrateDryToSpecifiedValueCommand()
		{
			var raw = 220;

			TestCalibrateToCurrentCommand ("dry", "D" + raw, -1, raw);
		}

		[Test]
		public void Test_CalibrateWetToCurrentValueCommand()
		{
			var percentage = 80;

			var raw = 880;

			TestCalibrateToCurrentCommand ("wet", "W", percentage, raw);
		}

		[Test]
		public void Test_CalibrateWetToSpecifiedValueCommand()
		{
			var raw = 880;

			TestCalibrateToCurrentCommand ("wet", "W" + raw, -1, raw);
		}

		public void TestCalibrateToCurrentCommand(string label, string command, int percentageIn, int expectedRaw)
		{

			Console.WriteLine ("");
			Console.WriteLine ("==============================");
			Console.WriteLine ("Starting calibrate " + label + " command test");
			Console.WriteLine ("");
			Console.WriteLine ("Percentage in: " + percentageIn);
			Console.WriteLine ("Expected raw: " + expectedRaw);

			SerialClient soilMoistureMonitor = null;
			ArduinoSerialDevice soilMoistureSimulator = null;

			try {
				soilMoistureMonitor = new SerialClient (GetDevicePort(), GetSerialBaudRate());
				soilMoistureSimulator = new ArduinoSerialDevice (GetSimulatorPort(), GetSerialBaudRate());

				Console.WriteLine("");
				Console.WriteLine("Connecting to serial devices...");
				Console.WriteLine("");

				soilMoistureMonitor.Open ();
				soilMoistureSimulator.Connect ();

				Thread.Sleep (1000);

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

				Thread.Sleep(2000);
				
				// Set output interval to 1
				soilMoistureMonitor.WriteLine ("V1");

				Thread.Sleep(2000);

				Console.WriteLine("");
				Console.WriteLine("Reading the output from the monitor device...");
				Console.WriteLine("");

				// Read the output
				output = soilMoistureMonitor.Read ();

				Console.WriteLine (output);
				Console.WriteLine ("");

				Thread.Sleep(1000);

				// If a percentage is specified for the simulator then set the simulated soil moisture value (otherwise skip)
				if (percentageIn > -1)
				{
					Console.WriteLine("");
					Console.WriteLine("Sending analog percentage to simulator: " + percentageIn);
					Console.WriteLine("");

					// Set the simulated soil moisture
					soilMoistureSimulator.AnalogWritePercentage (9, percentageIn);

					Thread.Sleep(2000);

					Console.WriteLine("");
					Console.WriteLine("Reading output from the monitor device...");
					Console.WriteLine("");
					// Read the output
					output = soilMoistureMonitor.Read ();

					Console.WriteLine (output);
					Console.WriteLine ("");

					// Parse the values in the data line
					var values = ParseOutputLine(GetLastDataLine(output));

					// Get the raw soil moisture value
					var rawValue = values["R"];

					Console.WriteLine("");
					Console.WriteLine("Checking the values from the monitor device...");
					Console.WriteLine("");

					// Ensure the raw value is in the valid range
					Assert.IsTrue(IsWithinRange(rawValue, expectedRaw, 10), "Raw value is outside the valid range: " + rawValue);
				}

				Console.WriteLine("");
				Console.WriteLine("Sending '" + command + "' command to monitor device...");
				Console.WriteLine("");

				// Send the command
				soilMoistureMonitor.WriteLine (command);

				Thread.Sleep(1000);

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

				// Check the output
				var expected = "Setting " + label + " soil moisture sensor calibration value:";
				Assert.IsTrue(output.Contains(expected), "Didn't find expected output");

				var lastLine = "";

				var lines = output.Split('\n');

				// Extract the line containing the calibration value
				for (int i = lines.Length-1; i>=0; i--)
				{
					var line = lines[i];
					if (line.StartsWith(expected))
					{
						lastLine = line;
						break;
					}
				}

				Console.WriteLine("Last line");
				Console.WriteLine(lastLine);
				Console.WriteLine("");

				// Extraction the calibration value
				int startPosition = lastLine.IndexOf(":")+2;
				var cvString = lastLine.Substring(startPosition, lastLine.Length-startPosition);
				var calibrationValue = Convert.ToInt32(cvString);

				Console.WriteLine("Calibration value: " + calibrationValue);
				Console.WriteLine("Expected raw: " + expectedRaw);
				Console.WriteLine("");

				// Ensure the calibration value is in the valid range
				Assert.IsTrue(IsWithinRange(calibrationValue, expectedRaw, 13), "Calibration value is outside the valid range: " + calibrationValue);

			} catch (IOException ex) {
				Console.WriteLine (ex.ToString ());
				Assert.Fail ();
			} finally {
				if (soilMoistureMonitor != null)
					soilMoistureMonitor.Close ();

				if (soilMoistureSimulator != null)
					soilMoistureSimulator.Disconnect ();
			}
		}
	}
}
