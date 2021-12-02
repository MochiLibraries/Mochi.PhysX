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
    [StructLayout(LayoutKind.Explicit, Size = 832)]
    public unsafe partial struct PxVehicleDriveNW
    {
        [FieldOffset(0)] public PxVehicleDrive Base;

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?allocate@PxVehicleDriveNW@physx@@SAPEAV12@I@Z", ExactSpelling = true)]
        public static extern PxVehicleDriveNW* allocate(uint nbWheels);

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?free@PxVehicleDriveNW@physx@@QEAAXXZ", ExactSpelling = true)]
        private static extern void free_PInvoke(PxVehicleDriveNW* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void free()
        {
            fixed (PxVehicleDriveNW* @this = &this)
            { free_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?setup@PxVehicleDriveNW@physx@@QEAAXPEAVPxPhysics@2@PEAVPxRigidDynamic@2@AEBVPxVehicleWheelsSimData@2@AEBVPxVehicleDriveSimDataNW@2@I@Z", ExactSpelling = true)]
        private static extern void setup_PInvoke(PxVehicleDriveNW* @this, PxPhysics* physics, PxRigidDynamic* vehActor, PxVehicleWheelsSimData* wheelsData, PxVehicleDriveSimDataNW* driveData, uint nbWheels);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void setup(PxPhysics* physics, PxRigidDynamic* vehActor, PxVehicleWheelsSimData* wheelsData, PxVehicleDriveSimDataNW* driveData, uint nbWheels)
        {
            fixed (PxVehicleDriveNW* @this = &this)
            { setup_PInvoke(@this, physics, vehActor, wheelsData, driveData, nbWheels); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?create@PxVehicleDriveNW@physx@@SAPEAV12@PEAVPxPhysics@2@PEAVPxRigidDynamic@2@AEBVPxVehicleWheelsSimData@2@AEBVPxVehicleDriveSimDataNW@2@I@Z", ExactSpelling = true)]
        public static extern PxVehicleDriveNW* create(PxPhysics* physics, PxRigidDynamic* vehActor, PxVehicleWheelsSimData* wheelsData, PxVehicleDriveSimDataNW* driveData, uint nbWheels);

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?setToRestState@PxVehicleDriveNW@physx@@QEAAXXZ", ExactSpelling = true)]
        private static extern void setToRestState_PInvoke(PxVehicleDriveNW* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void setToRestState()
        {
            fixed (PxVehicleDriveNW* @this = &this)
            { setToRestState_PInvoke(@this); }
        }

        [FieldOffset(288)] public PxVehicleDriveSimDataNW mDriveSimData;

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper225", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxVehicleDriveNW* @this, PxBaseFlags* baseFlags);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxVehicleDriveNW(PxBaseFlags* baseFlags)
        {
            fixed (PxVehicleDriveNW* @this = &this)
            { Constructor_PInvoke(@this, baseFlags); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??0PxVehicleDriveNW@physx@@QEAA@XZ", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxVehicleDriveNW* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxVehicleDriveNW()
        {
            fixed (PxVehicleDriveNW* @this = &this)
            { Constructor_PInvoke(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destructor()
        {
            fixed (PxVehicleDriveNW* @this = &this)
            { VirtualMethodTablePointer->__DeletingDestructorPointer(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?createObject@PxVehicleDriveNW@physx@@SAPEAV12@AEAPEAEAEAVPxDeserializationContext@2@@Z", ExactSpelling = true)]
        public static extern PxVehicleDriveNW* createObject(byte** address, PxDeserializationContext* context);

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getBinaryMetaData@PxVehicleDriveNW@physx@@SAXAEAVPxOutputStream@2@@Z", ExactSpelling = true)]
        public static extern void getBinaryMetaData(PxOutputStream* stream);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte* getConcreteTypeName()
        {
            fixed (PxVehicleDriveNW* @this = &this)
            { return VirtualMethodTablePointer->getConcreteTypeName(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool isKindOf(byte* name)
        {
            fixed (PxVehicleDriveNW* @this = &this)
            { return VirtualMethodTablePointer->isKindOf(@this, name); }
        }

        [FieldOffset(0)] public VirtualMethodTable* VirtualMethodTablePointer;

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct VirtualMethodTable
        {
            /// <summary>Virtual method pointer for `release`</summary>
            public delegate* unmanaged[Cdecl]<PxVehicleDriveNW*, void> release;
            /// <summary>Virtual method pointer for `getConcreteTypeName`</summary>
            public delegate* unmanaged[Cdecl]<PxVehicleDriveNW*, byte*> getConcreteTypeName;
            /// <summary>Virtual method pointer for `isReleasable`</summary>
            public delegate* unmanaged[Cdecl]<PxVehicleDriveNW*, NativeBoolean> isReleasable;
            /// <summary>Virtual method pointer for `~PxVehicleDriveNW`</summary>
            public delegate* unmanaged[Cdecl]<PxVehicleDriveNW*, void> __DeletingDestructorPointer;
            /// <summary>Virtual method pointer for `isKindOf`</summary>
            public delegate* unmanaged[Cdecl]<PxVehicleDriveNW*, byte*, NativeBoolean> isKindOf;
            /// <summary>Virtual method pointer for `init`</summary>
            public void* init;
            /// <summary>Virtual method pointer for `requiresObjects`</summary>
            public delegate* unmanaged[Cdecl]<PxVehicleDriveNW*, PxProcessPxBaseCallback*, void> requiresObjects;
            /// <summary>Virtual method pointer for `preExportDataReset`</summary>
            public delegate* unmanaged[Cdecl]<PxVehicleDriveNW*, void> preExportDataReset;
            /// <summary>Virtual method pointer for `exportExtraData`</summary>
            public delegate* unmanaged[Cdecl]<PxVehicleDriveNW*, PxSerializationContext*, void> exportExtraData;
        }
    }
}
