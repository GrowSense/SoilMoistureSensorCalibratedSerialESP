using System;
using NUnit.Framework;
using System.IO;
using System.Net.NetworkInformation;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
    public class MqttTestHelper
    {
        public string DeviceName;

        public List<Dictionary<string, string>> Data = new List<Dictionary<string, string>> ();
        public Dictionary<string, string> DataEntry = new Dictionary<string, string> ();

        public MqttClient Client;

        public GreenSenseHardwareTestHelper Hardware;

        public string ExistingStatusMessage;

        public int TimeoutWaitingForMqttData = 20 * 1000;

        public TimeoutHelper Timeout = new TimeoutHelper ();

        public MqttTestHelper (GreenSenseHardwareTestHelper hardware, string deviceName)
        {
            DeviceName = deviceName;
            Hardware = hardware;
        }

        public MqttTestHelper (GreenSenseHardwareTestHelper hardware)
        {
            Hardware = hardware;
        }

        public MqttTestHelper ()
        {
        }

        public void Start ()
        {
            Console.WriteLine ("");
            Console.WriteLine ("Starting MQTT test");
            Console.WriteLine ("");

            DeviceName = GetSecurityValue ("mqtt-device-name", "");
            var host = GetSecurityValue ("mqtt-host", "MQTT_HOST");
            var user = GetSecurityValue ("mqtt-username", "MQTT_USERNAME");
            var pass = GetSecurityValue ("mqtt-password", "MQTT_PASSWORD");

            Assert.IsNotNullOrEmpty (DeviceName, "Device name is not set.");
            Assert.IsNotNullOrEmpty (host, "MQTT_HOST environment variable is not set.");
            Assert.IsNotNullOrEmpty (user, "MQTT_USERNAME environment variable is not set.");
            Assert.IsNotNullOrEmpty (pass, "MQTT_PASSWORD environment variable is not set.");

            Console.WriteLine ("Device name: " + DeviceName);
            Console.WriteLine ("Host: " + host);
            Console.WriteLine ("Username: " + user);

            Client = new MqttClient (host);

            var clientId = Guid.NewGuid ().ToString ().Substring (0, 10);

            Client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            Client.Connect (clientId, user, pass);

            Client.Subscribe (new string[] { "/" + DeviceName + "/#" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }

        public string GetSecurityValue (string key, string environmentVariable)
        {
            //Console.WriteLine ("Retrieving security value: " + key);

            var value = String.Empty;

            if (!String.IsNullOrEmpty (environmentVariable))
                value = Environment.GetEnvironmentVariable (environmentVariable);

            if (String.IsNullOrEmpty (value)) {
                var projectDirectory = Path.GetFullPath ("../../../..");

                value = File.ReadAllText (Path.Combine (projectDirectory, key + ".security")).Trim ();
            }

            return value;
        }

        public void End ()
        {
            PublishSuccess ();
            Thread.Sleep (2000);
            Client.Disconnect ();

            Console.WriteLine ("");
            Console.WriteLine ("End of MQTT test");
            Console.WriteLine ("");
        }

        public void WaitForAccess ()
        {
            Console.WriteLine ("Waiting for access (ie. ensuring another test isn't already running)");

            var hasAccess = false;

            var maxWaitTime = new TimeSpan (
                                  0,
                                  0, // minutes
                                  10);
            var startWaitTime = DateTime.Now;

            while (!hasAccess) {
                WaitForData (1);

                var currentStatus = ExistingStatusMessage;
                var testIsReady = (currentStatus != "Testing");
                Console.WriteLine ("Test is ready: " + testIsReady);

                var waitedLongEnough = DateTime.Now.Subtract (startWaitTime) > maxWaitTime;
                Console.WriteLine ("Waited long enough: " + waitedLongEnough);

                if (testIsReady || waitedLongEnough) {
                    Console.WriteLine ("Access gained");
                    hasAccess = true;
                    break;
                }

                Console.Write (".");
                Thread.Sleep (10);
            }
        }

        public void WaitForData (int numberOfEntries)
        {
            Console.WriteLine ("Waiting for " + numberOfEntries + " of data entries...");
            ResetData ();
            Timeout.Start ();
            while (Data.Count < numberOfEntries) {
                Timeout.Check (TimeoutWaitingForMqttData, "Timed out waiting for MQTT data.");
            }
        }

        public double WaitUntilData (int numberOfEntries)
        {
            Console.WriteLine ("Waiting until data is detected...");
            ResetData ();
            var startTime = DateTime.Now;
            Timeout.Start ();
            while (Data.Count < numberOfEntries) {
                Timeout.Check (TimeoutWaitingForMqttData, "Timed out waiting for MQTT data.");
            }
            var totalTimeInSeconds = DateTime.Now.Subtract (startTime).TotalSeconds;
            return totalTimeInSeconds;
        }

        public void CheckDataEntryTimes (int expectedInterval)
        {
            Assert.IsTrue (Data.Count >= 2, "More data entries are needed");

            var secondLastTime = DateTime.Parse (Data [Data.Count - 2] ["Time"]);
            var lastTime = DateTime.Parse (Data [Data.Count - 1] ["Time"]);

            Console.WriteLine (secondLastTime.ToString ());
            Console.WriteLine (lastTime.ToString ());

            var timeSpan = lastTime.Subtract (secondLastTime);

            Console.WriteLine ("Time difference (seconds): " + timeSpan.TotalSeconds);

            Assert.AreEqual (expectedInterval, timeSpan.TotalSeconds, "Invalid time difference");
        }

        public void SendCommand (string key, int value)
        {
            SendCommand (key, value.ToString ());
        }

        public void SendCommand (string key, string value)
        {
            Console.WriteLine ("");
            Console.WriteLine ("Sending command...");
            Console.WriteLine ("Key: " + key);
            Console.WriteLine ("Value: " + value);
            var inTopic = "/" + DeviceName + "/" + key + "/in";

            Console.WriteLine ("Topic: " + inTopic);
            Client.Publish (inTopic, Encoding.UTF8.GetBytes (value.ToString ()));
            Console.WriteLine ("");

            Hardware.WaitForMessageReceived (key + value);
        }

        public void PublishSuccess ()
        {
            Console.WriteLine ("Publishing success");
            ClearErrorMessage ();
            PublishStatus (0, "Passed");
        }

        public void PublishError (string error)
        {
            Console.WriteLine ("Publishing error: " + error);
            var errorTopic = "/" + DeviceName + "/Error";
            Client.Publish (errorTopic, Encoding.UTF8.GetBytes (error),
                MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, // QoS level
                true);
            PublishStatus (1, "Failed");
        }

        public void ClearErrorMessage ()
        {
            var errorTopic = "/" + DeviceName + "/Error";
            Client.Publish (errorTopic, Encoding.UTF8.GetBytes (""));
        }

        public void PublishStatus (int status, string message)
        {
            PublishStatus (status);
            PublishStatusMessage (message);
        }

        public void PublishStatus (int status)
        {
            var statusTopic = "/" + DeviceName + "/Status";
            Client.Publish (
                statusTopic,
                Encoding.UTF8.GetBytes (status.ToString ()),
                MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, // QoS level
                true);
        }

        public void PublishStatusMessage (string message)
        {
            Console.WriteLine ("Publishing status message: " + message);
            var statusMessageTopic = "/" + DeviceName + "/StatusMessage";
            Client.Publish (statusMessageTopic, Encoding.UTF8.GetBytes (message),
                MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, // QoS level
                true);
        }

        public void client_MqttMsgPublishReceived (object sender, MqttMsgPublishEventArgs e)
        {
            var topic = e.Topic;

            var value = System.Text.Encoding.Default.GetString (e.Message);

            var key = GetTopicKey (topic);

            DataEntry [key] = value;

            if (key == "StatusMessage")
                ExistingStatusMessage = value;

            if (key == "Time" && !IsDuplicateEntry (DataEntry)) {
                Data.Add (DataEntry);
                DataEntry = new Dictionary<string, string> ();
            }
        }

        public bool IsDuplicateEntry (Dictionary<string, string> dataEntry)
        {
            foreach (var entry in Data) {
                if (entry ["Time"] == dataEntry ["Time"])
                    return true;
            }

            return false;
        }

        public void ConsoleWriteDataEntry (Dictionary<string, string> dataEntry)
        {
            Console.Write ("$ ");
            if (dataEntry != null) {
                foreach (var key in dataEntry.Keys) {
                    Console.Write (key + ":" + dataEntry [key] + ";");
                }
                Console.WriteLine (";");
            }
        }

        public string GetTopicKey (string topic)
        {
            var parts = topic.Split ('/');
            var key = parts [2];

            return key;
        }

        public void ResetData ()
        {
            Data = new List<Dictionary<string, string>> ();
        }
    }
}