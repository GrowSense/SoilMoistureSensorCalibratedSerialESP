using System;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

namespace SoilMoistureSensorCalibratedSerial.Tests.Integration
{
	public class BaseTestFixture
	{
		public int PwmHigh = 255;
		public int AnalogHigh = 1024;
		
		public bool CalibrationIsReversedByDefault = true; // Reversed by default to matches common soil moisture sensors
		
		public int DelayAfterConnecting = 10 * 1000;
		
		public BaseTestFixture ()
		{
		}

		[SetUp]
		public virtual void Initialize()
		{
		}

		[TearDown]
		public virtual void Finish()
		{
		}

		
		public string GetDevicePort()
		{
			var devicePort = Environment.GetEnvironmentVariable ("MONITOR_ESP_PORT");
			
			if (String.IsNullOrEmpty(devicePort))
				devicePort = "/dev/ttyUSB2";
			
			Console.WriteLine ("Device port: " + devicePort);
			
			return devicePort;
		}

		public string GetSimulatorPort()
		{
			var simulatorPort = Environment.GetEnvironmentVariable ("MONITOR_ESP_SIMULATOR_PORT");
			
			if (String.IsNullOrEmpty(simulatorPort))
				simulatorPort = "/dev/ttyUSB3";
			
			Console.WriteLine ("Simulator port: " + simulatorPort);
			
			return simulatorPort;
		}

		public int GetDeviceSerialBaudRate()
		{
			var baudRateString = Environment.GetEnvironmentVariable ("MONITOR_ESP_BAUD_RATE");
			
			var baudRate = 0;
			
			if (String.IsNullOrEmpty(baudRateString))
				baudRate = 115200;
			else
				baudRate = Convert.ToInt32(baudRateString);
			
			Console.WriteLine ("Device baud rate: " + baudRate);
			
			return baudRate;
		}

		public int GetSimulatorSerialBaudRate()
		{
			var baudRateString = Environment.GetEnvironmentVariable ("MONITOR_ESP_SIMULATOR_BAUD_RATE");
			
			var baudRate = 0;
			
			if (String.IsNullOrEmpty(baudRateString))
				baudRate = 9600;
			else
				baudRate = Convert.ToInt32(baudRateString);
			
			Console.WriteLine ("Simulator baud rate: " + baudRate);
			
			return baudRate;
		}
		
		public bool IsWithinRange(int expectedValue, int actualValue, int allowableMarginOfError)
		{
			Console.WriteLine("Checking value is within range...");
			Console.WriteLine("  Expected value: " + expectedValue);
			Console.WriteLine("  Actual value: " + actualValue);
			Console.WriteLine("");
			Console.WriteLine("  Allowable margin of error: " + allowableMarginOfError);
			
			var minAllowableValue = expectedValue-allowableMarginOfError;
			if (minAllowableValue < 0)
				minAllowableValue = 0;
			var maxAllowableValue = expectedValue+allowableMarginOfError;
			
			Console.WriteLine("  Max allowable value: " + maxAllowableValue);
			Console.WriteLine("  Min allowable value: " + minAllowableValue);
			
			var isWithinRange = actualValue <= maxAllowableValue &&
				actualValue >= minAllowableValue;
		
			Console.WriteLine("Is within range: " + isWithinRange);
		
			return isWithinRange;
		}

		public Dictionary<string, string> ParseOutputLine(string outputLine)
		{
			var dictionary = new Dictionary<string, string> ();

			if (IsValidOutputLine (outputLine)) {
				foreach (var pair in outputLine.Split(';')) {
					var parts = pair.Split (':');

					if (parts.Length == 2) {
						var key = parts [0];
						var value = parts [1];
						dictionary [key] = value;
					}
				}
			}

			return dictionary;
		}

		public string GetLastDataLine(string output)
		{
			var lines = output.Split ('\n');

			for (int i = lines.Length - 1; i >= 0; i--) {
				var line = lines [i].Trim();
				if (IsValidOutputLine(line))
					return line;
			}

			return String.Empty;
		}

		public bool IsValidOutputLine(string outputLine)
		{
			var dataPrefix = "D;";

			var dataPostFix = ";;";

			return outputLine.StartsWith(dataPrefix)
				&& outputLine.EndsWith(dataPostFix);
		}
	}
}
