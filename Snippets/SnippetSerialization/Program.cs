// Copyright (c) 2022 David Maas and Contributors. All rights reserved.
// Copyright (c) 2008-2021 NVIDIA Corporation. All rights reserved.
// Copyright (c) 2004-2008 AGEIA Technologies, Inc. All rights reserved.
// Copyright (c) 2001-2004 NovodeX AG. All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions
// are met:
//  * Redistributions of source code must retain the above copyright
//    notice, this list of conditions and the following disclaimer.
//  * Redistributions in binary form must reproduce the above copyright
//    notice, this list of conditions and the following disclaimer in the
//    documentation and/or other materials provided with the distribution.
//  * Neither the name of NVIDIA CORPORATION nor the names of its
//    contributors may be used to endorse or promote products derived
//    from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS ''AS IS'' AND ANY
// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
// PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE COPYRIGHT OWNER OR
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
// PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
// PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY
// OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#define USE_FILES // This isn't in the original snippet but I thought it was useful/interesting to test.

using Mochi.PhysX;
using static SnippetSerialization;

#if !USE_FILES
using static Mochi.PhysX.Globals;
#else
using SnippetCommon;
#endif

#if RENDER_SNIPPET
using static SnippetSerializationRender;
#else
using System;
#endif

unsafe
{
#if USE_FILES
    PinnedUtf8String sharedFileName = gUseBinarySerialization ? "SerializedShared.bin" : "SerializedShared.xml";
    PinnedUtf8String actorFileName = gUseBinarySerialization ? "SerializedActors.bin" : "SerializedActors.xml";
#endif

    initPhysics();
    // Alternatively PxDefaultFileOutputStream could be used
#if !USE_FILES
    PxDefaultMemoryOutputStream sharedOutputStream = new(ref *PxGetFoundation()->getAllocatorCallback()); //BIOQUIRK: Critical missing default
    PxDefaultMemoryOutputStream actorOutputStream = new(ref *PxGetFoundation()->getAllocatorCallback()); //BIOQUIRK: Critical missing default
    serializeObjects(ref sharedOutputStream, ref actorOutputStream);
#else
    {
        PxDefaultFileOutputStream sharedOutputStream = new(sharedFileName);
        PxDefaultFileOutputStream actorOutputStream = new(actorFileName);
        serializeObjects(ref sharedOutputStream, ref actorOutputStream);
        //BIOQUIRK: Manual destructor call (absolutely needed in order for the files to get closed.)
        //sharedOutputStream.Destructor();
        //actorOutputStream.Destructor();
        //BIOQUIRK: The translation of destructors' ABIs is not correct. https://github.com/MochiLibraries/Biohazrd/issues/243
        ((delegate* unmanaged[Cdecl]<PxDefaultFileOutputStream*, int, void>)actorOutputStream.VirtualMethodTablePointer->__DeletingDestructorPointer)(&actorOutputStream, 0);
        ((delegate* unmanaged[Cdecl]<PxDefaultFileOutputStream*, int, void>)sharedOutputStream.VirtualMethodTablePointer->__DeletingDestructorPointer)(&sharedOutputStream, 0);
    }
#endif
    cleanupPhysics();

    initPhysics();
    // Alternatively PxDefaultFileInputData could be used
#if !USE_FILES
    PxDefaultMemoryInputData sharedInputStream = new(sharedOutputStream.getData(), sharedOutputStream.getSize());
    PxDefaultMemoryInputData actorInputStream = new(actorOutputStream.getData(), actorOutputStream.getSize());
#else
    //BIOQUIRK: Destructors not being called
    PxDefaultFileInputData sharedInputStream = new(sharedFileName);
    PxDefaultFileInputData actorInputStream = new(actorFileName);
#endif
    deserializeObjects(ref sharedInputStream, ref actorInputStream);
#if RENDER_SNIPPET
    renderLoop();
#else
    const uint frameCount = 250;
    for (uint i = 0; i < frameCount; i++)
        stepPhysics();
    cleanupPhysics();
    Console.WriteLine("SnippetSerialization done.");
#endif

    return 0;
}
