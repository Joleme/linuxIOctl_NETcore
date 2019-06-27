using System;
using System.Runtime.InteropServices;

namespace testApp
{
    public class UnixDevice : IDisposable
    {
        static System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
        int fd = -1;
        public UnixDevice(string name, Libc.OpenFlags flags)
        {

            fd = Libc.open(encoding.GetBytes(name + char.MinValue), flags);
            if (fd < 0)
                throw new InvalidOperationException("Couldn't open device: " + name);
        }

        public MemoryArea MMap(uint size, int offset)
        {
            IntPtr ptr = Libc.mmap(IntPtr.Zero, size, Libc.ProtectionFlags.PROT_READ | Libc.ProtectionFlags.PROT_WRITE, Libc.MMapFlags.MAP_SHARED, fd, offset);
            if ((int)ptr == -1)
                throw new InvalidOperationException("MMap operation failed");
            return new MemoryArea(ptr, size);
        }

        public void Write(byte[] data)
        {
            IntPtr pnt = IntPtr.Zero;
            bool hasError = false;
            Exception inner = null;
            try
            {
                int size = Marshal.SizeOf(data[0]) * data.Length;
                pnt = Marshal.AllocHGlobal(size);
                Marshal.Copy(data, 0, pnt, data.Length);
                int bytesWritten = Libc.write(fd, pnt, (uint)size);
                if (bytesWritten == -1)
                    hasError = true;
            }
            catch (Exception e)
            {
                hasError = true;
                inner = e;
            }
            finally
            {
                if (pnt != IntPtr.Zero)
                    Marshal.FreeHGlobal(pnt);
            }
            if (hasError)
            {
                if (inner != null)
                {
                    throw inner;
                }
                else
                {
                    throw new InvalidOperationException("Failed to write to Unix device");
                }
            }
        }

        public byte[] Read(int length)
        {

            byte[] reply = new byte[length];
            Exception inner = null;
            IntPtr pnt = IntPtr.Zero;
            int bytesRead = 0;
            bool hasError = false;
            try
            {
                pnt = Marshal.AllocHGlobal(Marshal.SizeOf(reply[0]) * length);
                bytesRead = Libc.read(fd, pnt, (uint)length);
                if (bytesRead == -1)
                {
                    hasError = true;
                    Marshal.FreeHGlobal(pnt);
                    pnt = IntPtr.Zero;
                }
                else
                {
                    if (bytesRead != length)
                        reply = new byte[bytesRead];
                    Marshal.Copy(pnt, reply, 0, bytesRead);
                    Marshal.FreeHGlobal(pnt);
                    pnt = IntPtr.Zero;
                }

            }
            catch (Exception e)
            {
                hasError = true;
                inner = e;
            }
            finally
            {
                if (pnt != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pnt);
                }
            }
            if (hasError)
            {
                if (inner != null)
                {
                    throw inner;
                }
                else
                {
                    throw new InvalidOperationException("Failed to read from Unix device");
                }
            }
            return reply;
        }

        /// <summary>
        /// IO control command that copies to the IO output to a buffer
        /// </summary>
        /// <returns>Zero if successful</returns>
        /// <param name="cmd">IoCtl request code</param>
        /// <param name="input">Input arguments</param>
        /// <param name="output">Output buffer</param>
        /// <param name="ioOutputIndex">IO start index to copy to output buffer</param>
        public int IoCtl(int cmd, byte[] input, byte[] output, int indexToOutput)
        {
            IntPtr pnt = IntPtr.Zero;
            bool hasError = false;
            Exception inner = null;
            int result = -1;
            try
            {
                int size = Marshal.SizeOf(typeof(byte)) * input.Length;
                pnt = Marshal.AllocHGlobal(size);
                Marshal.Copy(input, 0, pnt, input.Length);
                result = Libc.ioctl(fd, cmd, pnt);
                if (result == -1)
                {
                    hasError = true;
                }
                else
                {
                    output = new byte[input.Length - indexToOutput];
                    Marshal.Copy(pnt, output, indexToOutput, input.Length - indexToOutput);
                }
            }
            catch (Exception e)
            {
                hasError = true;
                inner = e;
            }
            finally
            {
                if (pnt != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pnt);
                }
            }
            if (hasError)
            {
                if (inner != null)
                {
                    throw inner;
                }
                else
                {
                    throw new InvalidOperationException("Failed to excute IO control command");
                }
            }
            return result;
        }


        /// <summary>
        /// IO control command. Output is copied back to buffer
        /// </summary>
        /// <returns>Zero if successful</returns>
        /// <param name="requestCode">IoCtl request code</param>
        /// <param name="arguments">IO arguments</param>
        public int IoCtl(int requestCode, byte[] arguments)
        {
            IntPtr pnt = IntPtr.Zero;
            bool hasError = false;
            Exception inner = null;
            int result = -1;
            try
            {
                int size = Marshal.SizeOf(typeof(byte)) * arguments.Length;
                pnt = Marshal.AllocHGlobal(size);
                Marshal.Copy(arguments, 0, pnt, arguments.Length);
                result = Libc.ioctl(fd, requestCode, pnt);
                if (result == -1)
                {
                    hasError = true;
                }
                else
                {
                    Marshal.Copy(pnt, arguments, 0, arguments.Length);
                }
            }
            catch (Exception e)
            {
                hasError = true;
                inner = e;
            }
            finally
            {
                if (pnt != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pnt);
                }
            }
            if (hasError)
            {
                if (inner != null)
                {
                    throw inner;
                }
                else
                {
                    throw new InvalidOperationException("Failed to excute IO control command");
                }
            }
            return result;
        }
        /// <summary>
        /// IO control command. Output is copied back to buffer
        /// </summary>
        /// <returns>Zero if successful</returns>
        /// <param name="requestCode">IoCtl request code</param>
        /// <param name="arguments">IO arguments</param>
        public int IoCtl<T>(int requestCode, ref T rq, out T answer)
        {
            answer = default;
            IntPtr pnt = IntPtr.Zero;
            bool hasError = false;
            Exception inner = null;
            int result = -1;
            try
            {
                int len = Marshal.SizeOf(rq);
                //Console.WriteLine("size of rq: {0:D}", len);
                //byte[] arr = new byte[len];
                pnt = Marshal.AllocHGlobal(len);
                //Console.WriteLine("pointer: {0:D}", pnt);
                Marshal.StructureToPtr(rq, pnt, true);
                //Marshal.Copy(pnt, arr, 0, len);
                result = Libc.ioctl(fd, requestCode, pnt);
                if (result == -1)
                {
                    hasError = true;
                }
                else
                {
                    answer = (T)Marshal.PtrToStructure(pnt, typeof(T));
                    //Marshal.Copy(pnt, arr, 0, len);
                    //Marshal.StructureToPtr(rq, pnt, true);
                }
            }
            catch (Exception e)
            {
                hasError = true;
                inner = e;
            }
            finally
            {
                if (pnt != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pnt);
                }
            }
            if (hasError)
            {
                if (inner != null)
                {
                    throw inner;
                }
                else
                {
                    throw new InvalidOperationException("Failed to excute IO control command");
                }
            }
            return result;
        }


        public void Dispose()
        {
            Libc.close(fd);
            fd = -1;
        }

        ~UnixDevice()
        {
            if (fd >= 0)
            {
                Libc.close(fd);
            }
        }
    }
}
