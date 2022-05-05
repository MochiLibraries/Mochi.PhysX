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
using System.Runtime.CompilerServices;

namespace SnippetVehicleCommon;

//Data structure for quick setup of wheel query data structures.
public unsafe struct VehicleWheelQueryResults
{
    // VehicleWheelQueryResults() is omitted since they represent default behavior in C#.
    // ~VehicleWheelQueryResults() is omitted since it doesn't actually do anything.

    //Allocate wheel results for up to maxNumVehicles with up to maxNumWheelsPerVehicle.
    public static VehicleWheelQueryResults* allocate<TAllocator>(uint maxNumVehicles, uint maxNumWheelsPerVehicle, ref TAllocator allocator)
        where TAllocator : unmanaged, IPxAllocatorCallback
    {
        uint byteSize = (uint)(sizeof(VehicleWheelQueryResults) + sizeof(PxVehicleWheelQueryResult) * maxNumVehicles + sizeof(PxWheelQueryResult) * maxNumWheelsPerVehicle * maxNumVehicles);

        byte* buffer = (byte*)Unsafe.As<TAllocator, PxAllocatorCallback>(ref allocator).allocate(byteSize, null, null, 0); //BIOQUIRK: Awkward generic cast

        VehicleWheelQueryResults* vwqr = (VehicleWheelQueryResults*)(buffer);
        buffer += sizeof(VehicleWheelQueryResults);

        vwqr->mVehicleWheelQueryResults = (PxVehicleWheelQueryResult*)(buffer);
        buffer += sizeof(PxVehicleWheelQueryResult) * maxNumVehicles;

        for (uint i = 0; i < maxNumVehicles; i++)
        {
            PxWheelQueryResult* result = (PxWheelQueryResult*)buffer;
            *result = new PxWheelQueryResult();
            vwqr->mVehicleWheelQueryResults[i].wheelQueryResults = result;
            vwqr->mVehicleWheelQueryResults[i].nbWheelQueryResults = maxNumWheelsPerVehicle;
            buffer += sizeof(PxWheelQueryResult) * maxNumWheelsPerVehicle;
        }

        return vwqr;
    }

    //Free allocated buffer for scene queries of suspension raycasts.
    public void free<TAllocator>(ref TAllocator allocator)
        where TAllocator : unmanaged, IPxAllocatorCallback
    {
        Unsafe.As<TAllocator, PxAllocatorCallback>(ref allocator).deallocate(Unsafe.AsPointer(ref this)); //BIOQUIRK: Awkward generic cast
    }

    //Return the PxVehicleWheelQueryResult for a vehicle specified by an index.
    public PxVehicleWheelQueryResult* getVehicleWheelQueryResults(uint id)
    {
        return (mVehicleWheelQueryResults + id);
    }

    private PxVehicleWheelQueryResult* mVehicleWheelQueryResults;
}
