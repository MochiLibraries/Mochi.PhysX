// This file was automatically generated by Biohazrd and should not be modified by hand!
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit, Size = 8)]
public unsafe partial struct PxBVH33MidphaseDesc
{
    [FieldOffset(0)] public float meshSizePerformanceTradeOff;

    [FieldOffset(4)] public PxMeshCookingHint meshCookingHint;

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?setToDefault@PxBVH33MidphaseDesc@physx@@QEAAXXZ", ExactSpelling = true)]
    private static extern void setToDefault_PInvoke(PxBVH33MidphaseDesc* @this);

    public unsafe void setToDefault()
    {
        fixed (PxBVH33MidphaseDesc* @this = &this)
        { setToDefault_PInvoke(@this); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?isValid@PxBVH33MidphaseDesc@physx@@QEBA_NXZ", ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.I1)]
    private static extern bool isValid_PInvoke(PxBVH33MidphaseDesc* @this);

    public unsafe bool isValid()
    {
        fixed (PxBVH33MidphaseDesc* @this = &this)
        { return isValid_PInvoke(@this); }
    }
}