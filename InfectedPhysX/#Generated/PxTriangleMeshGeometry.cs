// This file was automatically generated by Biohazrd and should not be modified by hand!
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit, Size = 48)]
public unsafe partial struct PxTriangleMeshGeometry
{
    [FieldOffset(0)] public PxGeometry Base;

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??0PxTriangleMeshGeometry@physx@@QEAA@XZ", ExactSpelling = true)]
    private static extern void Constructor_PInvoke(PxTriangleMeshGeometry* @this);

    public unsafe void Constructor()
    {
        fixed (PxTriangleMeshGeometry* @this = &this)
        { Constructor_PInvoke(@this); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??0PxTriangleMeshGeometry@physx@@QEAA@PEAVPxTriangleMesh@1@AEBVPxMeshScale@1@V?$PxFlags@W4Enum@PxMeshGeometryFlag@physx@@E@1@@Z", ExactSpelling = true)]
    private static extern void Constructor_PInvoke(PxTriangleMeshGeometry* @this, PxTriangleMesh* mesh, PxMeshScale* scaling, PxMeshGeometryFlags flags);

    public unsafe void Constructor(PxTriangleMesh* mesh, PxMeshScale* scaling, PxMeshGeometryFlags flags)
    {
        fixed (PxTriangleMeshGeometry* @this = &this)
        { Constructor_PInvoke(@this, mesh, scaling, flags); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?isValid@PxTriangleMeshGeometry@physx@@QEBA_NXZ", ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.I1)]
    private static extern bool isValid_PInvoke(PxTriangleMeshGeometry* @this);

    public unsafe bool isValid()
    {
        fixed (PxTriangleMeshGeometry* @this = &this)
        { return isValid_PInvoke(@this); }
    }

    [FieldOffset(4)] public PxMeshScale scale;

    [FieldOffset(32)] public PxMeshGeometryFlags meshFlags;

    [FieldOffset(40)] public PxTriangleMesh* triangleMesh;
}