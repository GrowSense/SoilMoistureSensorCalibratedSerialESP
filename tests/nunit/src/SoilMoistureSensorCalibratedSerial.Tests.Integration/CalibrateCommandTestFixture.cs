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

			TestCalibrateToCurrentCommand ("dry", "D", -1, raw);
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

			TestCalibrateToCurrentCommand ("wet", "W", -1, raw);
		}

		public void TestCalibrateToCurrentCommand(string label, string letter, int percentageIn, int rawIn)
		{

			Console.WriteLine ("");
			Console.WriteLine ("==============================");
			Console.WriteLine ("Starting calibrate " + label + " command test");
			Console.WriteLine ("");
			Console.WriteLine ("Percentage in: " + percentageIn);
			Console.WriteLine ("Expected raw: " + rawIn);

			SerialClient soilMoistureMonitor = null;
			ArduinoSerialDevice soilMoistureSimulator = null;

			try {
				soilMoistureMonitor = new SerialClient (GetDevicePort(), GetDeviceSerialBaudRate());
				soilMoistureSimulator = new ArduinoSerialDevice (GetSimulatorPort(), GetSimulatorSerialBaudRate());

				Console.WriteLine("");
				Console.WriteLine("Connecting to serial devices...");
				Console.WriteLine("");

				soilMoistureMonitor.Open ();
				soilMoistureSimulator.Connect ();

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
					var rawValue = Convert.ToInt32(values["R"]);

					Console.WriteLine("");
					Console.WriteLine("Checking the values from the monitor device...");
					Console.WriteLine("");

					// Ensure the raw value is in the valid range
					Assert.IsTrue(IsWithinRange(rawValue, rawIn, 20), "Raw value is outside the valid range: " + rawValue);
				}

				var command = letter;
				
				// If the simulated percentage isn't set then pass the raw value as part of the command
				if (percentageIn == -1)
					command = command + rawIn;

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

				var newValues = ParseOutputLine(GetLastDataLine(output));
				
				Console.WriteLine("Letter: " + letter);
				
				var valueString = newValues[letter];
				
				Console.WriteLine("Value string: " + valueString);

				var calibrationValue = Convert.ToInt32(valueString);

				Console.WriteLine("Calibration value: " + calibrationValue);
				Console.WriteLine("Expected value: " + rawIn);
				Console.WriteLine(""); 

				// Ensure the calibration value is in the valid range
				Assert.IsTrue(IsWithinRange(calibrationValue, rawIn, 20), "Calibration value is outside the valid range: " + calibrationValue);

			} catch (Exception ex) {
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
