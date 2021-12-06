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
    [StructLayout(LayoutKind.Explicit, Size = 24)]
    public unsafe partial struct PxRigidActor
    {
        [FieldOffset(0)] public PxActor Base;

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void release()
        {
            fixed (PxRigidActor* @this = &this)
            { VirtualMethodTablePointer->release(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxTransform getGlobalPose()
        {
            fixed (PxRigidActor* @this = &this)
            {
                PxTransform __returnBuffer;
                VirtualMethodTablePointer->getGlobalPose(@this, &__returnBuffer);
                return __returnBuffer;
            }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void setGlobalPose(PxTransform* pose, bool autowake = true)
        {
            fixed (PxRigidActor* @this = &this)
            { VirtualMethodTablePointer->setGlobalPose(@this, pose, autowake); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool attachShape(PxShape* shape)
        {
            fixed (PxRigidActor* @this = &this)
            { return VirtualMethodTablePointer->attachShape(@this, shape); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void detachShape(PxShape* shape, bool wakeOnLostTouch = true)
        {
            fixed (PxRigidActor* @this = &this)
            { VirtualMethodTablePointer->detachShape(@this, shape, wakeOnLostTouch); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint getNbShapes()
        {
            fixed (PxRigidActor* @this = &this)
            { return VirtualMethodTablePointer->getNbShapes(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint getShapes(PxShape** userBuffer, uint bufferSize, uint startIndex = 0)
        {
            fixed (PxRigidActor* @this = &this)
            { return VirtualMethodTablePointer->getShapes(@this, userBuffer, bufferSize, startIndex); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint getNbConstraints()
        {
            fixed (PxRigidActor* @this = &this)
            { return VirtualMethodTablePointer->getNbConstraints(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint getConstraints(PxConstraint** userBuffer, uint bufferSize, uint startIndex = 0)
        {
            fixed (PxRigidActor* @this = &this)
            { return VirtualMethodTablePointer->getConstraints(@this, userBuffer, bufferSize, startIndex); }
        }

        [FieldOffset(0)] public VirtualMethodTable* VirtualMethodTablePointer;

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct VirtualMethodTable
        {
            /// <summary>Virtual method pointer for `release`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidActor*, void> release;
            /// <summary>Virtual method pointer for `getConcreteTypeName`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidActor*, byte*> getConcreteTypeName;
            /// <summary>Virtual method pointer for `isReleasable`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidActor*, NativeBoolean> isReleasable;
            /// <summary>Virtual method pointer for `~PxRigidActor`</summary>
            public void* __DeletingDestructorPointer;
            /// <summary>Virtual method pointer for `isKindOf`</summary>
            public void* isKindOf;
            /// <summary>Virtual method pointer for `getType`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidActor*, PxActorType> getType;
            /// <summary>Virtual method pointer for `getScene`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidActor*, PxScene*> getScene;
            /// <summary>Virtual method pointer for `setName`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidActor*, byte*, void> setName;
            /// <summary>Virtual method pointer for `getName`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidActor*, byte*> getName;
            /// <summary>Virtual method pointer for `getWorldBounds`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidActor*, PxBounds3*, float, PxBounds3*> getWorldBounds;
            /// <summary>Virtual method pointer for `setActorFlag`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidActor*, PxActorFlags, NativeBoolean, void> setActorFlag;
            /// <summary>Virtual method pointer for `setActorFlags`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidActor*, PxActorFlags*, void> setActorFlags;
            /// <summary>Virtual method pointer for `getActorFlags`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidActor*, PxActorFlags*, PxActorFlags*> getActorFlags;
            /// <summary>Virtual method pointer for `setDominanceGroup`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidActor*, byte, void> setDominanceGroup;
            /// <summary>Virtual method pointer for `getDominanceGroup`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidActor*, byte> getDominanceGroup;
            /// <summary>Virtual method pointer for `setOwnerClient`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidActor*, byte, void> setOwnerClient;
            /// <summary>Virtual method pointer for `getOwnerClient`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidActor*, byte> getOwnerClient;
            /// <summary>Virtual method pointer for `getAggregate`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidActor*, PxAggregate*> getAggregate;
            /// <summary>Virtual method pointer for `getGlobalPose`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidActor*, PxTransform*, PxTransform*> getGlobalPose;
            /// <summary>Virtual method pointer for `setGlobalPose`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidActor*, PxTransform*, NativeBoolean, void> setGlobalPose;
            /// <summary>Virtual method pointer for `attachShape`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidActor*, PxShape*, NativeBoolean> attachShape;
            /// <summary>Virtual method pointer for `detachShape`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidActor*, PxShape*, NativeBoolean, void> detachShape;
            /// <summary>Virtual method pointer for `getNbShapes`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidActor*, uint> getNbShapes;
            /// <summary>Virtual method pointer for `getShapes`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidActor*, PxShape**, uint, uint, uint> getShapes;
            /// <summary>Virtual method pointer for `getNbConstraints`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidActor*, uint> getNbConstraints;
            /// <summary>Virtual method pointer for `getConstraints`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidActor*, PxConstraint**, uint, uint, uint> getConstraints;
        }
    }
}