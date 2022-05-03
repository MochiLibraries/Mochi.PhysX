// Copyright (c) 2022 David Maas and Contributors. All rights reserved.
// Copyright (c) 2008-2021 NVIDIA Corporation. All rights reserved.
// Copyright (c) 2004-2008 AGEIA Technologies, Inc. All rights reserved.
// Copyright (c) 2001-2004 NovodeX AG. All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions
// are met:
//  * Redistributions of source code must retain the above copyright
//    notice, this list of conditions and the following disclaimer.
//  * Redistributions in binary form must reproduce the above copyright
//    notice, this list of conditions and the following disclaimer in the
//    documentation and/or other materials provided with the distribution.
//  * Neither the name of NVIDIA CORPORATION nor the names of its
//    contributors may be used to endorse or promote products derived
//    from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS ''AS IS'' AND ANY
// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
// PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE COPYRIGHT OWNER OR
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
// PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
// PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY
// OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

// ****************************************************************************
// This snippet illustrates simple use of joints in physx
//
// It creates a chain of objects joined by limited spherical joints, a chain
// joined by fixed joints which is breakable, and a chain of damped D6 joints
// ****************************************************************************
#pragma warning disable CS0414 // Unused field

using Mochi.PhysX;
using Mochi.PhysX.Infrastructure;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SnippetCommon;
using System;
using static Mochi.PhysX.Globals;
using static Mochi.PhysX.PxIDENTITY;

internal unsafe static class SnippetJoint
{
    static Pinned<PxDefaultAllocator> gAllocator = new PxDefaultAllocator();
    static Pinned<PxDefaultErrorCallback> gErrorCallback = new PxDefaultErrorCallback();

    static PxFoundation* gFoundation = null;
    static PxPhysics* gPhysics = null;

    static PxDefaultCpuDispatcher* gDispatcher = null;
    static PxScene* gScene = null;

    static PxMaterial* gMaterial = null;

    static PxPvd* gPvd = null;

    static float chainZ = 10.0f;

    static PxRigidDynamic* createDynamic<TGeometry>(in PxTransform t, in TGeometry geometry, in PxVec3 velocity = default)
        where TGeometry : unmanaged, IPxGeometry
    {
        PxRigidDynamic* dynamic = PxCreateDynamic(ref *gPhysics, t, geometry, ref *gMaterial, 10.0f, new PxTransform(PxIdentity)); //BIOQUIRK: Missing default parameter value
        dynamic->setAngularDamping(0.5f);
        dynamic->setLinearVelocity(velocity);
        gScene->addActor(ref *dynamic);
        return dynamic;
    }

    // spherical joint limited to an angle of at most pi/4 radians (45 degrees)
    static PxJoint* createLimitedSpherical(PxRigidActor* a0, in PxTransform t0, PxRigidActor* a1, in PxTransform t1)
    {
        PxSphericalJoint* j = PxSphericalJointCreate(ref *gPhysics, a0, t0, a1, t1);
        j->setLimitCone(new PxJointLimitCone(MathF.PI / 4, MathF.PI / 4, 0.05f));
        j->setSphericalJointFlag(PxSphericalJointFlags.eLIMIT_ENABLED, true);
        return (PxJoint*)j; //BIOQUIRK: Base cast
    }

    // revolute joint limited to an angle of at most pi/4 radians (45 degrees)

    // fixed, breakable joint
    static PxJoint* createBreakableFixed(PxRigidActor* a0, in PxTransform t0, PxRigidActor* a1, in PxTransform t1)
    {
        PxFixedJoint* j = PxFixedJointCreate(ref *gPhysics, a0, t0, a1, t1);
        j->setBreakForce(1000, 100000);
        j->setConstraintFlag(PxConstraintFlags.eDRIVE_LIMITS_ARE_FORCES, true);
        j->setConstraintFlag(PxConstraintFlags.eDISABLE_PREPROCESSING, true);
        return (PxJoint*)j; //BIOQUIRK: Base cast
    }

    // D6 joint with a spring maintaining its position
    static PxJoint* createDampedD6(PxRigidActor* a0, in PxTransform t0, PxRigidActor* a1, in PxTransform t1)
    {
        PxD6Joint* j = PxD6JointCreate(ref *gPhysics, a0, t0, a1, t1);
        j->setMotion(PxD6Axis.eSWING1, PxD6Motion.eFREE);
        j->setMotion(PxD6Axis.eSWING2, PxD6Motion.eFREE);
        j->setMotion(PxD6Axis.eTWIST, PxD6Motion.eFREE);
        j->setDrive(PxD6Drive.eSLERP, new PxD6JointDrive(0, 1000, float.MaxValue, true));
        return (PxJoint*)j; //BIOQUIRK: Base cast
    }

    //typedef PxJoint* (*JointCreateFunction)(PxRigidActor* a0, const PxTransform& t0, PxRigidActor* a1, const PxTransform& t1);

    // create a chain rooted at the origin and extending along the x-axis, all transformed by the argument t.

    static void createChain<TGeometry>(in PxTransform t, uint length, in TGeometry g, float separation, delegate*<PxRigidActor*, in PxTransform, PxRigidActor*, in PxTransform, PxJoint*> createJoint)
        where TGeometry : unmanaged, IPxGeometry
    {
        PxVec3 offset = new(separation / 2, 0, 0);
        PxTransform localTm = new(offset);
        PxRigidDynamic* prev = null;

        for (uint i = 0; i < length; i++)
        {
            PxRigidDynamic* current = PxCreateDynamic(ref *gPhysics, t.operator_Star(localTm), g, ref *gMaterial, 1.0f, new PxTransform(PxIdentity)); //BIOQUIRK: Operator overload -- BIOQUIRK: Missing default
            createJoint((PxRigidActor*)prev, prev != null ? new PxTransform(offset) : t, (PxRigidActor*)current, new PxTransform(offset.operator_Minus())); //BIOQUIRK: Operator overload -- BIOQUIRK: Base casts
            gScene->addActor(ref *current);
            prev = current;
            localTm.p.x += separation;
        }
    }

    public static void initPhysics(bool interactive)
    {
        gFoundation = PxCreateFoundation(PX_PHYSICS_VERSION, ref gAllocator.Value, ref gErrorCallback.Value);
        gPvd = PxCreatePvd(ref *gFoundation);
        PxPvdTransport* transport = PxDefaultPvdSocketTransportCreate(PVD_HOST, 5425, 10);
        gPvd->connect(ref *transport, PxPvdInstrumentationFlags.eALL);

        gPhysics = PxCreatePhysics(PX_PHYSICS_VERSION, ref *gFoundation, new PxTolerancesScale(), true, gPvd);
        PxInitExtensions(ref *gPhysics, gPvd);

        PxSceneDesc sceneDesc = new(*gPhysics->getTolerancesScale());
        sceneDesc.gravity = new PxVec3(0.0f, -9.81f, 0.0f);
        gDispatcher = PxDefaultCpuDispatcherCreate(2);
        sceneDesc.cpuDispatcher = (PxCpuDispatcher*)gDispatcher; //BIOQUIRK: Base cast
        sceneDesc.filterShader = PxDefaultSimulationFilterShader;
        gScene = gPhysics->createScene(sceneDesc);

        PxPvdSceneClient* pvdClient = gScene->getScenePvdClient();
        if (pvdClient != null)
        {
            pvdClient->setScenePvdFlag(PxPvdSceneFlags.eTRANSMIT_CONSTRAINTS, true);
            pvdClient->setScenePvdFlag(PxPvdSceneFlags.eTRANSMIT_CONTACTS, true);
            pvdClient->setScenePvdFlag(PxPvdSceneFlags.eTRANSMIT_SCENEQUERIES, true);
        }

        gMaterial = gPhysics->createMaterial(0.5f, 0.5f, 0.6f);

        PxRigidStatic* groundPlane = PxCreatePlane(ref *gPhysics, new PxPlane(0, 1, 0, 0), ref *gMaterial);
        gScene->addActor(ref *groundPlane);

        createChain(new PxTransform(new PxVec3(0.0f, 20.0f, 0.0f)), 5, new PxBoxGeometry(2.0f, 0.5f, 0.5f), 4.0f, &createLimitedSpherical);
        createChain(new PxTransform(new PxVec3(0.0f, 20.0f, -10.0f)), 5, new PxBoxGeometry(2.0f, 0.5f, 0.5f), 4.0f, &createBreakableFixed);
        createChain(new PxTransform(new PxVec3(0.0f, 20.0f, -20.0f)), 5, new PxBoxGeometry(2.0f, 0.5f, 0.5f), 4.0f, &createDampedD6);
    }

    public static void stepPhysics(bool interactive)
    {
        gScene->simulate(1.0f / 60.0f);
        gScene->fetchResults(true);
    }

    public static void cleanupPhysics(bool interactive)
    {
        if (gScene != null)
        {
            gScene->release();
            gScene = null;
        }

        if (gDispatcher != null)
        {
            gDispatcher->release();
            gDispatcher = null;
        }

        PxCloseExtensions();

        if (gPhysics != null)
        {
            gPhysics->release();
            gPhysics = null;
        }

        if (gPvd != null)
        {
            PxPvdTransport* transport = gPvd->getTransport();
            gPvd->release();
            gPvd = null;

            if (transport != null)
            {
                transport->release();
                transport = null;
            }
        }

        if (gFoundation != null)
        {
            gFoundation->release();
            gFoundation = null;
        }

        Console.WriteLine("SnippetJoint done.");
    }

    public static void keyPress(Keys key, in PxTransform camera)
    {
        switch (key)
        {
            case Keys.Space:
                createDynamic(camera, new PxSphereGeometry(3.0f), camera.rotate(new PxVec3(0, 0, -1)).operator_Star(200)); //BIOQUIRK: Overloaded operator
                break;
        }
    }
}
