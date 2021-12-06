@echo off
setlocal

:: Start in the directory containing this script
cd %~dp0

set BUILD_FOLDER=..\obj\Mochi.PhysX.Native\cmake\win-x64

:: Ensure build folder is protected from Directory.Build.* influences
if not exist %BUILD_FOLDER% (
    mkdir %BUILD_FOLDER%
    echo ^<Project^>^</Project^> > %BUILD_FOLDER%/Directory.Build.props
    echo ^<Project^>^</Project^> > %BUILD_FOLDER%/Directory.Build.targets
    echo # > %BUILD_FOLDER%/Directory.Build.rsp
)

:: (Re)generate the Visual Studio solution and build in all configurations
cmake -G "Visual Studio 16 2019" -S . -B %BUILD_FOLDER% || exit /B 1
echo ==============================================================================
echo Building Mochi.PhysX.Native debug build...
echo ==============================================================================
cmake --build %BUILD_FOLDER% --config debug || exit /B 1
echo ==============================================================================
echo Building Mochi.PhysX.Native checked build...
echo ==============================================================================
cmake --build %BUILD_FOLDER% --config checked || exit /B 1
echo ==============================================================================
echo Building Mochi.PhysX.Native profile build...
echo ==============================================================================
cmake --build %BUILD_FOLDER% --config profile || exit /B 1
echo ==============================================================================
echo Building Mochi.PhysX.Native release build...
echo ==============================================================================
cmake --build %BUILD_FOLDER% --config release || exit /B 1
