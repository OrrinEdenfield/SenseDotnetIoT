// This is built from 2 key bits of code:
//  Azure IoT Samples CSharp Iot Hub Quickstart:  https://github.com/Azure-Samples/azure-iot-samples-csharp/tree/main/iot-hub/Quickstarts/SimulatedDevice
//  SenseHatNet:  https://github.com/johannesegger/SenseHatNet

using Microsoft.Azure.Devices.Client;
using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Sense.RTIMU;

namespace SenseDotnetIoT
{
    internal class Program
    {
        private static DeviceClient s_deviceClient;
        private static readonly TransportType s_transportType = TransportType.Mqtt;

        // The device connection string to authenticate the device with your IoT hub:
        private static string s_connectionString = "<INSERT IOT HUB DEVICE CONNECTION STRING HERE>";

        private static async Task Main(string[] args)
        {
            Console.WriteLine("Starting Azure IoT device SenseHat ");

            // This sample accepts the device connection string as a parameter, if present
            ValidateConnectionString(args);

            // Connect to the IoT hub using the MQTT protocol
            s_deviceClient = DeviceClient.CreateFromConnectionString(s_connectionString, s_transportType);

            // Set up a condition to quit the sample
            Console.WriteLine("Press control-C to exit.");
            using var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                eventArgs.Cancel = true;
                cts.Cancel();
                Console.WriteLine("Exiting...");
            };

            // Run the telemetry loop
            await SendDeviceToCloudMessagesAsync(cts.Token);

            await s_deviceClient.CloseAsync();

            s_deviceClient.Dispose();
            Console.WriteLine("Device simulator finished.");
        }

        private static void ValidateConnectionString(string[] args)
        {
            if (args.Any())
            {
                try
                {
                    var cs = IotHubConnectionStringBuilder.Create(args[0]);
                    s_connectionString = cs.ToString();
                }
                catch (Exception)
                {
                    Console.WriteLine($"Error: Unrecognizable parameter '{args[0]}' as connection string.");
                    Environment.Exit(1);
                }
            }
            else
            {
                try
                {
                    _ = IotHubConnectionStringBuilder.Create(s_connectionString);
                }
                catch (Exception)
                {
                    Console.WriteLine("This sample needs a device connection string to run. Program.cs can be edited to specify it, or it can be included on the command-line as the only parameter.");
                    Environment.Exit(1);
                }
            }
        }

        // Async method to send simulated telemetry
        private static async Task SendDeviceToCloudMessagesAsync(CancellationToken ct)
        {
 
            var rand = new Random();

            while (!ct.IsCancellationRequested)
            {
 
                var settings = RTIMUSettings.CreateDefault();
                var humidity = settings.CreateHumidity();
                var humidityReadResult = humidity.Read();

                DateTime now = DateTime.Now;

                // Create JSON message
                string messageBody = JsonSerializer.Serialize(
                    new
                    {
                        timestamp = now,
                        deviceid = "IoTPi3Sense",
                        temperature = humidityReadResult.Temperatur,
                        humidity = humidityReadResult.Humidity,
                    });
                using var message = new Message(Encoding.ASCII.GetBytes(messageBody))
                {
                    ContentType = "application/json",
                    ContentEncoding = "utf-8",
                };

 
                // Send the telemetry message
                await s_deviceClient.SendEventAsync(message);
                Console.WriteLine($"{DateTime.Now} > Sending message: {messageBody}");

                await Task.Delay(1000);
            }
        }
    }
}