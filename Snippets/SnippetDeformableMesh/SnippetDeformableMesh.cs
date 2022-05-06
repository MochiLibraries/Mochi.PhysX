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
// This snippet shows how to use deformable meshes in PhysX.
// ****************************************************************************
#pragma warning disable CS0414 // Unused field

using Mochi.PhysX;
using Mochi.PhysX.Infrastructure;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SnippetCommon;
using System;
using static Mochi.PhysX.Globals;
using static Mochi.PhysX.PxIDENTITY;

internal unsafe static class SnippetDeformableMesh
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

    static PxTriangleMesh* gMesh = null;
    static PxRigidStatic* gActor = null;

    static float stackZ = 10.0f;

    const uint gGridSize = 8;
    const float gGridStep = 512.0f / (float)(gGridSize - 1);
    static float gTime = 0.0f;

    static PxRigidDynamic* createDynamic<TGeometry>(in PxTransform t, in TGeometry geometry, in PxVec3 velocity = default, float density = 1.0f)
        where TGeometry : unmanaged, IPxGeometry
    {
        PxRigidDynamic* dynamic = PxCreateDynamic(ref *gPhysics, t, geometry, ref *gMaterial, density, new PxTransform(PxIdentity)); //BIOQUIRK: Missing default
        dynamic->setLinearVelocity(velocity);
        gScene->addActor(ref *dynamic);
        return dynamic;
    }

    static void createStack(in PxTransform t, uint size, float halfExtent)
    {
        PxShape* shape = gPhysics->createShape(new PxBoxGeometry(halfExtent, halfExtent, halfExtent), *gMaterial,
            false, PxShapeFlags.eVISUALIZATION | PxShapeFlags.eSCENE_QUERY_SHAPE | PxShapeFlags.eSIMULATION_SHAPE); //BIOQUIRK: Missing defaults
        for (uint i = 0; i < size; i++)
        {
            for (uint j = 0; j < size - i; j++)
            {
                PxTransform localTm = new(new PxVec3((float)(j * 2) - (float)(size - i), (float)(i * 2 + 1), 0).operator_Star(halfExtent)); //BIOQUIRK: Operator overload
                PxRigidDynamic* body = gPhysics->createRigidDynamic(t.transform(localTm));
                body->attachShape(ref *shape);
                PxRigidBodyExt.updateMassAndInertia(ref *body, 10.0f);
                gScene->addActor(ref *body);
            }
        }
        shape->release();
    }

    struct Triangle
    {
        public uint ind0, ind1, ind2;
    };

    static void updateVertices(PxVec3* verts, float amplitude = 0.0f)
    {
        const uint gridSize = gGridSize;
        const float gridStep = gGridStep;

        for (uint a = 0; a < gridSize; a++)
        {
            float coeffA = (float)(a) / (float)(gridSize);
            for (uint b = 0; b < gridSize; b++)
            {
                float coeffB = (float)(b) / (float)(gridSize);

                float y = 20.0f + MathF.Sin(coeffA * MathF.PI * 2f) * MathF.Cos(coeffB * MathF.PI * 2) * amplitude;

                verts[a * gridSize + b] = new PxVec3(-400.0f + b * gridStep, y, -400.0f + a * gridStep);
            }
        }
    }

    static PxTriangleMesh* createMeshGround()
    {
        const uint gridSize = gGridSize;

        PxVec3* verts = stackalloc PxVec3[(int)(gridSize * gridSize)];

        const uint nbTriangles = 2 * (gridSize - 1) * (gridSize - 1);

        Triangle* indices = stackalloc Triangle[(int)nbTriangles];

        updateVertices(verts);

        for (uint a = 0; a < (gridSize - 1); ++a)
        {
            for (uint b = 0; b < (gridSize - 1); ++b)
            {
                ref Triangle tri0 = ref indices[(a * (gridSize - 1) + b) * 2];
                ref Triangle tri1 = ref indices[((a * (gridSize - 1) + b) * 2) + 1];

                tri0.ind0 = a * gridSize + b + 1;
                tri0.ind1 = a * gridSize + b;
                tri0.ind2 = (a + 1) * gridSize + b + 1;

                tri1.ind0 = (a + 1) * gridSize + b + 1;
                tri1.ind1 = a * gridSize + b;
                tri1.ind2 = (a + 1) * gridSize + b;
            }
        }

        PxTriangleMeshDesc meshDesc = new();
        meshDesc.points.data = verts;
        meshDesc.points.count = gridSize * gridSize;
        meshDesc.points.stride = (uint)sizeof(PxVec3);
        meshDesc.triangles.count = nbTriangles;
        meshDesc.triangles.data = indices;
        meshDesc.triangles.stride = (uint)sizeof(Triangle);

        PxTriangleMesh* triMesh = gCooking->createTriangleMesh(meshDesc, ref *gPhysics->getPhysicsInsertionCallback());

        return triMesh;
    }

    public static void initPhysics(bool interactive)
    {
        gFoundation = PxCreateFoundation(PX_PHYSICS_VERSION, ref gAllocator.Value, ref gErrorCallback.Value);

        gPvd = PxCreatePvd(ref *gFoundation);
        PxPvdTransport* transport = PxDefaultPvdSocketTransportCreate(PVD_HOST, 5425, 10);
        gPvd->connect(ref *transport, PxPvdInstrumentationFlags.eALL);

        gPhysics = PxCreatePhysics(PX_PHYSICS_VERSION, ref *gFoundation, new PxTolerancesScale(), true, gPvd);

        PxCookingParams cookingParams = new(*gPhysics->getTolerancesScale());

        // Deformable meshes are only supported with PxMeshMidPhase.eBVH33.
        cookingParams.midphaseDesc.setToDefault(PxMeshMidPhase.eBVH33);
        // We need to disable the mesh cleaning part so that the vertex mapping remains untouched.
        cookingParams.meshPreprocessParams = PxMeshPreprocessingFlags.eDISABLE_CLEAN_MESH;

        gCooking = PxCreateCooking(PX_PHYSICS_VERSION, ref *gFoundation, cookingParams);

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

        PxTriangleMesh* mesh = createMeshGround();
        gMesh = mesh;

        PxTriangleMeshGeometry geom = new(mesh, new PxMeshScale(), default); //BIOQUIRK: Missing defaults -- BIOQUIRK: No `None` member for flags enum.

        PxRigidStatic* groundMesh = gPhysics->createRigidStatic(new PxTransform(new PxVec3(0, 2, 0)));
        gActor = groundMesh;
        PxShape* shape = gPhysics->createShape(geom, *gMaterial,
            false, PxShapeFlags.eVISUALIZATION | PxShapeFlags.eSCENE_QUERY_SHAPE | PxShapeFlags.eSIMULATION_SHAPE); //BIOQUIRK: Missing defaults

        {
            shape->setContactOffset(0.02f);
            // A negative rest offset helps to avoid jittering when the deformed mesh moves away from objects resting on it.
            shape->setRestOffset(-0.5f);
        }

        groundMesh->attachShape(ref *shape);
        gScene->addActor(ref *groundMesh);

        createStack(new PxTransform(new PxVec3(0, 22, 0)), 10, 2.0f);
    }

    public static void stepPhysics(bool interactive)
    {
        {
            PxVec3* verts = gMesh->getVerticesForModification();
            gTime += 0.01f;
            updateVertices(verts, MathF.Sin(gTime) * 20.0f);
            PxBounds3 newBounds = gMesh->refitBVH();

            // Reset filtering to tell the broadphase about the new mesh bounds.
            gScene->resetFiltering(ref *gActor);
        }
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

        Console.WriteLine("SnippetDeformableMesh done.");
    }

    public static void keyPress(Keys key, in PxTransform camera)
    {
        switch (key)
        {
            case Keys.Space:
                createDynamic(camera, new PxSphereGeometry(3.0f), camera.rotate(new PxVec3(0, 0, -1)).operator_Star(200), 3.0f); //BIOQUIRK: Overloaded operator
                break;
        }
    }
}
