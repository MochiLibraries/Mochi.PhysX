// <auto-generated>
// This file was automatically generated by Biohazrd and should not be modified by hand!
// </auto-generated>
#nullable enable
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Mochi.PhysX
{
    [StructLayout(LayoutKind.Explicit, Size = 72)]
    public unsafe partial struct PxTriangleMeshDesc
    {
        [FieldOffset(0)] public PxSimpleTriangleMesh Base;

        [FieldOffset(56)] public /* Failed to emit TranslatedNormalField materialIndices: Failed to resolve `Ref resolved by PxTypedStridedData` during emit time. */
        int materialIndices;

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper154", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxTriangleMeshDesc* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxTriangleMeshDesc()
        {
            fixed (PxTriangleMeshDesc* @this = &this)
            { Constructor_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?setToDefault@PxTriangleMeshDesc@physx@@QEAAXXZ", ExactSpelling = true)]
        private static extern void setToDefault_PInvoke(PxTriangleMeshDesc* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void setToDefault()
        {
            fixed (PxTriangleMeshDesc* @this = &this)
            { setToDefault_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?isValid@PxTriangleMeshDesc@physx@@QEBA_NXZ", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool isValid_PInvoke(PxTriangleMeshDesc* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool isValid()
        {
            fixed (PxTriangleMeshDesc* @this = &this)
            { return isValid_PInvoke(@this); }
        }
    }
}