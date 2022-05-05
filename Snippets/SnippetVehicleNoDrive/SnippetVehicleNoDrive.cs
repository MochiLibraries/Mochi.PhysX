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
// This snippet illustrates simple use of PxVehicleNoDrive.
//
// It creates a vehicle on a plane and then controls the vehicle so that it performs a
// number of choreographed manoeuvres such as accelerate, reverse, brake, handbrake, and turn.

// It is a good idea to record and playback with pvd (PhysX Visual Debugger).
// ****************************************************************************

using Mochi.PhysX;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SnippetCommon;
using SnippetVehicleCommon;
using System;
using static Mochi.PhysX.Globals;
using static Mochi.PhysX.PxIDENTITY;
using static SnippetVehicleCommon.CollisionFlags;
using static SnippetVehicleCommon.Globals;
using static SnippetVehicleNoDrive.DriveMode;

internal unsafe static class SnippetVehicleNoDrive
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

    static PxRigidStatic* gGroundPlane = null;
    static PxVehicleNoDrive* gVehicleNoDrive = null;

    public enum DriveMode
    {
        eDRIVE_MODE_ACCEL_FORWARDS = 0,
        eDRIVE_MODE_ACCEL_REVERSE,
        eDRIVE_MODE_HARD_TURN_LEFT,
        eDRIVE_MODE_HANDBRAKE_TURN_LEFT,
        eDRIVE_MODE_HARD_TURN_RIGHT,
        eDRIVE_MODE_HANDBRAKE_TURN_RIGHT,
        eDRIVE_MODE_BRAKE,
        eDRIVE_MODE_NONE
    }

    static DriveMode[] gDriveModeOrder = new[]
    {
        eDRIVE_MODE_BRAKE,
        eDRIVE_MODE_ACCEL_FORWARDS,
        eDRIVE_MODE_BRAKE,
        eDRIVE_MODE_ACCEL_REVERSE,
        eDRIVE_MODE_BRAKE,
        eDRIVE_MODE_HARD_TURN_LEFT,
        eDRIVE_MODE_BRAKE,
        eDRIVE_MODE_HARD_TURN_RIGHT,
        eDRIVE_MODE_ACCEL_FORWARDS,
        eDRIVE_MODE_HANDBRAKE_TURN_LEFT,
        eDRIVE_MODE_ACCEL_FORWARDS,
        eDRIVE_MODE_HANDBRAKE_TURN_RIGHT,
        eDRIVE_MODE_NONE
    };

    static float gVehicleModeLifetime = 4.0f;
    static float gVehicleModeTimer = 0.0f;
    public static bool gVehicleOrderComplete = false;
    static uint gVehicleOrderProgress = 0;

    static VehicleDesc initVehicleDesc()
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

        VehicleDesc vehicleDesc = new();

        vehicleDesc.chassisMass = chassisMass;
        vehicleDesc.chassisDims = chassisDims;
        vehicleDesc.chassisMOI = chassisMOI;
        vehicleDesc.chassisCMOffset = chassisCMOffset;
        vehicleDesc.chassisMaterial = gMaterial;
        vehicleDesc.chassisSimFilterData = new PxFilterData((uint)COLLISION_FLAG_CHASSIS, (uint)COLLISION_FLAG_CHASSIS_AGAINST, 0, 0);

        vehicleDesc.wheelMass = wheelMass;
        vehicleDesc.wheelRadius = wheelRadius;
        vehicleDesc.wheelWidth = wheelWidth;
        vehicleDesc.wheelMOI = wheelMOI;
        vehicleDesc.numWheels = nbWheels;
        vehicleDesc.wheelMaterial = gMaterial;
        vehicleDesc.chassisSimFilterData = new PxFilterData((uint)COLLISION_FLAG_WHEEL, (uint)COLLISION_FLAG_WHEEL_AGAINST, 0, 0);

        return vehicleDesc;
    }

    static void startAccelerateForwardsMode()
    {
        gVehicleNoDrive->setDriveTorque(0, 1000.0f);
        gVehicleNoDrive->setDriveTorque(1, 1000.0f);
    }

    static void startAccelerateReverseMode()
    {
        gVehicleNoDrive->setDriveTorque(0, -1000.0f);
        gVehicleNoDrive->setDriveTorque(1, -1000.0f);
    }

    static void startBrakeMode()
    {
        gVehicleNoDrive->setBrakeTorque(0, 1000.0f);
        gVehicleNoDrive->setBrakeTorque(1, 1000.0f);
        gVehicleNoDrive->setBrakeTorque(2, 1000.0f);
        gVehicleNoDrive->setBrakeTorque(3, 1000.0f);
    }

    static void startTurnHardLeftMode()
    {
        gVehicleNoDrive->setDriveTorque(0, 1000.0f);
        gVehicleNoDrive->setDriveTorque(1, 1000.0f);
        gVehicleNoDrive->setSteerAngle(0, 1.0f);
        gVehicleNoDrive->setSteerAngle(1, 1.0f);
    }

    static void startTurnHardRightMode()
    {
        gVehicleNoDrive->setDriveTorque(0, 1000.0f);
        gVehicleNoDrive->setDriveTorque(1, 1000.0f);
        gVehicleNoDrive->setSteerAngle(0, -1.0f);
        gVehicleNoDrive->setSteerAngle(1, -1.0f);
    }

    static void startHandbrakeTurnLeftMode()
    {
        gVehicleNoDrive->setBrakeTorque(2, 1000.0f);
        gVehicleNoDrive->setBrakeTorque(3, 1000.0f);
        gVehicleNoDrive->setDriveTorque(0, 1000.0f);
        gVehicleNoDrive->setDriveTorque(1, 1000.0f);
        gVehicleNoDrive->setSteerAngle(0, 1.0f);
        gVehicleNoDrive->setSteerAngle(1, 1.0f);
    }

    static void startHandbrakeTurnRightMode()
    {
        gVehicleNoDrive->setBrakeTorque(2, 1000.0f);
        gVehicleNoDrive->setBrakeTorque(3, 1000.0f);
        gVehicleNoDrive->setDriveTorque(0, 1000.0f);
        gVehicleNoDrive->setDriveTorque(1, 1000.0f);
        gVehicleNoDrive->setSteerAngle(0, -1.0f);
        gVehicleNoDrive->setSteerAngle(1, -1.0f);
    }

    static void releaseAllControls()
    {
        gVehicleNoDrive->setDriveTorque(0, 0.0f);
        gVehicleNoDrive->setDriveTorque(1, 0.0f);
        gVehicleNoDrive->setDriveTorque(2, 0.0f);
        gVehicleNoDrive->setDriveTorque(3, 0.0f);

        gVehicleNoDrive->setBrakeTorque(0, 0.0f);
        gVehicleNoDrive->setBrakeTorque(1, 0.0f);
        gVehicleNoDrive->setBrakeTorque(2, 0.0f);
        gVehicleNoDrive->setBrakeTorque(3, 0.0f);

        gVehicleNoDrive->setSteerAngle(0, 0.0f);
        gVehicleNoDrive->setSteerAngle(1, 0.0f);
        gVehicleNoDrive->setSteerAngle(2, 0.0f);
        gVehicleNoDrive->setSteerAngle(3, 0.0f);
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
        sceneDesc.filterShader = &VehicleFilterShader;

        gScene = gPhysics->createScene(sceneDesc);

        PxPvdSceneClient* pvdClient = gScene->getScenePvdClient();
        if (pvdClient != null)
        {
            pvdClient->setScenePvdFlag(PxPvdSceneFlags.eTRANSMIT_CONSTRAINTS, true);
            pvdClient->setScenePvdFlag(PxPvdSceneFlags.eTRANSMIT_CONTACTS, true);
            pvdClient->setScenePvdFlag(PxPvdSceneFlags.eTRANSMIT_SCENEQUERIES, true);
        }
        gMaterial = gPhysics->createMaterial(0.5f, 0.5f, 0.6f);

        gCooking = PxCreateCooking(PX_PHYSICS_VERSION, ref *gFoundation, new PxCookingParams(new PxTolerancesScale()));

        /////////////////////////////////////////////

        PxInitVehicleSDK(ref *gPhysics);
        PxVehicleSetBasisVectors(new PxVec3(0, 1, 0), new PxVec3(0, 0, 1));
        PxVehicleSetUpdateMode(PxVehicleUpdateMode.eVELOCITY_CHANGE);

        //Create the batched scene queries for the suspension raycasts.
        gVehicleSceneQueryData = VehicleSceneQueryData.allocate(1, PX_MAX_NB_WHEELS, 1, 1, &WheelSceneQueryPreFilterBlocking, null, ref gAllocator.Value);
        gBatchQuery = VehicleSceneQueryData.setUpBatchedSceneQuery(0, *gVehicleSceneQueryData, gScene);

        //Create the friction table for each combination of tire and surface type.
        gFrictionPairs = createFrictionPairs(gMaterial);

        //Create a plane to drive on.
        PxFilterData groundPlaneSimFilterData = new((uint)COLLISION_FLAG_GROUND, (uint)COLLISION_FLAG_GROUND_AGAINST, 0, 0);
        gGroundPlane = createDrivablePlane(groundPlaneSimFilterData, gMaterial, gPhysics);
        gScene->addActor(ref *gGroundPlane);

        //Create a vehicle that will drive on the plane.
        VehicleDesc vehicleDesc = initVehicleDesc();
        gVehicleNoDrive = createVehicleNoDrive(vehicleDesc, gPhysics, gCooking);
        PxTransform startTransform = new(new PxVec3(0, (vehicleDesc.chassisDims.y * 0.5f + vehicleDesc.wheelRadius + 1.0f), 0), new PxQuat(PxIdentity));
        gVehicleNoDrive->getRigidDynamicActor()->setGlobalPose(startTransform);
        gScene->addActor(ref *gVehicleNoDrive->getRigidDynamicActor());

        //Set the vehicle to rest in first gear.
        //Set the vehicle to use auto-gears.
        gVehicleNoDrive->setToRestState();

        gVehicleModeTimer = 0.0f;
        gVehicleOrderProgress = 0;
        startBrakeMode();
    }

    static void incrementDrivingMode(float timestep)
    {
        gVehicleModeTimer += timestep;
        if (gVehicleModeTimer > gVehicleModeLifetime)
        {
            //Increment to next driving mode.
            gVehicleModeTimer = 0.0f;
            gVehicleOrderProgress++;
            releaseAllControls();

            //If we are at the end of the list of driving modes then start again.
            if (eDRIVE_MODE_NONE == gDriveModeOrder[gVehicleOrderProgress])
            {
                gVehicleOrderProgress = 0;
                gVehicleOrderComplete = true;
            }

            //Start driving in the selected mode.
            DriveMode eDriveMode = gDriveModeOrder[gVehicleOrderProgress];
            switch (eDriveMode)
            {
                case eDRIVE_MODE_ACCEL_FORWARDS:
                    startAccelerateForwardsMode();
                    break;
                case eDRIVE_MODE_ACCEL_REVERSE:
                    startAccelerateReverseMode();
                    break;
                case eDRIVE_MODE_HARD_TURN_LEFT:
                    startTurnHardLeftMode();
                    break;
                case eDRIVE_MODE_HANDBRAKE_TURN_LEFT:
                    startHandbrakeTurnLeftMode();
                    break;
                case eDRIVE_MODE_HARD_TURN_RIGHT:
                    startTurnHardRightMode();
                    break;
                case eDRIVE_MODE_HANDBRAKE_TURN_RIGHT:
                    startHandbrakeTurnRightMode();
                    break;
                case eDRIVE_MODE_BRAKE:
                    startBrakeMode();
                    break;
                case eDRIVE_MODE_NONE:
                    break;
            };
        }
    }

    public static void stepPhysics()
    {
        const float timestep = 1.0f / 60.0f;

        //Cycle through the driving modes to demonstrate how to accelerate/reverse/brake/turn etc.
        incrementDrivingMode(timestep);

        //Raycasts.
        PxVehicleNoDrive** vehicles = stackalloc PxVehicleNoDrive*[1] { gVehicleNoDrive };
        PxBatchQueryResult<PxRaycastHit>* raycastResults = gVehicleSceneQueryData->getRaycastQueryResultBuffer(0);
        uint raycastResultsSize = gVehicleSceneQueryData->getQueryResultBufferSize();
        PxVehicleSuspensionRaycasts(gBatchQuery, 1, vehicles, raycastResultsSize, raycastResults);

        //Vehicle update.
        PxVec3 grav = gScene->getGravity();
        PxWheelQueryResult* wheelQueryResults = stackalloc PxWheelQueryResult[PX_MAX_NB_WHEELS];
        PxVehicleWheelQueryResult* vehicleQueryResults = stackalloc PxVehicleWheelQueryResult[1]
        {
            new() { wheelQueryResults = wheelQueryResults, nbWheelQueryResults = gVehicleNoDrive->mWheelsSimData.getNbWheels() }
        };
        PxVehicleUpdates(timestep, grav, *gFrictionPairs, 1, vehicles, vehicleQueryResults);

        //Scene update.
        gScene->simulate(timestep);
        gScene->fetchResults(true);
    }

    public static void cleanupPhysics()
    {
        gVehicleNoDrive->getRigidDynamicActor()->release();
        gVehicleNoDrive->free();
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

        Console.WriteLine("SnippetVehicleNoDrive done.");
    }

    public static void keyPress(Keys key, in PxTransform camera)
    {
    }
}
