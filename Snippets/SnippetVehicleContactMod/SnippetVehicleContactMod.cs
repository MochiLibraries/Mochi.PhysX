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
// This snippet illustrates how to use contact modification and sweeps to allow wheels to
// collide and react more naturally with the scene.  In particular, the snippet shows how to
// use contact modification and ccd contact modification to select or ignore rigid body contact
// points between a shape representing a wheel and any other shape in the scene.  The snippet
// also demonstrates the use of suspension sweeps instead of suspension raycasts.

// The snippet creates various static capsules with different radii, a ground plane, and a
// vehicle.  The capsules are configured to be drivable surfaces.  Additionally, the
// capsule and wheel shapes are configured with simulation filter data so that
// they
//  (i)  collide with each other with discrete collision detection
// (ii)  collide with each other with continuous collision detection (CCD)
//(iii)  trigger a contact modification callback
// (iv)  trigger a ccd contact modification callback

// The contact modification callback is implemented with the class WheelContactModifyCallback.
// The function WheelContactModifyCallback::onContactModify identifies shape pairs that contain
// a wheel.  Contact points for the shape pair are ignored or accepted with the SDK function
// PxVehicleModifyWheelContacts.  CCD contact modification is implemented with the class
// WheelCCDContactModifyCallback.  The function WheelCCDContactModifyCallback::onContactModify
// performs exactly the same role as WheelContactModifyCallback::onContactModify

// The threshold values POINT_REJECT_ANGLE and NORMAL_REJECT_ANGLE can be tuned
// to modify the conditions under which wheel contact points are ignored or accepted.

// It is a good idea to record and playback with pvd (PhysX Visual Debugger).
// ****************************************************************************

//PhysX Vehicles support blocking and non-blocking sweeps.
//Experiment with this define to switch between the two regimes.
//#define BLOCKING_SWEEPS

using Mochi.PhysX;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SnippetCommon;
using SnippetVehicleCommon;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static Mochi.PhysX.Globals;
using static Mochi.PhysX.PxIDENTITY;
using static SnippetVehicleCommon.CollisionFlags;
using static SnippetVehicleCommon.Globals;

internal unsafe static class SnippetVehicleContactMod
{
    static Pinned<PxDefaultAllocator> gAllocator = new PxDefaultAllocator();
    static Pinned<PxDefaultErrorCallback> gErrorCallback = new PxDefaultErrorCallback();

    static PxFoundation* gFoundation = null;
    static PxPhysics* gPhysics = null;

    static PxDefaultCpuDispatcher* gDispatcher = null;
    public static PxScene* gScene = null;

    static PxCooking* gCooking = null;

    static PxMaterial* gMaterial = null;

    static PxPvd* gPvd = null;

    static VehicleSceneQueryData* gVehicleSceneQueryData = null;
    static PxBatchQuery* gBatchQuery = null;

    static PxVehicleDrivableSurfaceToTireFrictionPairs* gFrictionPairs = null;

    const int NUM_VEHICLES = 2;
    static PxRigidStatic* gGroundPlane = null;
    static PxVehicleDrive4W*[] gVehicle4W = new PxVehicleDrive4W*[NUM_VEHICLES];

    static PinnedArray<ActorUserData> gActorUserData = new PinnedArray<ActorUserData>(NUM_VEHICLES);
    static PinnedArray<ShapeUserData>[] gShapeUserDatas = new PinnedArray<ShapeUserData>[NUM_VEHICLES]
    {
        new PinnedArray<ShapeUserData>(PX_MAX_NB_WHEELS),
        new PinnedArray<ShapeUserData>(PX_MAX_NB_WHEELS)
    };
    static float[] xCoordVehicleStarts = new float[NUM_VEHICLES] { 0.0f, 20.0f };

    //Angle thresholds used to categorize contacts as suspension contacts or rigid body contacts.
    const float POINT_REJECT_ANGLE = MathF.PI / 4.0f;
    const float NORMAL_REJECT_ANGLE = MathF.PI / 4.0f;

    //Contact modification values.
    const float WHEEL_TANGENT_VELOCITY_MULTIPLIER = 0.1f;
    const float MAX_IMPULSE = float.MaxValue;

    //Define the maximum acceleration for dynamic bodies under the wheel.
    const float MAX_ACCELERATION = 50.0f;

    //Blocking sweeps require sweep hit buffers for just 1 hit per wheel.
    //Non-blocking sweeps require more hits per wheel because they return all touches on the swept shape.
#if BLOCKING_SWEEPS
    const ushort gNbQueryHitsPerWheel = 1;
#else
    const ushort gNbQueryHitsPerWheel = 8;
#endif

    //The class WheelContactModifyCallback identifies and modifies rigid body contacts
    //that involve a wheel.  Contacts that can be identified and managed by the suspension
    //system are ignored.  Any contacts that remain are modified to account for the rotation
    //speed of the wheel around the rolling axis.
    struct WheelContactModifyCallback
    {
        private PxContactModifyCallback Base;
        private static Pinned<PxContactModifyCallback.VirtualMethodTable> VTable;

        //BIOQUIRK: This junk should ideally be handled by a source generator or something
        // (If it does get handled by a source generator it also needs to be threadsafe, which this is not.)
        public WheelContactModifyCallback()
        {
            if (VTable.IsDefault)
            {
                VTable = new PxContactModifyCallback.VirtualMethodTable()
                {
                    onContactModify = &onContactModify,
                    //BIOQUIRK: Method is missing so vtable entry is untyped
                    __DeletingDestructorPointer = (delegate* unmanaged[Cdecl]<PxContactModifyCallback*, void>)&Destructor
                };
            }

            Base = new()
            {
                VirtualMethodTablePointer = VTable
            };
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static void Destructor(PxContactModifyCallback* @this)
        { }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static void onContactModify(PxContactModifyCallback* @this, PxContactModifyPair* pairs, uint count)
        {
            for (uint i = 0; i < count; i++)
            {
                //BIOQUIRK: Probably the "more correct" thing to do here is to use the generated constant array type, but these types currently have long confusing names.
                // A Span<PxRigidActor*> would also be nice, but isn't possible in C# today.
                // Could be improved by https://github.com/MochiLibraries/Biohazrd/issues/139
                PxRigidActor** actors = (PxRigidActor**)&pairs[i].actor; //BIOQUIRK: Unobvious cast.
                PxShape** shapes = (PxShape**)&pairs[i].shape; //BIOQUIRK: Unobvious cast

                //Search for actors that represent vehicles and shapes that represent wheels.
                for (int j = 0; j < 2; j++)
                {
                    PxRigidActor* actor = actors[j];
                    if (actor->userData != null && ((ActorUserData*)(actor->userData))->vehicle != null)
                    {
                        PxVehicleWheels* vehicle = ((ActorUserData*)(actor->userData))->vehicle;
                        Debug.Assert(vehicle->getRigidDynamicActor() == actors[j]);

                        PxShape* shape = shapes[j];
                        if (shape->userData != null && ((ShapeUserData*)(shape->userData))->isWheel)
                        {
                            uint wheelId = ((ShapeUserData*)(shape->userData))->wheelId;
                            Debug.Assert(wheelId < vehicle->mWheelsSimData.getNbWheels());

                            //Modify wheel contacts.
                            PxVehicleModifyWheelContacts(*vehicle, wheelId, WHEEL_TANGENT_VELOCITY_MULTIPLIER, MAX_IMPULSE, ref pairs[i]);
                        }
                    }
                }
            }
        }
    }

    static Pinned<WheelContactModifyCallback> gWheelContactModifyCallback = new WheelContactModifyCallback();

    //The class WheelCCDContactModifyCallback identifies and modifies ccd contacts
    //that involve a wheel.  Contacts that can be identified and managed by the suspension
    //system are ignored.  Any contacts that remain are modified to account for the rotation
    //speed of the wheel around the rolling axis.
    struct WheelCCDContactModifyCallback
    {
        private PxCCDContactModifyCallback Base;

        private static Pinned<PxCCDContactModifyCallback.VirtualMethodTable> VTable;

        //BIOQUIRK: This junk should ideally be handled by a source generator or something
        // (If it does get handled by a source generator it also needs to be threadsafe, which this is not.)
        public WheelCCDContactModifyCallback()
        {
            if (VTable.IsDefault)
            {
                VTable = new PxCCDContactModifyCallback.VirtualMethodTable()
                {
                    onCCDContactModify = &onCCDContactModify,
                    //BIOQUIRK: Method is missing so vtable entry is untyped
                    __DeletingDestructorPointer = (delegate* unmanaged[Cdecl]<PxCCDContactModifyCallback*, void>)&Destructor
                };
            }

            Base = new()
            {
                VirtualMethodTablePointer = VTable
            };
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static void Destructor(PxCCDContactModifyCallback* @this)
        { }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static void onCCDContactModify(PxCCDContactModifyCallback* @this, PxContactModifyPair* pairs, uint count)
        {
            for (uint i = 0; i < count; i++)
            {
                PxRigidActor** actors = (PxRigidActor**)&pairs[i].actor; //BIOQUIRK: Unobvious cast.
                PxShape** shapes = (PxShape**)&pairs[i].shape; //BIOQUIRK: Unobvious cast

                //Search for actors that represent vehicles and shapes that represent wheels.
                for (uint j = 0; j < 2; j++)
                {
                    PxRigidActor* actor = actors[j];
                    if (actor->userData != null && ((ActorUserData*)(actor->userData))->vehicle != null)
                    {
                        PxVehicleWheels* vehicle = ((ActorUserData*)(actor->userData))->vehicle;
                        Debug.Assert(vehicle->getRigidDynamicActor() == actors[j]);

                        PxShape* shape = shapes[j];
                        if (shape->userData != null && ((ShapeUserData*)(shape->userData))->isWheel)
                        {
                            uint wheelId = ((ShapeUserData*)(shape->userData))->wheelId;
                            Debug.Assert(wheelId < vehicle->mWheelsSimData.getNbWheels());

                            //Modify wheel contacts.
                            PxVehicleModifyWheelContacts(*vehicle, wheelId, WHEEL_TANGENT_VELOCITY_MULTIPLIER, MAX_IMPULSE, ref pairs[i]);
                        }
                    }
                }
            }
        }
    }

    static Pinned<WheelCCDContactModifyCallback> gWheelCCDContactModifyCallback = new WheelCCDContactModifyCallback();

    static VehicleDesc initVehicleDesc(in PxFilterData chassisSimFilterData, in PxFilterData wheelSimFilterData, uint vehicleId)
    {
        //Set up the chassis mass, dimensions, moment of inertia, and center of mass offset.
        //The moment of inertia is just the moment of inertia of a cuboid but modified for easier steering.
        //Center of mass offset is 0.65m above the base of the chassis and 0.25m towards the front.
        const float chassisMass = 1500.0f;
        PxVec3 chassisDims = new(2.5f, 2.0f, 5.0f);
        PxVec3 chassisMOI = new(
            (chassisDims.y * chassisDims.y + chassisDims.z * chassisDims.z) * chassisMass / 12.0f,
            (chassisDims.x * chassisDims.x + chassisDims.z * chassisDims.z) * 0.8f * chassisMass / 12.0f,
            (chassisDims.x * chassisDims.x + chassisDims.y * chassisDims.y) * chassisMass / 12.0f);
        PxVec3 chassisCMOffset = new(0.0f, -chassisDims.y * 0.5f + 0.65f, 0.25f);

        //Set up the wheel mass, radius, width, moment of inertia, and number of wheels.
        //Moment of inertia is just the moment of inertia of a cylinder.
        const float wheelMass = 20.0f;
        const float wheelRadius = 0.5f;
        const float wheelWidth = 0.4f;
        const float wheelMOI = 0.5f * wheelMass * wheelRadius * wheelRadius;
        const uint nbWheels = 4;

        VehicleDesc vehicleDesc;

        vehicleDesc.chassisMass = chassisMass;
        vehicleDesc.chassisDims = chassisDims;
        vehicleDesc.chassisMOI = chassisMOI;
        vehicleDesc.chassisCMOffset = chassisCMOffset;
        vehicleDesc.chassisMaterial = gMaterial;
        vehicleDesc.chassisSimFilterData = chassisSimFilterData;

        vehicleDesc.wheelMass = wheelMass;
        vehicleDesc.wheelRadius = wheelRadius;
        vehicleDesc.wheelWidth = wheelWidth;
        vehicleDesc.wheelMOI = wheelMOI;
        vehicleDesc.numWheels = nbWheels;
        vehicleDesc.wheelMaterial = gMaterial;
        vehicleDesc.wheelSimFilterData = wheelSimFilterData;

        vehicleDesc.actorUserData = &gActorUserData.Pointer[vehicleId];
        vehicleDesc.shapeUserDatas = gShapeUserDatas[vehicleId].Pointer;

        return vehicleDesc;
    }

    public static void initPhysics()
    {
        gFoundation = PxCreateFoundation(PX_PHYSICS_VERSION, ref gAllocator.Value, ref gErrorCallback.Value);
        gPvd = PxCreatePvd(ref *gFoundation);
        PxPvdTransport* transport = PxDefaultPvdSocketTransportCreate(PVD_HOST, 5425, 10);
        gPvd->connect(ref *transport, PxPvdInstrumentationFlags.eALL);

        gPhysics = PxCreatePhysics(PX_PHYSICS_VERSION, ref *gFoundation, new PxTolerancesScale(), true, gPvd);

        PxSceneDesc sceneDesc = new(*gPhysics->getTolerancesScale());
        sceneDesc.gravity = new PxVec3(0.0f, -9.81f, 0.0f);

        uint numWorkers = 1;
        gDispatcher = PxDefaultCpuDispatcherCreate(numWorkers);
        sceneDesc.cpuDispatcher = (PxCpuDispatcher*)gDispatcher; //BIOQUIRK: Base cast
        sceneDesc.filterShader = &VehicleFilterShader; //Set the filter shader
        sceneDesc.contactModifyCallback = (PxContactModifyCallback*)gWheelContactModifyCallback.Pointer; //Enable contact modification //BIOQUIRK: Base cast
        sceneDesc.ccdContactModifyCallback = (PxCCDContactModifyCallback*)gWheelCCDContactModifyCallback.Pointer; //Enable ccd contact modification //BIOQUIRK: Base cast
        sceneDesc.flags |= PxSceneFlags.eENABLE_CCD; //Enable ccd

        gScene = gPhysics->createScene(sceneDesc);
        PxPvdSceneClient* pvdClient = gScene->getScenePvdClient();
        if (pvdClient != null)
        {
            pvdClient->setScenePvdFlag(PxPvdSceneFlags.eTRANSMIT_CONSTRAINTS, true);
            pvdClient->setScenePvdFlag(PxPvdSceneFlags.eTRANSMIT_CONTACTS, true);
            pvdClient->setScenePvdFlag(PxPvdSceneFlags.eTRANSMIT_SCENEQUERIES, true);
        }
        gMaterial = gPhysics->createMaterial(0.1f, 0.1f, 0.01f);

        gCooking = PxCreateCooking(PX_PHYSICS_VERSION, ref *gFoundation, new PxCookingParams(new PxTolerancesScale()));

        /////////////////////////////////////////////

        PxInitVehicleSDK(ref *gPhysics);
        PxVehicleSetBasisVectors(new PxVec3(0, 1, 0), new PxVec3(0, 0, 1));
        PxVehicleSetUpdateMode(PxVehicleUpdateMode.eVELOCITY_CHANGE);
        PxVehicleSetSweepHitRejectionAngles(POINT_REJECT_ANGLE, NORMAL_REJECT_ANGLE);
        PxVehicleSetMaxHitActorAcceleration(MAX_ACCELERATION);

        //Create the batched scene queries for the suspension sweeps.
        //Use the post-filter shader to reject hit shapes that overlap the swept wheel at the start pose of the sweep.
        delegate* unmanaged[Cdecl]<PxFilterData*, PxFilterData*, void*, uint, PxHitFlags*, PxQueryHitType> sceneQueryPreFilter;
        delegate* unmanaged[Cdecl]<PxFilterData*, PxFilterData*, void*, uint, PxQueryHit*, PxQueryHitType> sceneQueryPostFilter;
#if BLOCKING_SWEEPS
        sceneQueryPreFilter = &WheelSceneQueryPreFilterBlocking;
        sceneQueryPostFilter = &WheelSceneQueryPostFilterBlocking;
#else
        sceneQueryPreFilter = &WheelSceneQueryPreFilterNonBlocking;
        sceneQueryPostFilter = &WheelSceneQueryPostFilterNonBlocking;
#endif
        gVehicleSceneQueryData = VehicleSceneQueryData.allocate(NUM_VEHICLES, PX_MAX_NB_WHEELS, gNbQueryHitsPerWheel, NUM_VEHICLES, sceneQueryPreFilter, sceneQueryPostFilter, ref gAllocator.Value);
        gBatchQuery = VehicleSceneQueryData.setUpBatchedSceneQuery(0, *gVehicleSceneQueryData, gScene);

        //Create the friction table for each combination of tire and surface type.
        gFrictionPairs = createFrictionPairs(gMaterial);

        //Create a plane to drive on.
        PxFilterData groundPlaneSimFilterData = new((uint)COLLISION_FLAG_GROUND, (uint)COLLISION_FLAG_GROUND_AGAINST, 0, 0);
        gGroundPlane = createDrivablePlane(groundPlaneSimFilterData, gMaterial, gPhysics);
        gScene->addActor(ref *gGroundPlane);

        //Create several static obstacles for the first vehicle to drive on.
        //  (i) collide only with wheel shapes
        // (ii) have continuous collision detection (CCD) enabled
        //(iii) have contact modification enabled
        // (iv) are configured to be drivable surfaces
        ReadOnlySpan<float> capsuleRadii = stackalloc float[4] { 0.05f, 0.1f, 0.125f, 0.135f };
        ReadOnlySpan<float> capsuleZ = stackalloc float[4] { 5.0f, 10.0f, 15.0f, 20.0f };
        for (int i = 0; i < 4; i++)
        {
            PxTransform t = new(new PxVec3(xCoordVehicleStarts[0], capsuleRadii[i], capsuleZ[i]), new PxQuat(PxIdentity));
            PxRigidStatic* rd = gPhysics->createRigidStatic(t);
            PxCapsuleGeometry capsuleGeom = new(capsuleRadii[i], 3.0f);
            PxShape* shape = PxRigidActorExt.createExclusiveShape(ref *rd, capsuleGeom, *gMaterial,
                PxShapeFlags.eVISUALIZATION | PxShapeFlags.eSCENE_QUERY_SHAPE | PxShapeFlags.eSIMULATION_SHAPE); //BIOQUIRK: Missing default
            PxFilterData simFilterData = new((uint)COLLISION_FLAG_OBSTACLE, (uint)COLLISION_FLAG_WHEEL, (uint)(PxPairFlags.eMODIFY_CONTACTS | PxPairFlags.eDETECT_CCD_CONTACT), 0);
            shape->setSimulationFilterData(simFilterData);
            PxFilterData qryFilterData = new();
            setupDrivableSurface(ref qryFilterData);
            shape->setQueryFilterData(qryFilterData);
            gScene->addActor(ref *rd);
        }
        ReadOnlySpan<float> boxHalfHeights = stackalloc float[1] { 1.0f };
        ReadOnlySpan<float> boxZ = stackalloc float[1] { 30.0f };
        for (int i = 0; i < 1; i++)
        {
            PxTransform t = new(new PxVec3(xCoordVehicleStarts[0], boxHalfHeights[i], boxZ[i]), new PxQuat(PxIdentity));
            PxRigidStatic* rd = gPhysics->createRigidStatic(t);

            PxBoxGeometry boxGeom = new(new PxVec3(3.0f, boxHalfHeights[i], 3.0f));
            PxShape* shape = PxRigidActorExt.createExclusiveShape(ref *rd, boxGeom, *gMaterial,
                PxShapeFlags.eVISUALIZATION | PxShapeFlags.eSCENE_QUERY_SHAPE | PxShapeFlags.eSIMULATION_SHAPE); //BIOQUIRK: Missing default

            PxFilterData simFilterData = new((uint)COLLISION_FLAG_OBSTACLE, (uint)COLLISION_FLAG_WHEEL, (uint)(PxPairFlags.eMODIFY_CONTACTS | PxPairFlags.eDETECT_CCD_CONTACT), 0);
            shape->setSimulationFilterData(simFilterData);
            PxFilterData qryFilterData = new();
            setupDrivableSurface(ref qryFilterData);
            shape->setQueryFilterData(qryFilterData);

            gScene->addActor(ref *rd);
        }

        //Create a pile of dynamic objects for the second vehicle to drive on.
        //  (i) collide only with wheel shapes
        // (ii) have continuous collision detection (CCD) enabled
        //(iii) have contact modification enabled
        // (iv) are configured to be drivable surfaces
        {
            for (uint i = 0; i < 64; i++)
            {
                PxTransform t = new(new PxVec3(xCoordVehicleStarts[1] + i * 0.01f, 2.0f + i * 0.25f, 20.0f + i * 0.025f), new PxQuat(MathF.PI * 0.5f, new PxVec3(0, 1, 0)));
                PxRigidDynamic* rd = gPhysics->createRigidDynamic(t);

                PxBoxGeometry boxGeom = new(new PxVec3(0.08f, 0.25f, 1.0f));
                PxShape* shape = PxRigidActorExt.createExclusiveShape(ref *rd, boxGeom, *gMaterial,
                    PxShapeFlags.eVISUALIZATION | PxShapeFlags.eSCENE_QUERY_SHAPE | PxShapeFlags.eSIMULATION_SHAPE); //BIOQUIRK: Missing default

                PxFilterData simFilterData = new((uint)COLLISION_FLAG_OBSTACLE, (uint)COLLISION_FLAG_OBSTACLE_AGAINST, (uint)(PxPairFlags.eMODIFY_CONTACTS | PxPairFlags.eDETECT_CCD_CONTACT), 0);
                shape->setSimulationFilterData(simFilterData);
                PxFilterData qryFilterData = new();
                setupDrivableSurface(ref qryFilterData);
                shape->setQueryFilterData(qryFilterData);

                PxRigidBodyExt.updateMassAndInertia(ref *rd, 30.0f);

                gScene->addActor(ref *rd);
            }
        }

        //Create two vehicles that will drive on the obstacles.
        //The vehicles are configured with wheels that
        //  (i) collide with obstacles
        // (ii) have continuous collision detection (CCD) enabled
        //(iii) have contact modification enabled
        //The vehicle chassis only collides with the ground to highlight the collision between the wheels and the obstacles.
        for (uint i = 0; i < NUM_VEHICLES; i++)
        {
            PxFilterData chassisSimFilterData = new((uint)COLLISION_FLAG_CHASSIS, (uint)COLLISION_FLAG_GROUND, 0, 0);
            PxFilterData wheelSimFilterData = new((uint)COLLISION_FLAG_WHEEL, (uint)COLLISION_FLAG_WHEEL, (uint)(PxPairFlags.eDETECT_CCD_CONTACT | PxPairFlags.eMODIFY_CONTACTS), 0);
            VehicleDesc vehicleDesc = initVehicleDesc(chassisSimFilterData, wheelSimFilterData, i);
            gVehicle4W[i] = createVehicle4W(vehicleDesc, gPhysics, gCooking);
            PxTransform startTransform = new(new PxVec3(xCoordVehicleStarts[i], (vehicleDesc.chassisDims.y * 0.5f + vehicleDesc.wheelRadius + 1.0f), 0), new PxQuat(PxIdentity));
            gVehicle4W[i]->getRigidDynamicActor()->setGlobalPose(startTransform);
            gVehicle4W[i]->getRigidDynamicActor()->setRigidBodyFlag(PxRigidBodyFlags.eENABLE_CCD, true);
            gScene->addActor(ref *gVehicle4W[i]->getRigidDynamicActor());

            //Set the vehicle to rest in first gear.
            //Set the vehicle to use auto-gears.
            gVehicle4W[i]->setToRestState();
            gVehicle4W[i]->mDriveDynData.forceGearChange((uint)PxVehicleGearsData.eFIRST);
            gVehicle4W[i]->mDriveDynData.setUseAutoGears(true);
        }
    }

    public static void stepPhysics()
    {
        const float timestep = 1.0f / 60.0f;

        //Set the vehicles to accelerate forwards.
        for (uint i = 0; i < 2; i++)
        {
            gVehicle4W[i]->mDriveDynData.setAnalogInput((uint)PxVehicleDrive4WControl.eANALOG_INPUT_ACCEL, 0.55f);
        }

        //Scene update.
        gScene->simulate(timestep);
        gScene->fetchResults(true);

        //Suspension sweeps (instead of raycasts).
        //Sweeps provide more information about the geometry under the wheel.
        PxVehicleDrive4W** vehicles = stackalloc PxVehicleDrive4W*[NUM_VEHICLES] { gVehicle4W[0], gVehicle4W[1] };
        PxBatchQueryResult<PxSweepHit>* sweepResults = gVehicleSceneQueryData->getSweepQueryResultBuffer(0);
        uint sweepResultsSize = gVehicleSceneQueryData->getQueryResultBufferSize();
        PxVehicleSuspensionSweeps(gBatchQuery, NUM_VEHICLES, vehicles, sweepResultsSize, sweepResults, gNbQueryHitsPerWheel, null, 1.0f, 1.01f);

        //Vehicle update.
        PxVec3 grav = gScene->getGravity();
        PxWheelQueryResult* wheelQueryResults = stackalloc PxWheelQueryResult[PX_MAX_NB_WHEELS * NUM_VEHICLES];
        PxVehicleWheelQueryResult* vehicleQueryResults = stackalloc PxVehicleWheelQueryResult[NUM_VEHICLES]
        {
            new() { wheelQueryResults = &wheelQueryResults[PX_MAX_NB_WHEELS * 0], nbWheelQueryResults = gVehicle4W[0]->mWheelsSimData.getNbWheels() },
            new() { wheelQueryResults = &wheelQueryResults[PX_MAX_NB_WHEELS * 1], nbWheelQueryResults = gVehicle4W[1]->mWheelsSimData.getNbWheels() },
        };
        PxVehicleUpdates(timestep, grav, *gFrictionPairs, NUM_VEHICLES, vehicles, vehicleQueryResults);
    }

    public static void cleanupPhysics()
    {
        for (uint i = 0; i < NUM_VEHICLES; i++)
        {
            gVehicle4W[i]->getRigidDynamicActor()->release();
            gVehicle4W[i]->free();
        }
        PX_RELEASE(ref gGroundPlane);
        PX_RELEASE(ref gBatchQuery);
        gVehicleSceneQueryData->free(ref gAllocator.Value);
        PX_RELEASE(ref gFrictionPairs);
        PxCloseVehicleSDK();

        PX_RELEASE(ref gMaterial);
        PX_RELEASE(ref gCooking);
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

        Console.WriteLine("SnippetVehicleContactMod done.");
    }

    public static void keyPress(Keys key, in PxTransform camera)
    {
    }
}
