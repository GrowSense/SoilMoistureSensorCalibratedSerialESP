using System;
using System.IO;

namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
  public class GrowSenseMqttHardwareTestHelper : GrowSenseHardwareTestHelper
  {
    public MqttTestHelper Mqtt;

    public string ConnectedToMqttText = "Subscribed to MQTT topics";
        
    public bool RequireMqttConnection = false;

    public GrowSenseMqttHardwareTestHelper ()
    {
    }

    public void EnableMqtt ()
    {
      RequireMqttConnection = true;

      Mqtt = new MqttTestHelper (this);
      Mqtt.Start ();
    }

    public void EnableMqtt (string deviceName)
    {
      Mqtt = new MqttTestHelper (this, deviceName);
      Mqtt.Start ();
    }

    public override void PrepareDeviceForTest (bool consoleWriteDeviceOutput)
    {
      if (RequireMqttConnection) {
        Console.WriteLine ("MQTT connection required for test. Setting up WiFi and MQTT.");
        TextToWaitForBeforeTest = ConnectedToMqttText;

        SetWiFiMqttSettings ();
      } else {
        Console.WriteLine ("MQTT connection not required for test. Disabling WiFi and MQTT.");
        SendDeviceCommand ("!");
      }

      base.PrepareDeviceForTest (consoleWriteDeviceOutput);

    }

    public void SetWiFiMqttSettings ()
    {
      var wiFiName = File.ReadAllText (Path.GetFullPath ("../../../../wifi-name.security")).Trim ();
      var wiFiPassword = File.ReadAllText (Path.GetFullPath ("../../../../wifi-password.security")).Trim ();

      var mqttHost = File.ReadAllText (Path.GetFullPath ("../../../../mqtt-host.security")).Trim ();
      var mqttUsername = File.ReadAllText (Path.GetFullPath ("../../../../mqtt-username.security")).Trim ();
      var mqttPassword = File.ReadAllText (Path.GetFullPath ("../../../../mqtt-password.security")).Trim ();
      var mqttPort = File.ReadAllText (Path.GetFullPath ("../../../../mqtt-port.security")).Trim ();

      var deviceName = File.ReadAllText (Path.GetFullPath ("../../../../device-name.security")).Trim ();

      SendDeviceCommands ("WN:" + wiFiName,
        "WPass:" + wiFiPassword,
        "MHost:" + mqttHost,
        "MUser:" + mqttUsername,
        "MPass:" + mqttPassword,
        "MPort:" + mqttPort,
        "Name:" + deviceName
      );
    }

  }
}
