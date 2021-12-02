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
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public unsafe partial struct PxVehicleDifferentialNWData
    {
        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper193", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxVehicleDifferentialNWData* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxVehicleDifferentialNWData()
        {
            fixed (PxVehicleDifferentialNWData* @this = &this)
            { Constructor_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?setDrivenWheel@PxVehicleDifferentialNWData@physx@@QEAAXI_N@Z", ExactSpelling = true)]
        private static extern void setDrivenWheel_PInvoke(PxVehicleDifferentialNWData* @this, uint wheelId, [MarshalAs(UnmanagedType.I1)] bool drivenState);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void setDrivenWheel(uint wheelId, bool drivenState)
        {
            fixed (PxVehicleDifferentialNWData* @this = &this)
            { setDrivenWheel_PInvoke(@this, wheelId, drivenState); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getIsDrivenWheel@PxVehicleDifferentialNWData@physx@@QEBA_NI@Z", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool getIsDrivenWheel_PInvoke(PxVehicleDifferentialNWData* @this, uint wheelId);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool getIsDrivenWheel(uint wheelId)
        {
            fixed (PxVehicleDifferentialNWData* @this = &this)
            { return getIsDrivenWheel_PInvoke(@this, wheelId); }
        }

        [FieldOffset(0)] public ConstantArray_physx__UNICODE_003A____UNICODE_003A__PxU32_1 mBitmapBuffer;

        [FieldOffset(4)] public uint mNbDrivenWheels;

        [FieldOffset(8)] public float mInvNbDrivenWheels;

        [FieldOffset(12)] public uint mPad;

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper194", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxVehicleDifferentialNWData* @this, PxEMPTY arg0);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxVehicleDifferentialNWData(PxEMPTY arg0)
        {
            fixed (PxVehicleDifferentialNWData* @this = &this)
            { Constructor_PInvoke(@this, arg0); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getDrivenWheelStatus@PxVehicleDifferentialNWData@physx@@QEBAIXZ", ExactSpelling = true)]
        private static extern uint getDrivenWheelStatus_PInvoke(PxVehicleDifferentialNWData* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint getDrivenWheelStatus()
        {
            fixed (PxVehicleDifferentialNWData* @this = &this)
            { return getDrivenWheelStatus_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?setDrivenWheelStatus@PxVehicleDifferentialNWData@physx@@QEAAXI@Z", ExactSpelling = true)]
        private static extern void setDrivenWheelStatus_PInvoke(PxVehicleDifferentialNWData* @this, uint status);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void setDrivenWheelStatus(uint status)
        {
            fixed (PxVehicleDifferentialNWData* @this = &this)
            { setDrivenWheelStatus_PInvoke(@this, status); }
        }
    }
}
