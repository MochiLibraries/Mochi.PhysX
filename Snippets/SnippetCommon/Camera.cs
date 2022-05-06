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
