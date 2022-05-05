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
// This snippet illustrates how to configure a PhysX vehicle when meters are not
// the chosen length scale.  The snippet sets up a vehicle with meters as the
// adopted length scale and then modifies the vehicle parameters so that they represent
// the same vehicle but with centimeters as the chosen length scale.  It is written in
// a way that allows any length scale to be chosen.  A key function here is the function
// customizeVehicleToLengthScale.

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
using static SnippetVehicleScale.DriveMode;

internal unsafe static class SnippetVehicleScale
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
    static PxVehicleDrive4W* gVehicle4W = null;

    static bool gIsVehicleInAir = true;

    static float[] gSteerVsForwardSpeedData = new float[2 * 8]
    {
        0.0f,  0.75f,
        5.0f,  0.75f,
        30.0f,  0.125f,
        120.0f,  0.1f,
        float.MaxValue, float.MaxValue,
        float.MaxValue, float.MaxValue,
        float.MaxValue, float.MaxValue,
        float.MaxValue, float.MaxValue
    };
    static PxFixedSizeLookupTable8 gSteerVsForwardSpeedTable = new(gSteerVsForwardSpeedData.AsSpan().Slice(0, 4 * 2));

    static PxVehicleKeySmoothingData gKeySmoothingData = gKeySmoothingData_Init();
    static PxVehicleKeySmoothingData gKeySmoothingData_Init() //BIOQUIRK: No easy way to initialize constant arrays inline like in the original
    {
        PxVehicleKeySmoothingData result = new();
        {
            result.mRiseRates[0] = 6.0f; //rise rate eANALOG_INPUT_ACCEL
            result.mRiseRates[1] = 6.0f; //rise rate eANALOG_INPUT_BRAKE
            result.mRiseRates[2] = 6.0f; //rise rate eANALOG_INPUT_HANDBRAKE
            result.mRiseRates[3] = 2.5f; //rise rate eANALOG_INPUT_STEER_LEFT
            result.mRiseRates[4] = 2.5f; //rise rate eANALOG_INPUT_STEER_RIGHT
        }
        {
            result.mFallRates[0] = 10.0f; //fall rate eANALOG_INPUT_ACCEL
            result.mFallRates[1] = 10.0f; //fall rate eANALOG_INPUT_BRAKE
            result.mFallRates[2] = 10.0f; //fall rate eANALOG_INPUT_HANDBRAKE
            result.mFallRates[3] = 5.0f; //fall rate eANALOG_INPUT_STEER_LEFT
            result.mFallRates[4] = 5.0f; //fall rate eANALOG_INPUT_STEER_RIGHT
        }
        return result;
    }

    static PxVehiclePadSmoothingData gPadSmoothingData = gPadSmoothingData_Init();
    static PxVehiclePadSmoothingData gPadSmoothingData_Init() //BIOQUIRK: No easy way to initialize constant arrays inline like in the original
    {
        PxVehiclePadSmoothingData result = new();
        {
            result.mRiseRates[0] = 6.0f; //rise rate eANALOG_INPUT_ACCEL
            result.mRiseRates[1] = 6.0f; //rise rate eANALOG_INPUT_BRAKE
            result.mRiseRates[2] = 6.0f; //rise rate eANALOG_INPUT_HANDBRAKE
            result.mRiseRates[3] = 2.5f; //rise rate eANALOG_INPUT_STEER_LEFT
            result.mRiseRates[4] = 2.5f; //rise rate eANALOG_INPUT_STEER_RIGHT
        }
        {
            result.mFallRates[0] = 10.0f; //fall rate eANALOG_INPUT_ACCEL
            result.mFallRates[1] = 10.0f; //fall rate eANALOG_INPUT_BRAKE
            result.mFallRates[2] = 10.0f; //fall rate eANALOG_INPUT_HANDBRAKE
            result.mFallRates[3] = 5.0f; //fall rate eANALOG_INPUT_STEER_LEFT
            result.mFallRates[4] = 5.0f; //fall rate eANALOG_INPUT_STEER_RIGHT
        }
        return result;
    }

    static PxVehicleDrive4WRawInputData gVehicleInputData = new();

    internal enum DriveMode
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
    static uint gVehicleOrderProgress = 0;
    public static bool gVehicleOrderComplete = false;
    static bool gMimicKeyInputs = true;

    const int eLengthScaleCentimeters = 0;
    const int eLengthScaleInches = 1;
    static float[] gLengthScales = { 100.0f, 39.3701f };
    public static float gLengthScale = gLengthScales[eLengthScaleInches];

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
        if (gMimicKeyInputs)
        {
            gVehicleInputData.setDigitalAccel(true);
        }
        else
        {
            gVehicleInputData.setAnalogAccel(1.0f);
        }
    }

    static void startAccelerateReverseMode()
    {
        gVehicle4W->mDriveDynData.forceGearChange((uint)PxVehicleGearsData.eREVERSE);

        if (gMimicKeyInputs)
        {
            gVehicleInputData.setDigitalAccel(true);
        }
        else
        {
            gVehicleInputData.setAnalogAccel(1.0f);
        }
    }

    static void startBrakeMode()
    {
        if (gMimicKeyInputs)
        {
            gVehicleInputData.setDigitalBrake(true);
        }
        else
        {
            gVehicleInputData.setAnalogBrake(1.0f);
        }
    }

    static void startTurnHardLeftMode()
    {
        if (gMimicKeyInputs)
        {
            gVehicleInputData.setDigitalAccel(true);
            gVehicleInputData.setDigitalSteerLeft(true);
        }
        else
        {
            gVehicleInputData.setAnalogAccel(1.0f);
            gVehicleInputData.setAnalogSteer(-1.0f);
        }
    }

    static void startTurnHardRightMode()
    {
        if (gMimicKeyInputs)
        {
            gVehicleInputData.setDigitalAccel(true);
            gVehicleInputData.setDigitalSteerRight(true);
        }
        else
        {
            gVehicleInputData.setAnalogAccel(1.0f);
            gVehicleInputData.setAnalogSteer(1.0f);
        }
    }

    static void startHandbrakeTurnLeftMode()
    {
        if (gMimicKeyInputs)
        {
            gVehicleInputData.setDigitalSteerLeft(true);
            gVehicleInputData.setDigitalHandbrake(true);
        }
        else
        {
            gVehicleInputData.setAnalogSteer(-1.0f);
            gVehicleInputData.setAnalogHandbrake(1.0f);
        }
    }

    static void startHandbrakeTurnRightMode()
    {
        if (gMimicKeyInputs)
        {
            gVehicleInputData.setDigitalSteerRight(true);
            gVehicleInputData.setDigitalHandbrake(true);
        }
        else
        {
            gVehicleInputData.setAnalogSteer(1.0f);
            gVehicleInputData.setAnalogHandbrake(1.0f);
        }
    }


    static void releaseAllControls()
    {
        if (gMimicKeyInputs)
        {
            gVehicleInputData.setDigitalAccel(false);
            gVehicleInputData.setDigitalSteerLeft(false);
            gVehicleInputData.setDigitalSteerRight(false);
            gVehicleInputData.setDigitalBrake(false);
            gVehicleInputData.setDigitalHandbrake(false);
        }
        else
        {
            gVehicleInputData.setAnalogAccel(0.0f);
            gVehicleInputData.setAnalogSteer(0.0f);
            gVehicleInputData.setAnalogBrake(0.0f);
            gVehicleInputData.setAnalogHandbrake(0.0f);
        }
    }

    public static void initPhysics()
    {
        gFoundation = PxCreateFoundation(PX_PHYSICS_VERSION, ref gAllocator.Value, ref gErrorCallback.Value);
        PxTolerancesScale scale = new();
        scale.length = gLengthScale;
        scale.speed = 10.0f * gLengthScale;
        gPvd = PxCreatePvd(ref *gFoundation);
        PxPvdTransport* transport = PxDefaultPvdSocketTransportCreate(PVD_HOST, 5425, 10);
        gPvd->connect(ref *transport, PxPvdInstrumentationFlags.eALL);
        gPhysics = PxCreatePhysics(PX_PHYSICS_VERSION, ref *gFoundation, scale, true, gPvd);

        PxSceneDesc sceneDesc = new(*gPhysics->getTolerancesScale());
        sceneDesc.gravity = new PxVec3(0.0f, -9.81f * gLengthScale, 0.0f);

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
        gVehicle4W = createVehicle4W(vehicleDesc, gPhysics, gCooking);
        //Convert the vehicle from meters to the chosen length scale.
        customizeVehicleToLengthScale(gLengthScale, gVehicle4W->getRigidDynamicActor(), &gVehicle4W->mWheelsSimData, &gVehicle4W->mDriveSimData);
        //Convert the steer angle vs forward speed table to the chosen length scale.
        for (uint i = 0; i < gSteerVsForwardSpeedTable.mNbDataPairs; i++)
        {
            gSteerVsForwardSpeedTable.mDataPairs[2 * i + 0] *= gLengthScale;
        }
        PxTransform startTransform = new(new PxVec3(0, ((vehicleDesc.chassisDims.y * 0.5f + vehicleDesc.wheelRadius + 1.0f) * gLengthScale), 0), new PxQuat(PxIdentity));
        gVehicle4W->getRigidDynamicActor()->setGlobalPose(startTransform);
        gScene->addActor(ref *gVehicle4W->getRigidDynamicActor());

        //Set the vehicle to rest in first gear.
        //Set the vehicle to use auto-gears.
        gVehicle4W->setToRestState();
        gVehicle4W->mDriveDynData.forceGearChange((uint)PxVehicleGearsData.eFIRST);
        gVehicle4W->mDriveDynData.setUseAutoGears(true);

        gVehicleModeTimer = 0.0f;
        gVehicleOrderProgress = 0;
        startBrakeMode();
    }

    static void incrementDrivingMode(float timestep)
    {
        gVehicleModeTimer += timestep;
        if (gVehicleModeTimer > gVehicleModeLifetime)
        {
            //If the mode just completed was eDRIVE_MODE_ACCEL_REVERSE then switch back to forward gears.
            if (eDRIVE_MODE_ACCEL_REVERSE == gDriveModeOrder[gVehicleOrderProgress])
            {
                gVehicle4W->mDriveDynData.forceGearChange((uint)PxVehicleGearsData.eFIRST);
            }

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

            //If the mode about to start is eDRIVE_MODE_ACCEL_REVERSE then switch to reverse gears.
            if (eDRIVE_MODE_ACCEL_REVERSE == gDriveModeOrder[gVehicleOrderProgress])
            {
                gVehicle4W->mDriveDynData.forceGearChange((uint)PxVehicleGearsData.eREVERSE);
            }
        }
    }

    public static void stepPhysics()
    {
        const float timestep = 1.0f / 60.0f;

        //Cycle through the driving modes to demonstrate how to accelerate/reverse/brake/turn etc.
        incrementDrivingMode(timestep);

        //Update the control inputs for the vehicle.
        if (gMimicKeyInputs)
        {
            PxVehicleDrive4WSmoothDigitalRawInputsAndSetAnalogInputs(gKeySmoothingData, gSteerVsForwardSpeedTable, gVehicleInputData, timestep, gIsVehicleInAir, ref *gVehicle4W);
        }
        else
        {
            PxVehicleDrive4WSmoothAnalogRawInputsAndSetAnalogInputs(gPadSmoothingData, gSteerVsForwardSpeedTable, gVehicleInputData, timestep, gIsVehicleInAir, ref *gVehicle4W);
        }

        //Raycasts.
        PxVehicleDrive4W** vehicles = stackalloc PxVehicleDrive4W*[1] { gVehicle4W };
        PxBatchQueryResult<PxRaycastHit>* raycastResults = gVehicleSceneQueryData->getRaycastQueryResultBuffer(0);
        uint raycastResultsSize = gVehicleSceneQueryData->getQueryResultBufferSize();
        PxVehicleSuspensionRaycasts(gBatchQuery, 1, vehicles, raycastResultsSize, raycastResults);

        //Vehicle update.
        PxVec3 grav = gScene->getGravity();
        PxWheelQueryResult* wheelQueryResults = stackalloc PxWheelQueryResult[PX_MAX_NB_WHEELS];
        PxVehicleWheelQueryResult* vehicleQueryResults = stackalloc PxVehicleWheelQueryResult[1]
        {
            new() { wheelQueryResults = wheelQueryResults, nbWheelQueryResults = gVehicle4W->mWheelsSimData.getNbWheels() }
        };
        PxVehicleUpdates(timestep, grav, *gFrictionPairs, 1, vehicles, vehicleQueryResults);

        //Work out if the vehicle is in the air.
        gIsVehicleInAir = gVehicle4W->getRigidDynamicActor()->isSleeping() ? false : PxVehicleIsInAir(vehicleQueryResults[0]);

        //Scene update.
        gScene->simulate(timestep);
        gScene->fetchResults(true);
    }

    public static void cleanupPhysics()
    {
        gVehicle4W->getRigidDynamicActor()->release();
        gVehicle4W->free();
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

        Console.WriteLine("SnippetVehicleScale done.");
    }

    public static void keyPress(Keys key, in PxTransform camera)
    {
    }
}
