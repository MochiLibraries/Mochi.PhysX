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
public unsafe struct VehicleConcurrency
{
    // VehicleConcurrency() is omitted since they represent default behavior in C#.
    // ~VehicleConcurrency() is omitted since it doesn't actually do anything.

    public static VehicleConcurrency* allocate<TAllocator>(uint maxNumVehicles, uint maxNumWheelsPerVehicle, ref TAllocator allocator)
        where TAllocator : unmanaged, IPxAllocatorCallback
    {
        uint byteSize =
            (uint)sizeof(VehicleConcurrency) +
            (uint)sizeof(PxVehicleConcurrentUpdateData) * maxNumVehicles +
            (uint)sizeof(PxVehicleWheelConcurrentUpdateData) * maxNumWheelsPerVehicle * maxNumVehicles;

        byte* buffer = (byte*)Unsafe.As<TAllocator, PxAllocatorCallback>(ref allocator).allocate(byteSize, null, null, 0); //BIOQUIRK: Awkward generic cast

        VehicleConcurrency* vc = (VehicleConcurrency*)(buffer);
        *vc = new VehicleConcurrency();
        buffer += sizeof(VehicleConcurrency);

        vc->mMaxNumVehicles = maxNumVehicles;
        vc->mMaxNumWheelsPerVehicle = maxNumWheelsPerVehicle;

        vc->mVehicleConcurrentUpdates = (PxVehicleConcurrentUpdateData*)(buffer);
        buffer += sizeof(PxVehicleConcurrentUpdateData) * maxNumVehicles;

        for (uint i = 0; i < maxNumVehicles; i++)
        {
            *(vc->mVehicleConcurrentUpdates + i) = new PxVehicleConcurrentUpdateData();

            vc->mVehicleConcurrentUpdates[i].nbConcurrentWheelUpdates = maxNumWheelsPerVehicle;

            vc->mVehicleConcurrentUpdates[i].concurrentWheelUpdates = (PxVehicleWheelConcurrentUpdateData*)(buffer);
            buffer += sizeof(PxVehicleWheelConcurrentUpdateData) * maxNumWheelsPerVehicle;

            for (uint j = 0; j < maxNumWheelsPerVehicle; j++)
            {
                *(vc->mVehicleConcurrentUpdates[i].concurrentWheelUpdates + j) = new PxVehicleWheelConcurrentUpdateData();
            }

        }

        return vc;
    }

    //Free allocated buffer for scene queries of suspension raycasts.
    public void free<TAllocator>(ref TAllocator allocator)
        where TAllocator : unmanaged, IPxAllocatorCallback
    {
        Unsafe.As<TAllocator, PxAllocatorCallback>(ref allocator).deallocate(Unsafe.AsPointer(ref this)); //BIOQUIRK: Awkward generic cast
    }

    //Return the PxVehicleConcurrentUpdate for a vehicle specified by an index.
    PxVehicleConcurrentUpdateData* getVehicleConcurrentUpdate(uint id)
    {
        return (mVehicleConcurrentUpdates + id);
    }

    //Return the entire array of PxVehicleConcurrentUpdates
    PxVehicleConcurrentUpdateData* getVehicleConcurrentUpdateBuffer()
    {
        return mVehicleConcurrentUpdates;
    }

    private uint mMaxNumVehicles;
    private uint mMaxNumWheelsPerVehicle;
    private PxVehicleConcurrentUpdateData* mVehicleConcurrentUpdates;
}
