// <auto-generated>
// This file was automatically generated by Biohazrd and should not be modified by hand!
// </auto-generated>
#nullable enable
using System.Runtime.InteropServices;

namespace Mochi.PhysX.Immediate
{
    [StructLayout(LayoutKind.Explicit, Size = 52)]
    public unsafe partial struct PxLinkData
    {
        [FieldOffset(0)] public PxTransform pose;

        [FieldOffset(28)] public PxVec3 linearVelocity;

        [FieldOffset(40)] public PxVec3 angularVelocity;
    }
}