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
    public unsafe partial struct PxVehicleWheelsDynData
    {
        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper208", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxVehicleWheelsDynData* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxVehicleWheelsDynData()
        {
            fixed (PxVehicleWheelsDynData* @this = &this)
            { Constructor_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper209", ExactSpelling = true)]
        private static extern void Destructor_PInvoke(PxVehicleWheelsDynData* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destructor()
        {
            fixed (PxVehicleWheelsDynData* @this = &this)
            { Destructor_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?setToRestState@PxVehicleWheelsDynData@physx@@QEAAXXZ", ExactSpelling = true)]
        private static extern void setToRestState_PInvoke(PxVehicleWheelsDynData* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void setToRestState()
        {
            fixed (PxVehicleWheelsDynData* @this = &this)
            { setToRestState_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?setTireForceShaderFunction@PxVehicleWheelsDynData@physx@@QEAAXP6AXPEBXMMMMMMMMMMMMAEAM111@Z@Z", ExactSpelling = true)]
        private static extern void setTireForceShaderFunction_PInvoke(PxVehicleWheelsDynData* @this, delegate* unmanaged[Cdecl]<void*, float, float, float, float, float, float, float, float, float, float, float, float, float*, float*, float*, float*, void> tireForceShaderFn);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void setTireForceShaderFunction(delegate* unmanaged[Cdecl]<void*, float, float, float, float, float, float, float, float, float, float, float, float, float*, float*, float*, float*, void> tireForceShaderFn)
        {
            fixed (PxVehicleWheelsDynData* @this = &this)
            { setTireForceShaderFunction_PInvoke(@this, tireForceShaderFn); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?setTireForceShaderData@PxVehicleWheelsDynData@physx@@QEAAXIPEBX@Z", ExactSpelling = true)]
        private static extern void setTireForceShaderData_PInvoke(PxVehicleWheelsDynData* @this, uint tireId, void* tireForceShaderData);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void setTireForceShaderData(uint tireId, void* tireForceShaderData)
        {
            fixed (PxVehicleWheelsDynData* @this = &this)
            { setTireForceShaderData_PInvoke(@this, tireId, tireForceShaderData); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getTireForceShaderData@PxVehicleWheelsDynData@physx@@QEBAPEBXI@Z", ExactSpelling = true)]
        private static extern void* getTireForceShaderData_PInvoke(PxVehicleWheelsDynData* @this, uint tireId);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void* getTireForceShaderData(uint tireId)
        {
            fixed (PxVehicleWheelsDynData* @this = &this)
            { return getTireForceShaderData_PInvoke(@this, tireId); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?setWheelRotationSpeed@PxVehicleWheelsDynData@physx@@QEAAXIM@Z", ExactSpelling = true)]
        private static extern void setWheelRotationSpeed_PInvoke(PxVehicleWheelsDynData* @this, uint wheelIdx, float speed);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void setWheelRotationSpeed(uint wheelIdx, float speed)
        {
            fixed (PxVehicleWheelsDynData* @this = &this)
            { setWheelRotationSpeed_PInvoke(@this, wheelIdx, speed); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getWheelRotationSpeed@PxVehicleWheelsDynData@physx@@QEBAMI@Z", ExactSpelling = true)]
        private static extern float getWheelRotationSpeed_PInvoke(PxVehicleWheelsDynData* @this, uint wheelIdx);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float getWheelRotationSpeed(uint wheelIdx)
        {
            fixed (PxVehicleWheelsDynData* @this = &this)
            { return getWheelRotationSpeed_PInvoke(@this, wheelIdx); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?setWheelRotationAngle@PxVehicleWheelsDynData@physx@@QEAAXIM@Z", ExactSpelling = true)]
        private static extern void setWheelRotationAngle_PInvoke(PxVehicleWheelsDynData* @this, uint wheelIdx, float angle);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void setWheelRotationAngle(uint wheelIdx, float angle)
        {
            fixed (PxVehicleWheelsDynData* @this = &this)
            { setWheelRotationAngle_PInvoke(@this, wheelIdx, angle); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getWheelRotationAngle@PxVehicleWheelsDynData@physx@@QEBAMI@Z", ExactSpelling = true)]
        private static extern float getWheelRotationAngle_PInvoke(PxVehicleWheelsDynData* @this, uint wheelIdx);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float getWheelRotationAngle(uint wheelIdx)
        {
            fixed (PxVehicleWheelsDynData* @this = &this)
            { return getWheelRotationAngle_PInvoke(@this, wheelIdx); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?setUserData@PxVehicleWheelsDynData@physx@@QEAAXIPEAX@Z", ExactSpelling = true)]
        private static extern void setUserData_PInvoke(PxVehicleWheelsDynData* @this, uint tireIdx, void* userData);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void setUserData(uint tireIdx, void* userData)
        {
            fixed (PxVehicleWheelsDynData* @this = &this)
            { setUserData_PInvoke(@this, tireIdx, userData); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getUserData@PxVehicleWheelsDynData@physx@@QEBAPEAXI@Z", ExactSpelling = true)]
        private static extern void* getUserData_PInvoke(PxVehicleWheelsDynData* @this, uint tireIdx);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void* getUserData(uint tireIdx)
        {
            fixed (PxVehicleWheelsDynData* @this = &this)
            { return getUserData_PInvoke(@this, tireIdx); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?copy@PxVehicleWheelsDynData@physx@@QEAAXAEBV12@II@Z", ExactSpelling = true)]
        private static extern void copy_PInvoke(PxVehicleWheelsDynData* @this, PxVehicleWheelsDynData* src, uint srcWheel, uint trgWheel);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void copy(PxVehicleWheelsDynData* src, uint srcWheel, uint trgWheel)
        {
            fixed (PxVehicleWheelsDynData* @this = &this)
            { copy_PInvoke(@this, src, srcWheel, trgWheel); }
        }

        [FieldOffset(0)] public PxVehicleWheels4DynData* mWheels4DynData;

        [FieldOffset(8)] public PxVehicleTireForceCalculator* mTireForceCalculators;

        [FieldOffset(16)] public void** mUserDatas;

        [FieldOffset(24)] public uint mNbWheels4;

        [FieldOffset(28)] public uint mNbActiveWheels;

        [FieldOffset(32)] public ConstantArray_physx__UNICODE_003A____UNICODE_003A__PxU32_3 mPad;

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getBinaryMetaData@PxVehicleWheelsDynData@physx@@SAXAEAVPxOutputStream@2@@Z", ExactSpelling = true)]
        public static extern void getBinaryMetaData(PxOutputStream* stream);

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getNbWheelRotationSpeed@PxVehicleWheelsDynData@physx@@QEBAIXZ", ExactSpelling = true)]
        private static extern uint getNbWheelRotationSpeed_PInvoke(PxVehicleWheelsDynData* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint getNbWheelRotationSpeed()
        {
            fixed (PxVehicleWheelsDynData* @this = &this)
            { return getNbWheelRotationSpeed_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getNbWheelRotationAngle@PxVehicleWheelsDynData@physx@@QEBAIXZ", ExactSpelling = true)]
        private static extern uint getNbWheelRotationAngle_PInvoke(PxVehicleWheelsDynData* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint getNbWheelRotationAngle()
        {
            fixed (PxVehicleWheelsDynData* @this = &this)
            { return getNbWheelRotationAngle_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getWheel4DynData@PxVehicleWheelsDynData@physx@@QEBAPEAVPxVehicleWheels4DynData@2@XZ", ExactSpelling = true)]
        private static extern PxVehicleWheels4DynData* getWheel4DynData_PInvoke(PxVehicleWheelsDynData* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxVehicleWheels4DynData* getWheel4DynData()
        {
            fixed (PxVehicleWheelsDynData* @this = &this)
            { return getWheel4DynData_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getNbConstraints@PxVehicleWheelsDynData@physx@@QEBAIXZ", ExactSpelling = true)]
        private static extern uint getNbConstraints_PInvoke(PxVehicleWheelsDynData* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint getNbConstraints()
        {
            fixed (PxVehicleWheelsDynData* @this = &this)
            { return getNbConstraints_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getConstraints@PxVehicleWheelsDynData@physx@@QEBAIPEAPEAVPxConstraint@2@II@Z", ExactSpelling = true)]
        private static extern uint getConstraints_PInvoke(PxVehicleWheelsDynData* @this, PxConstraint** userBuffer, uint bufferSize, uint startIndex);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint getConstraints(PxConstraint** userBuffer, uint bufferSize, uint startIndex = 0)
        {
            fixed (PxVehicleWheelsDynData* @this = &this)
            { return getConstraints_PInvoke(@this, userBuffer, bufferSize, startIndex); }
        }
    }
}
