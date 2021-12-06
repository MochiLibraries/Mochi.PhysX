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
    public unsafe partial struct PxRigidBody
    {
        [FieldOffset(0)] public PxRigidActor Base;

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void setCMassLocalPose(PxTransform* pose)
        {
            fixed (PxRigidBody* @this = &this)
            { VirtualMethodTablePointer->setCMassLocalPose(@this, pose); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxTransform getCMassLocalPose()
        {
            fixed (PxRigidBody* @this = &this)
            {
                PxTransform __returnBuffer;
                VirtualMethodTablePointer->getCMassLocalPose(@this, &__returnBuffer);
                return __returnBuffer;
            }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void setMass(float mass)
        {
            fixed (PxRigidBody* @this = &this)
            { VirtualMethodTablePointer->setMass(@this, mass); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float getMass()
        {
            fixed (PxRigidBody* @this = &this)
            { return VirtualMethodTablePointer->getMass(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float getInvMass()
        {
            fixed (PxRigidBody* @this = &this)
            { return VirtualMethodTablePointer->getInvMass(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void setMassSpaceInertiaTensor(PxVec3* m)
        {
            fixed (PxRigidBody* @this = &this)
            { VirtualMethodTablePointer->setMassSpaceInertiaTensor(@this, m); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxVec3 getMassSpaceInertiaTensor()
        {
            fixed (PxRigidBody* @this = &this)
            {
                PxVec3 __returnBuffer;
                VirtualMethodTablePointer->getMassSpaceInertiaTensor(@this, &__returnBuffer);
                return __returnBuffer;
            }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxVec3 getMassSpaceInvInertiaTensor()
        {
            fixed (PxRigidBody* @this = &this)
            {
                PxVec3 __returnBuffer;
                VirtualMethodTablePointer->getMassSpaceInvInertiaTensor(@this, &__returnBuffer);
                return __returnBuffer;
            }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void setLinearDamping(float linDamp)
        {
            fixed (PxRigidBody* @this = &this)
            { VirtualMethodTablePointer->setLinearDamping(@this, linDamp); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float getLinearDamping()
        {
            fixed (PxRigidBody* @this = &this)
            { return VirtualMethodTablePointer->getLinearDamping(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void setAngularDamping(float angDamp)
        {
            fixed (PxRigidBody* @this = &this)
            { VirtualMethodTablePointer->setAngularDamping(@this, angDamp); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float getAngularDamping()
        {
            fixed (PxRigidBody* @this = &this)
            { return VirtualMethodTablePointer->getAngularDamping(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxVec3 getLinearVelocity()
        {
            fixed (PxRigidBody* @this = &this)
            {
                PxVec3 __returnBuffer;
                VirtualMethodTablePointer->getLinearVelocity(@this, &__returnBuffer);
                return __returnBuffer;
            }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void setLinearVelocity(PxVec3* linVel, bool autowake = true)
        {
            fixed (PxRigidBody* @this = &this)
            { VirtualMethodTablePointer->setLinearVelocity(@this, linVel, autowake); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxVec3 getAngularVelocity()
        {
            fixed (PxRigidBody* @this = &this)
            {
                PxVec3 __returnBuffer;
                VirtualMethodTablePointer->getAngularVelocity(@this, &__returnBuffer);
                return __returnBuffer;
            }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void setAngularVelocity(PxVec3* angVel, bool autowake = true)
        {
            fixed (PxRigidBody* @this = &this)
            { VirtualMethodTablePointer->setAngularVelocity(@this, angVel, autowake); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void setMaxAngularVelocity(float maxAngVel)
        {
            fixed (PxRigidBody* @this = &this)
            { VirtualMethodTablePointer->setMaxAngularVelocity(@this, maxAngVel); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float getMaxAngularVelocity()
        {
            fixed (PxRigidBody* @this = &this)
            { return VirtualMethodTablePointer->getMaxAngularVelocity(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void setMaxLinearVelocity(float maxLinVel)
        {
            fixed (PxRigidBody* @this = &this)
            { VirtualMethodTablePointer->setMaxLinearVelocity(@this, maxLinVel); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float getMaxLinearVelocity()
        {
            fixed (PxRigidBody* @this = &this)
            { return VirtualMethodTablePointer->getMaxLinearVelocity(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void addForce(PxVec3* force, PxForceMode mode = PxForceMode.eFORCE, bool autowake = true)
        {
            fixed (PxRigidBody* @this = &this)
            { VirtualMethodTablePointer->addForce(@this, force, mode, autowake); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void addTorque(PxVec3* torque, PxForceMode mode = PxForceMode.eFORCE, bool autowake = true)
        {
            fixed (PxRigidBody* @this = &this)
            { VirtualMethodTablePointer->addTorque(@this, torque, mode, autowake); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void clearForce(PxForceMode mode = PxForceMode.eFORCE)
        {
            fixed (PxRigidBody* @this = &this)
            { VirtualMethodTablePointer->clearForce(@this, mode); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void clearTorque(PxForceMode mode = PxForceMode.eFORCE)
        {
            fixed (PxRigidBody* @this = &this)
            { VirtualMethodTablePointer->clearTorque(@this, mode); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void setForceAndTorque(PxVec3* force, PxVec3* torque, PxForceMode mode = PxForceMode.eFORCE)
        {
            fixed (PxRigidBody* @this = &this)
            { VirtualMethodTablePointer->setForceAndTorque(@this, force, torque, mode); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void setRigidBodyFlag(PxRigidBodyFlags flag, bool value)
        {
            fixed (PxRigidBody* @this = &this)
            { VirtualMethodTablePointer->setRigidBodyFlag(@this, flag, value); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void setRigidBodyFlags(PxRigidBodyFlags* inFlags)
        {
            fixed (PxRigidBody* @this = &this)
            { VirtualMethodTablePointer->setRigidBodyFlags(@this, inFlags); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxRigidBodyFlags getRigidBodyFlags()
        {
            fixed (PxRigidBody* @this = &this)
            {
                PxRigidBodyFlags __returnBuffer;
                VirtualMethodTablePointer->getRigidBodyFlags(@this, &__returnBuffer);
                return __returnBuffer;
            }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void setMinCCDAdvanceCoefficient(float advanceCoefficient)
        {
            fixed (PxRigidBody* @this = &this)
            { VirtualMethodTablePointer->setMinCCDAdvanceCoefficient(@this, advanceCoefficient); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float getMinCCDAdvanceCoefficient()
        {
            fixed (PxRigidBody* @this = &this)
            { return VirtualMethodTablePointer->getMinCCDAdvanceCoefficient(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void setMaxDepenetrationVelocity(float biasClamp)
        {
            fixed (PxRigidBody* @this = &this)
            { VirtualMethodTablePointer->setMaxDepenetrationVelocity(@this, biasClamp); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float getMaxDepenetrationVelocity()
        {
            fixed (PxRigidBody* @this = &this)
            { return VirtualMethodTablePointer->getMaxDepenetrationVelocity(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void setMaxContactImpulse(float maxImpulse)
        {
            fixed (PxRigidBody* @this = &this)
            { VirtualMethodTablePointer->setMaxContactImpulse(@this, maxImpulse); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float getMaxContactImpulse()
        {
            fixed (PxRigidBody* @this = &this)
            { return VirtualMethodTablePointer->getMaxContactImpulse(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint getInternalIslandNodeIndex()
        {
            fixed (PxRigidBody* @this = &this)
            { return VirtualMethodTablePointer->getInternalIslandNodeIndex(@this); }
        }

        [FieldOffset(0)] public VirtualMethodTable* VirtualMethodTablePointer;

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct VirtualMethodTable
        {
            /// <summary>Virtual method pointer for `release`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, void> release;
            /// <summary>Virtual method pointer for `getConcreteTypeName`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, byte*> getConcreteTypeName;
            /// <summary>Virtual method pointer for `isReleasable`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, NativeBoolean> isReleasable;
            /// <summary>Virtual method pointer for `~PxRigidBody`</summary>
            public void* __DeletingDestructorPointer;
            /// <summary>Virtual method pointer for `isKindOf`</summary>
            public void* isKindOf;
            /// <summary>Virtual method pointer for `getType`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, PxActorType> getType;
            /// <summary>Virtual method pointer for `getScene`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, PxScene*> getScene;
            /// <summary>Virtual method pointer for `setName`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, byte*, void> setName;
            /// <summary>Virtual method pointer for `getName`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, byte*> getName;
            /// <summary>Virtual method pointer for `getWorldBounds`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, PxBounds3*, float, PxBounds3*> getWorldBounds;
            /// <summary>Virtual method pointer for `setActorFlag`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, PxActorFlags, NativeBoolean, void> setActorFlag;
            /// <summary>Virtual method pointer for `setActorFlags`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, PxActorFlags*, void> setActorFlags;
            /// <summary>Virtual method pointer for `getActorFlags`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, PxActorFlags*, PxActorFlags*> getActorFlags;
            /// <summary>Virtual method pointer for `setDominanceGroup`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, byte, void> setDominanceGroup;
            /// <summary>Virtual method pointer for `getDominanceGroup`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, byte> getDominanceGroup;
            /// <summary>Virtual method pointer for `setOwnerClient`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, byte, void> setOwnerClient;
            /// <summary>Virtual method pointer for `getOwnerClient`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, byte> getOwnerClient;
            /// <summary>Virtual method pointer for `getAggregate`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, PxAggregate*> getAggregate;
            /// <summary>Virtual method pointer for `getGlobalPose`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, PxTransform*, PxTransform*> getGlobalPose;
            /// <summary>Virtual method pointer for `setGlobalPose`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, PxTransform*, NativeBoolean, void> setGlobalPose;
            /// <summary>Virtual method pointer for `attachShape`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, PxShape*, NativeBoolean> attachShape;
            /// <summary>Virtual method pointer for `detachShape`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, PxShape*, NativeBoolean, void> detachShape;
            /// <summary>Virtual method pointer for `getNbShapes`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, uint> getNbShapes;
            /// <summary>Virtual method pointer for `getShapes`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, PxShape**, uint, uint, uint> getShapes;
            /// <summary>Virtual method pointer for `getNbConstraints`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, uint> getNbConstraints;
            /// <summary>Virtual method pointer for `getConstraints`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, PxConstraint**, uint, uint, uint> getConstraints;
            /// <summary>Virtual method pointer for `setCMassLocalPose`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, PxTransform*, void> setCMassLocalPose;
            /// <summary>Virtual method pointer for `getCMassLocalPose`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, PxTransform*, PxTransform*> getCMassLocalPose;
            /// <summary>Virtual method pointer for `setMass`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, float, void> setMass;
            /// <summary>Virtual method pointer for `getMass`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, float> getMass;
            /// <summary>Virtual method pointer for `getInvMass`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, float> getInvMass;
            /// <summary>Virtual method pointer for `setMassSpaceInertiaTensor`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, PxVec3*, void> setMassSpaceInertiaTensor;
            /// <summary>Virtual method pointer for `getMassSpaceInertiaTensor`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, PxVec3*, PxVec3*> getMassSpaceInertiaTensor;
            /// <summary>Virtual method pointer for `getMassSpaceInvInertiaTensor`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, PxVec3*, PxVec3*> getMassSpaceInvInertiaTensor;
            /// <summary>Virtual method pointer for `setLinearDamping`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, float, void> setLinearDamping;
            /// <summary>Virtual method pointer for `getLinearDamping`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, float> getLinearDamping;
            /// <summary>Virtual method pointer for `setAngularDamping`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, float, void> setAngularDamping;
            /// <summary>Virtual method pointer for `getAngularDamping`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, float> getAngularDamping;
            /// <summary>Virtual method pointer for `getLinearVelocity`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, PxVec3*, PxVec3*> getLinearVelocity;
            /// <summary>Virtual method pointer for `setLinearVelocity`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, PxVec3*, NativeBoolean, void> setLinearVelocity;
            /// <summary>Virtual method pointer for `getAngularVelocity`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, PxVec3*, PxVec3*> getAngularVelocity;
            /// <summary>Virtual method pointer for `setAngularVelocity`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, PxVec3*, NativeBoolean, void> setAngularVelocity;
            /// <summary>Virtual method pointer for `setMaxAngularVelocity`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, float, void> setMaxAngularVelocity;
            /// <summary>Virtual method pointer for `getMaxAngularVelocity`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, float> getMaxAngularVelocity;
            /// <summary>Virtual method pointer for `setMaxLinearVelocity`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, float, void> setMaxLinearVelocity;
            /// <summary>Virtual method pointer for `getMaxLinearVelocity`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, float> getMaxLinearVelocity;
            /// <summary>Virtual method pointer for `addForce`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, PxVec3*, PxForceMode, NativeBoolean, void> addForce;
            /// <summary>Virtual method pointer for `addTorque`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, PxVec3*, PxForceMode, NativeBoolean, void> addTorque;
            /// <summary>Virtual method pointer for `clearForce`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, PxForceMode, void> clearForce;
            /// <summary>Virtual method pointer for `clearTorque`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, PxForceMode, void> clearTorque;
            /// <summary>Virtual method pointer for `setForceAndTorque`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, PxVec3*, PxVec3*, PxForceMode, void> setForceAndTorque;
            /// <summary>Virtual method pointer for `setRigidBodyFlag`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, PxRigidBodyFlags, NativeBoolean, void> setRigidBodyFlag;
            /// <summary>Virtual method pointer for `setRigidBodyFlags`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, PxRigidBodyFlags*, void> setRigidBodyFlags;
            /// <summary>Virtual method pointer for `getRigidBodyFlags`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, PxRigidBodyFlags*, PxRigidBodyFlags*> getRigidBodyFlags;
            /// <summary>Virtual method pointer for `setMinCCDAdvanceCoefficient`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, float, void> setMinCCDAdvanceCoefficient;
            /// <summary>Virtual method pointer for `getMinCCDAdvanceCoefficient`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, float> getMinCCDAdvanceCoefficient;
            /// <summary>Virtual method pointer for `setMaxDepenetrationVelocity`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, float, void> setMaxDepenetrationVelocity;
            /// <summary>Virtual method pointer for `getMaxDepenetrationVelocity`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, float> getMaxDepenetrationVelocity;
            /// <summary>Virtual method pointer for `setMaxContactImpulse`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, float, void> setMaxContactImpulse;
            /// <summary>Virtual method pointer for `getMaxContactImpulse`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, float> getMaxContactImpulse;
            /// <summary>Virtual method pointer for `getInternalIslandNodeIndex`</summary>
            public delegate* unmanaged[Cdecl]<PxRigidBody*, uint> getInternalIslandNodeIndex;
        }
    }
}