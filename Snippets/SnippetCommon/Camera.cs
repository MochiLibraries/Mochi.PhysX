using Mochi.PhysX;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Runtime.CompilerServices;

namespace Snippets;

public sealed class Camera
{
    private Vector3 mEye;
    private Vector3 mDir;
    private int mMouseX;
    private int mMouseY;

    public Camera(in Vector3 eye, in Vector3 dir)
    {
        mEye = eye;
        mDir = Vector3.Normalize(dir);
        mMouseX = 0;
        mMouseY = 0;
    }

    public void handleMouse(MouseButton button, InputAction state, int x, int y)
    {
        mMouseX = x;
        mMouseY = y;
    }

    public bool handleKey(Keys key, int x, int y, float speed = 1.0f)
    {
        Vector3 viewY = Vector3.Normalize(Vector3.Cross(mDir, new(0, 1, 0)));
        switch (key)
        {
            case Keys.W:
                mEye += mDir * 2.0f * speed;
                break;
            case Keys.S:
                mEye -= mDir * 2.0f * speed;
                break;
            case Keys.A:
                mEye -= viewY * 2.0f * speed;
                break;
            case Keys.D:
                mEye += viewY * 2.0f * speed;
                break;
            default:
                return false;
        }
        return true;
    }

    public void handleAnalogMove(float x, float y)
    {
        Vector3 viewY = Vector3.Normalize(Vector3.Cross(mDir, new(0, 1, 0)));
        mEye += mDir * y;
        mEye += viewY * x;
    }

    public void handleMotion(int x, int y)
    {
        int dx = mMouseX - x;
        int dy = mMouseY - y;

        Vector3 viewY = Vector3.Normalize(Vector3.Cross(mDir, new(0, 1, 0)));

        Quaternion qx = Quaternion.FromAxisAngle(new(0, 1, 0), MathF.PI * dx / 180.0f);
        mDir = Vector3.Transform(mDir, qx);
        Quaternion qy = Quaternion.FromAxisAngle(viewY, MathF.PI * dy / 180.0f);
        mDir = Vector3.Transform(mDir, qy);

        mDir = Vector3.Normalize(mDir);

        mMouseX = x;
        mMouseY = y;
    }

    public PxTransform getTransform()
    {
        Vector3 viewY = Vector3.Cross(mDir, new(0, 1, 0));

        if (viewY.Length < 1e-6f)
        { return new PxTransform(getEye()); }

        viewY = Vector3.Normalize(viewY);
        Vector3 col0 = Vector3.Cross(mDir, viewY);
        Vector3 col2 = -mDir;
        PxMat33 m = new
        (
            Unsafe.As<Vector3, PxVec3>(ref col0),
            Unsafe.As<Vector3, PxVec3>(ref viewY),
            Unsafe.As<Vector3, PxVec3>(ref col2)
        );
        return new PxTransform(getEye(), new PxQuat(m));
    }

    public PxVec3 getEye()
        => Unsafe.As<Vector3, PxVec3>(ref mEye);

    public PxVec3 getDir()
        => Unsafe.As<Vector3, PxVec3>(ref mDir);
}
