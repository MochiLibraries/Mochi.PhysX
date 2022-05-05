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
// This snippet illustrates simple use of PxVehicleDriveTank.
//
// It creates a tank on a plane and then controls the tank so that it performs a
// number of choreographed manoeuvres such as accelerate, reverse, soft turns,
// and hard turns.

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
using static SnippetVehicleTank.DriveMode;

internal unsafe static class SnippetVehicleTank
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
    static PxVehicleDriveTank* gTank = null;

    static PxVehicleKeySmoothingData gKeySmoothingData = gKeySmoothingData_Init();
    static PxVehicleKeySmoothingData gKeySmoothingData_Init() //BIOQUIRK: No easy way top initialize constant arrays inline like in the original
    {
        PxVehicleKeySmoothingData result = new();
        {
            result.mRiseRates[0] = 6.0f; //rise rate eANALOG_INPUT_ACCEL=0,
            result.mRiseRates[1] = 6.0f; //rise rate eANALOG_INPUT_BRAKE,
            result.mRiseRates[2] = 6.0f; //rise rate eANALOG_INPUT_HANDBRAKE,
            result.mRiseRates[3] = 2.5f; //rise rate eANALOG_INPUT_STEER_LEFT,
            result.mRiseRates[4] = 2.5f; //rise rate eANALOG_INPUT_STEER_RIGHT,
        }
        {
            result.mFallRates[0] = 10.0f; //fall rate eANALOG_INPUT_ACCEL=0,
            result.mFallRates[1] = 10.0f; //fall rate eANALOG_INPUT_BRAKE,
            result.mFallRates[2] = 10.0f; //fall rate eANALOG_INPUT_HANDBRAKE,
            result.mFallRates[3] = 5.0f; //fall rate eANALOG_INPUT_STEER_LEFT,
            result.mFallRates[4] = 5.0f; //fall rate eANALOG_INPUT_STEER_RIGHT,
        }
        return result;
    }

    static PxVehiclePadSmoothingData gPadSmoothingData = gPadSmoothingData_Init();
    static PxVehiclePadSmoothingData gPadSmoothingData_Init() //BIOQUIRK: No easy way to initialize constant arrays inline like in the original
    {
        PxVehiclePadSmoothingData result = new();
        {
            result.mRiseRates[0] = 6.0f; //rise rate eANALOG_INPUT_ACCEL=0,
            result.mRiseRates[1] = 6.0f; //rise rate eANALOG_INPUT_BRAKE,
            result.mRiseRates[2] = 6.0f; //rise rate eANALOG_INPUT_HANDBRAKE,
            result.mRiseRates[3] = 2.5f; //rise rate eANALOG_INPUT_STEER_LEFT,
            result.mRiseRates[4] = 2.5f; //rise rate eANALOG_INPUT_STEER_RIGHT,
        }
        {
            result.mFallRates[0] = 10.0f; //fall rate eANALOG_INPUT_ACCEL=0
            result.mFallRates[1] = 10.0f; //fall rate eANALOG_INPUT_BRAKE_LEFT
            result.mFallRates[2] = 10.0f; //fall rate eANALOG_INPUT_BRAKE_RIGHT
            result.mFallRates[3] = 5.0f; //fall rate eANALOG_INPUT_THRUST_LEFT
            result.mFallRates[4] = 5.0f; //fall rate eANALOG_INPUT_THRUST_RIGHT
        }
        return result;
    }

    static PxVehicleDriveTankRawInputData gVehicleInputData = new(PxVehicleDriveTankControlModel.eSTANDARD);

    public enum DriveMode
    {
        eDRIVE_MODE_ACCEL_FORWARDS = 0,
        eDRIVE_MODE_ACCEL_REVERSE,
        eDRIVE_MODE_HARD_TURN_LEFT,
        eDRIVE_MODE_SOFT_TURN_LEFT,
        eDRIVE_MODE_HARD_TURN_RIGHT,
        eDRIVE_MODE_SOFT_TURN_RIGHT,
        eDRIVE_MODE_BRAKE,
        eDRIVE_MODE_NONE
    };

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
     eDRIVE_MODE_BRAKE,
     eDRIVE_MODE_SOFT_TURN_LEFT,
     eDRIVE_MODE_BRAKE,
     eDRIVE_MODE_SOFT_TURN_RIGHT,
     eDRIVE_MODE_NONE
    };

    static float gTankModeLifetime = 4.0f;
    static float gTankModeTimer = 0.0f;
    static uint gTankOrderProgress = 0;
    public static bool gTankOrderComplete = false;
    static bool gMimicKeyInputs = false;

    static VehicleDesc initTankDesc()
    {
        //Set up the chassis mass, dimensions, moment of inertia, and center of mass offset.
        //The moment of inertia is just the moment of inertia of a cuboid but modified for easier steering.
        //Center of mass offset is 0.65m above the base of the chassis and 0.25m towards the front.
        const float chassisMass = 1500.0f;
        PxVec3 chassisDims = new(3.5f, 2.0f, 9.0f);
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
        const uint nbWheels = 14;

        VehicleDesc tankDesc = new();

        tankDesc.chassisMass = chassisMass;
        tankDesc.chassisDims = chassisDims;
        tankDesc.chassisMOI = chassisMOI;
        tankDesc.chassisCMOffset = chassisCMOffset;
        tankDesc.chassisMaterial = gMaterial;
        tankDesc.chassisSimFilterData = new PxFilterData((uint)COLLISION_FLAG_CHASSIS, (uint)COLLISION_FLAG_CHASSIS_AGAINST, 0, 0);

        tankDesc.wheelMass = wheelMass;
        tankDesc.wheelRadius = wheelRadius;
        tankDesc.wheelWidth = wheelWidth;
        tankDesc.wheelMOI = wheelMOI;
        tankDesc.numWheels = nbWheels;
        tankDesc.wheelMaterial = gMaterial;
        tankDesc.chassisSimFilterData = new PxFilterData((uint)COLLISION_FLAG_WHEEL, (uint)COLLISION_FLAG_WHEEL_AGAINST, 0, 0);

        return tankDesc;
    }

    static void startAccelerateForwardsMode()
    {
        if (gMimicKeyInputs)
        {
            gVehicleInputData.setDigitalAccel(true);
            gVehicleInputData.setDigitalLeftThrust(true);
            gVehicleInputData.setDigitalRightThrust(true);
        }
        else
        {
            gVehicleInputData.setAnalogAccel(1.0f);
            gVehicleInputData.setAnalogLeftThrust(1.0f);
            gVehicleInputData.setAnalogRightThrust(1.0f);
        }
    }

    static void startAccelerateReverseMode()
    {
        gTank->mDriveDynData.forceGearChange((uint)PxVehicleGearsData.eREVERSE);

        if (gMimicKeyInputs)
        {
            gVehicleInputData.setDigitalAccel(true);
            gVehicleInputData.setDigitalLeftThrust(true);
            gVehicleInputData.setDigitalRightThrust(true);
        }
        else
        {
            gVehicleInputData.setAnalogAccel(1.0f);
            gVehicleInputData.setAnalogLeftThrust(1.0f);
            gVehicleInputData.setAnalogRightThrust(1.0f);
        }
    }

    static void startBrakeMode()
    {
        if (gMimicKeyInputs)
        {
            gVehicleInputData.setDigitalLeftBrake(true);
            gVehicleInputData.setDigitalRightBrake(true);
        }
        else
        {
            gVehicleInputData.setAnalogLeftBrake(1.0f);
            gVehicleInputData.setAnalogRightBrake(1.0f);
        }
    }

    static void startTurnHardLeftMode()
    {
        if (gMimicKeyInputs)
        {
            gVehicleInputData.setDigitalAccel(true);
            gVehicleInputData.setDigitalLeftThrust(true);
            gVehicleInputData.setDigitalRightBrake(true);
        }
        else
        {
            gVehicleInputData.setAnalogAccel(1.0f);
            gVehicleInputData.setAnalogLeftThrust(1.0f);
            gVehicleInputData.setAnalogRightBrake(1.0f);
        }
    }

    static void startTurnHardRightMode()
    {
        if (gMimicKeyInputs)
        {
            gVehicleInputData.setDigitalAccel(true);
            gVehicleInputData.setDigitalRightThrust(true);
            gVehicleInputData.setDigitalLeftBrake(true);
        }
        else
        {
            gVehicleInputData.setAnalogAccel(1.0f);
            gVehicleInputData.setAnalogRightThrust(1.0f);
            gVehicleInputData.setAnalogLeftBrake(1.0f);
        }
    }

    static void startTurnSoftLeftMode()
    {
        if (gMimicKeyInputs)
        {
            gVehicleInputData.setDigitalAccel(true);
            gVehicleInputData.setDigitalLeftThrust(true);
        }
        else
        {
            gVehicleInputData.setAnalogAccel(1.0f);
            gVehicleInputData.setAnalogLeftThrust(1.0f);
            gVehicleInputData.setAnalogRightThrust(0.3f);
        }
    }

    static void startTurnSoftRightMode()
    {
        if (gMimicKeyInputs)
        {
            gVehicleInputData.setDigitalAccel(true);
            gVehicleInputData.setDigitalRightThrust(true);
        }
        else
        {
            gVehicleInputData.setAnalogAccel(1.0f);
            gVehicleInputData.setAnalogRightThrust(1.0f);
            gVehicleInputData.setAnalogLeftThrust(0.3f);
        }
    }

    static void releaseAllControls()
    {
        if (gMimicKeyInputs)
        {
            gVehicleInputData.setDigitalAccel(false);
            gVehicleInputData.setDigitalRightThrust(false);
            gVehicleInputData.setDigitalLeftThrust(false);
            gVehicleInputData.setDigitalRightBrake(false);
            gVehicleInputData.setDigitalLeftBrake(false);
        }
        else
        {
            gVehicleInputData.setAnalogAccel(0.0f);
            gVehicleInputData.setAnalogRightThrust(0.0f);
            gVehicleInputData.setAnalogLeftThrust(0.0f);
            gVehicleInputData.setAnalogRightBrake(0.0f);
            gVehicleInputData.setAnalogLeftBrake(0.0f);
        }
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

        //Create a tank that will drive on the plane.
        VehicleDesc tankDesc = initTankDesc();
        gTank = createVehicleTank(tankDesc, gPhysics, gCooking);
        PxTransform startTransform = new(new PxVec3(0, (tankDesc.chassisDims.y * 0.5f + tankDesc.wheelRadius + 1.0f), 0), new PxQuat(PxIdentity));
        gTank->getRigidDynamicActor()->setGlobalPose(startTransform);
        gScene->addActor(ref *gTank->getRigidDynamicActor());

        //Set the tank to rest in first gear.
        //Set the tank to use auto-gears.
        //Set the tank to use the standard control model
        gTank->setToRestState();
        gTank->mDriveDynData.forceGearChange((uint)PxVehicleGearsData.eFIRST);
        gTank->mDriveDynData.setUseAutoGears(true);
        gTank->setDriveModel(PxVehicleDriveTankControlModel.eSTANDARD);

        gTankModeTimer = 0.0f;
        gTankOrderProgress = 0;
        startBrakeMode();
    }

    static void incrementDrivingMode(float timestep)
    {
        gTankModeTimer += timestep;
        if (gTankModeTimer > gTankModeLifetime)
        {
            //If the mode just completed was eDRIVE_MODE_ACCEL_REVERSE then switch back to forward gears.
            if (eDRIVE_MODE_ACCEL_REVERSE == gDriveModeOrder[gTankOrderProgress])
            {
                gTank->mDriveDynData.forceGearChange((uint)PxVehicleGearsData.eFIRST);
            }

            //Increment to next driving mode.
            gTankModeTimer = 0.0f;
            gTankOrderProgress++;
            releaseAllControls();

            //If we are at the end of the list of driving modes then start again.
            if (eDRIVE_MODE_NONE == gDriveModeOrder[gTankOrderProgress])
            {
                gTankOrderProgress = 0;
                gTankOrderComplete = true;
            }

            //Start driving in the selected mode.
            DriveMode eDriveMode = gDriveModeOrder[gTankOrderProgress];
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
                case eDRIVE_MODE_SOFT_TURN_LEFT:
                    startTurnSoftLeftMode();
                    break;
                case eDRIVE_MODE_HARD_TURN_RIGHT:
                    startTurnHardRightMode();
                    break;
                case eDRIVE_MODE_SOFT_TURN_RIGHT:
                    startTurnSoftRightMode();
                    break;
                case eDRIVE_MODE_BRAKE:
                    startBrakeMode();
                    break;
                case eDRIVE_MODE_NONE:
                    break;
            };

            //If the mode about to start is eDRIVE_MODE_ACCEL_REVERSE then switch to reverse gears.
            if (eDRIVE_MODE_ACCEL_REVERSE == gDriveModeOrder[gTankOrderProgress])
            {
                gTank->mDriveDynData.forceGearChange((uint)PxVehicleGearsData.eREVERSE);
            }
        }
    }

    public static void stepPhysics()
    {
        const float timestep = 1.0f / 60.0f;

        //Cycle through the driving modes to demonstrate how to accelerate/reverse/brake/turn.
        incrementDrivingMode(timestep);

        //Update the control inputs for the tank.
        if (gMimicKeyInputs)
        {
            PxVehicleDriveTankSmoothDigitalRawInputsAndSetAnalogInputs(gKeySmoothingData, gVehicleInputData, timestep, ref *gTank);
        }
        else
        {
            PxVehicleDriveTankSmoothAnalogRawInputsAndSetAnalogInputs(gPadSmoothingData, gVehicleInputData, timestep, ref *gTank);
        }

        //Raycasts.
        PxVehicleDriveTank** vehicles = stackalloc PxVehicleDriveTank*[1] { gTank };
        uint raycastQueryResultsSize = gVehicleSceneQueryData->getQueryResultBufferSize();
        PxBatchQueryResult<PxRaycastHit>* raycastQueryResults = gVehicleSceneQueryData->getRaycastQueryResultBuffer(0);
        PxVehicleSuspensionRaycasts(gBatchQuery, 1, vehicles, raycastQueryResultsSize, raycastQueryResults);

        //Vehicle update.
        PxVec3 grav = gScene->getGravity();
        PxWheelQueryResult* wheelQueryResults = stackalloc PxWheelQueryResult[PX_MAX_NB_WHEELS];
        PxVehicleWheelQueryResult* vehicleQueryResults = stackalloc PxVehicleWheelQueryResult[1]
        {
            new() { wheelQueryResults = wheelQueryResults, nbWheelQueryResults = gTank->mWheelsSimData.getNbWheels() }
        };
        PxVehicleUpdates(timestep, grav, *gFrictionPairs, 1, vehicles, vehicleQueryResults);

        //Scene update.
        gScene->simulate(timestep);
        gScene->fetchResults(true);
    }

    public static void cleanupPhysics()
    {
        gTank->getRigidDynamicActor()->release();
        gTank->free();
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

        Console.WriteLine("SnippetVehicleTank done.");
    }

    public static void keyPress(Keys key, in PxTransform camera)
    {
    }
}
