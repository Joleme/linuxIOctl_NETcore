using ioctlWrapper;
using lghtIOWrapper;
using System;

//using lghtIOWrapper;

namespace testApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("(dir) << _IOC_DIRSHIFT: {0:D}", (IOCTL_H._IOC_READ | IOCTL_H._IOC_WRITE) << IOCTL_H._IOC_DIRSHIFT);
            Console.WriteLine("(uint)type) << _IOC_TYPESHIFT: {0:D}", 'q' << IOCTL_H._IOC_TYPESHIFT);
            Console.WriteLine("((uint)nr) << _IOC_NRSHIFT: {0:D}", 1 << IOCTL_H._IOC_NRSHIFT);
            Console.WriteLine("((uint)size) << _IOC_SIZESHIFT: {0:D}", IOCTL_H._IOC_TYPECHECK(typeof(lghtIO_arg_t)) << IOCTL_H._IOC_SIZESHIFT);

            Console.WriteLine("getLghtByIdx: {0:D}", lghtIO.getLghtByIdx());
            Console.WriteLine("setLghtByIdx: {0:D}", lghtIO.setLghtByIdx());
            Console.WriteLine("resetLghtByIdx: {0:D}", lghtIO.resetLghtByIdx());
            //string lghtIOdev = "/dev/lghtIO";
            //using (var lght = new UnixDevice(lghtIOdev, Libc.OpenFlags.O_RDWR))
            //{
            //    byte[] arg = new byte[1] { 0 };
            //    lghtIO_arg_t rq = new lghtIO_arg_t();
            //    lghtIO_arg_t answ = new lghtIO_arg_t();
            //    rq.idx = 13;
            //    rq.clrT = lghtColor.NoneColor;
            //    lght.IoCtl(lghtIO.getLghtByIdx(), ref rq, out answ);
            //    Console.WriteLine("color: {0:D}", (int)answ.clrT);
            //    lght.IoCtl(lghtIO.setLghtByIdx(), ref rq, out answ);
            //    Console.WriteLine("color: {0:D}", (int)answ.clrT);
            //    rq.clrT = lghtColor.Grn;
            //    lght.IoCtl(lghtIO.getLghtByIdx(), ref rq, out answ);
            //    Console.WriteLine("color: {0:D}", (int)answ.clrT);
            //    lght.IoCtl(lghtIO.setLghtByIdx(), ref rq, out answ);
            //    Console.WriteLine("color: {0:D}", (int)answ.clrT);
            //}
        }
    }
}