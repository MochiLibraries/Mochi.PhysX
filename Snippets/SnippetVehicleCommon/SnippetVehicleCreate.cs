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
using Mochi.PhysX.Infrastructure;
using System;
using System.Diagnostics;
using static Mochi.PhysX.Globals;
using static Mochi.PhysX.PxIDENTITY;

namespace SnippetVehicleCommon;

////////////////////////////////////////////////

public unsafe static partial class Globals
{
    public static PxRigidStatic* createDrivablePlane(in PxFilterData simFilterData, PxMaterial* material, PxPhysics* physics)
    {
        //Add a plane to the scene.
        PxRigidStatic* groundPlane = PxCreatePlane(ref *physics, new PxPlane(0, 1, 0, 0), ref *material);

        //Get the plane shape so we can set query and simulation filter data.
        PxShape** shapes = stackalloc PxShape*[1];
        groundPlane->getShapes(shapes, 1);

        //Set the query filter data of the ground plane so that the vehicle raycasts can hit the ground.
        PxFilterData qryFilterData = new();
        setupDrivableSurface(ref qryFilterData);
        shapes[0]->setQueryFilterData(qryFilterData);

        //Set the simulation filter data of the ground plane so that it collides with the chassis of a vehicle but not the wheels.
        shapes[0]->setSimulationFilterData(simFilterData);

        return groundPlane;
    }
}

////////////////////////////////////////////////

public unsafe struct ActorUserData
{
    // ActorUserData constructor omitted since it's default C# behavior.

    public PxVehicleWheels* vehicle;
    public PxActor* actor;
}

public unsafe struct ShapeUserData
{
    public ShapeUserData()
    {
        isWheel = false;
        wheelId = 0xffffffff;
    }

    public bool isWheel;
    public uint wheelId;
}

public unsafe struct VehicleDesc
{
    // VehicleDesc constructor omitted since it's default C# behavior.

    public float chassisMass;
    public PxVec3 chassisDims;
    public PxVec3 chassisMOI;
    public PxVec3 chassisCMOffset;
    public PxMaterial* chassisMaterial;
    public PxFilterData chassisSimFilterData; //word0 = collide type, word1 = collide against types, word2 = PxPairFlags

    public float wheelMass;
    public float wheelWidth;
    public float wheelRadius;
    public float wheelMOI;
    public PxMaterial* wheelMaterial;
    public uint numWheels;
    public PxFilterData wheelSimFilterData; //word0 = collide type, word1 = collide against types, word2 = PxPairFlags

    public ActorUserData* actorUserData;
    public ShapeUserData* shapeUserDatas;
}

// The following functions are defined in SnippetVehicleCreate.h but are declared in files beyond SnippetVehicleCreate.cpp so they are not implemented here:
// createVehicle4W
// createVehicleTank
// createVehicleNoDrive

////////////////////////////////////////////////

public unsafe static partial class Globals
{
    private static PxConvexMesh* createConvexMesh(PxVec3* verts, uint numVerts, ref PxPhysics physics, ref PxCooking cooking)
    {
        // Create descriptor for convex mesh
        PxConvexMeshDesc convexDesc = new();
        convexDesc.points.count = numVerts;
        convexDesc.points.stride = (uint)sizeof(PxVec3);
        convexDesc.points.data = verts;
        convexDesc.flags = PxConvexFlags.eCOMPUTE_CONVEX;

        PxConvexMesh* convexMesh = null;
        PxDefaultMemoryOutputStream buf = new(ref *PxGetFoundation()->getAllocatorCallback()); //BIOQUIRK: VERY IMPORTANT MISSING DEFAULT new() constructs an invalid object!
        if (cooking.cookConvexMesh(convexDesc, ref buf))
        {
            PxDefaultMemoryInputData id = new(buf.getData(), buf.getSize());
            convexMesh = physics.createConvexMesh(ref id);
        }

        return convexMesh;
    }

    public static PxConvexMesh* createChassisMesh(PxVec3 dims, ref PxPhysics physics, ref PxCooking cooking)
    {
        float x = dims.x * 0.5f;
        float y = dims.y * 0.5f;
        float z = dims.z * 0.5f;
        PxVec3* verts = stackalloc PxVec3[8]
        {
            new PxVec3(x, y, -z),
            new PxVec3(x, y, z),
            new PxVec3(x, -y, z),
            new PxVec3(x, -y, -z),
            new PxVec3(-x, y, -z),
            new PxVec3(-x, y, z),
            new PxVec3(-x, -y, z),
            new PxVec3(-x, -y, -z)
        };

        return createConvexMesh(verts, 8, ref physics, ref cooking);
    }

    public static PxConvexMesh* createWheelMesh(float width, float radius, ref PxPhysics physics, ref PxCooking cooking)
    {
        PxVec3* points = stackalloc PxVec3[2 * 16];
        for (uint i = 0; i < 16; i++)
        {
            float cosTheta = PxCos(i * MathF.PI * 2.0f / 16.0f);
            float sinTheta = PxSin(i * MathF.PI * 2.0f / 16.0f);
            float y = radius * cosTheta;
            float z = radius * sinTheta;
            points[2 * i + 0] = new PxVec3(-width / 2.0f, y, z);
            points[2 * i + 1] = new PxVec3(+width / 2.0f, y, z);
        }

        return createConvexMesh(points, 32, ref physics, ref cooking);
    }

    ////////////////////////////////////////////////

    public static PxRigidDynamic* createVehicleActor(
        in PxVehicleChassisData chassisData,
        PxMaterial** wheelMaterials, PxConvexMesh** wheelConvexMeshes, uint numWheels, in PxFilterData wheelSimFilterData,
        PxMaterial** chassisMaterials, PxConvexMesh** chassisConvexMeshes, uint numChassisMeshes, in PxFilterData chassisSimFilterData,
        ref PxPhysics physics)
    {
        //We need a rigid body actor for the vehicle.
        //Don't forget to add the actor to the scene after setting up the associated vehicle.
        PxRigidDynamic* vehActor = physics.createRigidDynamic(new PxTransform(PxIdentity));

        //Wheel and chassis query filter data.
        //Optional: cars don't drive on other cars.
        PxFilterData wheelQryFilterData = new();
        setupNonDrivableSurface(ref wheelQryFilterData);
        PxFilterData chassisQryFilterData = new();
        setupNonDrivableSurface(ref chassisQryFilterData);

        //Add all the wheel shapes to the actor.
        for (uint i = 0; i < numWheels; i++)
        {
            PxConvexMeshGeometry geom = new(wheelConvexMeshes[i],
                new PxMeshScale(), PxConvexMeshGeometryFlags.eTIGHT_BOUNDS); //BIOQUIRK: Missing defaults
            PxShape* wheelShape = PxRigidActorExt.createExclusiveShape(ref *vehActor, geom, *wheelMaterials[i],
                PxShapeFlags.eVISUALIZATION | PxShapeFlags.eSCENE_QUERY_SHAPE | PxShapeFlags.eSIMULATION_SHAPE); //BIOQUIRK: Missing default
            wheelShape->setQueryFilterData(wheelQryFilterData);
            wheelShape->setSimulationFilterData(wheelSimFilterData);
            wheelShape->setLocalPose(new PxTransform(PxIdentity));
        }

        //Add the chassis shapes to the actor.
        for (uint i = 0; i < numChassisMeshes; i++)
        {
            PxShape* chassisShape = PxRigidActorExt.createExclusiveShape(ref *vehActor,
                new PxConvexMeshGeometry(chassisConvexMeshes[i], new PxMeshScale(), PxConvexMeshGeometryFlags.eTIGHT_BOUNDS), // BIOQUIRK: Missing defaults
                *chassisMaterials[i],
                PxShapeFlags.eVISUALIZATION | PxShapeFlags.eSCENE_QUERY_SHAPE | PxShapeFlags.eSIMULATION_SHAPE); //BIOQUIRK: Missing default
            chassisShape->setQueryFilterData(chassisQryFilterData);
            chassisShape->setSimulationFilterData(chassisSimFilterData);
            chassisShape->setLocalPose(new PxTransform(PxIdentity));
        }

        vehActor->setMass(chassisData.mMass);
        vehActor->setMassSpaceInertiaTensor(chassisData.mMOI);
        vehActor->setCMassLocalPose(new PxTransform(chassisData.mCMOffset, new PxQuat(PxIdentity)));

        return vehActor;
    }

    //BIOQUIRK: Maybe we should use a source generator to automatically generate a generic overload for methods like this that handle the "awkward generic cast" issue?
    //  (This applies in many locations (especially in this assembly), but putting it here since it's where I had the thought.)
    public static void configureUserData<TPxVehicleWheels>(TPxVehicleWheels* vehicleT, ActorUserData* actorUserData, ShapeUserData* shapeUserDatas)
        where TPxVehicleWheels : unmanaged, IPxVehicleWheels
    {
        PxVehicleWheels* vehicle = (PxVehicleWheels*)vehicleT; //BIOQUIRK: Awkward generic cast

        if (actorUserData != null)
        {
            vehicle->getRigidDynamicActor()->userData = actorUserData;
            actorUserData->vehicle = vehicle;
        }

        if (shapeUserDatas != null)
        {
            PxShape** shapes = stackalloc PxShape*[PX_MAX_NB_WHEELS + 1];
            vehicle->getRigidDynamicActor()->getShapes(shapes, PX_MAX_NB_WHEELS + 1);
            for (uint i = 0; i < vehicle->mWheelsSimData.getNbWheels(); i++)
            {
                int shapeId = vehicle->mWheelsSimData.getWheelShapeMapping(i);
                shapes[shapeId]->userData = &shapeUserDatas[i];
                shapeUserDatas[i].isWheel = true;
                shapeUserDatas[i].wheelId = i;
            }
        }
    }

    ////////////////////////////////////////////////

    public static void customizeVehicleToLengthScale(float lengthScale, PxRigidDynamic* rigidDynamic, PxVehicleWheelsSimData* wheelsSimData, PxVehicleDriveSimData* driveSimData)
    {
        //Rigid body center of mass and moment of inertia.
        {
            PxTransform t = rigidDynamic->getCMassLocalPose();
            t.p.operator_StarEqual(lengthScale); //BIOQUIRK: Overloaded operator
            rigidDynamic->setCMassLocalPose(t);

            PxVec3 moi = rigidDynamic->getMassSpaceInertiaTensor();
            moi.operator_StarEqual(lengthScale * lengthScale); //BIOQUIRK: Overloaded operatror
            rigidDynamic->setMassSpaceInertiaTensor(moi);
        }

        //Wheels, suspensions, wheel centers, tire/susp force application points.
        {
            for (uint i = 0; i < wheelsSimData->getNbWheels(); i++)
            {
                PxVehicleWheelData wheelData = *wheelsSimData->getWheelData(i);
                wheelData.mRadius *= lengthScale;
                wheelData.mWidth *= lengthScale;
                wheelData.mDampingRate *= lengthScale * lengthScale;
                wheelData.mMaxBrakeTorque *= lengthScale * lengthScale;
                wheelData.mMaxHandBrakeTorque *= lengthScale * lengthScale;
                wheelData.mMOI *= lengthScale * lengthScale;
                wheelsSimData->setWheelData(i, wheelData);

                PxVehicleSuspensionData suspData = *wheelsSimData->getSuspensionData(i);
                suspData.mMaxCompression *= lengthScale;
                suspData.mMaxDroop *= lengthScale;
                wheelsSimData->setSuspensionData(i, suspData);

                PxVec3 v = *wheelsSimData->getWheelCentreOffset(i);
                v.operator_StarEqual(lengthScale); //BIOQUIRK: Operator overload
                wheelsSimData->setWheelCentreOffset(i, v);

                v = *wheelsSimData->getSuspForceAppPointOffset(i);
                v.operator_StarEqual(lengthScale); //BIOQUIRK: Operator overload
                wheelsSimData->setSuspForceAppPointOffset(i, v);

                v = *wheelsSimData->getTireForceAppPointOffset(i);
                v.operator_StarEqual(lengthScale); //BIOQUIRK: Operator overload
                wheelsSimData->setTireForceAppPointOffset(i, v);
            }
        }

        //Slow forward speed correction.
        {
            wheelsSimData->setSubStepCount(5.0f * lengthScale, 3, 1);
            wheelsSimData->setMinLongSlipDenominator(4.0f * lengthScale);
        }

        //Engine
        if (driveSimData != null)
        {
            PxVehicleEngineData engineData = *driveSimData->getEngineData();
            engineData.mMOI *= lengthScale * lengthScale;
            engineData.mPeakTorque *= lengthScale * lengthScale;
            engineData.mDampingRateFullThrottle *= lengthScale * lengthScale;
            engineData.mDampingRateZeroThrottleClutchEngaged *= lengthScale * lengthScale;
            engineData.mDampingRateZeroThrottleClutchDisengaged *= lengthScale * lengthScale;
            driveSimData->setEngineData(engineData);
        }

        //Clutch.
        if (driveSimData != null)
        {
            PxVehicleClutchData clutchData = *driveSimData->getClutchData();
            clutchData.mStrength *= lengthScale * lengthScale;
            driveSimData->setClutchData(clutchData);
        }

        //Scale the collision meshes too.
        {
            PxShape** shapes = stackalloc PxShape*[16];
            uint nbShapes = rigidDynamic->getShapes(shapes, 16);
            for (uint i = 0; i < nbShapes; i++)
            {
                switch (shapes[i]->getGeometryType())
                {
                    case PxGeometryType.eSPHERE:
                    {
                        PxSphereGeometry sphere = new();
                        shapes[i]->getSphereGeometry(ref sphere); //BIOQUIRK: These functions would more properly by exposed as out byref
                        sphere.radius *= lengthScale;
                        shapes[i]->setGeometry(sphere);
                    }
                    break;
                    case PxGeometryType.ePLANE:
                        Debug.Assert(false);
                        break;
                    case PxGeometryType.eCAPSULE:
                    {
                        PxCapsuleGeometry capsule = new();
                        shapes[i]->getCapsuleGeometry(ref capsule);
                        capsule.radius *= lengthScale;
                        capsule.halfHeight *= lengthScale;
                        shapes[i]->setGeometry(capsule);
                    }
                    break;
                    case PxGeometryType.eBOX:
                    {
                        PxBoxGeometry box = new();
                        shapes[i]->getBoxGeometry(ref box);
                        box.halfExtents.operator_StarEqual(lengthScale); //BIOQUIRK: Operator overload
                        shapes[i]->setGeometry(box);
                    }
                    break;
                    case PxGeometryType.eCONVEXMESH:
                    {
                        PxConvexMeshGeometry convexMesh = new();
                        shapes[i]->getConvexMeshGeometry(ref convexMesh);
                        convexMesh.scale.scale.operator_StarEqual(lengthScale); //BIOQUIRK: Operator overload
                        shapes[i]->setGeometry(convexMesh);
                    }
                    break;
                    case PxGeometryType.eTRIANGLEMESH:
                    {
                        PxTriangleMeshGeometry triMesh = new();
                        shapes[i]->getTriangleMeshGeometry(ref triMesh);
                        triMesh.scale.scale.operator_StarEqual(lengthScale); //BIOQUIRK: Operator overload
                        shapes[i]->setGeometry(triMesh);
                    }
                    break;
                    case PxGeometryType.eHEIGHTFIELD:
                    {
                        PxHeightFieldGeometry hf = new();
                        shapes[i]->getHeightFieldGeometry(ref hf);
                        hf.columnScale *= lengthScale;
                        hf.heightScale *= lengthScale;
                        hf.rowScale *= lengthScale;
                        shapes[i]->setGeometry(hf);
                    }
                    break;
                    case PxGeometryType.eINVALID:
                    case PxGeometryType.eGEOMETRY_COUNT:
                        break;
                }
            }
        }
    }

    public static void customizeVehicleToLengthScale(float lengthScale, PxRigidDynamic* rigidDynamic, PxVehicleWheelsSimData* wheelsSimData, PxVehicleDriveSimData4W* driveSimData)
    {
        customizeVehicleToLengthScale(lengthScale, rigidDynamic, wheelsSimData, static_cast<PxVehicleDriveSimData>(driveSimData));

        //Ackermann geometry.
        if (driveSimData != null)
        {
            PxVehicleAckermannGeometryData ackermannData = *driveSimData->getAckermannGeometryData();
            ackermannData.mAxleSeparation *= lengthScale;
            ackermannData.mFrontWidth *= lengthScale;
            ackermannData.mRearWidth *= lengthScale;
            driveSimData->setAckermannGeometryData(ackermannData);
        }
    }
}
