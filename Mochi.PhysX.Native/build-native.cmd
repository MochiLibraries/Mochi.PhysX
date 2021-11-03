@echo off
setlocal

:: Start in the directory containing this script
cd %~dp0

set BUILD_FOLDER=build-windows-x64

:: Ensure build folder is protected from Directory.Build.* influences
if not exist %BUILD_FOLDER% (
    mkdir %BUILD_FOLDER% 2>NUL
    echo ^<Project^>^</Project^> > %BUILD_FOLDER%/Directory.Build.props
    echo ^<Project^>^</Project^> > %BUILD_FOLDER%/Directory.Build.targets
    echo # > %BUILD_FOLDER%/Directory.Build.rsp
)

:: (Re)generate the Visual Studio solution and build in all configurations
cmake -G "Visual Studio 16 2019" -S . -B %BUILD_FOLDER%
cmake --build %BUILD_FOLDER% --config debug
cmake --build %BUILD_FOLDER% --config checked
cmake --build %BUILD_FOLDER% --config profile
cmake --build %BUILD_FOLDER% --config release
