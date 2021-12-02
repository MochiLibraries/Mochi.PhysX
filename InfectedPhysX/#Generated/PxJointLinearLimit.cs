// <auto-generated>
// This file was automatically generated by Biohazrd and should not be modified by hand!
// </auto-generated>
#nullable enable
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Mochi.PhysX
{
    [StructLayout(LayoutKind.Explicit, Size = 24)]
    public unsafe partial struct PxJointLinearLimit
    {
        [FieldOffset(0)] public PxJointLimitParameters Base;

        [FieldOffset(20)] public float value;

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper163", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxJointLinearLimit* @this, PxTolerancesScale* scale, float extent, float contactDist);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxJointLinearLimit(PxTolerancesScale* scale, float extent, float contactDist = -1f)
        {
            fixed (PxJointLinearLimit* @this = &this)
            { Constructor_PInvoke(@this, scale, extent, contactDist); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper164", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxJointLinearLimit* @this, float extent, PxSpring* spring);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxJointLinearLimit(float extent, PxSpring* spring)
        {
            fixed (PxJointLinearLimit* @this = &this)
            { Constructor_PInvoke(@this, extent, spring); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?isValid@PxJointLinearLimit@physx@@QEBA_NXZ", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool isValid_PInvoke(PxJointLinearLimit* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool isValid()
        {
            fixed (PxJointLinearLimit* @this = &this)
            { return isValid_PInvoke(@this); }
        }
    }
}
