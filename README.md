# Infected PhysX

[![MIT Licensed](https://img.shields.io/github/license/infectedlibraries/infectedphysx?style=flat-square)](LICENSE.txt)
[![Sponsor](https://img.shields.io/badge/sponsor-%E2%9D%A4-lightgrey?logo=github&style=flat-square)](https://github.com/sponsors/PathogenDavid)

This repo contains C# bindings for [NVIDIA PhysX](https://github.com/InfectedLibraries/PhysX) as well as a [Biohazrd](https://github.com/InfectedLibraries/Biohazrd)-powered generator for generating them.

This project is not ready to be used, if you're looking for a PhysX binding for C# I'd suggest watching releases on this repository and consider [sponsoring development of this library](https://github.com/sponsors/PathogenDavid).

This repository primarily exists to serve as an example what using Biohazrd looks like today with a C++ library that has a relatively complex API. For the sake of demonstration, the output of the generator for Windows x64 is committed under [InfectedPhysX/#Generated](InfectedPhysX/#Generated).

## License

This project is licensed under the MIT License. [See the license file for details](LICENSE.txt).

Additionally, this project has some third-party dependencies. [See the third-party notice listing for details](THIRD-PARTY-NOTICES.md).

## Generating the bindings

1. Ensure Git submodules are up-to-date with `git submodule update --init --recursive`
2. Build and run `InfectedPhysX.Generator`

Note: You may see many errors and warnings during generation. This is because Biohazrd doesn't support everything in PhysX tet. These errors only indicate the corresponding APIs were skipped, so the output should still be fine as long as you don't need those APIs.

## Building the sample

Building/running the sample is currently only supported on Windows x64 with Visual Studio 2019.

### Prerequisites

Tool | Recommended Version
-----|--------------------
[Visual Studio 2019](https://visualstudio.microsoft.com/vs/) | 16.8.0 Preview 5.0
[.NET Core SDK](http://dot.net/) | 5.0 RC2
[PhysX Visaual Debugger](https://developer.nvidia.com/physx-visual-debugger) | Latest

Visual Studio requires the "Desktop development with C++" and  ".NET desktop development" workloads to be installed.

(Note: I am unsure how whether CMake prefers preview or non-preview Visual Studio. You might need non-preview 2019 installed too.)

### Build Steps

This process will be streamlined eventually. For now we're avoiding deviating too far from how PhysX is normally built.

1. Ensure Git submodules are up-to-date with `git submodule update --init --recursive`
2. Run `external/PhysX/physx/generate_projects.bat InfectedPhysX_win64`
3. Open `external/PhysX/physx/compiler/InfectedPhysX_win64/PhysXSDK.sln` in Visual Studio
4. Build the `INSTALL` project in the `checked` configuration for the `x64` platform
5. Start the [PhysX Visual Debugger](https://developer.nvidia.com/physx-visual-debugger) (Optional, but you won't see the output otherwise.)
6. Open `InfectedImGui.sln` and build/run `InfectedImGui.Sample`

If you make any changes to the PhysX source code or change the branch it uses, you must re-generate the bindings using the instructions above.

Additionally you may need to rebuild PhysX if you modify the binding generator. This is because there's a small native component to expose inline methods to C# that would not otherwise get exported by the C++ compiler.
