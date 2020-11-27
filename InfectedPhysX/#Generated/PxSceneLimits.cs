// This file was automatically generated by Biohazrd and should not be modified by hand!
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit, Size = 32)]
public unsafe partial struct PxSceneLimits
{
    [FieldOffset(0)] public uint maxNbActors;

    [FieldOffset(4)] public uint maxNbBodies;

    [FieldOffset(8)] public uint maxNbStaticShapes;

    [FieldOffset(12)] public uint maxNbDynamicShapes;

    [FieldOffset(16)] public uint maxNbAggregates;

    [FieldOffset(20)] public uint maxNbConstraints;

    [FieldOffset(24)] public uint maxNbRegions;

    [FieldOffset(28)] public uint maxNbBroadPhaseOverlaps;

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??0PxSceneLimits@physx@@QEAA@XZ", ExactSpelling = true)]
    private static extern void Constructor_PInvoke(PxSceneLimits* @this);

    public unsafe void Constructor()
    {
        fixed (PxSceneLimits* @this = &this)
        { Constructor_PInvoke(@this); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?setToDefault@PxSceneLimits@physx@@QEAAXXZ", ExactSpelling = true)]
    private static extern void setToDefault_PInvoke(PxSceneLimits* @this);

    public unsafe void setToDefault()
    {
        fixed (PxSceneLimits* @this = &this)
        { setToDefault_PInvoke(@this); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?isValid@PxSceneLimits@physx@@QEBA_NXZ", ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.I1)]
    private static extern bool isValid_PInvoke(PxSceneLimits* @this);

    public unsafe bool isValid()
    {
        fixed (PxSceneLimits* @this = &this)
        { return isValid_PInvoke(@this); }
    }
}