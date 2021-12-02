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
    [StructLayout(LayoutKind.Explicit, Size = 144)]
    public unsafe partial struct PxVehicleGearsData
    {
        public enum Enum
        {
            eREVERSE = 0,
            eNEUTRAL,
            eFIRST,
            eSECOND,
            eTHIRD,
            eFOURTH,
            eFIFTH,
            eSIXTH,
            eSEVENTH,
            eEIGHTH,
            eNINTH,
            eTENTH,
            eELEVENTH,
            eTWELFTH,
            eTHIRTEENTH,
            eFOURTEENTH,
            eFIFTEENTH,
            eSIXTEENTH,
            eSEVENTEENTH,
            eEIGHTEENTH,
            eNINETEENTH,
            eTWENTIETH,
            eTWENTYFIRST,
            eTWENTYSECOND,
            eTWENTYTHIRD,
            eTWENTYFOURTH,
            eTWENTYFIFTH,
            eTWENTYSIXTH,
            eTWENTYSEVENTH,
            eTWENTYEIGHTH,
            eTWENTYNINTH,
            eTHIRTIETH,
            eGEARSRATIO_COUNT
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper187", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxVehicleGearsData* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxVehicleGearsData()
        {
            fixed (PxVehicleGearsData* @this = &this)
            { Constructor_PInvoke(@this); }
        }

        [FieldOffset(0)] public ConstantArray_physx__UNICODE_003A____UNICODE_003A__PxReal_32 mRatios;

        [FieldOffset(128)] public float mFinalRatio;

        [FieldOffset(132)] public uint mNbRatios;

        [FieldOffset(136)] public float mSwitchTime;

        [FieldOffset(140)] public float mPad;

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper188", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxVehicleGearsData* @this, PxEMPTY arg0);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxVehicleGearsData(PxEMPTY arg0)
        {
            fixed (PxVehicleGearsData* @this = &this)
            { Constructor_PInvoke(@this, arg0); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getGearRatio@PxVehicleGearsData@physx@@QEBAMW4Enum@12@@Z", ExactSpelling = true)]
        private static extern float getGearRatio_PInvoke(PxVehicleGearsData* @this, Enum a);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float getGearRatio(Enum a)
        {
            fixed (PxVehicleGearsData* @this = &this)
            { return getGearRatio_PInvoke(@this, a); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?setGearRatio@PxVehicleGearsData@physx@@QEAAXW4Enum@12@M@Z", ExactSpelling = true)]
        private static extern void setGearRatio_PInvoke(PxVehicleGearsData* @this, Enum a, float ratio);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void setGearRatio(Enum a, float ratio)
        {
            fixed (PxVehicleGearsData* @this = &this)
            { setGearRatio_PInvoke(@this, a, ratio); }
        }
    }
}
