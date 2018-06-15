using System;
using NUnit.Framework;
using duinocom;
using System.Threading;
using ArduinoSerialControllerClient;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
	[TestFixture(Category="Integration")]
	public class ReadIntervalCommandTestFixture : BaseTestFixture
	{
		[Test]
		public void Test_SetReadIntervalCommand_1Second()
		{
			using (var helper = new ReadIntervalCommandTestHelper())
			{
				helper.ReadInterval = 1;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestSetReadIntervalCommand();
			}
		}

		[Test]
		public void Test_SetReadIntervalCommand_5Seconds()
		{
			using (var helper = new ReadIntervalCommandTestHelper())
			{
				helper.ReadInterval = 5;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestSetReadIntervalCommand();
			}
		}
	}
}
