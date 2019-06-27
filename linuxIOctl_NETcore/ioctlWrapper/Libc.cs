using System;
using System.Runtime.InteropServices;

namespace testApp
{
    static public class Libc
    {
        public enum OpenFlags
        {
            O_RDONLY = 0x0000,      /* open for reading only */
            O_WRONLY = 0x0001,      /* open for writing only */
            O_RDWR = 0x0002,        /* open for reading and writing */
            O_ACCMODE = 0x0003      /* mask for above modes */
        }
        public enum ProtectionFlags
        {
            PROT_NONE = 0,
            PROT_READ = 1,
            PROT_WRITE = 2,
            PROT_EXEC = 4
        }
        public enum MMapFlags
        {
            MAP_FILE = 0,
            MAP_SHARED = 1,
            MAP_PRIVATE = 2,
            MAP_TYPE = 0xf,
            MAP_FIXED = 0x10,
            MAP_ANONYMOUS = 0x20,
            MAP_ANON = 0x20
        }

        [DllImport("libc.so.6")]
        public static extern int open(byte[] name, OpenFlags flags);

        [DllImport("libc.so.6")]
        public static extern IntPtr mmap(IntPtr addr, uint len, ProtectionFlags prot, MMapFlags flags, int fd, int offset);

        [DllImport("libc.so.6")]
        public static extern int write(int file, IntPtr buffer, uint count);

        [DllImport("libc.so.6")]
        public static extern int read(int file, IntPtr buffer, uint length);

        [DllImport("libc.so.6")]
        public static extern int ioctl(int fd, int cmd, IntPtr buffer);

        [DllImport("libc.so.6")]
        public static extern int close(int fd);
    }
}
