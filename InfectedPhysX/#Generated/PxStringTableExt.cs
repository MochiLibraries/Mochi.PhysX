// This file was automatically generated by Biohazrd and should not be modified by hand!
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit, Size = 1)]
public unsafe partial struct PxStringTableExt
{
    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?createStringTable@PxStringTableExt@physx@@SAAEAVPxStringTable@2@AEAVPxAllocatorCallback@2@@Z", ExactSpelling = true)]
    public static extern PxStringTable* createStringTable(PxAllocatorCallback* inAllocator);
}