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
// This snippet illustrates the use of binary and xml serialization
//
// It creates a chain of boxes and serializes them as two collections:
// a collection with shared objects and a collection with actors and joints
// which can be instantiated multiple times.
//
// Then physics is setup based on the serialized data. The collection with the
// actors and the joints is instantiated multiple times with different
// transforms.
//
// Finally phyics is teared down again, including deallocation of memory
// occupied by deserialized objects (in the case of binary serialization).
//
// ****************************************************************************

using Mochi.PhysX;
using Mochi.PhysX.Infrastructure;
using SnippetCommon;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static Mochi.PhysX.Globals;

internal unsafe static class SnippetSerialization
{
    public static bool gUseBinarySerialization = false;

    static Pinned<PxDefaultAllocator> gAllocator = new PxDefaultAllocator();
    static Pinned<PxDefaultErrorCallback> gErrorCallback = new PxDefaultErrorCallback();

    static PxFoundation* gFoundation = null;
    static PxPhysics* gPhysics = null;
    static PxCooking* gCooking = null;

    static PxDefaultCpuDispatcher* gDispatcher = null;
    static PxScene* gScene = null;

    static PxPvd* gPvd = null;
    const uint MAX_MEMBLOCKS = 10;
    static byte*[] gMemBlocks = new byte*[MAX_MEMBLOCKS];
    static uint gMemBlockCount = 0;

    /**
    Creates two example collections: 
    - collection with actors and joints that can be instantiated multiple times in the scene
    - collection with shared objects
    */
    static void createCollections(ref PxCollection* sharedCollection, ref PxCollection* actorCollection, ref PxSerializationRegistry sr)
    {
        PxMaterial* material = gPhysics->createMaterial(0.5f, 0.5f, 0.6f);

        float halfLength = 2.0f, height = 25.0f;
        PxVec3 offset = new(halfLength, 0, 0);
        PxRigidActor* prevActor = (PxRigidActor*)PxCreateStatic(ref *gPhysics, new PxTransform(new PxVec3(0, height, 0)), new PxSphereGeometry(halfLength), ref *material, new PxTransform(offset)); //BIOQUIRK: Base cast

        PxShape* shape = gPhysics->createShape(new PxBoxGeometry(halfLength, 1.0f, 1.0f), *material,
            false, PxShapeFlags.eVISUALIZATION | PxShapeFlags.eSCENE_QUERY_SHAPE | PxShapeFlags.eSIMULATION_SHAPE); //BIOQUIRK: Missing defaults
        for (uint i = 1; i < 8; i++)
        {
            PxTransform tm = new(new PxVec3((float)(i * 2) * halfLength, height, 0));
            PxRigidDynamic* dynamic = gPhysics->createRigidDynamic(tm);
            dynamic->attachShape(ref *shape);
            PxRigidBodyExt.updateMassAndInertia(ref *dynamic, 10.0f);

            PxSphericalJointCreate(ref *gPhysics, prevActor, new PxTransform(offset), dynamic, new PxTransform(offset.operator_Minus())); //BIOQUIRK: Operator overload
            prevActor = (PxRigidActor*)dynamic; //BIOQUIRK: Base cast
        }

        sharedCollection = PxCreateCollection(); // collection for all the shared objects
        actorCollection = PxCreateCollection(); // collection for all the nonshared objects

        sharedCollection->add(ref *shape);
        PxSerialization.complete(ref *sharedCollection, ref sr); // chases the pointer from shape to material, and adds it
        PxSerialization.createSerialObjectIds(ref *sharedCollection, 77); // arbitrary choice of base for references to shared objects

        actorCollection->add(ref *prevActor);
        PxSerialization.complete(ref *actorCollection, ref sr, sharedCollection, true); // chases all pointers and recursively adds actors and joints
    }

    /**
    Allocates 128 byte aligned memory block for binary serialized data 
    Stores pointer to memory in gMemBlocks for later deallocation
    */
    static void* createAlignedBlock(uint size)
    {
        Debug.Assert(gMemBlockCount < MAX_MEMBLOCKS);
        byte* baseAddr = (byte*)NativeMemory.Alloc(size + PX_SERIAL_FILE_ALIGN - 1);
        gMemBlocks[gMemBlockCount++] = baseAddr;
        void* alignedBlock = (void*)(((nint)(baseAddr) + PX_SERIAL_FILE_ALIGN - 1) & ~(PX_SERIAL_FILE_ALIGN - 1));
        return alignedBlock;
    }

    /**
    Create objects, add them to collections and serialize the collections to the steams gSharedStream and gActorStream
    This function doesn't setup the gPhysics global as the corresponding physics object is only used locally 
    */
    public static void serializeObjects<TSharedStream, TActorStream>(ref TSharedStream sharedStream, ref TActorStream actorStream)
        where TSharedStream : unmanaged, IPxOutputStream
        where TActorStream : unmanaged, IPxOutputStream
    {
        PxSerializationRegistry* sr = PxSerialization.createSerializationRegistry(ref *gPhysics);

        PxCollection* sharedCollection = null;
        PxCollection* actorCollection = null;
        createCollections(ref sharedCollection, ref actorCollection, ref *sr);

        // Alternatively to using PxDefaultMemoryOutputStream it would be possible to serialize to files using 
        // PxDefaultFileOutputStream or a similar implementation of PxOutputStream.
        if (gUseBinarySerialization)
        {
            PxSerialization.serializeCollectionToBinary(ref sharedStream, ref *sharedCollection, ref *sr);
            PxSerialization.serializeCollectionToBinary(ref actorStream, ref *actorCollection, ref *sr, sharedCollection);
        }
        else
        {
            PxSerialization.serializeCollectionToXml(ref sharedStream, ref *sharedCollection, ref *sr);
            PxSerialization.serializeCollectionToXml(ref actorStream, ref *actorCollection, ref *sr, null, sharedCollection);
        }

        actorCollection->release();
        sharedCollection->release();

        sr->release();
    }

    /**
    Deserialize shared data and use resulting collection to deserialize and instance actor collections
    */
    public static void deserializeObjects(ref PxInputData sharedData, ref PxInputData actorData)
    {
        PxSerializationRegistry* sr = PxSerialization.createSerializationRegistry(ref *gPhysics);

        PxCollection* sharedCollection = null;
        {
            if (gUseBinarySerialization)
            {
                void* alignedBlock = createAlignedBlock(sharedData.getLength());
                sharedData.read(alignedBlock, sharedData.getLength());
                sharedCollection = PxSerialization.createCollectionFromBinary(alignedBlock, ref *sr);
            }
            else
            {
                sharedCollection = PxSerialization.createCollectionFromXml(ref sharedData, ref *gCooking, ref *sr);
            }
        }

        // Deserialize collection and instantiate objects twice, each time with a different transform
        PxTransform* transforms = stackalloc PxTransform[2] { new PxTransform(new PxVec3(-5.0f, 0.0f, 0.0f)), new PxTransform(new PxVec3(5.0f, 0.0f, 0.0f)) };

        for (uint i = 0; i < 2; i++)
        {
            PxCollection* collection = null;

            // If the PxInputData actorData would refer to a file, it would be better to avoid reading from it twice.
            // This could be achieved by reading the file once to memory, and then working with copies.
            // This is particulary practical when using binary serialization, where the data can be directly 
            // converted to physics objects.
            actorData.seek(0);

            if (gUseBinarySerialization)
            {
                void* alignedBlock = createAlignedBlock(actorData.getLength());
                actorData.read(alignedBlock, actorData.getLength());
                collection = PxSerialization.createCollectionFromBinary(alignedBlock, ref *sr, sharedCollection);
            }
            else
            {
                collection = PxSerialization.createCollectionFromXml(ref actorData, ref *gCooking, ref *sr, sharedCollection);
            }

            for (uint o = 0; o < collection->getNbObjects(); o++)
            {
                //BIOQUIRK: is<T> is not translated https://github.com/MochiLibraries/Mochi.PhysX/issues/11
                //PxRigidActor* rigidActor = collection->getObject(o).is<PxRigidActor>();
                PxRigidActor* rigidActor = null;
                {
                    PxBase* obj = collection->getObject(o);
                    //BIOQUIRK: Because PxRigidActor is not concrete, this ends up going down the isKindOf path, which is not publicly accessible.
                    // This is special-cased for this snippet, ideally we should just expose is<T> in a more C#-friendly way.
                    if ((PxConcreteType)obj->getConcreteType() is PxConcreteType.eRIGID_STATIC or PxConcreteType.eARTICULATION_LINK or PxConcreteType.eRIGID_DYNAMIC)
                    { rigidActor = (PxRigidActor*)obj; }
                }

                if (rigidActor != null)
                {
                    PxTransform globalPose = rigidActor->getGlobalPose();
                    globalPose = globalPose.transform(transforms[i]);
                    rigidActor->setGlobalPose(globalPose);
                }
            }

            gScene->addCollection(*collection);
            collection->release();
        }
        sharedCollection->release();

        PxMaterial* material;
        gPhysics->getMaterials(&material, 1);
        PxRigidStatic* groundPlane = PxCreatePlane(ref *gPhysics, new PxPlane(0, 1, 0, 0), ref *material);
        gScene->addActor(ref *groundPlane);
        sr->release();
    }

    public static void deserializeObjects<TSharedData, TActorData>(ref TSharedData sharedData, ref TActorData actorData)
        where TSharedData : unmanaged, IPxInputData
        where TActorData : unmanaged, IPxInputData
        //BIOQUIRK: Generic casts
        => deserializeObjects(ref Unsafe.As<TSharedData, PxInputData>(ref sharedData), ref Unsafe.As<TActorData, PxInputData>(ref actorData));

    /**
    Initializes physics and creates a scene
    */
    public static void initPhysics()
    {
        gFoundation = PxCreateFoundation(PX_PHYSICS_VERSION, ref gAllocator.Value, ref gErrorCallback.Value);
        gPvd = PxCreatePvd(ref *gFoundation);
        PxPvdTransport* transport = PxDefaultPvdSocketTransportCreate(PVD_HOST, 5425, 10);
        gPvd->connect(ref *transport, PxPvdInstrumentationFlags.eALL);

        gPhysics = PxCreatePhysics(PX_PHYSICS_VERSION, ref *gFoundation, new PxTolerancesScale(), true, gPvd);
        gCooking = PxCreateCooking(PX_PHYSICS_VERSION, ref *gFoundation, new PxCookingParams(new PxTolerancesScale()));
        PxInitExtensions(ref *gPhysics, gPvd);

        uint numCores = SnippetUtils.getNbPhysicalCores();
        gDispatcher = PxDefaultCpuDispatcherCreate(numCores == 0 ? 0 : numCores - 1);
        PxSceneDesc sceneDesc = new(*gPhysics->getTolerancesScale());
        sceneDesc.gravity = new PxVec3(0, -9.81f, 0);
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
    }

    public static void stepPhysics()
    {
        gScene->simulate(1.0f / 60.0f);
        gScene->fetchResults(true);
    }

    /**
    Releases all physics objects, including memory blocks containing deserialized data
    */
    public static void cleanupPhysics()
    {
        PX_RELEASE(ref gScene);
        PX_RELEASE(ref gDispatcher);
        PxCloseExtensions();

        PX_RELEASE(ref gPhysics); // releases all objects 
        PX_RELEASE(ref gCooking);
        if (gPvd != null)
        {
            PxPvdTransport* transport = gPvd->getTransport();
            gPvd->release();
            gPvd = null;
            PX_RELEASE(ref transport);
        }

        // Now that the objects have been released, it's safe to release the space they occupy
        for (uint i = 0; i < gMemBlockCount; i++)
            NativeMemory.Free(gMemBlocks[i]);

        gMemBlockCount = 0;

        PX_RELEASE(ref gFoundation);
    }
}
