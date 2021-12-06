// <auto-generated>
// This file was automatically generated by Biohazrd and should not be modified by hand!
// </auto-generated>
#nullable enable
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Mochi.PhysX
{
    [StructLayout(LayoutKind.Explicit, Size = 56)]
    public unsafe partial struct PxObstacle
    {
        [FieldOffset(0)] public PxGeometryType mType;

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getType@PxObstacle@physx@@QEBA?AW4Enum@PxGeometryType@2@XZ", ExactSpelling = true)]
        private static extern PxGeometryType getType_PInvoke(PxObstacle* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxGeometryType getType()
        {
            fixed (PxObstacle* @this = &this)
            { return getType_PInvoke(@this); }
        }

        [FieldOffset(8)] public void* mUserData;

        [FieldOffset(16)] public PxExtendedVec3 mPos;

        [FieldOffset(40)] public PxQuat mRot;
    }
}