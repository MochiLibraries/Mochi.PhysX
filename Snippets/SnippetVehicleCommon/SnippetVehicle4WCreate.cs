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

using Mochi.PhysX;
using System;
using static Mochi.PhysX.Globals;
using static SnippetVehicleCommon.Globals;

namespace SnippetVehicleCommon;

internal unsafe static class fourwheel
{
    internal static void computeWheelCenterActorOffsets4W(float wheelFrontZ, float wheelRearZ, in PxVec3 chassisDims, float wheelWidth, float wheelRadius, uint numWheels, PxVec3* wheelCentreOffsets)
    {
        //chassisDims.z is the distance from the rear of the chassis to the front of the chassis.
        //The front has z = 0.5*chassisDims.z and the rear has z = -0.5*chassisDims.z.
        //Compute a position for the front wheel and the rear wheel along the z-axis.
        //Compute the separation between each wheel along the z-axis.
        float numLeftWheels = numWheels / 2.0f;
        float deltaZ = (wheelFrontZ - wheelRearZ) / (numLeftWheels - 1.0f);
        //Set the outside of the left and right wheels to be flush with the chassis.
        //Set the top of the wheel to be just touching the underside of the chassis.
        //Begin by setting the rear-left/rear-right/front-left,front-right wheels.
        wheelCentreOffsets[(int)PxVehicleDrive4WWheelOrder.eREAR_LEFT] = new PxVec3((-chassisDims.x + wheelWidth) * 0.5f, -(chassisDims.y / 2 + wheelRadius), wheelRearZ + 0 * deltaZ * 0.5f);
        wheelCentreOffsets[(int)PxVehicleDrive4WWheelOrder.eREAR_RIGHT] = new PxVec3((+chassisDims.x - wheelWidth) * 0.5f, -(chassisDims.y / 2 + wheelRadius), wheelRearZ + 0 * deltaZ * 0.5f);
        wheelCentreOffsets[(int)PxVehicleDrive4WWheelOrder.eFRONT_LEFT] = new PxVec3((-chassisDims.x + wheelWidth) * 0.5f, -(chassisDims.y / 2 + wheelRadius), wheelRearZ + (numLeftWheels - 1) * deltaZ);
        wheelCentreOffsets[(int)PxVehicleDrive4WWheelOrder.eFRONT_RIGHT] = new PxVec3((+chassisDims.x - wheelWidth) * 0.5f, -(chassisDims.y / 2 + wheelRadius), wheelRearZ + (numLeftWheels - 1) * deltaZ);
        //Set the remaining wheels.
        for (uint i = 2, wheelCount = 4; i < numWheels - 2; i += 2, wheelCount += 2)
        {
            wheelCentreOffsets[wheelCount + 0] = new PxVec3((-chassisDims.x + wheelWidth) * 0.5f, -(chassisDims.y / 2 + wheelRadius), wheelRearZ + i * deltaZ * 0.5f);
            wheelCentreOffsets[wheelCount + 1] = new PxVec3((+chassisDims.x - wheelWidth) * 0.5f, -(chassisDims.y / 2 + wheelRadius), wheelRearZ + i * deltaZ * 0.5f);
        }
    }

    internal static void setupWheelsSimulationData(
        float wheelMass, float wheelMOI, float wheelRadius, float wheelWidth,
        uint numWheels, PxVec3* wheelCenterActorOffsets,
        in PxVec3 chassisCMOffset, float chassisMass,
        PxVehicleWheelsSimData* wheelsSimData)
    {
        //Set up the wheels.
        PxVehicleWheelData* wheels = stackalloc PxVehicleWheelData[PX_MAX_NB_WHEELS];
        {
            //Set up the wheel data structures with mass, moi, radius, width.
            for (uint i = 0; i < numWheels; i++)
            {
                wheels[i] = new();
                wheels[i].mMass = wheelMass;
                wheels[i].mMOI = wheelMOI;
                wheels[i].mRadius = wheelRadius;
                wheels[i].mWidth = wheelWidth;
            }

            //Enable the handbrake for the rear wheels only.
            wheels[(int)PxVehicleDrive4WWheelOrder.eREAR_LEFT].mMaxHandBrakeTorque = 4000.0f;
            wheels[(int)PxVehicleDrive4WWheelOrder.eREAR_RIGHT].mMaxHandBrakeTorque = 4000.0f;
            //Enable steering for the front wheels only.
            wheels[(int)PxVehicleDrive4WWheelOrder.eFRONT_LEFT].mMaxSteer = MathF.PI * 0.3333f;
            wheels[(int)PxVehicleDrive4WWheelOrder.eFRONT_RIGHT].mMaxSteer = MathF.PI * 0.3333f;
        }

        //Set up the tires.
        PxVehicleTireData* tires = stackalloc PxVehicleTireData[PX_MAX_NB_WHEELS];
        {
            //Set up the tires.
            for (uint i = 0; i < numWheels; i++)
            {
                tires[i] = new();
                tires[i].mType = TIRE_TYPE_NORMAL;
            }
        }

        //Set up the suspensions
        PxVehicleSuspensionData* suspensions = stackalloc PxVehicleSuspensionData[PX_MAX_NB_WHEELS];
        {
            //Compute the mass supported by each suspension spring.
            float* suspSprungMasses = stackalloc float[PX_MAX_NB_WHEELS];
            PxVehicleComputeSprungMasses
                (numWheels, wheelCenterActorOffsets,
                 chassisCMOffset, chassisMass, 1, suspSprungMasses);

            //Set the suspension data.
            for (uint i = 0; i < numWheels; i++)
            {
                suspensions[i] = new();
                suspensions[i].mMaxCompression = 0.3f;
                suspensions[i].mMaxDroop = 0.1f;
                suspensions[i].mSpringStrength = 35000.0f;
                suspensions[i].mSpringDamperRate = 4500.0f;
                suspensions[i].mSprungMass = suspSprungMasses[i];
            }

            //Set the camber angles.
            const float camberAngleAtRest = 0.0f;
            const float camberAngleAtMaxDroop = 0.01f;
            const float camberAngleAtMaxCompression = -0.01f;
            for (uint i = 0; i < numWheels; i += 2)
            {
                suspensions[i + 0].mCamberAtRest = camberAngleAtRest;
                suspensions[i + 1].mCamberAtRest = -camberAngleAtRest;
                suspensions[i + 0].mCamberAtMaxDroop = camberAngleAtMaxDroop;
                suspensions[i + 1].mCamberAtMaxDroop = -camberAngleAtMaxDroop;
                suspensions[i + 0].mCamberAtMaxCompression = camberAngleAtMaxCompression;
                suspensions[i + 1].mCamberAtMaxCompression = -camberAngleAtMaxCompression;
            }
        }

        //Set up the wheel geometry.
        PxVec3* suspTravelDirections = stackalloc PxVec3[PX_MAX_NB_WHEELS];
        PxVec3* wheelCentreCMOffsets = stackalloc PxVec3[PX_MAX_NB_WHEELS];
        PxVec3* suspForceAppCMOffsets = stackalloc PxVec3[PX_MAX_NB_WHEELS];
        PxVec3* tireForceAppCMOffsets = stackalloc PxVec3[PX_MAX_NB_WHEELS];
        {
            //Set the geometry data.
            for (uint i = 0; i < numWheels; i++)
            {
                //Vertical suspension travel.
                suspTravelDirections[i] = new PxVec3(0, -1, 0);

                //Wheel center offset is offset from rigid body center of mass.
                wheelCentreCMOffsets[i] =
                    wheelCenterActorOffsets[i].operator_Minus(chassisCMOffset); //BIOQUIRK: Operator overload

                //Suspension force application point 0.3 metres below
                //rigid body center of mass.
                suspForceAppCMOffsets[i] =
                    new PxVec3(wheelCentreCMOffsets[i].x, -0.3f, wheelCentreCMOffsets[i].z);

                //Tire force application point 0.3 metres below
                //rigid body center of mass.
                tireForceAppCMOffsets[i] =
                    new PxVec3(wheelCentreCMOffsets[i].x, -0.3f, wheelCentreCMOffsets[i].z);
            }
        }

        //Set up the filter data of the raycast that will be issued by each suspension.
        PxFilterData qryFilterData = new();
        setupNonDrivableSurface(ref qryFilterData);

        //Set the wheel, tire and suspension data.
        //Set the geometry data.
        //Set the query filter data
        for (uint i = 0; i < numWheels; i++)
        {
            wheelsSimData->setWheelData(i, wheels[i]);
            wheelsSimData->setTireData(i, tires[i]);
            wheelsSimData->setSuspensionData(i, suspensions[i]);
            wheelsSimData->setSuspTravelDirection(i, suspTravelDirections[i]);
            wheelsSimData->setWheelCentreOffset(i, wheelCentreCMOffsets[i]);
            wheelsSimData->setSuspForceAppPointOffset(i, suspForceAppCMOffsets[i]);
            wheelsSimData->setTireForceAppPointOffset(i, tireForceAppCMOffsets[i]);
            wheelsSimData->setSceneQueryFilterData(i, qryFilterData);
            wheelsSimData->setWheelShapeMapping(i, (int)i);
        }

        //Add a front and rear anti-roll bar
        PxVehicleAntiRollBarData barFront = new();
        barFront.mWheel0 = (uint)PxVehicleDrive4WWheelOrder.eFRONT_LEFT;
        barFront.mWheel1 = (uint)PxVehicleDrive4WWheelOrder.eFRONT_RIGHT;
        barFront.mStiffness = 10000.0f;
        wheelsSimData->addAntiRollBarData(barFront);
        PxVehicleAntiRollBarData barRear = new();
        barRear.mWheel0 = (uint)PxVehicleDrive4WWheelOrder.eREAR_LEFT;
        barRear.mWheel1 = (uint)PxVehicleDrive4WWheelOrder.eREAR_RIGHT;
        barRear.mStiffness = 10000.0f;
        wheelsSimData->addAntiRollBarData(barRear);
    }
}

public unsafe static partial class Globals
{
    public static PxVehicleDrive4W* createVehicle4W(in VehicleDesc vehicle4WDesc, PxPhysics* physics, PxCooking* cooking)
    {
        PxVec3 chassisDims = vehicle4WDesc.chassisDims;
        float wheelWidth = vehicle4WDesc.wheelWidth;
        float wheelRadius = vehicle4WDesc.wheelRadius;
        uint numWheels = vehicle4WDesc.numWheels;

        ref readonly PxFilterData chassisSimFilterData = ref vehicle4WDesc.chassisSimFilterData;
        ref readonly PxFilterData wheelSimFilterData = ref vehicle4WDesc.wheelSimFilterData;

        //Construct a physx actor with shapes for the chassis and wheels.
        //Set the rigid body mass, moment of inertia, and center of mass offset.
        PxRigidDynamic* veh4WActor = null;
        {
            //Construct a convex mesh for a cylindrical wheel.
            PxConvexMesh* wheelMesh = createWheelMesh(wheelWidth, wheelRadius, ref *physics, ref *cooking);
            //Assume all wheels are identical for simplicity.
            PxConvexMesh** wheelConvexMeshes = stackalloc PxConvexMesh*[PX_MAX_NB_WHEELS];
            PxMaterial** wheelMaterials = stackalloc PxMaterial*[PX_MAX_NB_WHEELS];

            //Set the meshes and materials for the driven wheels.
            for (uint i = (uint)PxVehicleDrive4WWheelOrder.eFRONT_LEFT; i <= (uint)PxVehicleDrive4WWheelOrder.eREAR_RIGHT; i++)
            {
                wheelConvexMeshes[i] = wheelMesh;
                wheelMaterials[i] = vehicle4WDesc.wheelMaterial;
            }
            //Set the meshes and materials for the non-driven wheels
            for (uint i = (uint)PxVehicleDrive4WWheelOrder.eREAR_RIGHT + 1; i < numWheels; i++)
            {
                wheelConvexMeshes[i] = wheelMesh;
                wheelMaterials[i] = vehicle4WDesc.wheelMaterial;
            }

            //Chassis just has a single convex shape for simplicity.
            PxConvexMesh* chassisConvexMesh = createChassisMesh(chassisDims, ref *physics, ref *cooking);
            PxConvexMesh** chassisConvexMeshes = stackalloc PxConvexMesh*[1] { chassisConvexMesh };
            PxMaterial** chassisMaterials = stackalloc PxMaterial*[1] { vehicle4WDesc.chassisMaterial };

            //Rigid body data.
            PxVehicleChassisData rigidBodyData = new();
            rigidBodyData.mMOI = vehicle4WDesc.chassisMOI;
            rigidBodyData.mMass = vehicle4WDesc.chassisMass;
            rigidBodyData.mCMOffset = vehicle4WDesc.chassisCMOffset;

            veh4WActor = createVehicleActor
                (rigidBodyData,
                wheelMaterials, wheelConvexMeshes, numWheels, wheelSimFilterData,
                chassisMaterials, chassisConvexMeshes, 1, chassisSimFilterData,
                ref *physics);
        }

        //Set up the sim data for the wheels.
        PxVehicleWheelsSimData* wheelsSimData = PxVehicleWheelsSimData.allocate(numWheels);
        {
            //Compute the wheel center offsets from the origin.
            PxVec3* wheelCenterActorOffsets = stackalloc PxVec3[PX_MAX_NB_WHEELS];
            float frontZ = chassisDims.z * 0.3f;
            float rearZ = -chassisDims.z * 0.3f;
            fourwheel.computeWheelCenterActorOffsets4W(frontZ, rearZ, chassisDims, wheelWidth, wheelRadius, numWheels, wheelCenterActorOffsets);

            //Set up the simulation data for all wheels.
            fourwheel.setupWheelsSimulationData
                (vehicle4WDesc.wheelMass, vehicle4WDesc.wheelMOI, wheelRadius, wheelWidth,
                 numWheels, wheelCenterActorOffsets,
                 vehicle4WDesc.chassisCMOffset, vehicle4WDesc.chassisMass,
                 wheelsSimData);
        }

        //Set up the sim data for the vehicle drive model.
        PxVehicleDriveSimData4W driveSimData = new();
        {
            //Diff
            PxVehicleDifferential4WData diff = new();
            diff.mType = PxVehicleDifferential4WData.eDIFF_TYPE_LS_4WD;
            driveSimData.setDiffData(diff);

            //Engine
            PxVehicleEngineData engine = new();
            engine.mPeakTorque = 500.0f;
            engine.mMaxOmega = 600.0f;//approx 6000 rpm
            driveSimData.setEngineData(engine);

            //Gears
            PxVehicleGearsData gears = new();
            gears.mSwitchTime = 0.5f;
            driveSimData.setGearsData(gears);

            //Clutch
            PxVehicleClutchData clutch = new();
            clutch.mStrength = 10.0f;
            driveSimData.setClutchData(clutch);

            //Ackermann steer accuracy
            PxVehicleAckermannGeometryData ackermann;
            ackermann.mAccuracy = 1.0f;
            ackermann.mAxleSeparation =
                //BIOQUIRK: This comes up in a handful of places, but maybe we should revisit C++ ref-returning functions and have them return C# byref if they're const refs?
                wheelsSimData->getWheelCentreOffset((uint)PxVehicleDrive4WWheelOrder.eFRONT_LEFT)->z -
                wheelsSimData->getWheelCentreOffset((uint)PxVehicleDrive4WWheelOrder.eREAR_LEFT)->z;
            ackermann.mFrontWidth =
                wheelsSimData->getWheelCentreOffset((uint)PxVehicleDrive4WWheelOrder.eFRONT_RIGHT)->x -
                wheelsSimData->getWheelCentreOffset((uint)PxVehicleDrive4WWheelOrder.eFRONT_LEFT)->x;
            ackermann.mRearWidth =
                wheelsSimData->getWheelCentreOffset((uint)PxVehicleDrive4WWheelOrder.eREAR_RIGHT)->x -
                wheelsSimData->getWheelCentreOffset((uint)PxVehicleDrive4WWheelOrder.eREAR_LEFT)->x;
            driveSimData.setAckermannGeometryData(ackermann);
        }

        //Create a vehicle from the wheels and drive sim data.
        PxVehicleDrive4W* vehDrive4W = PxVehicleDrive4W.allocate(numWheels);
        vehDrive4W->setup(physics, veh4WActor, *wheelsSimData, driveSimData, numWheels - 4);

        //Configure the userdata
        configureUserData(vehDrive4W, vehicle4WDesc.actorUserData, vehicle4WDesc.shapeUserDatas);

        //Free the sim data because we don't need that any more.
        wheelsSimData->free();

        return vehDrive4W;
    }
}
