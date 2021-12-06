// <auto-generated>
// This file was automatically generated by Biohazrd and should not be modified by hand!
// </auto-generated>
#nullable enable
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Mochi.PhysX
{
    [StructLayout(LayoutKind.Explicit, Size = 48)]
    public unsafe partial struct PxCookingParams
    {
        [FieldOffset(0)] public float areaTestEpsilon;

        [FieldOffset(4)] public float planeTolerance;

        [FieldOffset(8)] public PxConvexMeshCookingType convexMeshCookingType;

        [FieldOffset(12)] [MarshalAs(UnmanagedType.I1)] public bool suppressTriangleMeshRemapTable;

        [FieldOffset(13)] [MarshalAs(UnmanagedType.I1)] public bool buildTriangleAdjacencies;

        [FieldOffset(14)] [MarshalAs(UnmanagedType.I1)] public bool buildGPUData;

        [FieldOffset(16)] public PxTolerancesScale scale;

        [FieldOffset(24)] public PxMeshPreprocessingFlags meshPreprocessParams;

        [FieldOffset(28)] public float meshWeldTolerance;

        [FieldOffset(32)] public PxMidphaseDesc midphaseDesc;

        [FieldOffset(44)] public uint gaussMapLimit;

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper157", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxCookingParams* @this, PxTolerancesScale* sc);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxCookingParams(PxTolerancesScale* sc)
        {
            fixed (PxCookingParams* @this = &this)
            { Constructor_PInvoke(@this, sc); }
        }
    }
}