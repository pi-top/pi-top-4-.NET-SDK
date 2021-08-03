using System;
using System.Device.I2c;
using System.Reactive.Disposables;
using PiTop.Abstractions;
using PiTop.MakerArchitecture.Foundation;
using PiTop.MakerArchitecture.Foundation.Sensors;
using Pocket;
using UnitsNet;

namespace PiTop.MakerArchitecture.Expansion.Sensors
{

    public class UltrasonicSensorSMBus: UltrasonicSensor
    {
        private I2cDevice? _bus;
        private byte _configRegister;
        private byte _dataRegister;
        

        public UltrasonicSensorSMBus()
        {
           
            AddToDisposables(Disposable.Create(() =>
            {
                _bus?.WriteByte(_configRegister, 0x00);
            }));
        }

        /// <inheritdoc />
        protected override Length GetDistance()
        {
            using var operation = Logger.Log.OnEnterAndConfirmOnExit();
            try
            {
                if (_bus != null)
                {
                    var data = _bus.ReadWord(_dataRegister);
                    operation.Info($"data  : {data}");
                    operation.Succeed();
                    return Length.FromCentimeters(data);
                   
                }
                operation.Fail(message:"no bus available.");
                return Length.Zero;
            }
            catch (Exception e)
            {
                operation.Fail(e);
                throw new SensorReadException($"Could not get reading from the sensor on port {Port.Name}",e);
            }
        }

        /// <inheritdoc />
        protected override void OnConnection()
        {
            byte data;
            switch (Port!.Name)
            {
                case "A1":
                    _configRegister = 0x0C;
                    _dataRegister = 0x0E;
                    data = 0xA1;
                    break;

                case "A3":
                    _configRegister = 0x0D;
                    _dataRegister = 0x0F;
                    data = 0xA3;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Port), Port.Name, "This sensor can only work on ports: A1 and A3");
            }

            Logger.Log.Info($"Using Data Register 0x{_dataRegister:X2} and Config Register 0x{_configRegister:X2}");
            
            _bus = Port.I2CDevice;
            _bus.WriteByte(_configRegister, data);
            var test = _bus.ReadByte(_configRegister);
            Logger.Log.Info($"Configured writing  0x{test:X2}");
        }
    }
}