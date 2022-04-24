#if RENDER_SNIPPET
using Mochi.PhysX;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SnippetCommon;
using static Mochi.PhysX.Globals;
using static SnippetCommon.FakeGlut;
using static SnippetHelloWorld;

internal unsafe static class SnippetHelloWorldRender
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

        SnippetRender.finishRender();
    }

    static void exitCallback()
    {
        sCamera = null!;
        cleanupPhysics(true);
    }

    public static void renderLoop()
    {
        sCamera = new Snippets.Camera(new(50.0f, 50.0f, 50.0f), new(-0.6f, -0.2f, -0.7f));

        SnippetRender.setupDefaultWindow("PhysX Snippet HelloWorld");
        SnippetRender.setupDefaultRenderState();

        //glutIdleFunc(idleCallback);
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
