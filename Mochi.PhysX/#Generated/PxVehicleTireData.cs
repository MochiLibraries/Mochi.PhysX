// <auto-generated>
// This file was automatically generated by Biohazrd and should not be modified by hand!
// </auto-generated>
#nullable enable
using Mochi.PhysX.Infrastructure;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Mochi.PhysX
{
    [StructLayout(LayoutKind.Explicit, Size = 64)]
    public unsafe partial struct PxVehicleTireData
    {
        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper204", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxVehicleTireData* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxVehicleTireData()
        {
            fixed (PxVehicleTireData* @this = &this)
            { Constructor_PInvoke(@this); }
        }

        [FieldOffset(0)] public float mLatStiffX;

        [FieldOffset(4)] public float mLatStiffY;

        [FieldOffset(8)] public float mLongitudinalStiffnessPerUnitGravity;

        [FieldOffset(12)] public float mCamberStiffnessPerUnitGravity;

        [FieldOffset(16)] public ConstantArray_physx__UNICODE_003A____UNICODE_003A__PxReal__UNICODE_0020____UNICODE_005B__2__UNICODE_005D___3 mFrictionVsSlipGraph;

        [FieldOffset(40)] public uint mType;

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getRecipLongitudinalStiffnessPerUnitGravity@PxVehicleTireData@physx@@QEBAMXZ", ExactSpelling = true)]
        private static extern float getRecipLongitudinalStiffnessPerUnitGravity_PInvoke(PxVehicleTireData* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float getRecipLongitudinalStiffnessPerUnitGravity()
        {
            fixed (PxVehicleTireData* @this = &this)
            { return getRecipLongitudinalStiffnessPerUnitGravity_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getFrictionVsSlipGraphRecipx1Minusx0@PxVehicleTireData@physx@@QEBAMXZ", ExactSpelling = true)]
        private static extern float getFrictionVsSlipGraphRecipx1Minusx0_PInvoke(PxVehicleTireData* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float getFrictionVsSlipGraphRecipx1Minusx0()
        {
            fixed (PxVehicleTireData* @this = &this)
            { return getFrictionVsSlipGraphRecipx1Minusx0_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getFrictionVsSlipGraphRecipx2Minusx1@PxVehicleTireData@physx@@QEBAMXZ", ExactSpelling = true)]
        private static extern float getFrictionVsSlipGraphRecipx2Minusx1_PInvoke(PxVehicleTireData* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float getFrictionVsSlipGraphRecipx2Minusx1()
        {
            fixed (PxVehicleTireData* @this = &this)
            { return getFrictionVsSlipGraphRecipx2Minusx1_PInvoke(@this); }
        }

        [FieldOffset(44)] public float mRecipLongitudinalStiffnessPerUnitGravity;

        [FieldOffset(48)] public float mFrictionVsSlipGraphRecipx1Minusx0;

        [FieldOffset(52)] public float mFrictionVsSlipGraphRecipx2Minusx1;

        [FieldOffset(56)] public ConstantArray_physx__UNICODE_003A____UNICODE_003A__PxReal_2 mPad;
    }
}