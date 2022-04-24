#pragma warning disable IDE1005 // Delegate invocation can be simplified. -- False positive, https://github.com/dotnet/roslyn/issues/50976
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace SnippetCommon;

public unsafe static partial class FakeGlut
{
    private static delegate*<void> DisplayFunc;
    private static delegate*<Keys, int, int, void> KeyboardFunc;
    private static delegate*<Keys, int, int, void> SpecialFunc;
    private static delegate*<MouseButton, InputAction, int, int, void> MouseFunc;
    private static delegate*<int, int, void> MotionFunc;

    public static void glutDisplayFunc(delegate*<void> displayFunc)
        => DisplayFunc = displayFunc;

    public static void glutKeyboardFunc(delegate*<Keys, int, int, void> keyboardFunc)
        => KeyboardFunc = keyboardFunc;

    public static void glutSpecialFunc(delegate*<Keys, int, int, void> specialFunc)
        => SpecialFunc = specialFunc;

    public static void glutMouseFunc(delegate*<MouseButton, InputAction, int, int, void> mouseFunc)
        => MouseFunc = mouseFunc;

    public static void glutMotionFunc(delegate*<int, int, void> motionFunc)
        => MotionFunc = motionFunc;

    public static void glutMainLoop()
    {
        NativeWindow window = SnippetRender.SnippetWindow;
        if (window is null)
        { throw new InvalidOperationException("Tried to start main loop before setting up the default window!"); }

        int lastMouseX = 0;
        int lastMouseY = 0;
        int mouseButtonsDown = 0;

        void KeyDown(KeyboardKeyEventArgs e)
        {
            // For the sake of simplicity, we do not bother emulating GLUT's behavior of only passing ASCII keys to the keybaord callback
            // (In theory it's closet to GLFW's char callback, but the way the snippets use the different callbacks doesn't mesh well with using it instead.)
            if (KeyboardFunc != null)
            { KeyboardFunc(e.Key, lastMouseX, lastMouseY); }

            if (SpecialFunc != null)
            { SpecialFunc(e.Key, lastMouseX, lastMouseY); }
        }

        void MouseUpOrDown(MouseButtonEventArgs e)
        {
            int buttonBit = 1 << (int)e.Button;
            if (e.Action != InputAction.Release)
            { mouseButtonsDown |= buttonBit; }
            else
            { mouseButtonsDown &= ~buttonBit; }

            if (MouseFunc != null && e.Action != InputAction.Repeat)
            { MouseFunc(e.Button, e.Action, lastMouseX, lastMouseY); }
        }

        void MouseMove(MouseMoveEventArgs e)
        {
            lastMouseX = (int)e.X;
            lastMouseY = (int)e.Y;

            // MotionFunc is only called if any buttons are down
            if (MotionFunc != null && mouseButtonsDown != 0)
            { MotionFunc((int)e.X, (int)e.Y); }
        }

        window.KeyDown += KeyDown;
        window.MouseUp += MouseUpOrDown;
        window.MouseDown += MouseUpOrDown;
        window.MouseMove += MouseMove;

        while (!GLFW.WindowShouldClose(window.WindowPtr))
        {
            window.ProcessEvents();

            if (DisplayFunc != null)
            { DisplayFunc(); }
        }

        window.KeyDown -= KeyDown;
        window.MouseUp -= MouseUpOrDown;
        window.MouseDown -= MouseUpOrDown;
        window.MouseMove -= MouseMove;
    }

    public static void SignalSnippetExit()
        => SnippetRender.SnippetWindow?.Close();
}
