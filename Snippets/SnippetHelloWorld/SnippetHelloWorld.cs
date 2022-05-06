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
// This snippet illustrates simple use of physx
//
// It creates a number of box stacks on a plane, and if rendering, allows the
// user to create new stacks and fire a ball from the camera position
// ****************************************************************************

using Mochi.PhysX;
using Mochi.PhysX.Infrastructure;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SnippetCommon;
using System;
using static Mochi.PhysX.Globals;
using static Mochi.PhysX.PxIDENTITY;

internal unsafe static class SnippetHelloWorld
{
    static Pinned<PxDefaultAllocator> gAllocator = new PxDefaultAllocator();
    static Pinned<PxDefaultErrorCallback> gErrorCallback = new PxDefaultErrorCallback();

    static PxFoundation* gFoundation = null;
    static PxPhysics* gPhysics = null;

    static PxDefaultCpuDispatcher* gDispatcher = null;
    static PxScene* gScene = null;

    static PxMaterial* gMaterial = null;

    static PxPvd* gPvd = null;

    static float stackZ = 10.0f;

    static PxRigidDynamic* createDynamic<TGeometry>(in PxTransform t, in TGeometry geometry, in PxVec3 velocity = default)
        where TGeometry : unmanaged, IPxGeometry
    {
        PxRigidDynamic* dynamic = PxCreateDynamic(ref *gPhysics, t, geometry, ref *gMaterial, 10.0f, new(PxIdentity)); //BIOQUIRK: Missing default
        dynamic->setAngularDamping(0.5f);
        dynamic->setLinearVelocity(velocity);
        gScene->addActor(ref *dynamic);
        return dynamic;
    }

    static void createStack(in PxTransform t, uint size, float halfExtent)
    {
        PxShape* shape = gPhysics->createShape(new PxBoxGeometry(halfExtent, halfExtent, halfExtent), *gMaterial, false, PxShapeFlags.eVISUALIZATION | PxShapeFlags.eSCENE_QUERY_SHAPE | PxShapeFlags.eSIMULATION_SHAPE); //BIOQUIRK: Missing default arguments
        for (uint i = 0; i < size; i++)
        {
            for (uint j = 0; j < size - i; j++)
            {
                PxTransform localTm = new(new PxVec3((float)(j * 2) - (float)(size - i), (float)(i * 2 + 1), 0).operator_Star(halfExtent)); //BIOQUIRK: Overloaded operator
                PxRigidDynamic* body = gPhysics->createRigidDynamic(t.transform(localTm));
                body->attachShape(ref *shape);
                PxRigidBodyExt.updateMassAndInertia(ref *body, 10.0f);
                gScene->addActor(ref *body);
            }
        }
        shape->release();
    }

    public static void initPhysics(bool interactive)
    {
        gFoundation = PxCreateFoundation(PX_PHYSICS_VERSION, ref gAllocator.Value, ref gErrorCallback.Value);

        gPvd = PxCreatePvd(ref *gFoundation);
        PxPvdTransport* transport = PxDefaultPvdSocketTransportCreate(PVD_HOST, 5425, 10);
        gPvd->connect(ref *transport, PxPvdInstrumentationFlags.eALL);

        gPhysics = PxCreatePhysics(PX_PHYSICS_VERSION, ref *gFoundation, new PxTolerancesScale(), true, gPvd);

        PxSceneDesc sceneDesc = new(*gPhysics->getTolerancesScale());
        sceneDesc.gravity = new PxVec3(0.0f, -9.81f, 0.0f);
        gDispatcher = PxDefaultCpuDispatcherCreate(2);
        sceneDesc.cpuDispatcher = (PxCpuDispatcher*)gDispatcher; //BIOQUIRK: Awkward base cast
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

        for (uint i = 0; i < 5; i++)
            createStack(new PxTransform(new PxVec3(0, 0, stackZ -= 10.0f)), 10, 2.0f);

        if (!interactive)
            createDynamic(new PxTransform(new PxVec3(0, 40, 100)), new PxSphereGeometry(10), new PxVec3(0, -50, -100));
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
        if (gPvd != null)
        {
            PxPvdTransport* transport = gPvd->getTransport();
            gPvd->release();
            gPvd = null;
            PX_RELEASE(ref transport);
        }
        PX_RELEASE(ref gFoundation);

        Console.WriteLine("SnippetHelloWorld done.");
    }

    public static void keyPress(Keys key, in PxTransform camera)
    {
        switch (key)
        {
            case Keys.B:
                createStack(new PxTransform(new PxVec3(0, 0, stackZ -= 10.0f)), 10, 2.0f);
                break;
            case Keys.Space:
                createDynamic(camera, new PxSphereGeometry(3.0f), camera.rotate(new PxVec3(0, 0, -1)).operator_Star(200)); //BIOQUIRK: Overloaded operator
                break;
        }
    }
}
