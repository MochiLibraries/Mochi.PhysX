@echo off
setlocal

:: Start in the directory containing this script
cd %~dp0

set PHYSX_SDK_ROOT=external\PhysX
set PHYSX_PRESET=mochi-physx-win-x64

set BUILD_MODE=%1

:: If we're only generating skip all the native build setup
:: (It's assumed that PhysX has been cloned and built already. If it hasn't the generator will complain)
if /i "%BUILD_MODE%" == "generate" (
    rem Initializing the Visual Studio tools is not strictly necessary, but it does affect Biohazrd.
    rem As such we do it for the sake of consistency between invoking build.cmd with and without arguments.
    call tooling/vs-tools.cmd
    goto GENERATE
)

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

    rem The Python script doesn't always exit with an error code when there's an error, so double check that CMake was invoked
    if not exist %PHYSX_SDK_ROOT%\physx\compiler\%PHYSX_PRESET%\INSTALL.vcxproj (
        echo Failed to generate PhysX Visual Studio solution! 1>&2
        exit /B 1
    )
)

:: Build PhysX in all configurations
call tooling/vs-tools.cmd
pushd %PHYSX_SDK_ROOT%\physx\compiler\%PHYSX_PRESET%\
if "%BUILD_MODE%" == "" (
    msbuild INSTALL.vcxproj /maxCpuCount /p:Platform=x64 /p:Configuration=debug
    msbuild INSTALL.vcxproj /maxCpuCount /p:Platform=x64 /p:Configuration=checked
    msbuild INSTALL.vcxproj /maxCpuCount /p:Platform=x64 /p:Configuration=profile
    msbuild INSTALL.vcxproj /maxCpuCount /p:Platform=x64 /p:Configuration=release
) else if /i "%BUILD_MODE%" == "debug" (
    msbuild INSTALL.vcxproj /maxCpuCount /p:Platform=x64 /p:Configuration=debug
) else if /i "%BUILD_MODE%" == "checked" (
    msbuild INSTALL.vcxproj /maxCpuCount /p:Platform=x64 /p:Configuration=checked
) else if /i "%BUILD_MODE%" == "profile" (
    msbuild INSTALL.vcxproj /maxCpuCount /p:Platform=x64 /p:Configuration=profile
) else if /i "%BUILD_MODE%" == "release" (
    msbuild INSTALL.vcxproj /maxCpuCount /p:Platform=x64 /p:Configuration=release
) else (
    echo '%BUILD_MODE%' is not a known configuration. 1>&2
    exit /B 1
)
popd

:: Run generator (will also build Mochi.PhysX.Native)
if "%BUILD_MODE%" == "" (
    :GENERATE
    rem vs-tools adds an environment variable named `Platform` which messes with `dotnet run`.
    set Platform=
    dotnet run --configuration Release --project Mochi.PhysX.Generator -- "external/PhysX/" "Mochi.PhysX/#Generated/" "Mochi.PhysX.Native/"
)
