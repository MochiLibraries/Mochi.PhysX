// This file was automatically generated by Biohazrd and should not be modified by hand!
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit, Size = 88)]
public unsafe partial struct PxControllerShapeHit
{
    [FieldOffset(0)] public PxControllerHit Base;

    [FieldOffset(64)] public PxShape* shape;

    [FieldOffset(72)] public PxRigidActor* actor;

    [FieldOffset(80)] public uint triangleIndex;
}