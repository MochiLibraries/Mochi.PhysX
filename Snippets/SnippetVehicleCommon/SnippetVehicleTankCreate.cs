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
using static Mochi.PhysX.Globals;
using static SnippetVehicleCommon.Globals;

namespace SnippetVehicleCommon;

internal unsafe static class tank
{
    internal static void computeWheelCenterActorOffsets(
        float wheelFrontZ, float wheelRearZ, in PxVec3 chassisDims, float wheelWidth, float wheelRadius, uint numWheels, PxVec3* wheelCentreOffsets)
    {
        //chassisDims.z is the distance from the rear of the chassis to the front of the chassis.
        //The front has z = 0.5*chassisDims.z and the rear has z = -0.5*chassisDims.z.
        //Compute a position for the front wheel and the rear wheel along the z-axis.
        //Compute the separation between each wheel along the z-axis.
        float numLeftWheels = numWheels / 2.0f;
        float deltaZ = (wheelFrontZ - wheelRearZ) / (numLeftWheels - 1.0f);
        //Set the outside of the left and right wheels to be flush with the chassis.
        //Set the top of the wheel to be just touching the underside of the chassis.
        for (uint i = 0; i < numWheels; i += 2)
        {
            //Left wheel offset from origin.
            wheelCentreOffsets[i + 0] = new PxVec3((-chassisDims.x + wheelWidth) * 0.5f, -(chassisDims.y / 2 + wheelRadius), wheelRearZ + i * deltaZ * 0.5f);
            //Right wheel offsets from origin.
            wheelCentreOffsets[i + 1] = new PxVec3((+chassisDims.x - wheelWidth) * 0.5f, -(chassisDims.y / 2 + wheelRadius), wheelRearZ + i * deltaZ * 0.5f);
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
            //Increase the damping on the wheel.
            for (uint i = 0; i < numWheels; i++)
            {
                wheels[i] = new();
                wheels[i].mMass = wheelMass;
                wheels[i].mMOI = wheelMOI;
                wheels[i].mRadius = wheelRadius;
                wheels[i].mWidth = wheelWidth;
                wheels[i].mDampingRate = 2.0f;
            }
        }

        //Set up the tires.
        PxVehicleTireData* tires = stackalloc PxVehicleTireData[PX_MAX_NB_WHEELS];
        {
            //Set all tire types to "normal" type.
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
            PxVehicleComputeSprungMasses(numWheels, wheelCenterActorOffsets, chassisCMOffset, chassisMass, 1, suspSprungMasses);

            //Set the suspension data.
            for (uint i = 0; i < numWheels; i++)
            {
                suspensions[i] = new();
                suspensions[i].mMaxCompression = 0.3f;
                suspensions[i].mMaxDroop = 0.1f;
                suspensions[i].mSpringStrength = 10000.0f;
                suspensions[i].mSpringDamperRate = 1500.0f;
                suspensions[i].mSprungMass = suspSprungMasses[i];
            }
        }

        //Set up the wheel geometry.
        PxVec3* suspTravelDirections = stackalloc PxVec3[PX_MAX_NB_WHEELS];
        PxVec3* wheelCentreCMOffsets = stackalloc PxVec3[PX_MAX_NB_WHEELS];
        PxVec3* suspForceAppCMOffsets = stackalloc PxVec3[PX_MAX_NB_WHEELS];
        PxVec3* tireForceAppCMOffsets = stackalloc PxVec3[PX_MAX_NB_WHEELS];
        {
            for (uint i = 0; i < numWheels; i++)
            {
                //Vertical suspension travel.
                suspTravelDirections[i] = new PxVec3(0, -1, 0);

                //Wheel center offset is offset from rigid body center of mass.
                wheelCentreCMOffsets[i] = wheelCenterActorOffsets[i].operator_Minus(chassisCMOffset); //BIOQUIRK: Operator overload

                //Suspension force application point 0.3 metres below rigid body center of mass.
                suspForceAppCMOffsets[i] = new PxVec3(wheelCentreCMOffsets[i].x, -0.3f, wheelCentreCMOffsets[i].z);

                //Tire force application point 0.3 metres below rigid body center of mass.
                tireForceAppCMOffsets[i] = new PxVec3(wheelCentreCMOffsets[i].x, -0.3f, wheelCentreCMOffsets[i].z);
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
    }
}

public unsafe static partial class Globals
{
    public static PxVehicleDriveTank* createVehicleTank(in VehicleDesc tankDesc, PxPhysics* physics, PxCooking* cooking)
    {
        PxVec3 chassisDims = tankDesc.chassisDims;
        float wheelWidth = tankDesc.wheelWidth;
        float wheelRadius = tankDesc.wheelRadius;
        uint numWheels = tankDesc.numWheels;

        ref readonly PxFilterData chassisSimFilterData = ref tankDesc.chassisSimFilterData;
        ref readonly PxFilterData wheelSimFilterData = ref tankDesc.wheelSimFilterData;

        //Construct a physx actor with shapes for the chassis and wheels.
        //Set the rigid body mass, moment of inertia, and center of mass offset.
        PxRigidDynamic* tankActor = null;
        {
            //Construct a convex mesh for a cylindrical wheel.
            PxConvexMesh* wheelMesh = createWheelMesh(wheelWidth, wheelRadius, ref *physics, ref *cooking);
            //Assume all wheels are identical for simplicity.
            PxConvexMesh** wheelConvexMeshes = stackalloc PxConvexMesh*[PX_MAX_NB_WHEELS];
            PxMaterial** wheelMaterials = stackalloc PxMaterial*[PX_MAX_NB_WHEELS];
            for (uint i = 0; i < numWheels; i++)
            {
                wheelConvexMeshes[i] = wheelMesh;
                wheelMaterials[i] = tankDesc.wheelMaterial;
            }

            //Chassis just has a single convex shape for simplicity.
            PxConvexMesh* chassisConvexMesh = createChassisMesh(chassisDims, ref *physics, ref *cooking);
            PxConvexMesh** chassisConvexMeshes = stackalloc PxConvexMesh*[1] { chassisConvexMesh };
            PxMaterial** chassisMaterials = stackalloc PxMaterial*[1] { tankDesc.chassisMaterial };

            //Rigid body data.
            PxVehicleChassisData rigidBodyData = new();
            rigidBodyData.mMOI = tankDesc.chassisMOI;
            rigidBodyData.mMass = tankDesc.chassisMass;
            rigidBodyData.mCMOffset = tankDesc.chassisCMOffset;

            tankActor = createVehicleActor
                (rigidBodyData,
                wheelMaterials, wheelConvexMeshes, numWheels, wheelSimFilterData,
                chassisMaterials, chassisConvexMeshes, 1, chassisSimFilterData,
                ref *physics);
        }

        //Set up the sim data for the wheels.
        PxVehicleWheelsSimData* wheelsSimData = PxVehicleWheelsSimData.allocate(numWheels);
        {
            //Compute the wheel center offsets from the origin.
            PxVec3* wheelCentreActorOffsets = stackalloc PxVec3[PX_MAX_NB_WHEELS];
            float frontZ = chassisDims.z * 0.35f;
            float rearZ = -chassisDims.z * 0.35f;
            tank.computeWheelCenterActorOffsets(frontZ, rearZ, chassisDims, wheelWidth, wheelRadius, numWheels, wheelCentreActorOffsets);

            tank.setupWheelsSimulationData
                (tankDesc.wheelMass, tankDesc.wheelMOI, wheelRadius, wheelWidth,
                 numWheels, wheelCentreActorOffsets,
                 tankDesc.chassisCMOffset, tankDesc.chassisMass,
                 wheelsSimData);
        }

        //Set up the sim data for the tank drive model.
        PxVehicleDriveSimData driveSimData = new();
        {
            //Set up the engine to be more powerful but also more damped than the default engine.
            PxVehicleEngineData engineData = *driveSimData.getEngineData();
            engineData.mPeakTorque *= 2.0f;
            engineData.mDampingRateZeroThrottleClutchEngaged = 2.0f;
            engineData.mDampingRateZeroThrottleClutchDisengaged = 0.5f;
            engineData.mDampingRateFullThrottle = 0.5f;
            driveSimData.setEngineData(engineData);
        }

        //Create a tank from the wheels and drive sim data.
        PxVehicleDriveTank* vehDriveTank = PxVehicleDriveTank.allocate(numWheels);
        vehDriveTank->setup(physics, tankActor, *wheelsSimData, driveSimData, numWheels);

        //Configure the userdata
        configureUserData(vehDriveTank, tankDesc.actorUserData, tankDesc.shapeUserDatas);

        //Free the sim data because we don't need that any more.
        wheelsSimData->free();

        return vehDriveTank;
    }
}
