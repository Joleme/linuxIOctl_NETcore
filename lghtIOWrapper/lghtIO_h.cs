using System.Runtime.InteropServices;
using ioctlWrapper;

namespace lghtIOWrapper
{
    //enum to describe possible colors of each traffic light
    public enum lghtColor { Red, Ylw, Grn, NoneColor = 987 };

    /*structure to align request from user and answer back*/
    [StructLayout(LayoutKind.Sequential)]
    public struct lghtIO_arg_t
    {
        public int idx;
        public lghtColor clrT;
    }
    public class lghtIO
    {
        public static int getLghtByIdx() => IOCTL_H._IOWR('q', 1, typeof(lghtIO_arg_t));
        public static int resetLghtByIdx() => IOCTL_H._IOWR('q', 2, typeof(lghtIO_arg_t));
        public static int setLghtByIdx() => IOCTL_H._IOWR('q', 3, typeof(lghtIO_arg_t));
    }
}