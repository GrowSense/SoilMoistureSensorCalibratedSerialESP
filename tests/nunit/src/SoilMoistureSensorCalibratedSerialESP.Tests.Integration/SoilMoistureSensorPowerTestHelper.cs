using System;
using System.Threading;
using NUnit.Framework;
namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
	public class SoilMoistureSensorPowerTestHelper : GreenSenseHardwareTestHelper
	{
		public int ReadInterval = 1;

		public void TestSoilMoistureSensorPower()
		{
			WriteTitleText("Starting soil moisture sensor power test");

			EnableDevices();

			SetDeviceReadInterval(ReadInterval);

			var data = WaitForDataEntry();

			AssertDataValueEquals(data, "V", ReadInterval);

			var sensorDoesTurnOff = ReadInterval > DelayAfterTurningSoilMoistureSensorOn;

			if (sensorDoesTurnOff)
			{
				Console.WriteLine("The soil moisture sensor should turn off when not in use.");

				TestSoilMoistureSensorPowerTurnsOnAndOff();
			}
			else
			{
				Console.WriteLine("The soil moisture sensor should stay on permanently.");

				TestSoilMoistureSensorPowerStaysOn();
			}
		}

		public void TestSoilMoistureSensorPowerStaysOn()
		{
			WriteParagraphTitleText("Waiting until the soil moisture sensor is on before starting the test...");

			WaitUntilSoilMoistureSensorPowerPinIs(On);

			var durationInSeconds = ReadInterval * 5;

			WriteParagraphTitleText("Checking that soil moisture sensor power pin stays on...");

			AssertSoilMoistureSensorPowerPinForDuration(On, durationInSeconds);
		}

		public void TestSoilMoistureSensorPowerTurnsOnAndOff()
		{
			WriteParagraphTitleText("Waiting until the soil moisture sensor has turned on then off before starting the test...");

			WaitUntilSoilMoistureSensorPowerPinIs(On);
			WaitUntilSoilMoistureSensorPowerPinIs(Off);

			CheckSoilMoistureSensorOnDuration();
			CheckSoilMoistureSensorOffDuration();
		}

		public void CheckSoilMoistureSensorOnDuration()
		{
			WriteParagraphTitleText("Getting the total on time...");

			var totalOnTime = WaitWhileSoilMoistureSensorPowerPinIs(On);

			WriteParagraphTitleText("Checking the total on time is correct...");

			var expectedOnTime = DelayAfterTurningSoilMoistureSensorOn;

			AssertIsWithinRange("total on time", expectedOnTime, totalOnTime, TimeErrorMargin);
		}

		public void CheckSoilMoistureSensorOffDuration()
		{
			WriteParagraphTitleText("Getting the total off time...");

			var totalOffTime = WaitWhileSoilMoistureSensorPowerPinIs(Off);

			WriteParagraphTitleText("Checking the total off time is correct...");

			var expectedOffTime = ReadInterval - DelayAfterTurningSoilMoistureSensorOn;

			AssertIsWithinRange("total off time", expectedOffTime, totalOffTime, TimeErrorMargin);
		}
	}
}
