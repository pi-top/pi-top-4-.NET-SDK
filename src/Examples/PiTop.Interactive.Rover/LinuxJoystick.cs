using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace PiTop.Interactive.Rover
{
    /// <summary>
    /// See https://www.kernel.org/doc/Documentation/input/joystick-api.txt
    /// </summary>
    public class LinuxJoystick : IDisposable
    {
        private int _fd = -1;

        public string Name { get; }
        public int NumAxes { get; }

        public LinuxJoystick(string device = "/dev/input/js0")
        {
            _fd = Interop.open(device, Interop.FileOpenFlags.O_RDONLY);
            if (_fd < 0)
            {
                throw new IOException($"Could not open {device} for reading");
            }
            Name = ReadName();
            NumAxes = GetAxisCount();
        }

        public jsevent ReadEvent()
        {
            var data = new jsevent();
            var res = Interop.read(_fd, ref data, 8);
            if (res < 0)
            {
                throw new IOException($"[errno {Marshal.GetLastWin32Error()}] Error reading from device: {res}");
            }
            return data;
        }

        private string ReadName()
        {
            var name = new StringBuilder(128);

            if (Interop.ioctl(_fd, Interop.JSIOCGNAME((uint)name.Capacity), name) < 0)
                return $"Unknown";

            return name.ToString();
        }

        private int GetAxisCount()
        {
            byte axiscount = 0;
            if (Interop.ioctl(_fd, Interop.JSIOCGAXES, ref axiscount) < 0)
            {
                return -1;
            }
            return axiscount;
        }

        public void Dispose()
        {
            Interop.close(_fd);
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 8)]
        public struct jsevent
        {
            public uint timestamp; /* event timestamp in milliseconds */
            public short value;    /* value */
            public byte type;      /* event type */
            public byte number;    /* axis/button number */
        };

        private static class Interop
        {
            [DllImport("libc", SetLastError = true)]
            internal extern static int ioctl(int fd, uint request, StringBuilder arg);
            [DllImport("libc", SetLastError = true)]
            internal extern static int ioctl(int fd, uint request, ref byte arg);

            /*                                                   function			        3rd arg  */
            internal const uint JSIOCGAXES = 0x80016a11;      /* get number of axes	  	    char	 */
            internal const uint JSIOCGBUTTONS = 0x80016a12;   /* get number of buttons	    char	 */
            internal static uint JSIOCGNAME(uint len)
            { return 0x80006a13 + (0x10000 * len); }          /* get identifier string	    char	 */

            [DllImport("libc", SetLastError = true)]
            internal static extern int open([MarshalAs(UnmanagedType.LPStr)] string pathname, FileOpenFlags flags);
            [DllImport("libc")]
            internal static extern int close(int fd);
            [DllImport("libc", SetLastError = true)]
            internal static extern int read(int fd, ref jsevent data, int count);

            internal enum FileOpenFlags
            {
                O_RDONLY = 0x00,
            }
        }
    }
}