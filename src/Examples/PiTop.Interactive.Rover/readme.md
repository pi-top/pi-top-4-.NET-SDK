# PiTop.Interactive.Rover

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
using System.IO;

roverBody.TiltController.Pan = Angle.Zero;
roverBody.TiltController.Tilt = Angle.Zero;
ClassificationResult lastScanResult = null;
Image lastFrame = null;
var quadrant = new []{false,false,false,false};
var currentQuadrant = -1;

resourceScanner.CaptureFromCamera(roverBody.Camera);
resourceScanner.LoadModel(new DirectoryInfo("/home/pi/models/v1"));

```

The kernel gives you access to the rover via the `roverBody` and `roverBrain` variables.

Using the `roverBrain` you can define its behaviour, the `roverBrain` follows the classic model of the [intelligent agents](https://en.wikipedia.org/wiki/Intelligent_agent)
```csharp
#!rover
lastScanResult = null;
lastFrame = null;
quadrant = new []{false,false,false,false};
rover.AllLightsOff();

//use the Perceive step to read sensors and camera
roverBrain.Perceive = (rover, now, token) => {
    lastFrame = rover.Camera.GetFrame();
};

//use the Plan step tp formulate a plan. If PlanningResult.NoPlan is returned then the Act step will not be executed
roverBrain.Plan = (rover, now, token) => {
    if (quadrant.All()) {
        return PlanningResult.NoPlan;
    }

    if(currentQuadrant >= 0)
    {   
        lastScanResult = resourceScanner.AnalyseFrame(lastFrame)?.Classification;
        quadrant[currentQuadrant] = true;
        currentQuadrant++;
        
    }else{
        currentQuadrant = 0;
    }   

    return PlanningResult.NewPlan;
};

//perform actions 
roverBrain.Act = (rover, now, token) => { 

    if (lastScanResult != null)
    {
        rover.BlinkAllLights();
    }

    switch(currentQuadrant) {
        case 0:
            rover.TiltController.Pan = Angle.FromDegrees(90);
            rover.TiltController.Tilt = Angle.FromDegrees(90);
        break;
        case 1:
            rover.TiltController.Pan = Angle.FromDegrees(90);
            rover.TiltController.Tilt = Angle.FromDegrees(90);
        break;
        case 2:
            rover.TiltController.Pan = Angle.FromDegrees(90);
            rover.TiltController.Tilt = Angle.FromDegrees(90);
        break;
        case 3:
            rover.TiltController.Pan = Angle.FromDegrees(90);
            rover.TiltController.Tilt = Angle.FromDegrees(90);
        break;
    }  
};
```
