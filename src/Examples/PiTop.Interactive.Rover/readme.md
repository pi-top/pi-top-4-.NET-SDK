# PiTop.Interactive.Rover

This app will bootsrtap a rover app that embeds .NET Interactive. The kernels are exposed both as stdio and http interface.

## Setting up your pi-top

To run the app the configuration of the rover is

*  right encoder motor on port `M1`
*  left encoder motor on port `M2`
*  tilt servo on port `S2`
*  pan servo on port `S1`
*  front green leds on ports `D3` and `D4`
*  back red leds on port `D0` and ``D5`
*  front ultrasound on port `D7`
*  back ultrasound on port `D6`
*  sound sensor on port `A3`
*  camera on plate usb port

## Streaming Camera depedencies

The rover app uses a special camera that streams the video over http, this has a dependency on the  [mjpg-streamer](https://github.com/jacksonliam/mjpg-streamer) project. Follow the instruction to isntalle the tool before running the app.

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
resourceScanner.CaptureFromCamera(roverBody.Camera);
var sign = 1;

```

The kernel gives you access to the rover via the `roverBody` and `roverBrain` variables.

This snippet will acces the camera and display the frame
```
#!rover
using PiTop.Interactive.Rover.ImageProcessing;
roverBody.Camera.GetFrame().Preview()
```

Using the `roverBrain` you can define its behaviour
```csharp
#!rover
lastScanResoult = null;
rover.AllLightsOff();

//use the Perceive step to read sensors and camera
roverBrain.Perceive = (rover, now, token) => {

};

//use the Plan step tp formulate a plan. If PlanningResult.NoPlan is returned then the Act step will not be executed
roverBrain.Plan = (rover, now, token) => {
    sign *= -1;
    Task.Delay(2000).Wait();
    if(lastScanResoult != null && lastScanResoult.Label != "no energy") {
        rover.BlinkAllLights();
        return PlanningResult.NoPlan;
    }
    else {
        return PlanningResult.NewPlan;
    }
};

//perform actions 
roverBrain.Act = (rover, now, token) => { 
    rover.TiltController.Pan = Angle.FromDegrees(30* sign);
    lastScanResoult = resourceScanner.Scan()?.Classification;     
};
```
