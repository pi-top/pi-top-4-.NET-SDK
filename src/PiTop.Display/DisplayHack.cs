using System;
using System.IO;
using PiTop.Abstractions;
using PiTop.OledDevice;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Filters;

namespace PiTop
{
    public class DisplayHackConnectionSettings
    { 
        public DisplayHackConnectionSettings(string imagePath, string exitBreadcrumb)
        {
            if (string.IsNullOrWhiteSpace(exitBreadcrumb))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(exitBreadcrumb));
            }

            if (string.IsNullOrWhiteSpace(imagePath))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(imagePath));
            }

            ExitBreadcrumb = exitBreadcrumb;
            ImagePath = imagePath;
        }

        public string ExitBreadcrumb { get; }
        public string ImagePath { get; }

        public static DisplayHackConnectionSettings Default => new DisplayHackConnectionSettings(@"/tmp/oled_image.png", @"/tmp/oled_exit");
    }
    public class DisplayHack : Display
    {
        private readonly string _exitBreadcrumb;
        private readonly string _imagePath;
        private readonly string _hackCode;

        public DisplayHack(DisplayHackConnectionSettings settings) : base(Sh1106.Width, Sh1106.Height)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            _exitBreadcrumb = settings.ExitBreadcrumb;
            _imagePath = settings.ImagePath;

            _hackCode = $@"
#!/usr/bin/python3
import os
from luma.core.interface.serial import spi
from luma.oled.device import sh1106
from PIL import Image
from time import sleep
import RPi.GPIO
RPi.GPIO.setwarnings(False)
image_path = ""{_imagePath}""
exit_breadcrumb = ""{_exitBreadcrumb}""
os.system(""sudo systemctl stop pt-sys-oled"")
device = sh1106(
    spi(
        port=1,
        device=0,
        bus_speed_hz=8000000,
        cs_high=False,
        transfer_size=4096,
        gpio_DC=17,
        gpio_RST=None,
        gpio=None,
    )
)
while True:
    if os.path.isfile(image_path):
        im = Image.open(image_path)
        im = im.resize(device.size)
        device.display(im.convert(device.mode))
        os.remove(image_path)
    if os.path.isfile(exit_breadcrumb):
        os.remove(exit_breadcrumb)
        break
    sleep(0.2)
os.system(""sudo systemctl start pt-sys-oled"")";

            if (File.Exists(_exitBreadcrumb))
            {
                File.Delete(_exitBreadcrumb);
            }

            AcquireDevice();

            RegisterForDisposal(ReleaseDevice);
        }

        private void ReleaseDevice()
        {
            File.WriteAllText(_exitBreadcrumb, "");
        }

        private void AcquireDevice()
        {
            var dst = InternalBitmap.Clone(i =>
            {
                i.ApplyProcessor(new BlackWhiteProcessor());
            });



            dst.Save(_imagePath);
        }

        public override void Show()
        {

        }

        public override void Hide()
        {

        }

        protected override void CommitBuffer()
        {
            var dst = InternalBitmap.Clone(i =>
            {
                i.ApplyProcessor(new BlackWhiteProcessor());
            });

            dst.Save(_imagePath);
        }
    }
}