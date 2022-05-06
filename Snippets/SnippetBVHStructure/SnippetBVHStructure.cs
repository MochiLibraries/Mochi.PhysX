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
// This snippet illustrates the usage of PxBVHStructure
//
// It creates a large number of small sphere shapes forming a large sphere. Large sphere
// represents an actor and the actor is inserted into the scene with BVHStructure
// that is precomputed from all the small spheres. When an actor is insterted this
// way the scene queries against this object behave actor centric rather than shape
// centric.
// Each actor that is added with a BVHSctructure does not update any of its shape bounds
// within a pruning structure. It does update just the actor bounds and the query then
// goes into actors bounds pruner, then a local query is done against the shapes in the
// actor.
// For a dynamic actor consisting of a large amound of shapes there can be a significant
// performance benefits. During fetch results, there is no need to synchronize all
// shape bounds into scene query system. Also when a new AABB tree is build inside
// scene query system these actors shapes are not contained there.
// ****************************************************************************

using Mochi.PhysX;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SnippetCommon;
using System;
using static Mochi.PhysX.Globals;

internal unsafe static class SnippetBVHStructure
{
    static Pinned<PxDefaultAllocator> gAllocator = new PxDefaultAllocator();
    static Pinned<PxDefaultErrorCallback> gErrorCallback = new PxDefaultErrorCallback();

    static PxFoundation* gFoundation = null;
    static PxPhysics* gPhysics = null;
    static PxCooking* gCooking = null;

    static PxDefaultCpuDispatcher* gDispatcher = null;
    static PxScene* gScene = null;

    static PxMaterial* gMaterial = null;

    static PxPvd* gPvd = null;

    static void createLargeSphere(in PxTransform t, uint density, float largeRadius, float radius, bool useAggregate)
    {
        PxRigidDynamic* body = gPhysics->createRigidDynamic(t);

        // generate the sphere shapes
        float gStep = MathF.PI / (float)(density);
        float tStep = 2.0f * MathF.PI / (float)(density);
        for (uint i = 0; i < density; i++)
        {
            for (uint j = 0; j < density; j++)
            {
                float sinG = PxSin(gStep * i);
                float cosG = PxCos(gStep * i);
                float sinT = PxSin(tStep * j);
                float cosT = PxCos(tStep * j);

                PxTransform localTm = new(new PxVec3(largeRadius * sinG * cosT, largeRadius * sinG * sinT, largeRadius * cosG));
                PxShape* shape = gPhysics->createShape(new PxSphereGeometry(radius), *gMaterial,
                    false, PxShapeFlags.eVISUALIZATION | PxShapeFlags.eSCENE_QUERY_SHAPE | PxShapeFlags.eSIMULATION_SHAPE); //BIOQUIRK: Missing defaults
                shape->setLocalPose(localTm);
                body->attachShape(ref *shape);
                shape->release();
            }
        }
        PxRigidBodyExt.updateMassAndInertia(ref *body, 10.0f);

        // get the bounds from the actor, this can be done through a helper function in PhysX extensions
        uint numBounds = 0;
        PxBounds3* bounds = PxRigidActorExt.getRigidActorShapeLocalBoundsList(*body, ref numBounds); //BIOQUIRK: Should this be out byref?

        // setup the PxBVHStructureDesc, it does contain only the PxBounds3 data
        PxBVHStructureDesc bvhDesc = new();
        bvhDesc.bounds.count = numBounds;
        bvhDesc.bounds.data = bounds;
        bvhDesc.bounds.stride = (uint)sizeof(PxBounds3);

        // cook the bvh structure
        PxBVHStructure* bvh = gCooking->createBVHStructure(bvhDesc, ref *gPhysics->getPhysicsInsertionCallback());

        // release the memory allocated within extensions, the bounds are not required anymore
        gAllocator.Value.deallocate(bounds);

        // add the actor to the scene and provide the bvh structure (regular path without aggregate usage)
        if (!useAggregate)
            gScene->addActor(ref *body, bvh);

        // Note that when objects with large amound of shapes are created it is also
        // recommended to create an aggregate from them, see the code below that would replace
        // the gScene->addActor(*body, bvh)
        if (useAggregate)
        {
            PxAggregate* aggregate = gPhysics->createAggregate(1, false);
            aggregate->addActor(ref *body, bvh);
            gScene->addAggregate(ref *aggregate);
        }


        // bvh can be released at this point, the precomputed BVH structure was copied to the SDK pruners.
        bvh->release();
    }

    public static void initPhysics(bool interactive)
    {
        gFoundation = PxCreateFoundation(PX_PHYSICS_VERSION, ref gAllocator.Value, ref gErrorCallback.Value);

        gPvd = PxCreatePvd(ref *gFoundation);
        PxPvdTransport* transport = PxDefaultPvdSocketTransportCreate(PVD_HOST, 5425, 10);
        gPvd->connect(ref *transport, PxPvdInstrumentationFlags.eALL);

        gPhysics = PxCreatePhysics(PX_PHYSICS_VERSION, ref *gFoundation, new PxTolerancesScale(), true, gPvd);
        gCooking = PxCreateCooking(PX_PHYSICS_VERSION, ref *gFoundation, new PxCookingParams(new PxTolerancesScale()));

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

        for (uint i = 0; i < 10; i++)
            createLargeSphere(new PxTransform(new PxVec3(200.0f * i, .0f, 100.0f)), 50, 30.0f, 1.0f, false);
    }

    public static void stepPhysics(bool interactive)
    {
        gScene->simulate(1.0f / 60.0f);
        gScene->fetchResults(true);
    }

    public static void cleanupPhysics(bool interactive)
    {
        PX_RELEASE(ref gScene);
        PX_RELEASE(ref gDispatcher);
        PX_RELEASE(ref gPhysics);
        PX_RELEASE(ref gCooking);
        if (gPvd != null)
        {
            PxPvdTransport* transport = gPvd->getTransport();
            gPvd->release();
            gPvd = null;
            PX_RELEASE(ref transport);
        }
        PX_RELEASE(ref gFoundation);

        Console.WriteLine("SnippetBVHStructure done.");
    }

    public static void keyPress(Keys key, in PxTransform camera)
    {
    }
}
