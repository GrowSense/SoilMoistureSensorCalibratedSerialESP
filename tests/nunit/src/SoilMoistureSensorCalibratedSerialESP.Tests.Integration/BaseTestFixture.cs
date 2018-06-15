using System;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
	public class BaseTestFixture
	{
		public BaseTestFixture()
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
			var devicePort = Environment.GetEnvironmentVariable("MONITOR_ESP_PORT");

			if (String.IsNullOrEmpty(devicePort))
				devicePort = "/dev/ttyUSB2";

			Console.WriteLine("Device port: " + devicePort);

			return devicePort;
		}

		public string GetSimulatorPort()
		{
			var simulatorPort = Environment.GetEnvironmentVariable("MONITOR_ESP_SIMULATOR_PORT");

			if (String.IsNullOrEmpty(simulatorPort))
				simulatorPort = "/dev/ttyUSB3";

			Console.WriteLine("Simulator port: " + simulatorPort);

			return simulatorPort;
		}

		public int GetDeviceSerialBaudRate()
		{
			var baudRateString = Environment.GetEnvironmentVariable("MONITOR_ESP_BAUD_RATE");

			var baudRate = 0;

			if (String.IsNullOrEmpty(baudRateString))
				baudRate = 115200;
			else
				baudRate = Convert.ToInt32(baudRateString);

			Console.WriteLine("Device baud rate: " + baudRate);

			return baudRate;
		}

		public int GetSimulatorSerialBaudRate()
		{
			var baudRateString = Environment.GetEnvironmentVariable("MONITOR_ESP_SIMULATOR_BAUD_RATE");

			var baudRate = 0;

			if (String.IsNullOrEmpty(baudRateString))
				baudRate = 9600;
			else
				baudRate = Convert.ToInt32(baudRateString);

			Console.WriteLine("Simulator baud rate: " + baudRate);

			return baudRate;
		}
	}
}
