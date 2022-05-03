﻿#if RENDER_SNIPPET
using Mochi.PhysX;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SnippetCommon;
using static Mochi.PhysX.Globals;
using static SnippetArticulation;
using static SnippetCommon.FakeGlut;

internal unsafe static class SnippetArticulationRender
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
        stepPhysics(true);

        SnippetRender.startRender(sCamera.getEye(), sCamera.getDir());

        PxScene* scene;
        PxGetPhysics()->getScenes(&scene, 1);
        uint nbActors = scene->getNbActors(PxActorTypeFlags.eRIGID_DYNAMIC | PxActorTypeFlags.eRIGID_STATIC);
        if (nbActors != 0)
        {
            // (Allocating on the heap here is not a good idea performance-wise, we only do it this way to keep close to the original snippet.)
            fixed (PxRigidActor** actors = new PxRigidActor*[nbActors])
            {
                scene->getActors(PxActorTypeFlags.eRIGID_DYNAMIC | PxActorTypeFlags.eRIGID_STATIC, (PxActor**)actors, nbActors);
                SnippetRender.renderActors(actors, nbActors, true);
            }
        }

        uint nbArticulations = scene->getNbArticulations();
        for (uint i = 0; i < nbArticulations; i++)
        {
            PxArticulationBase* articulation;
            scene->getArticulations(&articulation, 1, i);

            uint nbLinks = articulation->getNbLinks();
            // (Allocating on the heap here is not a good idea performance-wise, we only do it this way to keep close to the original snippet.)
            fixed (PxArticulationLink** links = new PxArticulationLink*[nbLinks])
            {
                articulation->getLinks(links, nbLinks);

                SnippetRender.renderActors(links, nbLinks, true);
            }
        }

        SnippetRender.finishRender();
    }

    static void exitCallback()
    {
        sCamera = null!;
        cleanupPhysics(true);
    }

    static readonly Vector3 gCamEyeChain = new(9.621917f, 24.677629f, 16.127209f);
    static readonly Vector3 gCamDirChain = new(-0.138525f, -0.468482f, -0.872546f);

    static readonly Vector3 gCamEyeLift = new(8.605188f, 4.050591f, 0.145860f);
    static readonly Vector3 gCamDirLift = new(-0.999581f, -0.026449f, 0.011790f);

    public static void renderLoop()
    {
        if (gCreateLiftScene)
            sCamera = new Snippets.Camera(gCamEyeLift, gCamDirLift);
        else
            sCamera = new Snippets.Camera(gCamEyeChain, gCamDirChain);

        SnippetRender.setupDefaultWindow("PhysX Snippet Articulation");
        SnippetRender.setupDefaultRenderState();

        //glutIdleFunc(&idleCallback);
        glutDisplayFunc(&renderCallback);
        glutKeyboardFunc(&keyboardCallback);
        glutMouseFunc(&mouseCallback);
        glutMotionFunc(&motionCallback);
        motionCallback(0, 0);

        initPhysics(true);
        glutMainLoop();
        exitCallback();
    }
}
#endif
