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

#if RENDER_SNIPPET
using Mochi.PhysX;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SnippetCommon;
using System;
using static Mochi.PhysX.Globals;
using static SnippetCommon.FakeGlut;
using static SnippetSerialization;

internal unsafe static class SnippetSerializationRender
{
    static Snippets.Camera sCamera = null!;

    static void motionCallback(int x, int y)
    {
        sCamera.handleMotion(x, y);
    }

    static void keyboardCallback(Keys key, int x, int y)
    {
        if (key == Keys.Escape)
            SignalSnippetExit();

        sCamera.handleKey(key, x, y);
    }

    static void mouseCallback(MouseButton button, InputAction state, int x, int y)
    {
        sCamera.handleMouse(button, state, x, y);
    }

    // Not applicable for Mochi-flavored snippet
    //static void idleCallback()
    //{
    //    glutPostRedisplay();
    //}

    static void renderCallback()
    {
        SnippetRender.startRender(sCamera.getEye(), sCamera.getDir());

        PxScene* scene;
        PxGetPhysics()->getScenes(&scene, 1);
        uint nbActors = scene->getNbActors(PxActorTypeFlags.eRIGID_DYNAMIC | PxActorTypeFlags.eRIGID_STATIC);
        if (nbActors != 0)
        {
            // (Allocating on the heap here is not a good idea performance-wise, we only do it this way to keep close to the original snippet.)
            fixed (PxRigidActor** actors = new PxRigidActor*[nbActors])
            {
                scene->getActors(PxActorTypeFlags.eRIGID_DYNAMIC | PxActorTypeFlags.eRIGID_STATIC, actors, nbActors);
                SnippetRender.renderActors(actors, nbActors, true);
            }
        }

        SnippetRender.finishRender();

        stepPhysics();
    }

    static void exitCallback()
    {
        sCamera = null!;
        cleanupPhysics();
        Console.WriteLine("SnippetSerialization done.");
    }

    public static void renderLoop()
    {
        sCamera = new Snippets.Camera(new(50.0f, 50.0f, 50.0f), new(-0.6f, -0.2f, -0.7f));

        SnippetRender.setupDefaultWindow("PhysX Snippet Serialization");
        SnippetRender.setupDefaultRenderState();

        //glutIdleFunc(idleCallback);
        glutDisplayFunc(&renderCallback);
        glutKeyboardFunc(&keyboardCallback);
        glutMouseFunc(&mouseCallback);
        glutMotionFunc(&motionCallback);
        motionCallback(0, 0);

        glutMainLoop();
        exitCallback();
    }
}
#endif
