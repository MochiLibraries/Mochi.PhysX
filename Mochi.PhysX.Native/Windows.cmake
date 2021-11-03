set(PHYSX_PRESET "Mochi.PhysX.Windows.x64")
set(DOTNET_RID "win-x64")
set(STATIC_LIBRARY_FILTER "*.lib")
set(TARGET_LIBRARIES_PREFIX "")
set(TARGET_LIBRARIES_SUFFIX "")

# Keep in sync with our mochi-physx-win-x64.xml preset
set(PX_BUILDSNIPPETS False)
set(PX_BUILDPUBLICSAMPLES False)
set(PX_GENERATE_STATIC_LIBRARIES True)
set(NV_USE_STATIC_WINCRT True)
set(NV_USE_DEBUG_WINCRT True)
set(PX_FLOAT_POINT_PRECISE_MATH False)

# Nvidia doesn't really define what preprocessor defines, etc we should use when linking to PhysX.
# For the sake of simplicity we import their base configuration and copy+paste their Windows-specific configuration
# This is maybe a little brittle, if it ever becomes a problem we'll switch to just setting things ourselves.
set(NV_USE_GAMEWORKS_OUTPUT_DIRS OFF)
include(NvidiaBuildOptions)

#====================================================================================================================================================
# Below this line is copied and slightly modified from physx/source/compiler/cmake/windows/CMakeLists.txt
#====================================================================================================================================================
##
## Redistribution and use in source and binary forms, with or without
## modification, are permitted provided that the following conditions
## are met:
##  * Redistributions of source code must retain the above copyright
##    notice, this list of conditions and the following disclaimer.
##  * Redistributions in binary form must reproduce the above copyright
##    notice, this list of conditions and the following disclaimer in the
##    documentation and/or other materials provided with the distribution.
##  * Neither the name of NVIDIA CORPORATION nor the names of its
##    contributors may be used to endorse or promote products derived
##    from this software without specific prior written permission.
##
## THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS ''AS IS'' AND ANY
## EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
## IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
## PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE COPYRIGHT OWNER OR
## CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
## EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
## PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
## PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY
## OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
## (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
## OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
##
## Copyright (c) 2008-2021 NVIDIA Corporation. All rights reserved.

SET(PHYSX_WARNING_DISABLES "/wd4514 /wd4820 /wd4127 /wd4710 /wd4711 /wd4577 /wd4996")

# Cache the CXX flags so the other CMakeLists.txt can use them if needed
IF(PX_FLOAT_POINT_PRECISE_MATH)
    SET(PHYSX_FP_MODE "/fp:precise")
ELSE()
    SET(PHYSX_FP_MODE "/fp:fast")
ENDIF()
IF(CMAKE_CL_64)
    SET(PHYSX_CXX_FLAGS "/d2Zi+ /MP /WX /W4 /GF /GS- /GR- /Gd ${PHYSX_FP_MODE} /Oy ${PHYSX_WARNING_DISABLES}" CACHE INTERNAL "PhysX CXX")
ELSE()
    SET(PHYSX_CXX_FLAGS "/arch:SSE2 /d2Zi+ /MP /WX /W4 /GF /GS- /GR- /Gd ${PHYSX_FP_MODE} /Oy ${PHYSX_WARNING_DISABLES}" CACHE INTERNAL "PhysX CXX")
ENDIF()

SET(PHYSX_CXX_FLAGS_DEBUG "/Od ${WINCRT_DEBUG} /RTCu /Zi" CACHE INTERNAL "PhysX Debug CXX Flags")
# PT: changed /Ox to /O2 because "the /Ox compiler option enables only a subset of the speed optimization options enabled by /O2."
# See https://docs.microsoft.com/en-us/cpp/build/reference/ox-full-optimization?view=vs-2019
SET(PHYSX_CXX_FLAGS_CHECKED "/O2 ${WINCRT_NDEBUG} /Zi" CACHE INTERNAL "PhysX Checked CXX Flags")
SET(PHYSX_CXX_FLAGS_PROFILE "/O2 ${WINCRT_NDEBUG} /Zi" CACHE INTERNAL "PhysX Profile CXX Flags")
SET(PHYSX_CXX_FLAGS_RELEASE "/O2 ${WINCRT_NDEBUG} /Zi" CACHE INTERNAL "PhysX Release CXX Flags")

# cache lib type defs
IF(PX_GENERATE_STATIC_LIBRARIES)
    SET(PHYSX_LIBTYPE_DEFS "PX_PHYSX_STATIC_LIB;" CACHE INTERNAL "PhysX lib type defs")
ENDIF()

# These flags are local to the directory the CMakeLists.txt is in, so don't get carried over to OTHER CMakeLists.txt (thus the CACHE variables above)
SET(CMAKE_CXX_FLAGS ${PHYSX_CXX_FLAGS})

SET(CMAKE_CXX_FLAGS_DEBUG   ${PHYSX_CXX_FLAGS_DEBUG})
SET(CMAKE_CXX_FLAGS_CHECKED ${PHYSX_CXX_FLAGS_CHECKED})
SET(CMAKE_CXX_FLAGS_PROFILE ${PHYSX_CXX_FLAGS_PROFILE})
SET(CMAKE_CXX_FLAGS_RELEASE ${PHYSX_CXX_FLAGS_RELEASE})

# Build PDBs for all configurations
SET(CMAKE_SHARED_LINKER_FLAGS "/DEBUG /INCREMENTAL:NO")
SET(CMAKE_SHARED_LINKER_FLAGS_DEBUG "/DEBUG /INCREMENTAL:NO")
SET(CMAKE_SHARED_LINKER_FLAGS_CHECKED "/DEBUG /INCREMENTAL:NO /OPT:REF")
SET(CMAKE_SHARED_LINKER_FLAGS_PROFILE "/DEBUG /INCREMENTAL:NO /OPT:REF")
SET(CMAKE_SHARED_LINKER_FLAGS_RELEASE "/DEBUG /INCREMENTAL:NO /OPT:REF")

IF(CMAKE_CL_64)
    SET(WIN64_FLAG "WIN64")
ENDIF(CMAKE_CL_64)

IF(PX_SCALAR_MATH)
    SET(SCALAR_MATH_FLAG "PX_SIMD_DISABLED")
ENDIF()

# Controls PX_NVTX for all projects
IF(PX_USE_NVTX)
    SET(NVTX_FLAG "PX_NVTX=1")
ELSE()
    SET(NVTX_FLAG "PX_NVTX=0")
ENDIF()

SET(PHYSX_COMPILE_DEFS "WIN32;${WIN64_FLAG};${SCALAR_MATH_FLAG};${CUDA_FLAG};_CRT_SECURE_NO_DEPRECATE;_CRT_NONSTDC_NO_DEPRECATE;_WINSOCK_DEPRECATED_NO_WARNINGS;${PHYSX_AUTOBUILD}" CACHE INTERNAL "Base PhysX preprocessor definitions")

SET(PHYSX_DEBUG_COMPILE_DEFS   "PX_DEBUG=1;PX_CHECKED=1;${NVTX_FLAG};PX_SUPPORT_PVD=1" CACHE INTERNAL "Debug PhysX preprocessor definitions")
SET(PHYSX_CHECKED_COMPILE_DEFS "PX_CHECKED=1;${NVTX_FLAG};PX_SUPPORT_PVD=1" CACHE INTERNAL "Checked PhysX preprocessor definitions")
SET(PHYSX_PROFILE_COMPILE_DEFS "PX_PROFILE=1;${NVTX_FLAG};PX_SUPPORT_PVD=1" CACHE INTERNAL "Profile PhysX preprocessor definitions")
SET(PHYSX_RELEASE_COMPILE_DEFS "PX_SUPPORT_PVD=0" CACHE INTERNAL "Release PhysX preprocessor definitions")
