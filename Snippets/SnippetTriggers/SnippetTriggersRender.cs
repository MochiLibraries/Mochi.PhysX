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
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SnippetCommon;
using System.Diagnostics;
using static Mochi.PhysX.Globals;
using static SnippetCommon.FakeGlut;
using static SnippetTriggers;

internal unsafe static class SnippetTriggersRender
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

        if (!sCamera.handleKey(key, x, y))
            keyPress(key, sCamera.getTransform());
    }

    static void keyboardCallback2(Keys key, int x, int y)
    {
        keyPress(key, sCamera.getTransform());
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

    static void InitLighting()
    {
        GL.Enable(EnableCap.ColorMaterial);

        Color4 zero = new(0.0f, 0.0f, 0.0f, 0.0f);
        GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, zero);
        GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Ambient, zero);
        GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Specular, zero);
        GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Emission, zero);
        GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Shininess, 0.0f);

        GL.LightModel(LightModelParameter.LightModelAmbient, &zero.R);

        GL.Enable(EnableCap.Lighting);
        Vector3 Dir = new(-1.0f, 1.0f, 0.5f);
        //Vector3 Dir = new(0.0f, 1.0f, 0.0f);
        Dir = Vector3.Normalize(Dir);

        const float AmbientValue = 0.3f;
        Color4 ambientColor0 = new(AmbientValue, AmbientValue, AmbientValue, 0.0f);
        GL.Light(LightName.Light0, LightParameter.Ambient, ambientColor0);

        Color4 specularColor0 = new(0.0f, 0.0f, 0.0f, 0.0f);
        GL.Light(LightName.Light0, LightParameter.Specular, specularColor0);

        Color4 diffuseColor0 = new(0.7f, 0.7f, 0.7f, 0.0f);
        GL.Light(LightName.Light0, LightParameter.Diffuse, diffuseColor0);

        Color4 position0 = new(Dir.X, Dir.Y, Dir.Z, 0.0f);
        GL.Light(LightName.Light0, LightParameter.Position, position0);

        GL.Enable(EnableCap.Light0);

        // GL.Color4(1.0f, 1.0f, 1.0f, 1.0f);
        // GL.Color4(0.0f, 0.0f, 0.0f, 0.0f);

#if false
        {
            GL.Enable(EnableCap.Fog);
            GL.Fog(FogParameter.FogMode, (int)FogMode.Linear);
            // GL.Fog(FogParameter.FogMode, (int)FogMode.Exp);
            // GL.Fog(FogParameter.FogMode, (int)FogMode.Exp2);
            GL.Fog(FogParameter.FogStart, 0.0f);
            GL.Fog(FogParameter.FogEnd, 100.0f);
            GL.Fog(FogParameter.FogDensity, 0.005f);
            // GL.ClearColor(0.2f, 0.2f, 0.2f, 1.0);
            // Vector3 FogColor = new(0.2f, 0.2f, 0.2f);
            Vector3 FogColor = new(1.0f);
            GL.Fog(FogParameter.FogColor, &FogColor.X);
        }
#endif
    }

    // Not applicable for Mochi-flavored snippets
    // class MyTriggerRender : public Snippets::TriggerRender
    // {
    //     public:
    //     virtual bool isTrigger(physx::PxShape* shape) const
    //     {
    //         return ::isTriggerShape(shape);
    //     }
    // };

    static void renderCallback()
    {
        stepPhysics(true);

#if false
        {
            PxVec3 camPos = sCamera.getEye();
            PxVec3 camDir = sCamera.getDir();
            Console.WriteLine($"camPos: ({camPos.x}, {camPos.y}, {camPos.z})");
            Console.WriteLine($"camDir: ({camDir.x}, {camDir.y}, {camDir.z})");
        }
#endif

        SnippetRender.startRender(sCamera.getEye(), sCamera.getDir());
        InitLighting();

        PxScene* scene;
        PxGetPhysics()->getScenes(&scene, 1);
        uint nbActors = scene->getNbActors(PxActorTypeFlags.eRIGID_DYNAMIC | PxActorTypeFlags.eRIGID_STATIC);
        if (nbActors != 0)
        {
            // (Allocating on the heap here is not a good idea performance-wise, we only do it this way to keep close to the original snippet.)
            fixed (PxRigidActor** actors = new PxRigidActor*[nbActors])
            {
                scene->getActors(PxActorTypeFlags.eRIGID_DYNAMIC | PxActorTypeFlags.eRIGID_STATIC, (PxActor**)actors, nbActors);

                SnippetRender.renderActors(actors, nbActors, true, new(0.0f, 0.75f, 0.0f), &isTriggerShape);
            }
        }

        SnippetRender.finishRender();
    }

    static void exitCallback()
    {
        sCamera = null!;
        cleanupPhysics(true);
    }

    public static void renderLoop()
    {
        Debug.Assert(sCamera is null);
        sCamera = new Snippets.Camera(new(8.757190f, 12.367847f, 23.541956f), new(-0.407947f, -0.042438f, -0.912019f));

        SnippetRender.setupDefaultWindow("PhysX Snippet Triggers");
        SnippetRender.setupDefaultRenderState();

        //glutIdleFunc(&idleCallback);
        glutDisplayFunc(&renderCallback);
        glutKeyboardFunc(&keyboardCallback);
        glutSpecialFunc(&keyboardCallback2);
        glutMouseFunc(&mouseCallback);
        glutMotionFunc(&motionCallback);
        motionCallback(0, 0);

        initPhysics(true);
        glutMainLoop();
        exitCallback();
    }
}
#endif
