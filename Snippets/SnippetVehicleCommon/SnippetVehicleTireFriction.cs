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

namespace SnippetVehicleCommon;

public unsafe static partial class Globals
{
    //Drivable surface types.
    public const int SURFACE_TYPE_TARMAC = 0;
    public const int MAX_NUM_SURFACE_TYPES = 1;

    //Tire types.
    public const int TIRE_TYPE_NORMAL = 0;
    public const int TIRE_TYPE_WORN = 1;
    public const int MAX_NUM_TIRE_TYPES = 2;

    //Tire model friction for each combination of drivable surface type and tire type.
    private static float[,] gTireFrictionMultipliers = new float[MAX_NUM_SURFACE_TYPES, MAX_NUM_TIRE_TYPES]
    {
        //NORMAL,  WORN
        {1.00f,    0.1f}//TARMAC
    };

    public static PxVehicleDrivableSurfaceToTireFrictionPairs* createFrictionPairs(PxMaterial* defaultMaterial)
    {
        PxVehicleDrivableSurfaceType* surfaceTypes = stackalloc PxVehicleDrivableSurfaceType[1];
        surfaceTypes[0].mType = SURFACE_TYPE_TARMAC;

        PxMaterial** surfaceMaterials = stackalloc PxMaterial*[1];
        surfaceMaterials[0] = defaultMaterial;

        PxVehicleDrivableSurfaceToTireFrictionPairs* surfaceTirePairs =
            PxVehicleDrivableSurfaceToTireFrictionPairs.allocate(MAX_NUM_TIRE_TYPES, MAX_NUM_SURFACE_TYPES);

        surfaceTirePairs->setup(MAX_NUM_TIRE_TYPES, MAX_NUM_SURFACE_TYPES, surfaceMaterials, surfaceTypes);

        for (uint i = 0; i < MAX_NUM_SURFACE_TYPES; i++)
        {
            for (uint j = 0; j < MAX_NUM_TIRE_TYPES; j++)
            {
                surfaceTirePairs->setTypePairFriction(i, j, gTireFrictionMultipliers[i, j]);
            }
        }
        return surfaceTirePairs;
    }
}
