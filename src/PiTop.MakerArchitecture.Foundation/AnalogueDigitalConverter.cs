using System;
using System.Device.I2c;
using PiTop.Abstractions;

namespace PiTop.MakerArchitecture.Foundation
{
    internal class AnalogueDigitalConverter
    {
        private readonly I2cDevice _device;
        private readonly int _channel;

        public AnalogueDigitalConverter(I2cDevice device, int channel)
        {
            _device = device ?? throw new ArgumentNullException(nameof(device));
            _channel = channel;
        }

        public int ReadRaw()
        {
            var registerAddress = (byte)(0x10 + _channel);
            return ReadRegister(registerAddress, _device);
        }

        public int ReadVoltage()
        {
            var registerAddress = (byte)(0x20 + _channel);
            return ReadRegister(registerAddress, _device);
        }

        public int ReadPeak()
        {
            var registerAddress = (byte)(0x18 + _channel);
            return ReadRegister(registerAddress, _device);
        }

        public int Read()
        {
            var registerAddress = (byte)(0x30 + _channel);
            return ReadRegister(registerAddress, _device);
        }

        public double ReadSample(int numberOfSamples = 1, int delayBetweenSamplesMs = 50, bool peakDetection = false)
        {
            if (numberOfSamples <= 0)
            {
                return 0;
            }
            var value = 0.0;
            var delay = Math.Abs(delayBetweenSamplesMs);
            for (var i = 0; i < numberOfSamples; i++)
            {
                if (peakDetection)
                {
                    value += ReadPeak();
                }
                else
                {
                    value += Read();
                }
                System.Threading.Thread.Sleep(delay);
            }
            return value / numberOfSamples;
        }



        private int ReadRegister(byte registerAddress, I2cDevice device)
        {
            return device.ReadWord(registerAddress);
        }
    }
}