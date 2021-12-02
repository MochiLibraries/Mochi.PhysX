@echo off
setlocal

:: Start in the directory containing this script
cd %~dp0

set PHYSX_SDK_ROOT=external\PhysX
set PHYSX_PRESET=mochi-physx-win-x64

:: Ensure PhysX SDK has been cloned
if not exist %PHYSX_SDK_ROOT%\physx\ (
    echo PhysX SDK not found, did you forget to clone recursively? 1>&2
    exit /B 1
)

:: Copy our .gitignore into the PhysX SDK if necessary
if not exist %PHYSX_SDK_ROOT%\.gitignore (
    copy /Y Mochi.PhysX.Modifications\physx.gitignore %PHYSX_SDK_ROOT%\.gitignore >NUL
)

:: Copy our PhysX presets into the PhysX SDK
if exist %PHYSX_SDK_ROOT%\physx\buildtools\presets\public\ (
    copy /Y Mochi.PhysX.Modifications\*.xml %PHYSX_SDK_ROOT%\physx\buildtools\presets\public\ >NUL
) else (
    copy /Y Mochi.PhysX.Modifications\*.xml %PHYSX_SDK_ROOT%\physx\buildtools\presets\ >NUL
)

:: Generate the PhysX Visual Studio solution if necessary
if not exist %PHYSX_SDK_ROOT%\physx\compiler\%PHYSX_PRESET%\INSTALL.vcxproj (
    cmd /C %PHYSX_SDK_ROOT%\physx\generate_projects.bat %PHYSX_PRESET%

    if not errorlevel 0 (
        echo Failed to generate PhysX Visual Studio solution! 1>&2
        exit /B 1
    )

    :: The Python script doesn't always exit with an error code when there's an error, so double check that CMake was invoked
    if not exist %PHYSX_SDK_ROOT%\physx\compiler\%PHYSX_PRESET%\INSTALL.vcxproj (
        echo Failed to generate PhysX Visual Studio solution! 1>&2
        exit /B 1
    )
)

:: Build PhysX in all configurations
call tooling/vs-tools.cmd
pushd %PHYSX_SDK_ROOT%\physx\compiler\%PHYSX_PRESET%\
msbuild INSTALL.vcxproj /p:Configuration=debug /p:Platform=x64
msbuild INSTALL.vcxproj /p:Configuration=checked /p:Platform=x64
msbuild INSTALL.vcxproj /p:Configuration=profile /p:Platform=x64
msbuild INSTALL.vcxproj /p:Configuration=release /p:Platform=x64
popd

:: Run generator (will also build Mochi.PhysX.Native)
dotnet run --configuration Release --project InfectedPhysX.Generator -- "external/PhysX/" "InfectedPhysX/#Generated/" "Mochi.PhysX.Native/"
