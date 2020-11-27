// This file was automatically generated by Biohazrd and should not be modified by hand!
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit, Size = 24)]
public unsafe partial struct PxRigidBody
{
    [FieldOffset(0)] public PxRigidActor Base;

    public unsafe void setCMassLocalPose(PxTransform* pose)
    {
        fixed (PxRigidBody* @this = &this)
        { VirtualMethodTablePointer->setCMassLocalPose(@this, pose); }
    }

    public unsafe PxTransform getCMassLocalPose()
    {
        fixed (PxRigidBody* @this = &this)
        {
            PxTransform __returnBuffer;
            VirtualMethodTablePointer->getCMassLocalPose(@this, out __returnBuffer);
            return __returnBuffer;
        }
    }

    public unsafe void setMass(float mass)
    {
        fixed (PxRigidBody* @this = &this)
        { VirtualMethodTablePointer->setMass(@this, mass); }
    }

    public unsafe float getMass()
    {
        fixed (PxRigidBody* @this = &this)
        { return VirtualMethodTablePointer->getMass(@this); }
    }

    public unsafe float getInvMass()
    {
        fixed (PxRigidBody* @this = &this)
        { return VirtualMethodTablePointer->getInvMass(@this); }
    }

    public unsafe void setMassSpaceInertiaTensor(PxVec3* m)
    {
        fixed (PxRigidBody* @this = &this)
        { VirtualMethodTablePointer->setMassSpaceInertiaTensor(@this, m); }
    }

    public unsafe PxVec3 getMassSpaceInertiaTensor()
    {
        fixed (PxRigidBody* @this = &this)
        {
            PxVec3 __returnBuffer;
            VirtualMethodTablePointer->getMassSpaceInertiaTensor(@this, out __returnBuffer);
            return __returnBuffer;
        }
    }

    public unsafe PxVec3 getMassSpaceInvInertiaTensor()
    {
        fixed (PxRigidBody* @this = &this)
        {
            PxVec3 __returnBuffer;
            VirtualMethodTablePointer->getMassSpaceInvInertiaTensor(@this, out __returnBuffer);
            return __returnBuffer;
        }
    }

    public unsafe void setLinearDamping(float linDamp)
    {
        fixed (PxRigidBody* @this = &this)
        { VirtualMethodTablePointer->setLinearDamping(@this, linDamp); }
    }

    public unsafe float getLinearDamping()
    {
        fixed (PxRigidBody* @this = &this)
        { return VirtualMethodTablePointer->getLinearDamping(@this); }
    }

    public unsafe void setAngularDamping(float angDamp)
    {
        fixed (PxRigidBody* @this = &this)
        { VirtualMethodTablePointer->setAngularDamping(@this, angDamp); }
    }

    public unsafe float getAngularDamping()
    {
        fixed (PxRigidBody* @this = &this)
        { return VirtualMethodTablePointer->getAngularDamping(@this); }
    }

    public unsafe PxVec3 getLinearVelocity()
    {
        fixed (PxRigidBody* @this = &this)
        {
            PxVec3 __returnBuffer;
            VirtualMethodTablePointer->getLinearVelocity(@this, out __returnBuffer);
            return __returnBuffer;
        }
    }

    public unsafe void setLinearVelocity(PxVec3* linVel, bool autowake = true)
    {
        fixed (PxRigidBody* @this = &this)
        { VirtualMethodTablePointer->setLinearVelocity(@this, linVel, autowake); }
    }

    public unsafe PxVec3 getAngularVelocity()
    {
        fixed (PxRigidBody* @this = &this)
        {
            PxVec3 __returnBuffer;
            VirtualMethodTablePointer->getAngularVelocity(@this, out __returnBuffer);
            return __returnBuffer;
        }
    }

    public unsafe void setAngularVelocity(PxVec3* angVel, bool autowake = true)
    {
        fixed (PxRigidBody* @this = &this)
        { VirtualMethodTablePointer->setAngularVelocity(@this, angVel, autowake); }
    }

    public unsafe void setMaxAngularVelocity(float maxAngVel)
    {
        fixed (PxRigidBody* @this = &this)
        { VirtualMethodTablePointer->setMaxAngularVelocity(@this, maxAngVel); }
    }

    public unsafe float getMaxAngularVelocity()
    {
        fixed (PxRigidBody* @this = &this)
        { return VirtualMethodTablePointer->getMaxAngularVelocity(@this); }
    }

    public unsafe void setMaxLinearVelocity(float maxLinVel)
    {
        fixed (PxRigidBody* @this = &this)
        { VirtualMethodTablePointer->setMaxLinearVelocity(@this, maxLinVel); }
    }

    public unsafe float getMaxLinearVelocity()
    {
        fixed (PxRigidBody* @this = &this)
        { return VirtualMethodTablePointer->getMaxLinearVelocity(@this); }
    }

    public unsafe void addForce(PxVec3* force, PxForceMode mode = PxForceMode.eFORCE, bool autowake = true)
    {
        fixed (PxRigidBody* @this = &this)
        { VirtualMethodTablePointer->addForce(@this, force, mode, autowake); }
    }

    public unsafe void addTorque(PxVec3* torque, PxForceMode mode = PxForceMode.eFORCE, bool autowake = true)
    {
        fixed (PxRigidBody* @this = &this)
        { VirtualMethodTablePointer->addTorque(@this, torque, mode, autowake); }
    }

    public unsafe void clearForce(PxForceMode mode = PxForceMode.eFORCE)
    {
        fixed (PxRigidBody* @this = &this)
        { VirtualMethodTablePointer->clearForce(@this, mode); }
    }

    public unsafe void clearTorque(PxForceMode mode = PxForceMode.eFORCE)
    {
        fixed (PxRigidBody* @this = &this)
        { VirtualMethodTablePointer->clearTorque(@this, mode); }
    }

    public unsafe void setForceAndTorque(PxVec3* force, PxVec3* torque, PxForceMode mode = PxForceMode.eFORCE)
    {
        fixed (PxRigidBody* @this = &this)
        { VirtualMethodTablePointer->setForceAndTorque(@this, force, torque, mode); }
    }

    public unsafe void setRigidBodyFlag(PxRigidBodyFlags flag, bool value)
    {
        fixed (PxRigidBody* @this = &this)
        { VirtualMethodTablePointer->setRigidBodyFlag(@this, flag, value); }
    }

    public unsafe void setRigidBodyFlags(PxRigidBodyFlags inFlags)
    {
        fixed (PxRigidBody* @this = &this)
        { VirtualMethodTablePointer->setRigidBodyFlags(@this, inFlags); }
    }

    public unsafe PxRigidBodyFlags getRigidBodyFlags()
    {
        fixed (PxRigidBody* @this = &this)
        { return VirtualMethodTablePointer->getRigidBodyFlags(@this); }
    }

    public unsafe void setMinCCDAdvanceCoefficient(float advanceCoefficient)
    {
        fixed (PxRigidBody* @this = &this)
        { VirtualMethodTablePointer->setMinCCDAdvanceCoefficient(@this, advanceCoefficient); }
    }

    public unsafe float getMinCCDAdvanceCoefficient()
    {
        fixed (PxRigidBody* @this = &this)
        { return VirtualMethodTablePointer->getMinCCDAdvanceCoefficient(@this); }
    }

    public unsafe void setMaxDepenetrationVelocity(float biasClamp)
    {
        fixed (PxRigidBody* @this = &this)
        { VirtualMethodTablePointer->setMaxDepenetrationVelocity(@this, biasClamp); }
    }

    public unsafe float getMaxDepenetrationVelocity()
    {
        fixed (PxRigidBody* @this = &this)
        { return VirtualMethodTablePointer->getMaxDepenetrationVelocity(@this); }
    }

    public unsafe void setMaxContactImpulse(float maxImpulse)
    {
        fixed (PxRigidBody* @this = &this)
        { VirtualMethodTablePointer->setMaxContactImpulse(@this, maxImpulse); }
    }

    public unsafe float getMaxContactImpulse()
    {
        fixed (PxRigidBody* @this = &this)
        { return VirtualMethodTablePointer->getMaxContactImpulse(@this); }
    }

    public unsafe uint getInternalIslandNodeIndex()
    {
        fixed (PxRigidBody* @this = &this)
        { return VirtualMethodTablePointer->getInternalIslandNodeIndex(@this); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??0PxRigidBody@physx@@IEAA@GV?$PxFlags@W4Enum@PxBaseFlag@physx@@G@1@@Z", ExactSpelling = true)]
    private static extern void Constructor_PInvoke(PxRigidBody* @this, ushort concreteType, PxBaseFlags baseFlags);

    public unsafe void Constructor(ushort concreteType, PxBaseFlags baseFlags)
    {
        fixed (PxRigidBody* @this = &this)
        { Constructor_PInvoke(@this, concreteType, baseFlags); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??0PxRigidBody@physx@@IEAA@V?$PxFlags@W4Enum@PxBaseFlag@physx@@G@1@@Z", ExactSpelling = true)]
    private static extern void Constructor_PInvoke(PxRigidBody* @this, PxBaseFlags baseFlags);

    public unsafe void Constructor(PxBaseFlags baseFlags)
    {
        fixed (PxRigidBody* @this = &this)
        { Constructor_PInvoke(@this, baseFlags); }
    }

    public unsafe void Destructor()
    {
        fixed (PxRigidBody* @this = &this)
        { VirtualMethodTablePointer->__DeletingDestructorPointer(@this); }
    }

    public unsafe bool isKindOf(byte* name)
    {
        fixed (PxRigidBody* @this = &this)
        { return VirtualMethodTablePointer->isKindOf(@this, name); }
    }


    [FieldOffset(0)] public VirtualMethodTable* VirtualMethodTablePointer;

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct VirtualMethodTable
    {
        /// <summary>Virtual method pointer for `release`</summary>
        public delegate* unmanaged[Cdecl]<PxRigidActor*, void> release;
        /// <summary>Virtual method pointer for `getConcreteTypeName`</summary>
        public delegate* unmanaged[Cdecl]<PxBase*, byte*> getConcreteTypeName;
        /// <summary>Virtual method pointer for `isReleasable`</summary>
        public delegate* unmanaged[Cdecl]<PxBase*, NativeBoolean> isReleasable;
        /// <summary>Virtual method pointer for `~PxRigidBody`</summary>
        public delegate* unmanaged[Cdecl]<PxRigidBody*, void> __DeletingDestructorPointer;
        /// <summary>Virtual method pointer for `isKindOf`</summary>
        public delegate* unmanaged[Cdecl]<PxRigidBody*, byte*, NativeBoolean> isKindOf;
        /// <summary>Virtual method pointer for `getType`</summary>
        public delegate* unmanaged[Cdecl]<PxActor*, PxActorType> getType;
        /// <summary>Virtual method pointer for `getScene`</summary>
        public delegate* unmanaged[Cdecl]<PxActor*, PxScene*> getScene;
        /// <summary>Virtual method pointer for `setName`</summary>
        public delegate* unmanaged[Cdecl]<PxActor*, byte*, void> setName;
        /// <summary>Virtual method pointer for `getName`</summary>
        public delegate* unmanaged[Cdecl]<PxActor*, byte*> getName;
        /// <summary>Virtual method pointer for `getWorldBounds`</summary>
        public delegate* unmanaged[Cdecl]<PxActor*, out PxBounds3, float, void> getWorldBounds;
        /// <summary>Virtual method pointer for `setActorFlag`</summary>
        public delegate* unmanaged[Cdecl]<PxActor*, PxActorFlags, NativeBoolean, void> setActorFlag;
        /// <summary>Virtual method pointer for `setActorFlags`</summary>
        public delegate* unmanaged[Cdecl]<PxActor*, PxActorFlags, void> setActorFlags;
        /// <summary>Virtual method pointer for `getActorFlags`</summary>
        public delegate* unmanaged[Cdecl]<PxActor*, PxActorFlags> getActorFlags;
        /// <summary>Virtual method pointer for `setDominanceGroup`</summary>
        public delegate* unmanaged[Cdecl]<PxActor*, byte, void> setDominanceGroup;
        /// <summary>Virtual method pointer for `getDominanceGroup`</summary>
        public delegate* unmanaged[Cdecl]<PxActor*, byte> getDominanceGroup;
        /// <summary>Virtual method pointer for `setOwnerClient`</summary>
        public delegate* unmanaged[Cdecl]<PxActor*, byte, void> setOwnerClient;
        /// <summary>Virtual method pointer for `getOwnerClient`</summary>
        public delegate* unmanaged[Cdecl]<PxActor*, byte> getOwnerClient;
        /// <summary>Virtual method pointer for `getAggregate`</summary>
        public delegate* unmanaged[Cdecl]<PxActor*, PxAggregate*> getAggregate;
        /// <summary>Virtual method pointer for `getGlobalPose`</summary>
        public delegate* unmanaged[Cdecl]<PxRigidActor*, out PxTransform, void> getGlobalPose;
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
        /// <summary>Virtual method pointer for `setCMassLocalPose`</summary>
        public delegate* unmanaged[Cdecl]<PxRigidBody*, PxTransform*, void> setCMassLocalPose;
        /// <summary>Virtual method pointer for `getCMassLocalPose`</summary>
        public delegate* unmanaged[Cdecl]<PxRigidBody*, out PxTransform, void> getCMassLocalPose;
        /// <summary>Virtual method pointer for `setMass`</summary>
        public delegate* unmanaged[Cdecl]<PxRigidBody*, float, void> setMass;
        /// <summary>Virtual method pointer for `getMass`</summary>
        public delegate* unmanaged[Cdecl]<PxRigidBody*, float> getMass;
        /// <summary>Virtual method pointer for `getInvMass`</summary>
        public delegate* unmanaged[Cdecl]<PxRigidBody*, float> getInvMass;
        /// <summary>Virtual method pointer for `setMassSpaceInertiaTensor`</summary>
        public delegate* unmanaged[Cdecl]<PxRigidBody*, PxVec3*, void> setMassSpaceInertiaTensor;
        /// <summary>Virtual method pointer for `getMassSpaceInertiaTensor`</summary>
        public delegate* unmanaged[Cdecl]<PxRigidBody*, out PxVec3, void> getMassSpaceInertiaTensor;
        /// <summary>Virtual method pointer for `getMassSpaceInvInertiaTensor`</summary>
        public delegate* unmanaged[Cdecl]<PxRigidBody*, out PxVec3, void> getMassSpaceInvInertiaTensor;
        /// <summary>Virtual method pointer for `setLinearDamping`</summary>
        public delegate* unmanaged[Cdecl]<PxRigidBody*, float, void> setLinearDamping;
        /// <summary>Virtual method pointer for `getLinearDamping`</summary>
        public delegate* unmanaged[Cdecl]<PxRigidBody*, float> getLinearDamping;
        /// <summary>Virtual method pointer for `setAngularDamping`</summary>
        public delegate* unmanaged[Cdecl]<PxRigidBody*, float, void> setAngularDamping;
        /// <summary>Virtual method pointer for `getAngularDamping`</summary>
        public delegate* unmanaged[Cdecl]<PxRigidBody*, float> getAngularDamping;
        /// <summary>Virtual method pointer for `getLinearVelocity`</summary>
        public delegate* unmanaged[Cdecl]<PxRigidBody*, out PxVec3, void> getLinearVelocity;
        /// <summary>Virtual method pointer for `setLinearVelocity`</summary>
        public delegate* unmanaged[Cdecl]<PxRigidBody*, PxVec3*, NativeBoolean, void> setLinearVelocity;
        /// <summary>Virtual method pointer for `getAngularVelocity`</summary>
        public delegate* unmanaged[Cdecl]<PxRigidBody*, out PxVec3, void> getAngularVelocity;
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
        public delegate* unmanaged[Cdecl]<PxRigidBody*, PxRigidBodyFlags, void> setRigidBodyFlags;
        /// <summary>Virtual method pointer for `getRigidBodyFlags`</summary>
        public delegate* unmanaged[Cdecl]<PxRigidBody*, PxRigidBodyFlags> getRigidBodyFlags;
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