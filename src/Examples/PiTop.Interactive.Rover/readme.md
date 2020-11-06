# PiTop.Interactive.Rover

![pi-top Rover](./../../../resources/pi-top-rover-green-mat.gif)

This app will bootstrap a rover app that embeds .NET Interactive. The kernels are exposed both as stdio and http interface.

## Setting up your pi-top

To run the app the configuration of the rover is

*  right encoder motor on port `M1`
*  left encoder motor on port `M2`
*  tilt servo on port `S2`
*  pan servo on port `S1`
*  front green leds on ports `D3` and `D4`
*  back red leds on port `D0` and `D5`
*  front ultrasound on port `D7`
*  back ultrasound on port `D6`
*  sound sensor on port `A3`
*  camera on plate usb port

## Streaming Camera dependencies

The rover app uses a special camera that streams the video over HTTP. This has a dependency on the [mjpg-streamer](https://github.com/jacksonliam/mjpg-streamer) project. Follow the install instructions before running.

## Usin lobe

The rover uses [lobe](https://lobe.ai/) and the [lobe .NET api](https://github.com/lobe/lobe.NET) to classify frames. In our example it is trained to detect pokemon energy cards. Get your copy of the app
and create your own model.

You can export the model or use directly the app http endpoint.

To use the app directly click on export and select the http api option, that will provide you with the url you need. 

In your code then do
```csharp
// this way you use the lobe app directly
resourceScanner.UseUri(new Uri("the address the lobe app is proiding"));
```

To use onnx models directly on pi-top jsut follow the instructions to export a tensorflow model and convert it to onnx, then copy the `signature.json` and `saved_model.onnx` to your pi-top (here we assume the destination apth to be `/home/pi/models/v1`)
```csharp
resourceScanner.LoadModel(new DirectoryInfo("/home/pi/models/v1"));
```

## Runnign and connecting to the kernel

To start the app execute
```sh
dotnet PiTop.Interactive.Rover.dll --http-port 1024
```

You can now connect from a notebook by evaluation the following code
```csharp
#!connect signalR --kernel-name rover --hub-url http://localhost:1024/kenrelhub
```

At this point you can submit code to the rover by using the `#!rover` kernel, for example

```csharp
#!rover
//initialise the rover and the state
Microsoft.DotNet.Interactive.Formatting.Formatter.ListExpansionLimit = 25;
var scannedSectors = CameraSector
.CreateSectors(5,5, Angle.FromDegrees(-60),Angle.FromDegrees(60),Angle.FromDegrees(-15),Angle.FromDegrees(30))
.Distinct()
.ToArray();
CameraSector currentSector = null;

resourceScanner.CaptureFromCamera(roverBody.Camera);

// this way you load an exported onnx model
resourceScanner.LoadModel(new DirectoryInfo("/home/pi/models/v1"));


void ResetSectors(IEnumerable<CameraSector> sectors){
    foreach (var sector in sectors){
        sector.Reset();
    }
}

bool IsResource(ClassificationResults result){
    return result.Prediction.Label.Contains("no energy") != true;
}

bool AllSectorScanned(IEnumerable<CameraSector> sectors){
    return scannedSectors.All(v => v.Marked);
}

bool FoundResources(IEnumerable<CameraSector> sectors, int requiredCount){
    return sectors.Where(s => s.ClassificationResults!= null && IsResource(s.ClassificationResults))
    .Select(s => s.ClassificationResults.Prediction.Label)
    .Distinct()
    .Count() >= requiredCount;
}

```

The kernel gives you access to the rover via the `roverBody` and `roverBrain` variables.

Using the `roverBrain` you can define its behaviour, the `roverBrain` follows the classic model of the [intelligent agents](https://en.wikipedia.org/wiki/Intelligent_agent)
```csharp
#!rover
ResetSectors(scannedSectors);
currentSector = null;
roverBody.AllLightsOff();
roverBody.TiltController.Reset();

//use the Perceive step to read sensors and camera
roverBrain.Perceive = (rover, now, token) => {
    if(currentSector != null && currentSector.CapturedFrame == null){
        Task.Delay(500).Wait();
        currentSector.CapturedFrame = roverBody.Camera.GetFrame().Focus();
    }
};

//use the Plan step tp formulate a plan. If PlanningResult.NoPlan is returned then the Act step will not be executed
roverBrain.Plan = (rover, now, token) => {

    if (AllSectorScanned(scannedSectors) || FoundResources(scannedSectors, 4)){
        roverBody.AllLightsOff();
        return PlanningResult.NoPlan;
    }
   
    if(currentSector != null) {
        if(currentSector.CapturedFrame != null) {
            currentSector.ClassificationResults = resourceScanner.AnalyseFrame(currentSector.CapturedFrame);
            if(currentSector.ClassificationResults!= null && IsResource(currentSector.ClassificationResults)) {
                rover.BlinkAllLights();
            }
            else {
                roverBody.AllLightsOff();
            }       
        }  

        currentSector.Marked = true;
    }
    
    currentSector = scannedSectors.FirstOrDefault(s => s.Marked == false);

    if(currentSector != null)
    {
        return PlanningResult.NewPlan;
    }

    roverBody.AllLightsOff();
    return PlanningResult.NoPlan;
};

//perform actions 
roverBrain.Act = (rover, now, token) => { 
    if(currentSector != null){
        rover.TiltController.GoToSector(currentSector);
    }   
};
```
