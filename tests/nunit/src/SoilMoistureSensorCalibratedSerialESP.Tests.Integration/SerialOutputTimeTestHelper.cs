using System;
using NUnit.Framework;

namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
  public class SerialOutputTimeTestHelper : GrowSenseMqttHardwareTestHelper
  {
    public int ReadInterval = 1;

    public void TestSerialOutputTime()
    {
      WriteTitleText("Starting serial output time test");

      Console.WriteLine("Read interval: " + ReadInterval);

      RequireMqttConnection = false;

      ConnectDevices();

      SetDeviceReadInterval(ReadInterval);

      ReadFromDeviceAndOutputToConsole();

      var expectedTimeBetweenDataLines = ReadInterval;

      var maxAttempts = 10;

      var didSucceed = false;

      var attemptsRequired = 0;

      for (int i = 1; i <= maxAttempts; i++)
      {
        Console.WriteLine("Attempt #: " + i);

        // Skip some data before checking the output time
        WaitForData(1);

        // Get the time until the next data line
        var secondsBetweenDataLines = WaitUntilDataLine();


        Console.WriteLine("Time between data lines: " + secondsBetweenDataLines + " seconds");

        var isWithinRange = IsWithinRange(expectedTimeBetweenDataLines, secondsBetweenDataLines, TimeErrorMargin);

        if (isWithinRange)
        {
          didSucceed = true;
          break;
        }
        //AssertIsWithinRange("serial output time", expectedTimeBetweenDataLines, secondsBetweenDataLines, TimeErrorMargin);

        attemptsRequired = i;
      }

      Console.WriteLine("Attempts required: " + attemptsRequired);
      Assert.IsTrue(didSucceed, "Serial output time didn't match expected.");
    }
  }
}