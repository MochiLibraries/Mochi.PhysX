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

// *******************************************************************************************************
// In addition to the simulate() function, which performs both collision detection and dynamics update,
// the PhysX SDK provides an api for separate execution of the collision detection and dynamics update steps.
// We shall refer to this feature as "split sim". This snippet demonstrates two ways to use the split sim feature
// so that application work can be performed concurrently with the collision detection step.

// The snippet creates a list of kinematic box actors along with a number of dynamic actors that
// interact with the kinematic actors.

//The defines OVERLAP_COLLISION_AND_RENDER_WITH_NO_LAG and OVERLAP_COLLISION_AND_RENDER_WITH_ONE_FRAME_LAG
//demonstrate two distinct modes of split sim operation:

// (1)Enabling OVERLAP_COLLISION_AND_RENDER_WITH_NO_LAG allows the collision detection step to run in parallel
//    with the renderer and with the update of the kinematic target poses without introducing any lag between
//    application time and physics time.  This is equivalent to calling simulate() and fetchResults() with the key
//    difference being that the application can schedule work to run concurrently with the collision detection.
//    A consequence of this approach is that the first frame is more expensive than subsequent frames because it has to
//    perform blocking collision detection and dynamics update calls.

// (2)OVERLAP_COLLISION_AND_RENDER_WITH_ONE_FRAME_LAG also allows the collision to run in parallel with
//    the renderer and the update of the kinematic target poses but this time with a lag between physics time and
//    application time; that is, the physics is always a single timestep behind the application because the first
//    frame merely starts the collision detection for the subsequent frame.  A consequence of this approach is that
//    the first frame is cheaper than subsequent frames.
// ********************************************************************************************************

//This will allow the split sim to overlap collision and render and game logic.
#define OVERLAP_COLLISION_AND_RENDER_WITH_NO_LAG
//#define OVERLAP_COLLISION_AND_RENDER_WITH_ONE_FRAME_LAG

using Mochi.PhysX;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SnippetCommon;
using System;
using static Mochi.PhysX.Globals;
using static Mochi.PhysX.PxIDENTITY;

internal unsafe static class SnippetSplitSim
{
    static Pinned<PxDefaultAllocator> gAllocator = new PxDefaultAllocator();
    static Pinned<PxDefaultErrorCallback> gErrorCallback = new PxDefaultErrorCallback();

    static PxFoundation* gFoundation = null;
    static PxPhysics* gPhysics = null;

    static PxDefaultCpuDispatcher* gDispatcher = null;
    static PxScene* gScene = null;

    static PxMaterial* gMaterial = null;

    static PxPvd* gPvd = null;

    const int NB_KINE_X = 16;
    const int NB_KINE_Y = 16;
    const float KINE_SCALE = 3.1f;

    static bool isFirstFrame = true;

    static PxRigidDynamic*[,] gKinematics = new PxRigidDynamic*[NB_KINE_Y, NB_KINE_X];

    static PxQuat setRotY(out PxMat33 m, float angle)
    {
        m = new PxMat33(PxIdentity);

        float cos = MathF.Cos(angle);
        float sin = MathF.Sin(angle);

        //BIOQUIRK: Overloaded indexers
        //BIOQUIRK: Also the lifetime of these pointers is wildly illegal if the PxMat33 is on the managed heap
        *(m.operator_Subscript(0)->operator_Subscript(0)) = *(m.operator_Subscript(2)->operator_Subscript(2)) = cos;
        *(m.operator_Subscript(0)->operator_Subscript(2)) = -sin;
        *(m.operator_Subscript(2)->operator_Subscript(0)) = sin;

        return new PxQuat(m);
    }

    static void createDynamics()
    {
        const uint NbX = 8;
        const uint NbY = 8;

        PxVec3 dims = new(0.2f, 0.1f, 0.2f);
        const float sphereRadius = 0.2f;
        const float capsuleRadius = 0.2f;
        const float halfHeight = 0.5f;

        const uint NbLayers = 3;
        const float YScale = 0.4f;
        const float YStart = 6.0f;
        PxShape* boxShape = gPhysics->createShape(new PxBoxGeometry(dims), *gMaterial,
            false, PxShapeFlags.eVISUALIZATION | PxShapeFlags.eSCENE_QUERY_SHAPE | PxShapeFlags.eSIMULATION_SHAPE); //BIOQUIRK: Missing default arguments
        PxShape* sphereShape = gPhysics->createShape(new PxSphereGeometry(sphereRadius), *gMaterial,
            false, PxShapeFlags.eVISUALIZATION | PxShapeFlags.eSCENE_QUERY_SHAPE | PxShapeFlags.eSIMULATION_SHAPE); //BIOQUIRK: Missing default arguments
        PxShape* capsuleShape = gPhysics->createShape(new PxCapsuleGeometry(capsuleRadius, halfHeight), *gMaterial,
            false, PxShapeFlags.eVISUALIZATION | PxShapeFlags.eSCENE_QUERY_SHAPE | PxShapeFlags.eSIMULATION_SHAPE); //BIOQUIRK: Missing default arguments
        PxMat33 m;
        for (uint j = 0; j < NbLayers; j++)
        {
            float angle = (float)j * 0.08f;
            PxQuat rot = setRotY(out m, angle);

            const float ScaleX = 4.0f;
            const float ScaleY = 4.0f;

            for (uint y = 0; y < NbY; y++)
            {
                for (uint x = 0; x < NbX; x++)
                {
                    float xf = ((float)(x) - (float)(NbX) * 0.5f) * ScaleX;
                    float yf = ((float)(y) - (float)(NbY) * 0.5f) * ScaleY;

                    PxRigidDynamic* dynamic = null;

                    uint v = j & 3;
                    PxVec3 pos = new PxVec3(xf, YStart + (float)(j) * YScale, yf);

                    switch (v)
                    {
                        case 0:
                        {
                            PxTransform pose = new(pos, rot);
                            dynamic = gPhysics->createRigidDynamic(pose);
                            dynamic->attachShape(ref *boxShape);
                            break;
                        }
                        case 1:
                        {
                            PxTransform pose = new(pos, new PxQuat(PxIdentity));
                            dynamic = gPhysics->createRigidDynamic(pose);
                            dynamic->attachShape(ref *sphereShape);
                            break;
                        }
                        default:
                        {
                            PxTransform pose = new(pos, rot);
                            dynamic = gPhysics->createRigidDynamic(pose);
                            dynamic->attachShape(ref *capsuleShape);
                            break;
                        }
                    };

                    PxRigidBodyExt.updateMassAndInertia(ref *dynamic, 10f);

                    gScene->addActor(ref *dynamic);

                }
            }
        }
    }

    static void createGroudPlane()
    {
        PxTransform pose = new PxTransform(new PxVec3(0.0f, 0.0f, 0.0f), new PxQuat(MathF.PI * 0.5f, new PxVec3(0.0f, 0.0f, 1.0f)));
        PxRigidStatic* actor = gPhysics->createRigidStatic(pose);
        PxShape* shape = PxRigidActorExt.createExclusiveShape(ref *actor, new PxPlaneGeometry(), *gMaterial,
            PxShapeFlags.eVISUALIZATION | PxShapeFlags.eSCENE_QUERY_SHAPE | PxShapeFlags.eSIMULATION_SHAPE); //BIOQUIRK: Missing default
        gScene->addActor(ref *actor);
    }

    static void createKinematics()
    {
        const uint NbX = NB_KINE_X;
        const uint NbY = NB_KINE_Y;

        PxVec3 dims = new(1.5f, 0.2f, 1.5f);
        PxQuat rot = new PxQuat(PxIdentity);

        const float YScale = 0.4f;

        PxShape* shape = gPhysics->createShape(new PxBoxGeometry(dims), *gMaterial,
            false, PxShapeFlags.eVISUALIZATION | PxShapeFlags.eSCENE_QUERY_SHAPE | PxShapeFlags.eSIMULATION_SHAPE); //BIOQUIRK: Missing default arguments


        const float ScaleX = KINE_SCALE;
        const float ScaleY = KINE_SCALE;
        for (uint y = 0; y < NbY; y++)
        {
            for (uint x = 0; x < NbX; x++)
            {
                float xf = ((float)(x) - (float)(NbX) * 0.5f) * ScaleX;
                float yf = ((float)(y) - (float)(NbY) * 0.5f) * ScaleY;
                PxTransform pose = new(new PxVec3(xf, 0.2f + YScale, yf), rot);
                PxRigidDynamic* body = gPhysics->createRigidDynamic(pose);
                body->attachShape(ref *shape);
                gScene->addActor(ref *body);
                body->setRigidBodyFlag(PxRigidBodyFlags.eKINEMATIC, true);

                gKinematics[y, x] = body;
            }
        }

    }

    static float gTime = 0.0f;
    static void updateKinematics(float timeStep)
    {
        const float YScale = 0.4f;

        PxTransform motion;
        motion.q = new PxQuat(PxIdentity);

        //static float gTime = 0.0f;
        gTime += timeStep;

        const uint NbX = NB_KINE_X;
        const uint NbY = NB_KINE_Y;

        const float Coeff = 0.2f;

        const float ScaleX = KINE_SCALE;
        const float ScaleY = KINE_SCALE;
        for (uint y = 0; y < NbY; y++)
        {
            for (uint x = 0; x < NbX; x++)
            {
                float xf = ((float)(x) - (float)(NbX) * 0.5f) * ScaleX;
                float yf = ((float)(y) - (float)(NbY) * 0.5f) * ScaleY;

                float h = MathF.Sin(gTime * 2.0f + (float)(x) * Coeff + +(float)(y) * Coeff) * 2.0f;
                motion.p = new PxVec3(xf, h + 2.0f + YScale, yf);

                PxRigidDynamic* kine = gKinematics[y, x];
                kine->setKinematicTarget(motion);
            }
        }
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

        createKinematics();
        createDynamics();

    }

#if OVERLAP_COLLISION_AND_RENDER_WITH_NO_LAG
    public static void stepPhysics(bool interactive)
    {
        const float timeStep = 1.0f / 60.0f;

        if (isFirstFrame)
        {
            //Run the first frame's collision detection
            gScene->collide(timeStep);
            isFirstFrame = false;
        }
        //update the kinematice target pose in parallel with collision running
        updateKinematics(timeStep);
        gScene->fetchCollision(true);
        gScene->advance();
        gScene->fetchResults(true);

        //Run the deferred collision detection for the next frame. This will run in parallel with render.
        gScene->collide(timeStep);
    }
#elif OVERLAP_COLLISION_AND_RENDER_WITH_ONE_FRAME_LAG

    public static void stepPhysics(bool interactive)
    {
        float timeStep = 1.0f/60.0f;

        //update the kinematice target pose in parallel with collision running
        updateKinematics(timeStep);
        if(!isFirstFrame)
        {
            gScene->fetchCollision(true);
            gScene->advance();
            gScene->fetchResults(true);
        }

        isFirstFrame = false;
        //Run the deferred collision detection for the next frame. This will run in parallel with render.
        gScene->collide(timeStep);
    }

#else

    public static void stepPhysics(bool interactive)
    {
        float timeStep = 1.0f/60.0f;
        //update the kinematice target pose in parallel with collision running
        gScene->collide(timeStep);
        updateKinematics(timeStep);
        gScene->fetchCollision(true);
        gScene->advance();
        gScene->fetchResults(true);
    }
#endif

    public static void cleanupPhysics(bool interactive)
    {
#if OVERLAP_COLLISION_AND_RENDER_WITH_NO_LAG || OVERLAP_COLLISION_AND_RENDER_WITH_ONE_FRAME_LAG
        //Close out remainder of previously running scene. If we don't do this, it will be implicitly done
        //in gScene->release() but a warning will be issued.
        gScene->fetchCollision(true);
        gScene->advance();
        gScene->fetchResults(true);
#endif

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

        Console.WriteLine("SnippetSplitSim done.");
    }

    public static void keyPress(Keys key, in PxTransform camer)
    {
    }
}

