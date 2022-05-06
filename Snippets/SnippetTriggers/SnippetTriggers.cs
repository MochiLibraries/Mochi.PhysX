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
// This snippet illustrates the use of built-in triggers, and how to emulate
// them with regular shapes if you need CCD or trigger-trigger notifications.
//
// ****************************************************************************

using Mochi.PhysX;
using Mochi.PhysX.Infrastructure;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SnippetCommon;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static Mochi.PhysX.Globals;
using static Mochi.PhysX.PxIDENTITY;
using static SnippetTriggers.TriggerImpl;

internal unsafe static class SnippetTriggers
{
    internal enum TriggerImpl
    {
        // Uses built-in triggers (PxShapeFlag::eTRIGGER_SHAPE).
        REAL_TRIGGERS,

        // Emulates triggers using a filter shader. Needs one reserved value in PxFilterData.
        FILTER_SHADER,

        // Emulates triggers using a filter callback. Doesn't use PxFilterData but needs user-defined way to mark a shape as a trigger.
        FILTER_CALLBACK,
    }

    struct ScenarioData
    {
        public TriggerImpl mImpl;
        public bool mCCD;
        public bool mTriggerTrigger;

        public ScenarioData(TriggerImpl impl, bool ccd, bool triggerTrigger)
            => (mImpl, mCCD, mTriggerTrigger) = (impl, ccd, triggerTrigger);
    }

    static int SCENARIO_COUNT => gData.Length;

    static ScenarioData[] gData = new ScenarioData[]
    {
        new(REAL_TRIGGERS,     false,  false),
        new(FILTER_SHADER,     false,  false),
        new(FILTER_CALLBACK,   false,  false),
        new(REAL_TRIGGERS,     true,   false),
        new(FILTER_SHADER,     true,   false),
        new(FILTER_CALLBACK,   true,   false),
        new(REAL_TRIGGERS,     false,  true),
        new(FILTER_SHADER,     false,  true),
        new(FILTER_CALLBACK,   false,  true),
    };

    static int gScenario = 0;

    static TriggerImpl getImpl() => gData[gScenario].mImpl;
    static bool usesCCD() => gData[gScenario].mCCD;
    static bool usesTriggerTrigger() => gData[gScenario].mTriggerTrigger;

    static Pinned<PxDefaultAllocator> gAllocator = new PxDefaultAllocator();
    static Pinned<PxDefaultErrorCallback> gErrorCallback = new PxDefaultErrorCallback();

    static PxFoundation* gFoundation = null;
    static PxPhysics* gPhysics = null;

    static PxDefaultCpuDispatcher* gDispatcher = null;
    static PxScene* gScene = null;
    static PxMaterial* gMaterial = null;
    static PxPvd* gPvd = null;

    static bool gPause = false;
    static bool gOneFrame = false;

    // Detects a trigger using the shape's simulation filter data. See createTriggerShape() function.
    static bool isTrigger(in PxFilterData data)
    {
        if (data.word0 != 0xffffffff)
            return false;
        if (data.word1 != 0xffffffff)
            return false;
        if (data.word2 != 0xffffffff)
            return false;
        if (data.word3 != 0xffffffff)
            return false;
        return true;
    }

    internal static bool isTriggerShape(PxShape* shape)
    {
        TriggerImpl impl = getImpl();

        // Detects native built-in triggers.
        if (impl == REAL_TRIGGERS && shape->getFlags().HasFlag(PxShapeFlags.eTRIGGER_SHAPE))
            return true;

        // Detects our emulated triggers using the simulation filter data. See createTriggerShape() function.
        if (impl == FILTER_SHADER && isTrigger(shape->getSimulationFilterData()))
            return true;

        // Detects our emulated triggers using the simulation filter callback. See createTriggerShape() function.
        if (impl == FILTER_CALLBACK && shape->userData != null)
            return true;

        return false;
    }

    //BIOQUIRK: Exposed return buffer
    //BOPQUIRK: PxPairFlags should ideally be a byref
    //BIOQUIRK: Both PxFilterData parameters are implicity byref
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    static PxFilterFlags* triggersUsingFilterShader(PxFilterFlags* __retBuf, uint attributes0, PxFilterData* filterData0,
                                                   uint attributes1, PxFilterData* filterData1,
                                                   PxPairFlags* pairFlags, void* constantBlock, uint constantBlockSize)
    {
        // Console.WriteLine("contactReportFilterShader");

        Debug.Assert(getImpl() == FILTER_SHADER);

        // We need to detect whether one of the shapes is a trigger.
        bool isTriggerPair = isTrigger(*filterData0) || isTrigger(*filterData1);

        // If we have a trigger, replicate the trigger codepath from PxDefaultSimulationFilterShader
        if (isTriggerPair)
        {
            *pairFlags = PxPairFlags.eTRIGGER_DEFAULT;

            if (usesCCD())
                *pairFlags |= PxPairFlags.eDETECT_CCD_CONTACT;

            *__retBuf = PxFilterFlags.eDEFAULT;
            return __retBuf;
        }
        else
        {
            // Otherwise use the default flags for regular pairs
            *pairFlags = PxPairFlags.eCONTACT_DEFAULT;
            *__retBuf = PxFilterFlags.eDEFAULT;
            return __retBuf;
        }
    }

    //BIOQUIRK: Exposed return buffer
    //BOPQUIRK: PxPairFlags should ideally be a byref
    //BIOQUIRK: Both PxFilterData parameters are implicity byref
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    static PxFilterFlags* triggersUsingFilterCallback(PxFilterFlags* __retBuf, uint attributes0, PxFilterData* filterData0,
                                                     uint attributes1, PxFilterData* filterData1,
                                                     PxPairFlags* pairFlags, void* constantBlock, uint constantBlockSize)
    {
        // Console.WriteLine("contactReportFilterShader");

        Debug.Assert(getImpl() == FILTER_CALLBACK);

        *pairFlags = PxPairFlags.eCONTACT_DEFAULT;

        if (usesCCD())
            *pairFlags |= PxPairFlags.eDETECT_CCD_CONTACT | PxPairFlags.eNOTIFY_TOUCH_CCD;

        *__retBuf = PxFilterFlags.eCALLBACK;
        return __retBuf;
    }

    struct TriggersFilterCallback
    {
        private PxSimulationFilterCallback Base;

        private static Pinned<PxSimulationFilterCallback.VirtualMethodTable> VTable;

        //BIOQUIRK: This junk should ideally be handled by a source generator or something
        // (If it does get handled by a source generator it also needs to be threadsafe, which this is not.)
        public TriggersFilterCallback()
        {
            if (VTable.IsDefault)
            {
                VTable = new PxSimulationFilterCallback.VirtualMethodTable()
                {
                    pairFound = &pairFound,
                    pairLost = &pairLost,
                    statusChange = &statusChange,
                    //BIOQUIRK: Method is missing so vtable entry is untyped
                    __DeletingDestructorPointer = (delegate* unmanaged[Cdecl]<PxSimulationFilterCallback*, void>)&Destructor
                };
            }

            Base = new()
            {
                VirtualMethodTablePointer = VTable
            };
        }

        //BIOQUIRK: Exposed return buffer
        //BIOQUIRK: PxPairFlags should ideally be byref
        //BIOQUIRK: PxFilterData is implicit byref
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static PxFilterFlags* pairFound(PxSimulationFilterCallback* @this, PxFilterFlags* _retBuf, uint pairID,
            uint attributes0, PxFilterData* filterData0, PxActor* a0, PxShape* s0,
            uint attributes1, PxFilterData* filterData1, PxActor* a1, PxShape* s1,
            PxPairFlags* pairFlags)
        {
            // Console.WriteLine("pairFound");

            if (s0->userData != null || s1->userData != null) // See createTriggerShape() function
            {
                *pairFlags = PxPairFlags.eTRIGGER_DEFAULT;

                if (usesCCD())
                    *pairFlags |= PxPairFlags.eDETECT_CCD_CONTACT | PxPairFlags.eNOTIFY_TOUCH_CCD;
            }
            else
                *pairFlags = PxPairFlags.eCONTACT_DEFAULT;

            *_retBuf = default;
            return _retBuf;
        }

        //BIOQUIRK: PxFilterData is implicit byref
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static void pairLost(PxSimulationFilterCallback* @this, uint pairID,
            uint attributes0, PxFilterData* filterData0,
            uint attributes1, PxFilterData* filterData1,
            NativeBoolean objectRemoved)
        {
            // Console.WriteLine("pairLost");
        }

        //BIOQUIRK: All of these parameters should be byref
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static NativeBoolean statusChange(PxSimulationFilterCallback* @this, uint* pairID, PxPairFlags* pairFlags, PxFilterFlags* filterFlags)
        {
            // Console.WriteLine("statusChange");
            return false;
        }

        //BIOQUIRK: Should just use no-op base implementation
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static void Destructor(PxSimulationFilterCallback* @this)
        { }
    }

    static Pinned<TriggersFilterCallback> gTriggersFilterCallback = new TriggersFilterCallback();

    struct ContactReportCallback
    {
        private PxSimulationEventCallback Base;

        private static Pinned<PxSimulationEventCallback.VirtualMethodTable> VTable;

        //BIOQUIRK: This junk should ideally be handled by a source generator or something
        // (If it does get handled by a source generator it also needs to be threadsafe, which this is not.)
        public ContactReportCallback()
        {
            if (VTable.IsDefault)
            {
                VTable = new PxSimulationEventCallback.VirtualMethodTable()
                {
                    onConstraintBreak = &onConstraintBreak,
                    onWake = &onWake,
                    onSleep = &onSleep,
                    onContact = &onContact,
                    onTrigger = &onTrigger,
                    onAdvance = &onAdvance,
                    __DeletingDestructorPointer = &Destructor
                };
            }

            Base = new()
            {
                VirtualMethodTablePointer = VTable
            };
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static void onConstraintBreak(PxSimulationEventCallback* @this, PxConstraintInfo* constraints, uint count)
            => Console.WriteLine("onConstraintBreak");

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static void onWake(PxSimulationEventCallback* @this, PxActor** actors, uint count)
            => Console.WriteLine("onWake");

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static void onSleep(PxSimulationEventCallback* @this, PxActor** actors, uint count)
            => Console.WriteLine("onSleep");

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static void onTrigger(PxSimulationEventCallback* @this, PxTriggerPair* pairs, uint count)
        {
            // Console.WriteLine($"onTrigger: {count} trigger pairs");
            while ((count--) != 0)
            {
                ref PxTriggerPair current = ref *pairs++;
                if (current.status.HasFlag(PxPairFlags.eNOTIFY_TOUCH_FOUND))
                    Console.WriteLine("Shape is entering trigger volume");
                if (current.status.HasFlag(PxPairFlags.eNOTIFY_TOUCH_LOST))
                    Console.WriteLine("Shape is leaving trigger volume");
            }
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static void onAdvance(PxSimulationEventCallback* @this, PxRigidBody** bodyBuffer, PxTransform* poseBuffer, uint count)
            => Console.WriteLine("onAdvance");

        //BIOQUIRK: pairHeader should be byref
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static void onContact(PxSimulationEventCallback* @this, PxContactPairHeader* pairHeader, PxContactPair* pairs, uint count)
        {
            // Console.WriteLine($"onContact: {count} pairs");

            while ((count--) != 0)
            {
                ref PxContactPair current = ref *pairs++;

                // The reported pairs can be trigger pairs or not. We only enabled contact reports for
                // trigger pairs in the filter shader, so we don't need to do further checks here. In a
                // real-world scenario you would probably need a way to tell whether one of the shapes
                // is a trigger or not. You could e.g. reuse the PxFilterData like we did in the filter
                // shader, or maybe use the shape's userData to identify triggers, or maybe put triggers
                // in a hash-set and test the reported shape pointers against it. Many options here.

                if ((current.events & (PxPairFlags.eNOTIFY_TOUCH_FOUND | PxPairFlags.eNOTIFY_TOUCH_CCD)) != 0)
                    Console.WriteLine("Shape is entering trigger volume");
                if (current.events.HasFlag(PxPairFlags.eNOTIFY_TOUCH_LOST))
                    Console.WriteLine("Shape is leaving trigger volume");

                if (isTriggerShape(current.shapes[0]) && isTriggerShape(current.shapes[1]))
                    Console.WriteLine("Trigger-trigger overlap detected");
            }
        }

        //BIOQUIRK: Should just use no-op base implementation
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static void Destructor(PxSimulationEventCallback* @this)
        { }
    }

    static Pinned<ContactReportCallback> gContactReportCallback = new ContactReportCallback();

    static PxShape* createTriggerShape<TGeometry>(in TGeometry geom, bool isExclusive)
        where TGeometry : unmanaged, IPxGeometry
    {
        TriggerImpl impl = getImpl();

        PxShape* shape = null;
        if (impl == REAL_TRIGGERS)
        {
            const PxShapeFlags shapeFlags = PxShapeFlags.eVISUALIZATION | PxShapeFlags.eTRIGGER_SHAPE;
            shape = gPhysics->createShape(geom, *gMaterial, isExclusive, shapeFlags);
        }
        else if (impl == FILTER_SHADER)
        {
            PxShapeFlags shapeFlags = PxShapeFlags.eVISUALIZATION | PxShapeFlags.eSIMULATION_SHAPE;
            shape = gPhysics->createShape(geom, *gMaterial, isExclusive, shapeFlags);

            // For this method to work, you need a way to mark shapes as triggers without using PxShapeFlag::eTRIGGER_SHAPE
            // (so that trigger-trigger pairs are reported), and without calling a PxShape function (so that the data is
            // available in a filter shader).
            //
            // One way is to reserve a special PxFilterData value/mask for triggers. It may not always be possible depending
            // on how you otherwise use the filter data).
            PxFilterData triggerFilterData = new(0xffffffff, 0xffffffff, 0xffffffff, 0xffffffff);
            shape->setSimulationFilterData(triggerFilterData);
        }
        else if (impl == FILTER_CALLBACK)
        {
            // We will have access to shape pointers in the filter callback so we just mark triggers in an arbitrary way here,
            // for example using the shape's userData.
            PxShapeFlags _flags = PxShapeFlags.eVISUALIZATION | PxShapeFlags.eSCENE_QUERY_SHAPE | PxShapeFlags.eSIMULATION_SHAPE; //BIOQUIRK: Missing default
            shape = gPhysics->createShape(geom, *gMaterial, isExclusive, _flags);
            shape->userData = shape;    // Arbitrary rule: it's a trigger if non null
        }
        return shape;
    }

    static void createDefaultScene()
    {
        bool ccd = usesCCD();
        PxQuat defaultQuaternion = new(PxIdentity); //BIOQUIRK: Missing default

        // Create trigger shape
        {
            PxVec3 halfExtent = new(10.0f, ccd ? 0.01f : 1.0f, 10.0f);
            PxShape* shape = createTriggerShape(new PxBoxGeometry(halfExtent), false);

            if (shape != null)
            {
                PxRigidStatic* body = gPhysics->createRigidStatic(new PxTransform(0.0f, 10.0f, 0.0f, defaultQuaternion));
                body->attachShape(ref *shape);
                gScene->addActor(ref *body);
                shape->release();
            }
        }

        // Create falling rigid body
        {
            PxVec3 halfExtent = new(ccd ? 0.1f : 1.0f);

            PxShapeFlags _flags = PxShapeFlags.eVISUALIZATION | PxShapeFlags.eSCENE_QUERY_SHAPE | PxShapeFlags.eSIMULATION_SHAPE; //BIOQUIRK: Missing default
            PxShape* shape = gPhysics->createShape(new PxBoxGeometry(halfExtent), *gMaterial, false, _flags); //BIOQUIRK: Missing default

            PxRigidDynamic* body = gPhysics->createRigidDynamic(new PxTransform(0.0f, ccd ? 30.0f : 20.0f, 0.0f, defaultQuaternion));
            body->attachShape(ref *shape);

            PxRigidBodyExt.updateMassAndInertia(ref *body, 1.0f);
            gScene->addActor(ref *body);
            shape->release();

            if (ccd)
            {
                body->setRigidBodyFlag(PxRigidBodyFlags.eENABLE_CCD, true);
                body->setLinearVelocity(new PxVec3(0.0f, -140.0f, 0.0f));
            }
        }
    }

    static void createTriggerTriggerScene()
    {
        static void createSphereActor(in PxVec3 pos, in PxVec3 linVel)
        {
            PxShapeFlags _flags = PxShapeFlags.eVISUALIZATION | PxShapeFlags.eSCENE_QUERY_SHAPE | PxShapeFlags.eSIMULATION_SHAPE; //BIOQUIRK: Missing default
            PxShape* sphereShape = gPhysics->createShape(new PxSphereGeometry(1.0f), *gMaterial, false, _flags);

            PxRigidDynamic* body = gPhysics->createRigidDynamic(new PxTransform(pos));
            body->attachShape(ref *sphereShape);

            PxShape* triggerShape = createTriggerShape(new PxSphereGeometry(4.0f), true);
            body->attachShape(ref *triggerShape);

            bool isTriggershape = triggerShape->getFlags().HasFlag(PxShapeFlags.eTRIGGER_SHAPE);
            if (!isTriggershape)
                triggerShape->setFlag(PxShapeFlags.eSIMULATION_SHAPE, false);
            PxRigidBodyExt.updateMassAndInertia(ref *body, 1.0f);
            if (!isTriggershape)
                triggerShape->setFlag(PxShapeFlags.eSIMULATION_SHAPE, true);
            gScene->addActor(ref *body);
            sphereShape->release();
            triggerShape->release();

            body->setLinearVelocity(linVel);
        }

        createSphereActor(new PxVec3(-5.0f, 1.0f, 0.0f), new PxVec3(1.0f, 0.0f, 0.0f));
        createSphereActor(new PxVec3(5.0f, 1.0f, 0.0f), new PxVec3(-1.0f, 0.0f, 0.0f));
    }

    static void initScene()
    {
        TriggerImpl impl = getImpl();

        PxSceneDesc sceneDesc = new(*gPhysics->getTolerancesScale());
        // sceneDesc.flags &= ~PxSceneFlags.eENABLE_PCM;
        sceneDesc.cpuDispatcher = (PxCpuDispatcher*)gDispatcher; //BIOQUIRK: Base cast
        sceneDesc.gravity = new PxVec3(0, -9.81f, 0);
        sceneDesc.simulationEventCallback = (PxSimulationEventCallback*)gContactReportCallback; //BIOQUIRK: Base cast
        if (impl == REAL_TRIGGERS)
        {
            sceneDesc.filterShader = PxDefaultSimulationFilterShader;
            Console.WriteLine("- Using built-in triggers.");
        }
        else if (impl == FILTER_SHADER)
        {
            sceneDesc.filterShader = &triggersUsingFilterShader;
            Console.WriteLine("- Using regular shapes emulating triggers with a filter shader.");
        }
        else if (impl == FILTER_CALLBACK)
        {
            sceneDesc.filterShader = &triggersUsingFilterCallback;
            sceneDesc.filterCallback = (PxSimulationFilterCallback*)gTriggersFilterCallback; //BIOQUIRK: Base cast
            Console.WriteLine("- Using regular shapes emulating triggers with a filter callback.");
        }

        if (usesCCD())
        {
            sceneDesc.flags |= PxSceneFlags.eENABLE_CCD;
            Console.WriteLine("- Using CCD.");
        }
        else
        {
            Console.WriteLine("- Using no CCD.");
        }

        gScene = gPhysics->createScene(sceneDesc);

        PxPvdSceneClient* pvdClient = gScene->getScenePvdClient();
        if (pvdClient != null)
            pvdClient->setScenePvdFlag(PxPvdSceneFlags.eTRANSMIT_CONTACTS, true);

        PxRigidStatic* groundPlane = PxCreatePlane(ref *gPhysics, new PxPlane(0, 1, 0, 0), ref *gMaterial);
        gScene->addActor(ref *groundPlane);

        if (usesTriggerTrigger())
            createTriggerTriggerScene();
        else
            createDefaultScene();
    }

    static void releaseScene()
    {
        PX_RELEASE(ref gScene);
    }

    public static void stepPhysics(bool interactive)
    {
        if (gPause && !gOneFrame)
            return;
        gOneFrame = false;

        if (gScene != null)
        {
            gScene->simulate(1.0f / 60.0f);
            gScene->fetchResults(true);
        }
    }

    public static void initPhysics(bool interactive)
    {
        Console.WriteLine("Press keys F1 to F9 to select a scenario.");

        gFoundation = PxCreateFoundation(PX_PHYSICS_VERSION, ref gAllocator.Value, ref gErrorCallback.Value);
        gPvd = PxCreatePvd(ref *gFoundation);
        PxPvdTransport* transport = PxDefaultPvdSocketTransportCreate(PVD_HOST, 5425, 10);
        gPvd->connect(ref *transport, PxPvdInstrumentationFlags.eALL);
        gPhysics = PxCreatePhysics(PX_PHYSICS_VERSION, ref *gFoundation, new PxTolerancesScale(), true, gPvd);
        PxInitExtensions(ref *gPhysics, gPvd);
        uint numCores = SnippetUtils.getNbPhysicalCores();
        gDispatcher = PxDefaultCpuDispatcherCreate(numCores == 0 ? 0 : numCores - 1);
        gMaterial = gPhysics->createMaterial(1.0f, 1.0f, 0.0f);

        initScene();
    }

    public static void cleanupPhysics(bool interactive)
    {
        releaseScene();

        PX_RELEASE(ref gDispatcher);
        PxCloseExtensions();
        PX_RELEASE(ref gPhysics);
        if (gPvd != null)
        {
            PxPvdTransport* transport = gPvd->getTransport();
            PX_RELEASE(ref gPvd);
            PX_RELEASE(ref transport);
        }
        PX_RELEASE(ref gFoundation);

        Console.WriteLine("SnippetTriggers done.");
    }

    public static void keyPress(Keys key, in PxTransform camera)
    {
        if (key == Keys.P)
            gPause = !gPause;

        if (key == Keys.O)
        {
            gPause = true;
            gOneFrame = true;
        }

        if (gScene != null)
        {
            if (key >= Keys.F1 && key < (Keys.F1 + SCENARIO_COUNT))
            {
                gScenario = key - Keys.F1;
                releaseScene();
                initScene();
            }

            if (key == Keys.R)
            {
                releaseScene();
                initScene();
            }
        }
    }
}
