cmake_minimum_required(VERSION 3.16)
project(Mochi.PhysX.Native)

set(PHYSX_SDK_ROOT "${CMAKE_CURRENT_SOURCE_DIR}/../external/PhysX")
set(CMAKE_MODULE_PATH "${PHYSX_SDK_ROOT}/externals/cmakemodules")

# Platform-specific configuration
# Build-configuration is based on what PhysX its self uses, IE: physx/source/compiler/cmake/<platform-name>/CMakeLists.txt
set(PLATFORM_SPECIFIC_CONFIG "${CMAKE_CURRENT_SOURCE_DIR}/${CMAKE_SYSTEM_NAME}.cmake")
if(EXISTS "${PLATFORM_SPECIFIC_CONFIG}")
    include(${PLATFORM_SPECIFIC_CONFIG})
else()
    message(FATAL_ERROR "Unsupported target platform '${CMAKE_SYSTEM_NAME}'")
endif()

# Find the primary PhysX binary folder to consume
# In theory we could *probably* cooperate with the CMake project within PhysX but that'd require us to deviate from how PhysX is intended to be built.
set(PHYSX_BIN_ROOT_DIRECTORY "${PHYSX_SDK_ROOT}/physx/install/${PHYSX_PRESET}/bin")
file(GLOB PHYSX_BIN_DIRECTORIES "${PHYSX_BIN_ROOT_DIRECTORY}/*")
list(LENGTH PHYSX_BIN_DIRECTORIES PHYSX_BIN_DIRECTORY_COUNT)

if(${PHYSX_BIN_DIRECTORY_COUNT} GREATER 1)
    message(FATAL_ERROR "'${PHYSX_BIN_ROOT_DIRECTORY}' contains more than one binary subrdirectory!")
elseif(${PHYSX_BIN_DIRECTORY_COUNT} LESS 1)
    message(FATAL_ERROR "'${PHYSX_BIN_ROOT_DIRECTORY}' does not contain any binary subdirectories!")
else()
    list(GET PHYSX_BIN_DIRECTORIES 0 PHYSX_BIN_DIRECTORY)
endif()

message("Using PhysX libraries from '${PHYSX_BIN_DIRECTORY}'...")

set(PHYSX_INCLUDE_DIRECTORIES "${PHYSX_SDK_ROOT}/physx/include/" "${PHYSX_SDK_ROOT}/pxshared/include/")

# Set output directories
set(OUTPUT_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}/../bin/Mochi.PhysX.Native/${DOTNET_RID}/")

if(NOT GENERATOR_IS_MULTI_CONFIG)
    # Make sure configuration is appended to output directory for single-configuration generators
    set(OUTPUT_DIRECTORY "${OUTPUT_DIRECTORY}${CMAKE_BUILD_TYPE}/")
endif()

set(CMAKE_RUNTIME_OUTPUT_DIRECTORY "${OUTPUT_DIRECTORY}")
set(CMAKE_LIBRARY_OUTPUT_DIRECTORY "${OUTPUT_DIRECTORY}")
set(CMAKE_ARCHIVE_OUTPUT_DIRECTORY "${OUTPUT_DIRECTORY}")

# Set the CMake configuration types using the same logic as PhysX
if(CMAKE_CONFIGURATION_TYPES)
    set(CMAKE_CONFIGURATION_TYPES debug checked profile release)
    set(CMAKE_CONFIGURATION_TYPES "${CMAKE_CONFIGURATION_TYPES}" CACHE STRING "Reset config to what we need"  FORCE)

    set(CMAKE_SHARED_LINKER_FLAGS_CHECKED "")
    set(CMAKE_SHARED_LINKER_FLAGS_PROFILE "")

    # Build PDBs for all configurations
    #TODO: PhysX does this here but this seems like the wrong place for it
    set(CMAKE_SHARED_LINKER_FLAGS "/DEBUG")
endif()

# Define the library
add_library(Mochi.PhysX.Native SHARED
    "Mochi.PhysX.cpp"
    "InlineExportHelper.gen.cpp"
)

if(${CMAKE_SYSTEM_NAME} STREQUAL "Windows")
    target_sources(Mochi.PhysX.Native PUBLIC "Exports.gen.cpp")
endif()

file(GLOB PHYSX_LIBS_DEBUG "${PHYSX_BIN_DIRECTORY}/debug/${STATIC_LIBRARY_FILTER}")
file(GLOB PHYSX_LIBS_CHCKED "${PHYSX_BIN_DIRECTORY}/checked/${STATIC_LIBRARY_FILTER}")
file(GLOB PHYSX_LIBS_PROFILE "${PHYSX_BIN_DIRECTORY}/profile/${STATIC_LIBRARY_FILTER}")
file(GLOB PHYSX_LIBS_RELEASE "${PHYSX_BIN_DIRECTORY}/release/${STATIC_LIBRARY_FILTER}")
target_link_libraries(Mochi.PhysX.Native
    "${TARGET_LIBRARIES_PREFIX}"
    "$<$<CONFIG:debug>:${PHYSX_LIBS_DEBUG}>"
    "$<$<CONFIG:checked>:${PHYSX_LIBS_CHCKED}>"
    "$<$<CONFIG:profile>:${PHYSX_LIBS_PROFILE}>"
    "$<$<CONFIG:release>:${PHYSX_LIBS_RELEASE}>"
    "${TARGET_LIBRARIES_SUFFIX}"
)

target_compile_definitions(Mochi.PhysX.Native
    PRIVATE
    ${PHYSX_COMPILE_DEFS}
    ${PHYSX_LIBTYPE_DEFS}
    ${PHYSXGPU_LIBTYPE_DEFS}
    "$<$<CONFIG:debug>:${PHYSX_DEBUG_COMPILE_DEFS}>"
    "$<$<CONFIG:checked>:${PHYSX_CHECKED_COMPILE_DEFS}>"
    "$<$<CONFIG:profile>:${PHYSX_PROFILE_COMPILE_DEFS}>"
    "$<$<CONFIG:release>:${PHYSX_RELEASE_COMPILE_DEFS}>"

    "$<$<CONFIG:debug>:MOCHI_PHYSX_BUILD_VARIANT=debug>"
    "$<$<CONFIG:checked>:MOCHI_PHYSX_BUILD_VARIANT=checked>"
    "$<$<CONFIG:profile>:MOCHI_PHYSX_BUILD_VARIANT=profile>"
    "$<$<CONFIG:release>:MOCHI_PHYSX_BUILD_VARIANT=release>"
)

include_directories("${PHYSX_INCLUDE_DIRECTORIES}")

set_property(TARGET Mochi.PhysX.Native PROPERTY MSVC_RUNTIME_LIBRARY "MultiThreaded$<$<CONFIG:Debug>:Debug>")
