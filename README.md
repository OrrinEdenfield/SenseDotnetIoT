# SenseDotnetIoT
Simple C# code to collect data readings from Raspberry Pi SenseHat temperature &amp; humidity sensors and send to Azure IoT Hub.

Two repos were helpful when I put this together:
  - Azure IoT Samples CSharp Iot Hub Quickstart:  https://github.com/Azure-Samples/azure-iot-samples-csharp/tree/main/iot-hub/Quickstarts/SimulatedDevice
  - SenseHatNet:  https://github.com/johannesegger/SenseHatNet

This could be expanded to include other sensors on the SenseHat device, I'm only using temperature and humidity. 

Instructions to get this working:

  1. [Install dotnet SDK](https://docs.microsoft.com/dotnet/iot/deployment)
  1. Download the repo or copy the code in the Program.cs and SenseDotnetIoT.csproj files.
  1. Navidate into the directory where Program.cs and SenseDotnetIoT.csproj files are stored on the device.
  3. Modify the connection string near the top of Program.cs to be the connection string to your Azure IoT device ([create one](https://docs.microsoft.com/azure/iot-edge/how-to-register-device) if needed). 
  4. Save the Program.cs file with the updated connection string.
  5. Run `dotnet build`
  6. Run `dotnet run`
  7. Monitor sensor data flowing into Azure IoT Hub.

I'm happy to take suggestions or improvements to this code. Thank you!
