// <auto-generated>
// This file was automatically generated by Biohazrd and should not be modified by hand!
// </auto-generated>
#nullable enable
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Mochi.PhysX
{
    [StructLayout(LayoutKind.Explicit, Size = 28)]
    public unsafe partial struct PxMeshScale
    {
        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper87", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxMeshScale* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxMeshScale()
        {
            fixed (PxMeshScale* @this = &this)
            { Constructor_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper88", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxMeshScale* @this, float r);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxMeshScale(float r)
        {
            fixed (PxMeshScale* @this = &this)
            { Constructor_PInvoke(@this, r); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper89", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxMeshScale* @this, PxVec3* s);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxMeshScale(PxVec3* s)
        {
            fixed (PxMeshScale* @this = &this)
            { Constructor_PInvoke(@this, s); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper90", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxMeshScale* @this, PxVec3* s, PxQuat* r);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxMeshScale(PxVec3* s, PxQuat* r)
        {
            fixed (PxMeshScale* @this = &this)
            { Constructor_PInvoke(@this, s, r); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?isIdentity@PxMeshScale@physx@@QEBA_NXZ", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool isIdentity_PInvoke(PxMeshScale* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool isIdentity()
        {
            fixed (PxMeshScale* @this = &this)
            { return isIdentity_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getInverse@PxMeshScale@physx@@QEBA?AV12@XZ", ExactSpelling = true)]
        private static extern PxMeshScale* getInverse_PInvoke(PxMeshScale* @this, out PxMeshScale __returnBuffer);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxMeshScale getInverse()
        {
            fixed (PxMeshScale* @this = &this)
            {
                PxMeshScale __returnBuffer;
                getInverse_PInvoke(@this, out __returnBuffer);
                return __returnBuffer;
            }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?toMat33@PxMeshScale@physx@@QEBA?AVPxMat33@2@XZ", ExactSpelling = true)]
        private static extern PxMat33* toMat33_PInvoke(PxMeshScale* @this, out PxMat33 __returnBuffer);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxMat33 toMat33()
        {
            fixed (PxMeshScale* @this = &this)
            {
                PxMat33 __returnBuffer;
                toMat33_PInvoke(@this, out __returnBuffer);
                return __returnBuffer;
            }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?hasNegativeDeterminant@PxMeshScale@physx@@QEBA_NXZ", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool hasNegativeDeterminant_PInvoke(PxMeshScale* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool hasNegativeDeterminant()
        {
            fixed (PxMeshScale* @this = &this)
            { return hasNegativeDeterminant_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?transform@PxMeshScale@physx@@QEBA?AVPxVec3@2@AEBV32@@Z", ExactSpelling = true)]
        private static extern PxVec3* transform_PInvoke(PxMeshScale* @this, out PxVec3 __returnBuffer, PxVec3* v);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxVec3 transform(PxVec3* v)
        {
            fixed (PxMeshScale* @this = &this)
            {
                PxVec3 __returnBuffer;
                transform_PInvoke(@this, out __returnBuffer, v);
                return __returnBuffer;
            }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?isValidForTriangleMesh@PxMeshScale@physx@@QEBA_NXZ", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool isValidForTriangleMesh_PInvoke(PxMeshScale* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool isValidForTriangleMesh()
        {
            fixed (PxMeshScale* @this = &this)
            { return isValidForTriangleMesh_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?isValidForConvexMesh@PxMeshScale@physx@@QEBA_NXZ", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool isValidForConvexMesh_PInvoke(PxMeshScale* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool isValidForConvexMesh()
        {
            fixed (PxMeshScale* @this = &this)
            { return isValidForConvexMesh_PInvoke(@this); }
        }

        [FieldOffset(0)] public PxVec3 scale;

        [FieldOffset(12)] public PxQuat rotation;
    }
}
