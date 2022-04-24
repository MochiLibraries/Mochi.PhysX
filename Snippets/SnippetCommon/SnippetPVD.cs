using System;
using System.Runtime.CompilerServices;
using System.Text;

public unsafe static class SnippetPVD
{
    private const string PVD_HOST_STRING = "127.0.0.1"; //Set this to the IP address of the system running the PhysX Visual Debugger that you want to connect to.

    private static byte[]? _PVD_HOST;
    public static byte* PVD_HOST
    {
        get
        {
            if (_PVD_HOST is null)
            {
                int count = Encoding.ASCII.GetMaxByteCount(PVD_HOST_STRING.Length) + 1;
                _PVD_HOST = GC.AllocateArray<byte>(count, pinned: true);
                int encodedCount = Encoding.ASCII.GetBytes(PVD_HOST_STRING, _PVD_HOST);
            }

            return (byte*)Unsafe.AsPointer(ref _PVD_HOST[0]);
        }
    }
}
