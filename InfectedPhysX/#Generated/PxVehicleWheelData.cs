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
    [StructLayout(LayoutKind.Explicit, Size = 48)]
    public unsafe partial struct PxVehicleWheelData
    {
        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper201", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxVehicleWheelData* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxVehicleWheelData()
        {
            fixed (PxVehicleWheelData* @this = &this)
            { Constructor_PInvoke(@this); }
        }

        [FieldOffset(0)] public float mRadius;

        [FieldOffset(4)] public float mWidth;

        [FieldOffset(8)] public float mMass;

        [FieldOffset(12)] public float mMOI;

        [FieldOffset(16)] public float mDampingRate;

        [FieldOffset(20)] public float mMaxBrakeTorque;

        [FieldOffset(24)] public float mMaxHandBrakeTorque;

        [FieldOffset(28)] public float mMaxSteer;

        [FieldOffset(32)] public float mToeAngle;

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getRecipRadius@PxVehicleWheelData@physx@@QEBAMXZ", ExactSpelling = true)]
        private static extern float getRecipRadius_PInvoke(PxVehicleWheelData* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float getRecipRadius()
        {
            fixed (PxVehicleWheelData* @this = &this)
            { return getRecipRadius_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getRecipMOI@PxVehicleWheelData@physx@@QEBAMXZ", ExactSpelling = true)]
        private static extern float getRecipMOI_PInvoke(PxVehicleWheelData* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float getRecipMOI()
        {
            fixed (PxVehicleWheelData* @this = &this)
            { return getRecipMOI_PInvoke(@this); }
        }

        [FieldOffset(36)] public float mRecipRadius;

        [FieldOffset(40)] public float mRecipMOI;

        [FieldOffset(44)] public ConstantArray_physx__UNICODE_003A____UNICODE_003A__PxReal_1 mPad;
    }
}
