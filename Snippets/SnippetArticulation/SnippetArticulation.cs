// ****************************************************************************
// This snippet demonstrates the use of articulations.
// ****************************************************************************
#define USE_REDUCED_COORDINATE_ARTICULATION
#define CREATE_SCISSOR_LIFT

#pragma warning disable CS0162 // Unreachable code detected

using Mochi.PhysX;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SnippetCommon;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static Mochi.PhysX.Globals;
using static Mochi.PhysX.PxIDENTITY;

internal unsafe static class SnippetArticulation
{
#if CREATE_SCISSOR_LIFT
    public static bool gCreateLiftScene = true;
#else
    public static bool gCreateLiftScene = false;
#endif

    static Pinned<PxDefaultAllocator> gAllocator = new PxDefaultAllocator();
    static Pinned<PxDefaultErrorCallback> gErrorCallback = new PxDefaultErrorCallback();

    static PxFoundation* gFoundation = null;
    static PxPhysics* gPhysics = null;

    static PxDefaultCpuDispatcher* gDispatcher = null;
    static PxScene* gScene = null;

    static PxMaterial* gMaterial = null;

    static PxPvd* gPvd = null;

#if USE_REDUCED_COORDINATE_ARTICULATION
    static PxArticulationReducedCoordinate* gArticulation = null;
    static PxArticulationJointReducedCoordinate* gDriveJoint = null;
#else
    static PxArticulation* gArticulation = null;
#endif

#if USE_REDUCED_COORDINATE_ARTICULATION
    //BIOQUIRK: Exposed return buffer
    //BOPQUIRK: PxPairFlags should ideally be a byref
    //BIOQUIRK: Both PxFilterData parameters are implicity byref
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    static PxFilterFlags* scissorFilter(PxFilterFlags* __retBuf,
                                        uint attributes0, PxFilterData* filterData0,
                                        uint attributes1, PxFilterData* filterData1,
                                        PxPairFlags* pairFlags, void* constantBlock, uint constantBlockSize)
    {
        if (filterData0->word2 != 0 && filterData0->word2 == filterData1->word2)
        {
            *__retBuf = PxFilterFlags.eKILL;
            return __retBuf;
        }

        *pairFlags |= PxPairFlags.eCONTACT_DEFAULT;
        *__retBuf = PxFilterFlags.eDEFAULT;
        return __retBuf;
    }

    static void createScissorLift()
    {
        const float runnerLength = 2f;
        const float placementDistance = 1.8f;

        const float cosAng = (placementDistance) / (runnerLength);

        float angle = PxAcos(cosAng);

        float sinAng = PxSin(angle);

        PxQuat leftRot = new(-angle, new PxVec3(1f, 0f, 0f));
        PxQuat rightRot = new(angle, new PxVec3(1f, 0f, 0f));

        //(1) Create base...
        PxArticulationLink* @base = gArticulation->createLink(null, new PxTransform(new PxVec3(0f, 0.25f, 0f)));
        const PxShapeFlags defaultShapeFlags = PxShapeFlags.eVISUALIZATION | PxShapeFlags.eSCENE_QUERY_SHAPE | PxShapeFlags.eSIMULATION_SHAPE; //BIOQUIRK: Many missing defaults
        PxRigidActorExt.createExclusiveShape(ref *@base, new PxBoxGeometry(0.5f, 0.25f, 1.5f), *gMaterial, defaultShapeFlags);
        PxRigidBodyExt.updateMassAndInertia(ref *@base, 3f);

        //Now create the slider and fixed joints...

        gArticulation->setSolverIterationCounts(32);

        PxArticulationLink* leftRoot = gArticulation->createLink(@base, new PxTransform(new PxVec3(0f, 0.55f, -0.9f)));
        PxRigidActorExt.createExclusiveShape(ref *leftRoot, new PxBoxGeometry(0.5f, 0.05f, 0.05f), *gMaterial, defaultShapeFlags);
        PxRigidBodyExt.updateMassAndInertia(ref *leftRoot, 1f);

        PxArticulationLink* rightRoot = gArticulation->createLink(@base, new PxTransform(new PxVec3(0f, 0.55f, 0.9f)));
        PxRigidActorExt.createExclusiveShape(ref *rightRoot, new PxBoxGeometry(0.5f, 0.05f, 0.05f), *gMaterial, defaultShapeFlags);
        PxRigidBodyExt.updateMassAndInertia(ref *rightRoot, 1f);

        PxArticulationJointReducedCoordinate* joint = static_cast<PxArticulationJointReducedCoordinate>(leftRoot->getInboundJoint());
        joint->setJointType(PxArticulationJointType.eFIX);
        joint->setParentPose(new PxTransform(new PxVec3(0f, 0.25f, -0.9f)));
        joint->setChildPose(new PxTransform(new PxVec3(0f, -0.05f, 0f)));

        //Set up the drive joint...
        gDriveJoint = static_cast<PxArticulationJointReducedCoordinate>(rightRoot->getInboundJoint());
        gDriveJoint->setJointType(PxArticulationJointType.ePRISMATIC);
        gDriveJoint->setMotion(PxArticulationAxis.eZ, PxArticulationMotions.eLIMITED);
        gDriveJoint->setLimit(PxArticulationAxis.eZ, -1.4f, 0.2f);
        gDriveJoint->setDrive(PxArticulationAxis.eZ, 100000f, 0f, float.MaxValue);

        gDriveJoint->setParentPose(new PxTransform(new PxVec3(0f, 0.25f, 0.9f)));
        gDriveJoint->setChildPose(new PxTransform(new PxVec3(0f, -0.05f, 0f)));


        const uint linkHeight = 3;
        PxArticulationLink* currLeft = leftRoot, currRight = rightRoot;

        PxQuat rightParentRot = new(PxIdentity);
        PxQuat leftParentRot = new(PxIdentity);
        for (uint i = 0; i < linkHeight; ++i)
        {
            PxVec3 pos = new(0.5f, 0.55f + 0.1f * (1 + i), 0f);
            PxArticulationLink* leftLink = gArticulation->createLink(currLeft, new PxTransform(pos.operator_Plus(new PxVec3(0f, sinAng * (2 * i + 1), 0f)), leftRot)); //BIOQUIRK: Operator overload
            PxRigidActorExt.createExclusiveShape(ref *leftLink, new PxBoxGeometry(0.05f, 0.05f, 1f), *gMaterial, defaultShapeFlags);
            PxRigidBodyExt.updateMassAndInertia(ref *leftLink, 1f);

            PxVec3 leftAnchorLocation = pos.operator_Plus(new PxVec3(0f, sinAng * (2 * i), -0.9f)); //BIOQUIRK: Operator overload

            joint = static_cast<PxArticulationJointReducedCoordinate>(leftLink->getInboundJoint());
            joint->setParentPose(new PxTransform(currLeft->getGlobalPose().transformInv(leftAnchorLocation), leftParentRot));
            joint->setChildPose(new PxTransform(new PxVec3(0f, 0f, -1f), rightRot));
            joint->setJointType(PxArticulationJointType.eREVOLUTE);

            leftParentRot = leftRot;

            joint->setMotion(PxArticulationAxis.eTWIST, PxArticulationMotions.eLIMITED);
            joint->setLimit(PxArticulationAxis.eTWIST, -MathF.PI, angle);


            PxArticulationLink* rightLink = gArticulation->createLink(currRight, new PxTransform(pos.operator_Plus(new PxVec3(0f, sinAng * (2 * i + 1), 0f)), rightRot)); //BIOQUIRK: Operator overload
            PxRigidActorExt.createExclusiveShape(ref *rightLink, new PxBoxGeometry(0.05f, 0.05f, 1f), *gMaterial, defaultShapeFlags);
            PxRigidBodyExt.updateMassAndInertia(ref *rightLink, 1f);

            PxVec3 rightAnchorLocation = pos.operator_Plus(new PxVec3(0f, sinAng * (2 * i), 0.9f)); //BIOQUIRK: Operator overload

            joint = static_cast<PxArticulationJointReducedCoordinate>(rightLink->getInboundJoint());
            joint->setJointType(PxArticulationJointType.eREVOLUTE);
            joint->setParentPose(new PxTransform(currRight->getGlobalPose().transformInv(rightAnchorLocation), rightParentRot));
            joint->setChildPose(new PxTransform(new PxVec3(0f, 0f, 1f), leftRot));
            joint->setMotion(PxArticulationAxis.eTWIST, PxArticulationMotions.eLIMITED);
            joint->setLimit(PxArticulationAxis.eTWIST, -angle, MathF.PI);

            rightParentRot = rightRot;

            PxD6Joint* d6joint = PxD6JointCreate(ref *gPhysics, leftLink, new PxTransform(PxIdentity), rightLink, new PxTransform(PxIdentity));

            d6joint->setMotion(PxD6Axis.eTWIST, PxD6Motion.eFREE);
            d6joint->setMotion(PxD6Axis.eSWING2, PxD6Motion.eFREE);
            d6joint->setMotion(PxD6Axis.eSWING1, PxD6Motion.eFREE);

            currLeft = rightLink;
            currRight = leftLink;
        }


        PxArticulationLink* leftTop = gArticulation->createLink(currLeft, currLeft->getGlobalPose().transform(new PxTransform(new PxVec3(-0.5f, 0f, -1.0f), leftParentRot)));
        PxRigidActorExt.createExclusiveShape(ref *leftTop, new PxBoxGeometry(0.5f, 0.05f, 0.05f), *gMaterial, defaultShapeFlags);
        PxRigidBodyExt.updateMassAndInertia(ref *leftTop, 1f);

        PxArticulationLink* rightTop = gArticulation->createLink(currRight, currRight->getGlobalPose().transform(new PxTransform(new PxVec3(-0.5f, 0f, 1.0f), rightParentRot)));
        PxRigidActorExt.createExclusiveShape(ref *rightTop, new PxCapsuleGeometry(0.05f, 0.8f), *gMaterial, defaultShapeFlags);
        //PxRigidActorExt.createExclusiveShape(ref *rightTop, PxBoxGeometry(0.5f, 0.05f, 0.05f), *gMaterial, defaultShapeFlags);
        PxRigidBodyExt.updateMassAndInertia(ref *rightTop, 1f);

        joint = static_cast<PxArticulationJointReducedCoordinate>(leftTop->getInboundJoint());
        joint->setParentPose(new PxTransform(new PxVec3(0f, 0f, -1f), currLeft->getGlobalPose().q.getConjugate()));
        joint->setChildPose(new PxTransform(new PxVec3(0.5f, 0f, 0f), leftTop->getGlobalPose().q.getConjugate()));
        joint->setJointType(PxArticulationJointType.eREVOLUTE);
        joint->setMotion(PxArticulationAxis.eTWIST, PxArticulationMotions.eFREE);
        //joint->setDrive(PxArticulationAxis.eTWIST, 0f, 10f, float.MaxValue);

        joint = static_cast<PxArticulationJointReducedCoordinate>(rightTop->getInboundJoint());
        joint->setParentPose(new PxTransform(new PxVec3(0f, 0f, 1f), currRight->getGlobalPose().q.getConjugate()));
        joint->setChildPose(new PxTransform(new PxVec3(0.5f, 0f, 0f), rightTop->getGlobalPose().q.getConjugate()));
        joint->setJointType(PxArticulationJointType.eREVOLUTE);
        joint->setMotion(PxArticulationAxis.eTWIST, PxArticulationMotions.eFREE);
        //joint->setDrive(PxArticulationAxis.eTWIST, 0f, 10f, float.MaxValue);


        currLeft = leftRoot;
        currRight = rightRoot;

        rightParentRot = new PxQuat(PxIdentity);
        leftParentRot = new PxQuat(PxIdentity);

        for (uint i = 0; i < linkHeight; ++i)
        {
            PxVec3 pos = new(-0.5f, 0.55f + 0.1f * (1 + i), 0f);
            PxArticulationLink* leftLink = gArticulation->createLink(currLeft, new PxTransform(pos.operator_Plus(new PxVec3(0f, sinAng * (2 * i + 1), 0f)), leftRot)); //BIOQURK: Operator overload
            PxRigidActorExt.createExclusiveShape(ref *leftLink, new PxBoxGeometry(0.05f, 0.05f, 1f), *gMaterial, defaultShapeFlags);
            PxRigidBodyExt.updateMassAndInertia(ref *leftLink, 1f);

            PxVec3 leftAnchorLocation = pos.operator_Plus(new PxVec3(0f, sinAng * (2 * i), -0.9f)); //BIOQUIRK: Operator overload

            joint = static_cast<PxArticulationJointReducedCoordinate>(leftLink->getInboundJoint());
            joint->setJointType(PxArticulationJointType.eREVOLUTE);
            joint->setParentPose(new PxTransform(currLeft->getGlobalPose().transformInv(leftAnchorLocation), leftParentRot));
            joint->setChildPose(new PxTransform(new PxVec3(0f, 0f, -1f), rightRot));

            leftParentRot = leftRot;

            joint->setMotion(PxArticulationAxis.eTWIST, PxArticulationMotions.eLIMITED);
            joint->setLimit(PxArticulationAxis.eTWIST, -float.MaxValue, angle);

            PxArticulationLink* rightLink = gArticulation->createLink(currRight, new PxTransform(pos.operator_Plus(new PxVec3(0f, sinAng * (2 * i + 1), 0f)), rightRot)); //BIOQUIRK: Operator overload
            PxRigidActorExt.createExclusiveShape(ref *rightLink, new PxBoxGeometry(0.05f, 0.05f, 1f), *gMaterial, defaultShapeFlags);
            PxRigidBodyExt.updateMassAndInertia(ref *rightLink, 1f);

            PxVec3 rightAnchorLocation = pos.operator_Plus(new PxVec3(0f, sinAng * (2 * i), 0.9f)); //BIOQUIRK: Operator overload

            /*joint = PxD6JointCreate(ref *getPhysics(), currRight, new PxTransform(currRight->getGlobalPose().transformInv(rightAnchorLocation)),
            rightLink, new PxTransform(new PxVec3(0f, 0f, 1f)));*/

            joint = static_cast<PxArticulationJointReducedCoordinate>(rightLink->getInboundJoint());
            joint->setParentPose(new PxTransform(currRight->getGlobalPose().transformInv(rightAnchorLocation), rightParentRot));
            joint->setJointType(PxArticulationJointType.eREVOLUTE);
            joint->setChildPose(new PxTransform(new PxVec3(0f, 0f, 1f), leftRot));
            joint->setMotion(PxArticulationAxis.eTWIST, PxArticulationMotions.eLIMITED);
            joint->setLimit(PxArticulationAxis.eTWIST, -angle, float.MaxValue);

            rightParentRot = rightRot;

            PxD6Joint* d6joint = PxD6JointCreate(ref *gPhysics, leftLink, new PxTransform(PxIdentity), rightLink, new PxTransform(PxIdentity));

            d6joint->setMotion(PxD6Axis.eTWIST, PxD6Motion.eFREE);
            d6joint->setMotion(PxD6Axis.eSWING1, PxD6Motion.eFREE);
            d6joint->setMotion(PxD6Axis.eSWING2, PxD6Motion.eFREE);

            currLeft = rightLink;
            currRight = leftLink;
        }

        {
            PxD6Joint* d6joint = PxD6JointCreate(ref *gPhysics, currLeft, new PxTransform(new PxVec3(0f, 0f, -1f)), leftTop, new PxTransform(new PxVec3(-0.5f, 0f, 0f)));

            d6joint->setMotion(PxD6Axis.eTWIST, PxD6Motion.eFREE);
            d6joint->setMotion(PxD6Axis.eSWING1, PxD6Motion.eFREE);
            d6joint->setMotion(PxD6Axis.eSWING2, PxD6Motion.eFREE);

            d6joint = PxD6JointCreate(ref *gPhysics, currRight, new PxTransform(new PxVec3(0f, 0f, 1f)), rightTop, new PxTransform(new PxVec3(-0.5f, 0f, 0f)));

            d6joint->setMotion(PxD6Axis.eTWIST, PxD6Motion.eFREE);
            d6joint->setMotion(PxD6Axis.eSWING1, PxD6Motion.eFREE);
            d6joint->setMotion(PxD6Axis.eSWING2, PxD6Motion.eFREE);
        }

        PxTransform topPose = new(new PxVec3(0f, leftTop->getGlobalPose().p.y + 0.15f, 0f));

        PxArticulationLink* top = gArticulation->createLink(leftTop, topPose);
        PxRigidActorExt.createExclusiveShape(ref *top, new PxBoxGeometry(0.5f, 0.1f, 1.5f), *gMaterial, defaultShapeFlags);
        PxRigidBodyExt.updateMassAndInertia(ref *top, 1f);

        joint = static_cast<PxArticulationJointReducedCoordinate>(top->getInboundJoint());
        joint->setJointType(PxArticulationJointType.eFIX);
        joint->setParentPose(new PxTransform(new PxVec3(0f, 0.0f, 0f)));
        joint->setChildPose(new PxTransform(new PxVec3(0f, -0.15f, -0.9f)));

        gScene->addArticulation(ref *gArticulation);

        for (uint i = 0; i < gArticulation->getNbLinks(); ++i)
        {
            PxArticulationLink* link;
            gArticulation->getLinks(&link, 1, i);

            link->setLinearDamping(0.2f);
            link->setAngularDamping(0.2f);

            link->setMaxAngularVelocity(20f);
            link->setMaxLinearVelocity(100f);

            if (link != top)
            {
                for (uint b = 0; b < link->getNbShapes(); ++b)
                {
                    PxShape* shape;
                    link->getShapes(&shape, 1, b);

                    shape->setSimulationFilterData(new PxFilterData(0, 0, 1, 0));
                }
            }
        }

        PxVec3 halfExt = new(0.25f);
        const float density = 0.5f;

        PxRigidDynamic* box0 = gPhysics->createRigidDynamic(new PxTransform(new PxVec3(-0.25f, 5f, 0.5f)));
        PxRigidActorExt.createExclusiveShape(ref *box0, new PxBoxGeometry(halfExt), *gMaterial, defaultShapeFlags);
        PxRigidBodyExt.updateMassAndInertia(ref *box0, density);

        gScene->addActor(ref *box0);

        PxRigidDynamic* box1 = gPhysics->createRigidDynamic(new PxTransform(new PxVec3(0.25f, 5f, 0.5f)));
        PxRigidActorExt.createExclusiveShape(ref *box1, new PxBoxGeometry(halfExt), *gMaterial, defaultShapeFlags);
        PxRigidBodyExt.updateMassAndInertia(ref *box1, density);

        gScene->addActor(ref *box1);

        PxRigidDynamic* box2 = gPhysics->createRigidDynamic(new PxTransform(new PxVec3(-0.25f, 4.5f, 0.5f)));
        PxRigidActorExt.createExclusiveShape(ref *box2, new PxBoxGeometry(halfExt), *gMaterial, defaultShapeFlags);
        PxRigidBodyExt.updateMassAndInertia(ref *box2, density);

        gScene->addActor(ref *box2);

        PxRigidDynamic* box3 = gPhysics->createRigidDynamic(new PxTransform(new PxVec3(0.25f, 4.5f, 0.5f)));
        PxRigidActorExt.createExclusiveShape(ref *box3, new PxBoxGeometry(halfExt), *gMaterial, defaultShapeFlags);
        PxRigidBodyExt.updateMassAndInertia(ref *box3, density);

        gScene->addActor(ref *box3);

        PxRigidDynamic* box4 = gPhysics->createRigidDynamic(new PxTransform(new PxVec3(-0.25f, 5f, 0f)));
        PxRigidActorExt.createExclusiveShape(ref *box4, new PxBoxGeometry(halfExt), *gMaterial, defaultShapeFlags);
        PxRigidBodyExt.updateMassAndInertia(ref *box4, density);

        gScene->addActor(ref *box4);

        PxRigidDynamic* box5 = gPhysics->createRigidDynamic(new PxTransform(new PxVec3(0.25f, 5f, 0f)));
        PxRigidActorExt.createExclusiveShape(ref *box5, new PxBoxGeometry(halfExt), *gMaterial, defaultShapeFlags);
        PxRigidBodyExt.updateMassAndInertia(ref *box5, density);

        gScene->addActor(ref *box5);

        PxRigidDynamic* box6 = gPhysics->createRigidDynamic(new PxTransform(new PxVec3(-0.25f, 4.5f, 0f)));
        PxRigidActorExt.createExclusiveShape(ref *box6, new PxBoxGeometry(halfExt), *gMaterial, defaultShapeFlags);
        PxRigidBodyExt.updateMassAndInertia(ref *box6, density);

        gScene->addActor(ref *box6);

        PxRigidDynamic* box7 = gPhysics->createRigidDynamic(new PxTransform(new PxVec3(0.25f, 4.5f, 0f)));
        PxRigidActorExt.createExclusiveShape(ref *box7, new PxBoxGeometry(halfExt), *gMaterial, defaultShapeFlags);
        PxRigidBodyExt.updateMassAndInertia(ref *box7, density);

        gScene->addActor(ref *box7);
    }
#endif

    static void createLongChain()
    {
        const float scale = 0.25f;
        const float radius = 0.5f * scale;
        const float halfHeight = 1.0f * scale;
        const uint nbCapsules = 40;
        const float capsuleMass = 1.0f;

        PxVec3 initPos = new(0.0f, 24.0f, 0.0f);
        PxVec3 pos = initPos;
        PxShape* capsuleShape = gPhysics->createShape(new PxCapsuleGeometry(radius, halfHeight), *gMaterial,
            false, PxShapeFlags.eVISUALIZATION | PxShapeFlags.eSCENE_QUERY_SHAPE | PxShapeFlags.eSIMULATION_SHAPE); //BIOQUIRK: Missing defaults
        PxArticulationLink* firstLink = null;
        PxArticulationLink* parent = null;

        const bool overlappingLinks = true; // Change this for another kind of rope

        gArticulation->setSolverIterationCounts(16);

        // Create rope
        for (uint i = 0; i < nbCapsules; i++)
        {
            PxArticulationLink* link = gArticulation->createLink(parent, new PxTransform(pos));
            if (firstLink == null)
                firstLink = link;

            link->attachShape(ref *capsuleShape);
            PxRigidBodyExt.setMassAndUpdateInertia(ref *link, capsuleMass);

            link->setLinearDamping(0.1f);
            link->setAngularDamping(0.1f);

            link->setMaxAngularVelocity(30f);
            link->setMaxLinearVelocity(100f);

            PxArticulationJointBase* joint = link->getInboundJoint();

            if (joint != null) // Will be null for root link
            {
#if USE_REDUCED_COORDINATE_ARTICULATION
                PxArticulationJointReducedCoordinate* rcJoint = static_cast<PxArticulationJointReducedCoordinate>(joint);
                rcJoint->setJointType(PxArticulationJointType.eSPHERICAL);
                rcJoint->setMotion(PxArticulationAxis.eSWING2, PxArticulationMotions.eFREE);
                rcJoint->setMotion(PxArticulationAxis.eSWING1, PxArticulationMotions.eFREE);
                rcJoint->setMotion(PxArticulationAxis.eTWIST, PxArticulationMotions.eFREE);
                rcJoint->setFrictionCoefficient(1f);
                rcJoint->setMaxJointVelocity(1000000f);
#endif
                if (overlappingLinks)
                {
                    joint->setParentPose(new PxTransform(new PxVec3(halfHeight, 0.0f, 0.0f)));
                    joint->setChildPose(new PxTransform(new PxVec3(-halfHeight, 0.0f, 0.0f)));
                }
                else
                {

                    joint->setParentPose(new PxTransform(new PxVec3(radius + halfHeight, 0.0f, 0.0f)));
                    joint->setChildPose(new PxTransform(new PxVec3(-radius - halfHeight, 0.0f, 0.0f)));
                }
            }

            if (overlappingLinks)
                pos.x += (radius + halfHeight * 2.0f);
            else
                pos.x += (radius + halfHeight) * 2.0f;
            parent = link;
        }

        //Attach large & heavy box at the end of the rope
        {
            const float boxMass = 50.0f;
            const float boxSize = 1.0f;
            PxShape* boxShape = gPhysics->createShape(new PxBoxGeometry(boxSize, boxSize, boxSize), *gMaterial,
                false, PxShapeFlags.eVISUALIZATION | PxShapeFlags.eSCENE_QUERY_SHAPE | PxShapeFlags.eSIMULATION_SHAPE); //BIOQUIRK: Missing defaults

            pos.x -= (radius + halfHeight) * 2.0f;
            pos.x += (radius + halfHeight) + boxSize;

            PxArticulationLink* link = gArticulation->createLink(parent, new PxTransform(pos));

            link->setLinearDamping(0.1f);
            link->setAngularDamping(0.1f);
            link->setMaxAngularVelocity(30f);
            link->setMaxLinearVelocity(100f);

            link->attachShape(ref *boxShape);
            PxRigidBodyExt.setMassAndUpdateInertia(ref *link, boxMass);

            PxArticulationJointBase* joint = link->getInboundJoint();
#if USE_REDUCED_COORDINATE_ARTICULATION
            PxArticulationJointReducedCoordinate* rcJoint = static_cast<PxArticulationJointReducedCoordinate>(joint);
            rcJoint->setJointType(PxArticulationJointType.eSPHERICAL);
            rcJoint->setMotion(PxArticulationAxis.eSWING2, PxArticulationMotions.eFREE);
            rcJoint->setMotion(PxArticulationAxis.eSWING1, PxArticulationMotions.eFREE);
            rcJoint->setMotion(PxArticulationAxis.eTWIST, PxArticulationMotions.eFREE);
            rcJoint->setFrictionCoefficient(1f);
            rcJoint->setMaxJointVelocity(1000000f);
#endif
            if (joint != null) // Will be null for root link
            {
                joint->setParentPose(new PxTransform(new PxVec3(radius + halfHeight, 0.0f, 0.0f)));
                joint->setChildPose(new PxTransform(new PxVec3(-boxSize, 0.0f, 0.0f)));
            }
        }
        gScene->addArticulation(ref *gArticulation);

#if USE_REDUCED_COORDINATE_ARTICULATION
        gArticulation->setArticulationFlags(PxArticulationFlags.eFIX_BASE);
#else
        // Attach articulation to static world
        {
            PxShape* anchorShape = gPhysics->createShape(new PxSphereGeometry(0.05f), *gMaterial,
                false, PxShapeFlags.eVISUALIZATION | PxShapeFlags.eSCENE_QUERY_SHAPE | PxShapeFlags.eSIMULATION_SHAPE); //BIOQUIRK: Missing defaults
            PxRigidStatic* anchor = PxCreateStatic(ref *gPhysics, new PxTransform(initPos), ref *anchorShape);
            gScene->addActor(ref *anchor);
            PxSphericalJoint* j = PxSphericalJointCreate(ref *gPhysics, anchor, new PxTransform(new PxVec3(0.0f)), firstLink, new PxTransform(new PxVec3(0.0f)));
        }
#endif

        // Create obstacle
        {
            PxShape* boxShape = gPhysics->createShape(new PxBoxGeometry(1.0f, 0.1f, 2.0f), *gMaterial,
                false, PxShapeFlags.eVISUALIZATION | PxShapeFlags.eSCENE_QUERY_SHAPE | PxShapeFlags.eSIMULATION_SHAPE); //BIOQUIRK: Missing defaults
            PxRigidStatic* obstacle = PxCreateStatic(ref *gPhysics, new PxTransform(initPos.operator_Plus(new PxVec3(10.0f, -3.0f, 0.0f))), ref *boxShape); //BIOQUIRK: Overloaded operator
            gScene->addActor(ref *obstacle);
        }
    }

    public static void initPhysics(bool interactive)
    {
        gFoundation = PxCreateFoundation(PX_PHYSICS_VERSION, ref gAllocator.Value, ref gErrorCallback.Value);
        gPvd = PxCreatePvd(ref *gFoundation);
        PxPvdTransport* transport = PxDefaultPvdSocketTransportCreate(PVD_HOST, 5425, 10);
        gPvd->connect(ref *transport, PxPvdInstrumentationFlags.eALL);

        gPhysics = PxCreatePhysics(PX_PHYSICS_VERSION, ref *gFoundation, new PxTolerancesScale(), true, gPvd);
        PxInitExtensions(ref *gPhysics, gPvd);

        PxSceneDesc sceneDesc = new(*gPhysics->getTolerancesScale());
        sceneDesc.gravity = new PxVec3(0.0f, -9.81f, 0.0f);

        uint numCores = SnippetUtils.getNbPhysicalCores();
        gDispatcher = PxDefaultCpuDispatcherCreate(numCores == 0 ? 0 : numCores - 1);
        sceneDesc.cpuDispatcher = (PxCpuDispatcher*)gDispatcher; //BIOQUIRK: Base cast
        sceneDesc.filterShader = PxDefaultSimulationFilterShader;

#if USE_REDUCED_COORDINATE_ARTICULATION
        sceneDesc.solverType = PxSolverType.eTGS;
#if CREATE_SCISSOR_LIFT
        sceneDesc.filterShader = &scissorFilter;
#endif

#endif

        gScene = gPhysics->createScene(sceneDesc);
        PxPvdSceneClient* pvdClient = gScene->getScenePvdClient();
        if (pvdClient != null)
        {
            pvdClient->setScenePvdFlag(PxPvdSceneFlags.eTRANSMIT_CONSTRAINTS, true);
            pvdClient->setScenePvdFlag(PxPvdSceneFlags.eTRANSMIT_CONTACTS, true);
            pvdClient->setScenePvdFlag(PxPvdSceneFlags.eTRANSMIT_SCENEQUERIES, true);
        }

        gMaterial = gPhysics->createMaterial(0.5f, 0.5f, 0f);

        PxRigidStatic* groundPlane = PxCreatePlane(ref *gPhysics, new PxPlane(0, 1, 0, 0), ref *gMaterial);
        gScene->addActor(ref *groundPlane);

#if USE_REDUCED_COORDINATE_ARTICULATION
        gArticulation = gPhysics->createArticulationReducedCoordinate();
#else
        gArticulation = gPhysics->createArticulation();

        // Stabilization can create artefacts on jointed objects so we just disable it
        gArticulation->setStabilizationThreshold(0.0f);

        gArticulation->setMaxProjectionIterations(16);
        gArticulation->setSeparationTolerance(0.001f);
#endif

#if USE_REDUCED_COORDINATE_ARTICULATION && CREATE_SCISSOR_LIFT
        createScissorLift();
#else
        createLongChain();
#endif
    }

#if USE_REDUCED_COORDINATE_ARTICULATION
    static bool gClosing = true;
#endif

    public static void stepPhysics(bool interactive)
    {
        const float dt = 1.0f / 60f;
#if USE_REDUCED_COORDINATE_ARTICULATION && CREATE_SCISSOR_LIFT
        float driveValue = gDriveJoint->getDriveTarget(PxArticulationAxis.eZ);

        if (gClosing && driveValue < -1.2f)
            gClosing = false;
        else if (!gClosing && driveValue > 0)
            gClosing = true;

        if (gClosing)
            driveValue -= dt * 0.25f;
        else
            driveValue += dt * 0.25f;
        gDriveJoint->setDriveTarget(PxArticulationAxis.eZ, driveValue);
#endif

        gScene->simulate(dt);
        gScene->fetchResults(true);
    }

    public static void cleanupPhysics(bool interactive)
    {
        gArticulation->release();
        gScene->release();
        gDispatcher->release();
        gPhysics->release();
        PxPvdTransport* transport = gPvd->getTransport();
        gPvd->release();
        transport->release();
        PxCloseExtensions();
        gFoundation->release();

        Console.WriteLine("SnippetArticulation done.");
    }

    public static void keyPress(Keys key, in PxTransform camera)
    {
    }
}
