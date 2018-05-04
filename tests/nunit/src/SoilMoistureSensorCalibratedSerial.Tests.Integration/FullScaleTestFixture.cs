using System;
using NUnit.Framework;
using duinocom;
using System.Threading;
using ArduinoSerialControllerClient;
using System.Collections.Generic;
using System.IO;

namespace SoilMoistureSensorCalibratedSerial.Tests.Integration
{
	[TestFixture(Category="Integration")]
	public class FullScaleTestFixture : BaseTestFixture
	{
		[Test]
		public void Test_Complete()
		{
			Console.WriteLine ("");
			Console.WriteLine ("==============================");
			Console.WriteLine ("Starting full scale test");
			Console.WriteLine ("");

			SerialClient soilMoistureMonitor = null;
			ArduinoSerialDevice soilMoistureSimulator = null;

			try {
				soilMoistureMonitor = new SerialClient (GetDevicePort(), GetDeviceSerialBaudRate());
				soilMoistureSimulator = new ArduinoSerialDevice (GetSimulatorPort(), GetSimulatorSerialBaudRate());

				Console.WriteLine("");
				Console.WriteLine("Opening serial connections...");
				Console.WriteLine("");

				soilMoistureMonitor.Open ();
				soilMoistureSimulator.Connect ();

				Thread.Sleep (2000);

				Console.WriteLine("");
				Console.WriteLine("Reading the output from the monitor device...");
				Console.WriteLine("");

				// Read the output
				var output = soilMoistureMonitor.Read ();

				Console.WriteLine (output);
				Console.WriteLine ("");

				// Reset defaults
				soilMoistureMonitor.WriteLine ("X");

				Thread.Sleep (1000);
				
				// Set output interval to 1
				soilMoistureMonitor.WriteLine ("V1");

				Thread.Sleep(1000);

				Console.WriteLine("");
				Console.WriteLine("Reading the output from the monitor device...");
				Console.WriteLine("");

				// Read the output
				output = soilMoistureMonitor.Read ();

				Console.WriteLine (output);
				Console.WriteLine ("");

				int step = 25;

				for (int i = 100; i >= 0; i -= step) {
					RunCalibrationTest (i, CalibrationIsReversedByDefault, soilMoistureMonitor, soilMoistureSimulator);
        
					Thread.Sleep (1000);
				}
        
				for (int i = 0; i < 100; i += step) {
					RunCalibrationTest (i, CalibrationIsReversedByDefault, soilMoistureMonitor, soilMoistureSimulator);

					Thread.Sleep (1000);
				}

			} catch (IOException ex) {
				Console.WriteLine (ex.ToString ());
			} finally {
				if (soilMoistureMonitor != null)
					soilMoistureMonitor.Close ();
          
				if (soilMoistureSimulator != null)
					soilMoistureSimulator.Disconnect ();
			}
		}
		
		public void RunCalibrationTest(int soilMoisturePercentage, bool calibrationIsReversed, SerialClient soilMoistureMonitor, ArduinoSerialDevice soilMoistureSimulator)
		{
		
			Console.WriteLine ("");
			Console.WriteLine ("==============================");
			Console.WriteLine ("Starting calibration test");
			Console.WriteLine ("");
      
			int percentageValue = soilMoisturePercentage;        
      
			Console.WriteLine ("");
			Console.WriteLine ("Sending percentage to simulator: " + percentageValue);
			Console.WriteLine ("");
      
			soilMoistureSimulator.AnalogWritePercentage (9, percentageValue);
      
			Thread.Sleep (2000);
      
			Console.WriteLine ("");
			Console.WriteLine ("Reading data from soil moisture monitor");
			Console.WriteLine ("");
      
			var output = soilMoistureMonitor.Read ();
      
			Console.WriteLine (output);
			Console.WriteLine ("");
      
			var data = ParseOutputLine (GetLastDataLine(output));
      
			Console.WriteLine ("");
			Console.WriteLine ("Checking calibrated value");
			Console.WriteLine ("");
			var expectedCalibratedValue = percentageValue;
      
			if (calibrationIsReversed)
				expectedCalibratedValue = ArduinoConvert.ReversePercentage (percentageValue);
      
			var calibratedValueIsWithinRange = IsWithinRange (expectedCalibratedValue, data ["C"], 8);
      
			Assert.IsTrue (calibratedValueIsWithinRange, "Invalid value for 'C' (calibrated value).");
      
			Console.WriteLine ("");
			Console.WriteLine ("Checking raw value");
      
			var expectedRawValue = ArduinoConvert.PercentageToAnalog (percentageValue);
      
			var rawValueIsWithinRange = IsWithinRange (expectedRawValue, data ["R"], 80);
      
			Assert.IsTrue (rawValueIsWithinRange, "Invalid value for 'R' (raw value).");
      
			Console.WriteLine ("");
			Console.WriteLine ("Finished calibration test");
			Console.WriteLine ("==============================");
			Console.WriteLine ("");
		}
		
		public Dictionary<string, int> ParseOutputLine(string outputLine)
		{
			var dictionary = new Dictionary<string, int> ();
		  
			if (IsValidOutputLine (outputLine)) {
				foreach (var pair in outputLine.Split(';')) {
					var parts = pair.Split (':');
  		    
					if (parts.Length == 2) {
						var key = parts [0];
						var value = 0;
						try {
							value = Convert.ToInt32 (parts [1]);
  		      
							dictionary [key] = value;
						} catch {
							Console.WriteLine ("Warning: Invalid key/value pair '" + pair + "'");
						}
					}
				}
			}
		  
			return dictionary;
		}
	}
}
