// This file was automatically generated by Biohazrd and should not be modified by hand!
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit, Size = 32)]
public unsafe partial struct PxVehicleChassisData
{
    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??0PxVehicleChassisData@physx@@QEAA@XZ", ExactSpelling = true)]
    private static extern void Constructor_PInvoke(PxVehicleChassisData* @this);

    public unsafe void Constructor()
    {
        fixed (PxVehicleChassisData* @this = &this)
        { Constructor_PInvoke(@this); }
    }

    [FieldOffset(0)] public PxVec3 mMOI;

    [FieldOffset(12)] public float mMass;

    [FieldOffset(16)] public PxVec3 mCMOffset;

    [FieldOffset(28)] public float pad;

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?isValid@PxVehicleChassisData@physx@@AEBA_NXZ", ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.I1)]
    private static extern bool isValid_PInvoke(PxVehicleChassisData* @this);

    public unsafe bool isValid()
    {
        fixed (PxVehicleChassisData* @this = &this)
        { return isValid_PInvoke(@this); }
    }
}