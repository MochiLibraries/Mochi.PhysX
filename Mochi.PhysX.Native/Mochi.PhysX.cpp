#include "PxPhysicsVersion.h"

#ifndef PX_PHYSICS_VERSION_MAJOR
#error PX_PHYSICS_VERSION_MAJOR is not defined.
#endif

#ifndef PX_PHYSICS_VERSION_MINOR
#error PX_PHYSICS_VERSION_MINOR is not defined.
#endif

#ifndef PX_PHYSICS_VERSION_BUGFIX
#error PX_PHYSICS_VERSION_BUGFIX is not defined.
#endif

#ifndef MOCHI_PHYSX_BUILD_VARIANT
#error MOCHI_PHYSX_BUILD_VARIANT is not defined.
#endif

#define _STR(x) #x
#define STR(x) _STR(x)

#ifdef WIN32
#define EXPORT __declspec(dllexport)
#else
#define EXPORT
#endif

extern "C" EXPORT const char* GetMochiPhysXBuildInfo()
{
    return "PhysX " STR(PX_PHYSICS_VERSION_MAJOR) "." STR(PX_PHYSICS_VERSION_MINOR) "." STR(PX_PHYSICS_VERSION_BUGFIX) " " STR(MOCHI_PHYSX_BUILD_VARIANT);
}
