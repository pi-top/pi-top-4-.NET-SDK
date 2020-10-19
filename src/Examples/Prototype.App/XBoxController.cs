using PiTop.MakerArchitecture.Expansion;

using System;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using PiTop.Algorithms;

namespace Prototype
{
    class XBoxController
    {
        private readonly FileStream _fs;

        public IConnectableObservable<JoystickEvent> Events { get; }

        public XBoxController(string device = "/dev/input/js0")
        {
            try
            {
                _fs = File.OpenRead(device);
            }
            catch (FileNotFoundException ex)
            {
                throw new ControllerNotConnectedExeption("Please connect the controller", ex);
            }

            Events = Observable.Create<JoystickEvent>(async (subject, token) =>
              {
                  try
                  {
                      while (true)
                      {
                          if (token.IsCancellationRequested)
                          {
                              subject.OnCompleted();
                              return;
                          }

                          var data = new byte[8];
                          var n = await _fs.ReadAsync(data, token);
                          if (!token.IsCancellationRequested)
                          {
                              if (n != data.Length)
                              {
                                  throw new IOException($"expected 8 bytes, got {n}");
                              }
                              var value = BitConverter.ToInt16(data, 4);
                              var timestamp = BitConverter.ToUInt32(data);

                              if (data[6] == 1)
                              {
                                  subject.OnNext(new ButtonEvent(timestamp, (Button)data[7], value != 0));
                              }
                              else if (data[6] == 2)
                              {
                                  subject.OnNext(new AxisEvent(timestamp, (Axis)data[7], value));
                              }
                          }
                      }
                  }

                  catch (Exception ex)
                  {
                      subject.OnError(ex);
                  }
              }).Publish();

            LeftStick = new Stick(Events, Axis.LeftStickX, Axis.LeftStickY);
            RightStick = new Stick(Events, Axis.RightStickX, Axis.RightStickY);

            Events.Connect();
        }

        /// <summary>
        /// Represents current stick state. Up/Left positive, deazone of 1% around the middle
        /// </summary>
        public class Stick
        {
            internal Stick(IObservable<JoystickEvent> events, Axis xAxis, Axis yAxis)
            {
                events.OfType<AxisEvent>().Where(e => e.Axis == xAxis)
                    .Subscribe(e => X = -e.Position.Interpolate(-1, 1).WithDeadZone(-1, 1, .01));
                events.OfType<AxisEvent>().Where(e => e.Axis == yAxis)
                    .Subscribe(e => Y = -e.Position.Interpolate(-1, 1).WithDeadZone(-1, 1, .01));
            }

            public double X { get; private set; }
            public double Y { get; private set; }
        }

        public Stick LeftStick { get; }
        public Stick RightStick { get; }
    }
}

