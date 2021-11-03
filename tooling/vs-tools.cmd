@echo off
:: Check if we're already running from a Visual Studio command prompt
:: If it's the right version/platform, do nothing. Otherwise error.
if not '%VisualStudioVersion%'=='' (
    if not '%VisualStudioVersion%'=='16.0' (
        echo Running from Visual Studio command prompt that isn't the right version.
        echo Run this command from a clean environment or from the 2019 x64 developer command prompt.
        exit /b 1
    )

    if not '%Platform%'=='x64' (
        echo Running from Visual Studio command prompt that isn't the right platform.
        echo Run this command from a clean environment or from the 2019 x64 developer command prompt.
        exit /b 1
    )

    exit /b 0
)

:: Check if vswhere is available
:: (It might be tempting to use the version that comes with PhysX, but it's pretty old and doesn't support the `-find` switch.)
set VSWHERE="%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe"
if not exist %VSWHERE% (
    echo vswhere.exe could not be found at `%VSWHERE%`, is Visual Studio installed?
    exit /b 1
)

:: Initialize Visual Studio command prompt
for /f "usebackq tokens=*" %%i in (`call %VSWHERE% -latest -requires Microsoft.VisualStudio.Workload.NativeDesktop -version "[16.0,17.0)" -find VC\Auxiliary\Build\vcvars64.bat`) do (
    call "%%i" && exit /b 0
)

:: If we got this far, vswhere didn't return a result
echo Failed to find a compatible version of Visual Studio
echo Make sure you have a non-pre-release version of Visual Studio 2019 with the "Desktop development with C++" workload installed.
exit /b 1
