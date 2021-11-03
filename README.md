# Infected PhysX

[![MIT Licensed](https://img.shields.io/github/license/infectedlibraries/infectedphysx?style=flat-square)](LICENSE.txt)
[![Sponsor](https://img.shields.io/badge/sponsor-%E2%9D%A4-lightgrey?logo=github&style=flat-square)](https://github.com/sponsors/PathogenDavid)

This repo contains C# bindings for [NVIDIA PhysX](https://github.com/InfectedLibraries/PhysX) as well as a [Biohazrd](https://github.com/InfectedLibraries/Biohazrd)-powered generator for generating them.

This project is not ready to be used, if you're looking for a PhysX binding for C# I'd suggest watching releases on this repository and consider [sponsoring development of this library](https://github.com/sponsors/PathogenDavid).

## License

This project is licensed under the MIT License. [See the license file for details](LICENSE.txt).

Additionally, this project has some third-party dependencies. [See the third-party notice listing for details](THIRD-PARTY-NOTICES.md).

## Building PhysX and generating the bindings

1. Ensure Git submodules are up-to-date with `git submodule update --init --recursive`
2. Build and run `build.cmd` from the repository root

Note: You may see many errors and warnings during generation. This is because Biohazrd doesn't support everything in PhysX yet. These errors only indicate the corresponding APIs were skipped, so the output should still be fine as long as you don't need those APIs.

If you make any changes to the PhysX source code or change the branch it uses, you must re-generate the bindings using `build.cmd`.

## Building the sample

Building/running the sample is currently only supported on Windows x64 with Visual Studio 2019.

The sample does not currently have a graphical output, but will automatically connect to [the PhysX Visual Debugger](https://developer.nvidia.com/physx-visual-debugger) if it's running.

### Prerequisites

Tool | Recommended Version
-----|--------------------
[Visual Studio 2019](https://visualstudio.microsoft.com/vs/) | 16.9.4
[.NET Core SDK](http://dot.net/) | 5.0
[PhysX Visual Debugger](https://developer.nvidia.com/physx-visual-debugger) | Latest

Visual Studio requires the "Desktop development with C++" and  ".NET desktop development" workloads to be installed.
