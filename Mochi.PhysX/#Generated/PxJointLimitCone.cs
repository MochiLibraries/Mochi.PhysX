// <auto-generated>
// This file was automatically generated by Biohazrd and should not be modified by hand!
// </auto-generated>
#nullable enable
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Mochi.PhysX
{
    [StructLayout(LayoutKind.Explicit, Size = 28)]
    public unsafe partial struct PxJointLimitCone
    {
        [FieldOffset(0)] public PxJointLimitParameters Base;

        [FieldOffset(20)] public float yAngle;

        [FieldOffset(24)] public float zAngle;

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper169", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxJointLimitCone* @this, float yLimitAngle, float zLimitAngle, float contactDist);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxJointLimitCone(float yLimitAngle, float zLimitAngle, float contactDist = -1f)
        {
            fixed (PxJointLimitCone* @this = &this)
            { Constructor_PInvoke(@this, yLimitAngle, zLimitAngle, contactDist); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper170", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxJointLimitCone* @this, float yLimitAngle, float zLimitAngle, PxSpring* spring);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxJointLimitCone(float yLimitAngle, float zLimitAngle, PxSpring* spring)
        {
            fixed (PxJointLimitCone* @this = &this)
            { Constructor_PInvoke(@this, yLimitAngle, zLimitAngle, spring); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?isValid@PxJointLimitCone@physx@@QEBA_NXZ", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool isValid_PInvoke(PxJointLimitCone* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool isValid()
        {
            fixed (PxJointLimitCone* @this = &this)
            { return isValid_PInvoke(@this); }
        }
    }
}