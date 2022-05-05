﻿// Copyright (c) 2022 David Maas and Contributors. All rights reserved.
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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SnippetVehicleCommon;

public unsafe static partial class Globals
{
    public const uint DRIVABLE_SURFACE = 0xffff0000;
    public const uint UNDRIVABLE_SURFACE = 0x0000ffff;

    public static void setupDrivableSurface(ref PxFilterData filterData)
    {
        filterData.word3 = DRIVABLE_SURFACE;
    }

    public static void setupNonDrivableSurface(ref PxFilterData filterData)
    {
        filterData.word3 = UNDRIVABLE_SURFACE;
    }

    public static PxQueryHitType WheelSceneQueryPreFilterBlocking(
        PxFilterData filterData0, PxFilterData filterData1,
        void* constantBlock, uint constantBlockSize,
        ref PxHitFlags queryFlags)
    {
        //filterData0 is the vehicle suspension query.
        //filterData1 is the shape potentially hit by the query.
        return ((0 == (filterData1.word3 & DRIVABLE_SURFACE)) ? PxQueryHitType.eNONE : PxQueryHitType.eBLOCK);
    }

    public static PxQueryHitType WheelSceneQueryPostFilterBlocking(
        PxFilterData filterData0, PxFilterData filterData1,
        void* constantBlock, uint constantBlockSize,
        in PxQueryHit hit)
    {
        if ((static_cast<PxQueryHit, PxSweepHit>(hit)).hadInitialOverlap()) //BIOQUIRK: Awkward derived cast
            return PxQueryHitType.eNONE;
        return PxQueryHitType.eBLOCK;
    }

    public static PxQueryHitType WheelSceneQueryPreFilterNonBlocking(
        PxFilterData filterData0, PxFilterData filterData1,
        void* constantBlock, uint constantBlockSize,
        ref PxHitFlags queryFlags)
    {
        //filterData0 is the vehicle suspension query.
        //filterData1 is the shape potentially hit by the query.
        return ((0 == (filterData1.word3 & DRIVABLE_SURFACE)) ? PxQueryHitType.eNONE : PxQueryHitType.eTOUCH);
    }

    public static PxQueryHitType WheelSceneQueryPostFilterNonBlocking(
        PxFilterData filterData0, PxFilterData filterData1,
        void* constantBlock, uint constantBlockSize,
        in PxQueryHit hit)
    {
        if ((static_cast<PxQueryHit, PxSweepHit>(hit)).hadInitialOverlap())
            return PxQueryHitType.eNONE;
        return PxQueryHitType.eTOUCH;
    }

    //BIOQUIRK: It'd be nice if these could be generated by a source generator
    #region ABI Glue
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static PxQueryHitType WheelSceneQueryPreFilterBlocking(PxFilterData* filterData0, PxFilterData* filterData1, void* constantBlock, uint constantBlockSize, PxHitFlags* queryFlags)
        => WheelSceneQueryPreFilterBlocking(*filterData0, *filterData1, constantBlock, constantBlockSize, ref Unsafe.AsRef<PxHitFlags>(queryFlags));

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static PxQueryHitType WheelSceneQueryPostFilterBlocking(PxFilterData* filterData0, PxFilterData* filterData1, void* constantBlock, uint constantBlockSize, PxQueryHit* queryFlags)
        => WheelSceneQueryPostFilterBlocking(*filterData0, *filterData1, constantBlock, constantBlockSize, in Unsafe.AsRef<PxQueryHit>(queryFlags));

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static PxQueryHitType WheelSceneQueryPreFilterNonBlocking(PxFilterData* filterData0, PxFilterData* filterData1, void* constantBlock, uint constantBlockSize, PxHitFlags* queryFlags)
        => WheelSceneQueryPreFilterNonBlocking(*filterData0, *filterData1, constantBlock, constantBlockSize, ref Unsafe.AsRef<PxHitFlags>(queryFlags));

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static PxQueryHitType WheelSceneQueryPostFilterNonBlocking(PxFilterData* filterData0, PxFilterData* filterData1, void* constantBlock, uint constantBlockSize, PxQueryHit* queryFlags)
        => WheelSceneQueryPostFilterNonBlocking(*filterData0, *filterData1, constantBlock, constantBlockSize, in Unsafe.AsRef<PxQueryHit>(queryFlags));
    #endregion
}

//Data structure for quick setup of scene queries for suspension queries.
public unsafe struct VehicleSceneQueryData
{
    // VehicleSceneQueryData() is omitted since they represent default behavior in C#.
    // ~VehicleSceneQueryData() is omitted since it doesn't actually do anything.

    //Allocate scene query data for up to maxNumVehicles and up to maxNumWheelsPerVehicle with numVehiclesInBatch per batch query.
    public static VehicleSceneQueryData* allocate<TAllocator>(
        uint maxNumVehicles, uint maxNumWheelsPerVehicle, uint maxNumHitPointsPerWheel, uint numVehiclesInBatch,
        delegate* unmanaged[Cdecl]<PxFilterData*, PxFilterData*, void*, uint, PxHitFlags*, PxQueryHitType> preFilterShader,
        delegate* unmanaged[Cdecl]<PxFilterData*, PxFilterData*, void*, uint, PxQueryHit*, PxQueryHitType> postFilterShader,
        ref TAllocator allocator)
        where TAllocator : unmanaged, IPxAllocatorCallback
    {
        uint sqDataSize = (((uint)sizeof(VehicleSceneQueryData) + 15u) & ~15u);

        uint maxNumWheels = maxNumVehicles * maxNumWheelsPerVehicle;
        uint raycastResultSize = (((uint)sizeof(PxBatchQueryResult<PxRaycastHit>) * maxNumWheels + 15u) & ~15u);
        uint sweepResultSize = (((uint)sizeof(PxBatchQueryResult<PxSweepHit>) * maxNumWheels + 15u) & ~15u);

        uint maxNumHitPoints = maxNumWheels * maxNumHitPointsPerWheel;
        uint raycastHitSize = (((uint)sizeof(PxRaycastHit) * maxNumHitPoints + 15u) & ~15u);
        uint sweepHitSize = (((uint)sizeof(PxSweepHit) * maxNumHitPoints + 15u) & ~15u);

        uint size = sqDataSize + raycastResultSize + raycastHitSize + sweepResultSize + sweepHitSize;
        byte* buffer = (byte*)Unsafe.As<TAllocator, PxAllocatorCallback>(ref allocator).allocate(size, null, null, 0); //BIOQUIRK: Awkward generic cast

        VehicleSceneQueryData* sqData = (VehicleSceneQueryData*)buffer;
        *sqData = new VehicleSceneQueryData();
        sqData->mNumQueriesPerBatch = numVehiclesInBatch * maxNumWheelsPerVehicle;
        sqData->mNumHitResultsPerQuery = maxNumHitPointsPerWheel;
        buffer += sqDataSize;

        sqData->mRaycastResults = (PxBatchQueryResult<PxRaycastHit>*)(buffer);
        buffer += raycastResultSize;

        sqData->mRaycastHitBuffer = (PxRaycastHit*)(buffer);
        buffer += raycastHitSize;

        sqData->mSweepResults = (PxBatchQueryResult<PxSweepHit>*)(buffer);
        buffer += sweepResultSize;

        sqData->mSweepHitBuffer = (PxSweepHit*)(buffer);
        buffer += sweepHitSize;

        for (uint i = 0; i < maxNumWheels; i++)
        {
            *(sqData->mRaycastResults + i) = new PxBatchQueryResult<PxRaycastHit>();
            *(sqData->mSweepResults + i) = new PxBatchQueryResult<PxSweepHit>();
        }

        for (uint i = 0; i < maxNumHitPoints; i++)
        {
            *(sqData->mRaycastHitBuffer + i) = new PxRaycastHit();
            *(sqData->mSweepHitBuffer + i) = new PxSweepHit();
        }

        sqData->mPreFilterShader = preFilterShader;
        sqData->mPostFilterShader = postFilterShader;

        return sqData;
    }

    //Free allocated buffers.
    public void free<TAllocator>(ref TAllocator allocator)
        where TAllocator : unmanaged, IPxAllocatorCallback
    {
        Unsafe.As<TAllocator, PxAllocatorCallback>(ref allocator).deallocate(Unsafe.AsPointer(ref this)); //BIOQUIRK: Awkward generic cast
    }

    //Create a PxBatchQuery instance that will be used for a single specified batch.
    public static PxBatchQuery* setUpBatchedSceneQuery(uint batchId, in VehicleSceneQueryData vehicleSceneQueryData, PxScene* scene)
    {
        uint maxNumQueriesInBatch = vehicleSceneQueryData.mNumQueriesPerBatch;
        uint maxNumHitResultsInBatch = vehicleSceneQueryData.mNumQueriesPerBatch * vehicleSceneQueryData.mNumHitResultsPerQuery;

        PxBatchQueryDesc sqDesc = new(maxNumQueriesInBatch, maxNumQueriesInBatch, 0);

        sqDesc.queryMemory.userRaycastResultBuffer = vehicleSceneQueryData.mRaycastResults + batchId * maxNumQueriesInBatch;
        sqDesc.queryMemory.userRaycastTouchBuffer = vehicleSceneQueryData.mRaycastHitBuffer + batchId * maxNumHitResultsInBatch;
        sqDesc.queryMemory.raycastTouchBufferSize = maxNumHitResultsInBatch;

        sqDesc.queryMemory.userSweepResultBuffer = vehicleSceneQueryData.mSweepResults + batchId * maxNumQueriesInBatch;
        sqDesc.queryMemory.userSweepTouchBuffer = vehicleSceneQueryData.mSweepHitBuffer + batchId * maxNumHitResultsInBatch;
        sqDesc.queryMemory.sweepTouchBufferSize = maxNumHitResultsInBatch;

        sqDesc.preFilterShader = vehicleSceneQueryData.mPreFilterShader;

        sqDesc.postFilterShader = vehicleSceneQueryData.mPostFilterShader;

        return scene->createBatchQuery(sqDesc);
    }

    //Return an array of scene query results for a single specified batch.
    public PxBatchQueryResult<PxRaycastHit>* getRaycastQueryResultBuffer(uint batchId)
    {
        return (mRaycastResults + batchId * mNumQueriesPerBatch);
    }

    //Return an array of scene query results for a single specified batch.
    public PxBatchQueryResult<PxSweepHit>* getSweepQueryResultBuffer(uint batchId)
    {
        return (mSweepResults + batchId * mNumQueriesPerBatch);
    }

    //Get the number of scene query results that have been allocated for a single batch.
    public readonly uint getQueryResultBufferSize()
    {
        return mNumQueriesPerBatch;
    }

    //Number of queries per batch
    private uint mNumQueriesPerBatch;

    //Number of hit results per query
    private uint mNumHitResultsPerQuery;

    //One result for each wheel.
    private PxBatchQueryResult<PxRaycastHit>* mRaycastResults;
    private PxBatchQueryResult<PxSweepHit>* mSweepResults;

    //One hit for each wheel.
    private PxRaycastHit* mRaycastHitBuffer;
    private PxSweepHit* mSweepHitBuffer;

    //Filter shader used to filter drivable and non-drivable surfaces
    private delegate* unmanaged[Cdecl]<PxFilterData*, PxFilterData*, void*, uint, PxHitFlags*, PxQueryHitType> mPreFilterShader;

    //Filter shader used to reject hit shapes that initially overlap sweeps.
    private delegate* unmanaged[Cdecl]<PxFilterData*, PxFilterData*, void*, uint, PxQueryHit*, PxQueryHitType> mPostFilterShader;
}
