using Mochi.PhysX;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using static SnippetCommon.FakeGlut;

namespace SnippetCommon;

public unsafe static class SnippetRender
{
    static int MAX_NUM_ACTOR_SHAPES = 128;

    static float[] gCylinderData = new[]
    {
        1.0f,0.0f,1.0f,1.0f,0.0f,1.0f,1.0f,0.0f,0.0f,1.0f,0.0f,0.0f,
        0.866025f,0.500000f,1.0f,0.866025f,0.500000f,1.0f,0.866025f,0.500000f,0.0f,0.866025f,0.500000f,0.0f,
        0.500000f,0.866025f,1.0f,0.500000f,0.866025f,1.0f,0.500000f,0.866025f,0.0f,0.500000f,0.866025f,0.0f,
        -0.0f,1.0f,1.0f,-0.0f,1.0f,1.0f,-0.0f,1.0f,0.0f,-0.0f,1.0f,0.0f,
        -0.500000f,0.866025f,1.0f,-0.500000f,0.866025f,1.0f,-0.500000f,0.866025f,0.0f,-0.500000f,0.866025f,0.0f,
        -0.866025f,0.500000f,1.0f,-0.866025f,0.500000f,1.0f,-0.866025f,0.500000f,0.0f,-0.866025f,0.500000f,0.0f,
        -1.0f,-0.0f,1.0f,-1.0f,-0.0f,1.0f,-1.0f,-0.0f,0.0f,-1.0f,-0.0f,0.0f,
        -0.866025f,-0.500000f,1.0f,-0.866025f,-0.500000f,1.0f,-0.866025f,-0.500000f,0.0f,-0.866025f,-0.500000f,0.0f,
        -0.500000f,-0.866025f,1.0f,-0.500000f,-0.866025f,1.0f,-0.500000f,-0.866025f,0.0f,-0.500000f,-0.866025f,0.0f,
        0.0f,-1.0f,1.0f,0.0f,-1.0f,1.0f,0.0f,-1.0f,0.0f,0.0f,-1.0f,0.0f,
        0.500000f,-0.866025f,1.0f,0.500000f,-0.866025f,1.0f,0.500000f,-0.866025f,0.0f,0.500000f,-0.866025f,0.0f,
        0.866026f,-0.500000f,1.0f,0.866026f,-0.500000f,1.0f,0.866026f,-0.500000f,0.0f,0.866026f,-0.500000f,0.0f,
        1.0f,0.0f,1.0f,1.0f,0.0f,1.0f,1.0f,0.0f,0.0f,1.0f,0.0f,0.0f
    };

    static int MAX_NUM_MESH_VEC3S => gVertexBuffer.Length;
    static PxVec3[] gVertexBuffer = new PxVec3[1024];

    static void renderGeometry(ref PxGeometry geom)
    {
        switch (geom.getType())
        {
            case PxGeometryType.eBOX:
            {
                ref PxBoxGeometry boxGeom = ref Unsafe.As<PxGeometry, PxBoxGeometry>(ref geom);
                float hx = boxGeom.halfExtents.x;
                float hy = boxGeom.halfExtents.y;
                float hz = boxGeom.halfExtents.z;

                // +x
                GL.Begin(PrimitiveType.Quads);
                GL.Normal3(1f, 0f, 0f);
                GL.Vertex3(+hx, +hy, +hz);
                GL.Vertex3(+hx, -hy, +hz);
                GL.Vertex3(+hx, -hy, -hz);
                GL.Vertex3(+hx, +hy, -hz);
                GL.End();

                // -x
                GL.Begin(PrimitiveType.Quads);
                GL.Normal3(-1f, 0f, 0f);
                GL.Vertex3(-hx, +hy, +hz);
                GL.Vertex3(-hx, +hy, -hz);
                GL.Vertex3(-hx, -hy, -hz);
                GL.Vertex3(-hx, -hy, +hz);
                GL.End();

                // +y
                GL.Begin(PrimitiveType.Quads);
                GL.Normal3(0f, 1f, 0f);
                GL.Vertex3(+hx, +hy, +hz);
                GL.Vertex3(-hx, +hy, +hz);
                GL.Vertex3(-hx, +hy, -hz);
                GL.Vertex3(+hx, +hy, -hz);
                GL.End();

                // -y
                GL.Begin(PrimitiveType.Quads);
                GL.Normal3(0f, -1f, 0f);
                GL.Vertex3(+hx, -hy, +hz);
                GL.Vertex3(+hx, -hy, -hz);
                GL.Vertex3(-hx, -hy, -hz);
                GL.Vertex3(-hx, -hy, +hz);
                GL.End();

                // +z
                GL.Begin(PrimitiveType.Quads);
                GL.Normal3(0f, 0f, 1f);
                GL.Vertex3(+hx, +hy, +hz);
                GL.Vertex3(-hx, +hy, +hz);
                GL.Vertex3(-hx, -hy, +hz);
                GL.Vertex3(+hx, -hy, +hz);
                GL.End();

                // -z
                GL.Begin(PrimitiveType.Quads);
                GL.Normal3(0f, 0f, -1f);
                GL.Vertex3(+hx, +hy, -hz);
                GL.Vertex3(+hx, -hy, -hz);
                GL.Vertex3(-hx, -hy, -hz);
                GL.Vertex3(-hx, +hy, -hz);
                GL.End();
            }
            break;

            case PxGeometryType.eSPHERE:
            {
                ref PxSphereGeometry sphereGeom = ref Unsafe.As<PxGeometry, PxSphereGeometry>(ref geom);
                glutSolidSphere(sphereGeom.radius, 10, 10);
            }
            break;

            case PxGeometryType.eCAPSULE:
            {
                ref PxCapsuleGeometry capsuleGeom = ref Unsafe.As<PxGeometry, PxCapsuleGeometry>(ref geom);
                float radius = capsuleGeom.radius;
                float halfHeight = capsuleGeom.halfHeight;

                //Sphere
                GL.PushMatrix();
                GL.Translate(halfHeight, 0.0f, 0.0f);
                GL.Scale(radius, radius, radius);
                glutSolidSphere(1, 10, 10);
                GL.PopMatrix();

                //Sphere
                GL.PushMatrix();
                GL.Translate(-halfHeight, 0.0f, 0.0f);
                GL.Scale(radius, radius, radius);
                glutSolidSphere(1, 10, 10);
                GL.PopMatrix();

                //Cylinder
                GL.PushMatrix();
                GL.Translate(-halfHeight, 0.0f, 0.0f);
                GL.Scale(2.0f * halfHeight, radius, radius);
                GL.Rotate(90.0f, 0.0f, 1.0f, 0.0f);
                GL.EnableClientState(ArrayCap.VertexArray);
                GL.EnableClientState(ArrayCap.NormalArray);
                GL.VertexPointer(3, VertexPointerType.Float, 2 * 3 * sizeof(float), gCylinderData);
                GL.NormalPointer(NormalPointerType.Float, 2 * 3 * sizeof(float), ref gCylinderData[3]);
                GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 13 * 2);
                GL.DisableClientState(ArrayCap.VertexArray);
                GL.DisableClientState(ArrayCap.NormalArray);
                GL.PopMatrix();
            }
            break;

            case PxGeometryType.eCONVEXMESH:
            {
                ref PxConvexMeshGeometry convexGeom = ref Unsafe.As<PxGeometry, PxConvexMeshGeometry>(ref geom);

                //Compute triangles for each polygon.
                PxVec3 scale = convexGeom.scale.scale;
                PxConvexMesh* mesh = convexGeom.convexMesh;
                uint nbPolys = mesh->getNbPolygons();
                byte* polygons = mesh->getIndexBuffer();
                PxVec3* verts = mesh->getVertices();
                uint nbVerts = mesh->getNbVertices();

                uint numTotalTriangles = 0;
                for (uint i = 0; i < nbPolys; i++)
                {
                    PxHullPolygon data = default;
                    mesh->getPolygonData(i, ref data); //BIOQUIRK: Should this be an out byref?

                    uint nbTris = (uint)(data.mNbVerts - 2);
                    byte vref0 = polygons[data.mIndexBase + 0];
                    Debug.Assert(vref0 < nbVerts);
                    for (uint j = 0; j < nbTris; j++)
                    {
                        uint vref1 = polygons[data.mIndexBase + 0 + j + 1];
                        uint vref2 = polygons[data.mIndexBase + 0 + j + 2];

                        //generate face normal:
                        PxVec3 e0 = verts[vref1].operator_Minus(verts[vref0]); //BIOQUIRK: Overloaded operator
                        PxVec3 e1 = verts[vref2].operator_Minus(verts[vref0]); //BIOQUIRK: Overloaded operator

                        Debug.Assert(vref1 < nbVerts);
                        Debug.Assert(vref2 < nbVerts);

                        PxVec3 fnormal = e0.cross(e1);
                        fnormal.normalize();

                        if (numTotalTriangles * 6 < MAX_NUM_MESH_VEC3S)
                        {
                            gVertexBuffer[numTotalTriangles * 6 + 0] = fnormal;
                            gVertexBuffer[numTotalTriangles * 6 + 1] = verts[vref0];
                            gVertexBuffer[numTotalTriangles * 6 + 2] = fnormal;
                            gVertexBuffer[numTotalTriangles * 6 + 3] = verts[vref1];
                            gVertexBuffer[numTotalTriangles * 6 + 4] = fnormal;
                            gVertexBuffer[numTotalTriangles * 6 + 5] = verts[vref2];
                            numTotalTriangles++;
                        }
                    }
                }
                GL.PushMatrix();
                GL.Scale(scale.x, scale.y, scale.z);
                GL.EnableClientState(ArrayCap.NormalArray);
                GL.EnableClientState(ArrayCap.VertexArray);
                GL.NormalPointer(NormalPointerType.Float, 2 * 3 * sizeof(float), gVertexBuffer);
                GL.VertexPointer(3, VertexPointerType.Float, 2 * 3 * sizeof(float), ref gVertexBuffer[1]);
                GL.DrawArrays(PrimitiveType.Triangles, 0, (int)(numTotalTriangles * 3));
                GL.PopMatrix();
            }
            break;

            case PxGeometryType.eTRIANGLEMESH:
            {
                ref PxTriangleMeshGeometry triGeom = ref Unsafe.As<PxGeometry, PxTriangleMeshGeometry>(ref geom);

                ref PxTriangleMesh mesh = ref *triGeom.triangleMesh;
                PxVec3 scale = triGeom.scale.scale;

                uint triangleCount = mesh.getNbTriangles();
                bool has16BitIndices = mesh.getTriangleMeshFlags().HasFlag(PxTriangleMeshFlags.e16_BIT_INDICES);
                void* indexBuffer = mesh.getTriangles();

                PxVec3* vertexBuffer = mesh.getVertices();

                uint* intIndices = (uint*)indexBuffer;
                ushort* shortIndices = (ushort*)indexBuffer;
                uint numTotalTriangles = 0;
                for (uint i = 0; i < triangleCount; ++i)
                {
                    PxVec3 triVert0;
                    PxVec3 triVert1;
                    PxVec3 triVert2;

                    if (has16BitIndices)
                    {
                        triVert0 = vertexBuffer[*shortIndices++];
                        triVert1 = vertexBuffer[*shortIndices++];
                        triVert2 = vertexBuffer[*shortIndices++];
                    }
                    else
                    {
                        triVert0 = vertexBuffer[*intIndices++];
                        triVert1 = vertexBuffer[*intIndices++];
                        triVert2 = vertexBuffer[*intIndices++];
                    }

                    PxVec3 fnormal = (triVert1.operator_Minus(triVert0)).cross(triVert2.operator_Minus(triVert0)); //BIOQUIRK: Overloaded operator
                    fnormal.normalize();

                    if (numTotalTriangles * 6 < MAX_NUM_MESH_VEC3S)
                    {
                        gVertexBuffer[numTotalTriangles * 6 + 0] = fnormal;
                        gVertexBuffer[numTotalTriangles * 6 + 1] = triVert0;
                        gVertexBuffer[numTotalTriangles * 6 + 2] = fnormal;
                        gVertexBuffer[numTotalTriangles * 6 + 3] = triVert1;
                        gVertexBuffer[numTotalTriangles * 6 + 4] = fnormal;
                        gVertexBuffer[numTotalTriangles * 6 + 5] = triVert2;
                        numTotalTriangles++;
                    }
                }
                GL.PushMatrix();
                GL.Scale(scale.x, scale.y, scale.z);
                GL.EnableClientState(ArrayCap.NormalArray);
                GL.EnableClientState(ArrayCap.VertexArray);
                GL.NormalPointer(NormalPointerType.Float, 2 * 3 * sizeof(float), gVertexBuffer);
                GL.VertexPointer(3, VertexPointerType.Float, 2 * 3 * sizeof(float), ref gVertexBuffer[1]);
                GL.DrawArrays(PrimitiveType.Triangles, 0, (int)(numTotalTriangles * 3));
                GL.DisableClientState(ArrayCap.VertexArray);
                GL.DisableClientState(ArrayCap.NormalArray);
                GL.PopMatrix();
            }
            break;

            case PxGeometryType.eINVALID:
            case PxGeometryType.eHEIGHTFIELD:
            case PxGeometryType.eGEOMETRY_COUNT:
            case PxGeometryType.ePLANE:
                break;
        }
    }

    static void renderGeometryHolder(in PxGeometryHolder h)
        => renderGeometry(ref *h.any());

    static void reshapeCallback(int width, int height)
        => GL.Viewport(0, 0, width, height);

    internal static NativeWindow SnippetWindow = null!;
    public static void setupDefaultWindow(string name)
    {
        if (SnippetWindow is not null)
        { throw new InvalidOperationException("The window has already been created."); }

        NativeWindowSettings settings = new()
        {
            Size = new(512, 512),
            Title = name,
            APIVersion = new(1, 1), // Yes the PhysX snippets are really written against OpenGL 1.1 :(
            Profile = ContextProfile.Any,
        };
        SnippetWindow = new NativeWindow(settings);
        SnippetWindow.Context.MakeCurrent();
        SnippetWindow.Resize += e => reshapeCallback(e.Width, e.Height);
    }

    public static void setupDefaultRenderState()
    {
        // Setup default render states
        GL.ClearColor(0.3f, 0.4f, 0.5f, 1.0f);
        GL.Enable(EnableCap.DepthTest);
        GL.Enable(EnableCap.ColorMaterial);

        // Setup lighting
        GL.Enable(EnableCap.Lighting);
        Color4 ambientColor = new(0.0f, 0.1f, 0.2f, 0.0f);
        Color4 diffuseColor = new(1.0f, 1.0f, 1.0f, 0.0f);
        Color4 specularColor = new(0.0f, 0.0f, 0.0f, 0.0f);
        Color4 position = new(100.0f, 100.0f, 400.0f, 1.0f);
        GL.Light(LightName.Light0, LightParameter.Ambient, ambientColor);
        GL.Light(LightName.Light0, LightParameter.Diffuse, diffuseColor);
        GL.Light(LightName.Light0, LightParameter.Specular, specularColor);
        GL.Light(LightName.Light0, LightParameter.Position, position);
        GL.Enable(EnableCap.Light0);
    }

    public static void startRender(in PxVec3 cameraEye, in PxVec3 cameraDir, float clipNear = 1f, float clipFar = 10000f)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        // Setup camera
        GL.MatrixMode(MatrixMode.Projection);
        GL.LoadIdentity();
        Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(60f * (MathF.PI / 180f), ((float)SnippetWindow.ClientSize.X) / ((float)SnippetWindow.ClientSize.Y), clipNear, clipFar);
        GL.MultMatrix(ref projection);

        GL.MatrixMode(MatrixMode.Modelview);
        GL.LoadIdentity();
        Matrix4 lookAt = Matrix4.LookAt
        (
            cameraEye.x, cameraEye.y, cameraEye.z,
            cameraEye.x + cameraDir.x, cameraEye.y + cameraDir.y, cameraEye.z + cameraDir.z,
            0f, 1f, 0f
        );
        GL.MultMatrix(ref lookAt);

        GL.Color4(0.4f, 0.4f, 0.4f, 1.0f);
    }

    public static void finishRender()
        => GLFW.SwapBuffers(SnippetWindow.WindowPtr);

    public static void renderActors(PxRigidActor** actors, uint numActors, bool shadows, in Vector3 color, delegate*<PxShape*, bool> cb = null)
    {
        Vector3 shadowDir = new(0.0f, -0.7071067f, -0.7071067f);
        Matrix4 shadowMat = new(1, 0, 0, 0, -shadowDir.X / shadowDir.Y, 0, -shadowDir.Z / shadowDir.Y, 0, 0, 0, 1, 0, 0, 0, 0, 1);

        PxShape** shapes = stackalloc PxShape*[MAX_NUM_ACTOR_SHAPES];
        for (uint i = 0; i < numActors; i++)
        {
            uint nbShapes = actors[i]->getNbShapes();
            Debug.Assert(nbShapes <= MAX_NUM_ACTOR_SHAPES);
            actors[i]->getShapes(shapes, nbShapes);
            //BIOQUIRK: is<T> is not exposed to C#, which forces us to use GetConcreteType which is a bit awkward.
            // We could probably recreate this in C# if we special-case translation of is<T> and typeMatch<T>.
            // Note that we can almost certianly get away with ignoring the string-based isKindOf fallback, I'm pretty sure it's a legacy artifact.
            //bool sleeping = actors[i]->is<PxRigidDynamic>() ? actors[i]->is<PxRigidDynamic>()->isSleeping() : false;
            bool sleeping;
            if (actors[i]->Base.Base.getConcreteType() == (ushort)PxConcreteType.eRIGID_DYNAMIC)
            { sleeping = ((PxRigidDynamic*)actors[i])->isSleeping(); }
            else
            { sleeping = false; }

            for (uint j = 0; j < nbShapes; j++)
            {
                PxMat44 shapePose = new(PxShapeExt.getGlobalPose(*shapes[j], *actors[i]));
                PxGeometryHolder h = shapes[j]->getGeometry();

                bool isTrigger = cb != null ? cb(shapes[j]) : shapes[j]->getFlags().HasFlag(PxShapeFlags.eTRIGGER_SHAPE);
                if (isTrigger)
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

                // render object
                GL.PushMatrix();
                GL.MultMatrix(&shapePose.column0.x);
                if (sleeping)
                {
                    Vector3 darkColor = color * 0.25f;
                    GL.Color4(darkColor.X, darkColor.Y, darkColor.Z, 1.0f);
                }
                else
                    GL.Color4(color.X, color.Y, color.Z, 1.0f);
                renderGeometryHolder(h);
                GL.PopMatrix();

                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

                if (shadows)
                {
                    GL.PushMatrix();
                    GL.MultMatrix(ref shadowMat);
                    GL.MultMatrix(&shapePose.column0.x);
                    GL.Disable(EnableCap.Lighting);
                    GL.Color4(0.1f, 0.2f, 0.3f, 1.0f);
                    renderGeometryHolder(h);
                    GL.Enable(EnableCap.Lighting);
                    GL.PopMatrix();
                }
            }
        }
    }

    public static void renderActors(PxRigidActor** actors, uint numActors, bool shadows = false, delegate*<PxShape*, bool> cb = null)
        => renderActors(actors, numActors, shadows, new(0.0f, 0.75f, 0.0f), cb);
}
