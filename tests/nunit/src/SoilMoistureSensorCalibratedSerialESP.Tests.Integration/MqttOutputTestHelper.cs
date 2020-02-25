using System;
using NUnit.Framework;

namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
  public class MqttOutputTestHelper : GrowSenseMqttHardwareTestHelper
  {
    public int ReadInterval = 1;
    public int SimulatedSoilMoistureSensorValue = -1;

    public void TestMqttOutput ()
    {
      WriteTitleText ("Starting MQTT output test");

      Console.WriteLine ("Read interval: " + ReadInterval);
      Console.WriteLine ("Soil moisture sensor value: " + SimulatedSoilMoistureSensorValue);

      RequireMqttConnection = true;

      ConnectDevices ();

      EnableMqtt ();

      SetDeviceReadInterval (ReadInterval);

      SimulateSoilMoisture (SimulatedSoilMoistureSensorValue);

      Mqtt.Data.Clear ();

      Console.WriteLine ("Waiting for MQTT data...");

      var numberOfEntriesToWaitFor = 5; // TODO: See if this can be reduced

      Mqtt.WaitForData (numberOfEntriesToWaitFor);

      Assert.AreEqual (numberOfEntriesToWaitFor, Mqtt.Data.Count, "Incorrect number of entries returned.");

      var latestEntry = Mqtt.Data [Mqtt.Data.Count - 1];

      Assert.IsNotNull (latestEntry, "The latest MQTT entry is null.");

      Mqtt.ConsoleWriteDataEntry (latestEntry);

      Assert.IsTrue (latestEntry.ContainsKey ("C"), "The latest MQTT entry doesn't contain a 'C' key/value.");

      var valueString = latestEntry ["C"];

      Console.WriteLine ("Calibrated value string: \"" + valueString + "\"");

      var containsWhitespace = valueString.Trim ().Length != valueString.Length;

      Assert.IsFalse (containsWhitespace, "The calibrated value contains whitespace: \"" + valueString + "\"");

      var isDecimal = valueString.Contains (".");

      Assert.IsFalse (isDecimal, "The calibrated value contains a decimal point when it shouldn't.");

      Console.WriteLine ("MQTT calibrated value string: " + valueString);

      var value = Convert.ToInt32 (valueString);

      AssertIsWithinRange ("MQTT calibrated soil moisture", ApplyOffset (SimulatedSoilMoistureSensorValue, ExpectedCalibratedValueOffset), value, CalibratedValueMarginOfError);
    }
  }
}
