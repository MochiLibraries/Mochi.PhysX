// <auto-generated>
// This file was automatically generated by Biohazrd and should not be modified by hand!
// </auto-generated>
#nullable enable
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Mochi.PhysX
{
    [StructLayout(LayoutKind.Explicit, Size = 20)]
    public unsafe partial struct PxJointLimitParameters
    {
        [FieldOffset(0)] public float restitution;

        [FieldOffset(4)] public float bounceThreshold;

        [FieldOffset(8)] public float stiffness;

        [FieldOffset(12)] public float damping;

        [FieldOffset(16)] public float contactDistance;

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper161", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxJointLimitParameters* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxJointLimitParameters()
        {
            fixed (PxJointLimitParameters* @this = &this)
            { Constructor_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper162", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxJointLimitParameters* @this, PxJointLimitParameters* p);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxJointLimitParameters(PxJointLimitParameters* p)
        {
            fixed (PxJointLimitParameters* @this = &this)
            { Constructor_PInvoke(@this, p); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?isValid@PxJointLimitParameters@physx@@QEBA_NXZ", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool isValid_PInvoke(PxJointLimitParameters* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool isValid()
        {
            fixed (PxJointLimitParameters* @this = &this)
            { return isValid_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?isSoft@PxJointLimitParameters@physx@@QEBA_NXZ", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool isSoft_PInvoke(PxJointLimitParameters* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool isSoft()
        {
            fixed (PxJointLimitParameters* @this = &this)
            { return isSoft_PInvoke(@this); }
        }
    }
}
