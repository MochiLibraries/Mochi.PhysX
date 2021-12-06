// <auto-generated>
// This file was automatically generated by Biohazrd and should not be modified by hand!
// </auto-generated>
#nullable enable
using Mochi.PhysX.Infrastructure;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Mochi.PhysX
{
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public unsafe partial struct PxConstraintVisualizer
    {
        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void visualizeJointFrames(PxTransform* parent, PxTransform* child)
        {
            fixed (PxConstraintVisualizer* @this = &this)
            { VirtualMethodTablePointer->visualizeJointFrames(@this, parent, child); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void visualizeLinearLimit(PxTransform* t0, PxTransform* t1, float value, bool active)
        {
            fixed (PxConstraintVisualizer* @this = &this)
            { VirtualMethodTablePointer->visualizeLinearLimit(@this, t0, t1, value, active); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void visualizeAngularLimit(PxTransform* t0, float lower, float upper, bool active)
        {
            fixed (PxConstraintVisualizer* @this = &this)
            { VirtualMethodTablePointer->visualizeAngularLimit(@this, t0, lower, upper, active); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void visualizeLimitCone(PxTransform* t, float tanQSwingY, float tanQSwingZ, bool active)
        {
            fixed (PxConstraintVisualizer* @this = &this)
            { VirtualMethodTablePointer->visualizeLimitCone(@this, t, tanQSwingY, tanQSwingZ, active); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void visualizeDoubleCone(PxTransform* t, float angle, bool active)
        {
            fixed (PxConstraintVisualizer* @this = &this)
            { VirtualMethodTablePointer->visualizeDoubleCone(@this, t, angle, active); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void visualizeLine(PxVec3* p0, PxVec3* p1, uint color)
        {
            fixed (PxConstraintVisualizer* @this = &this)
            { VirtualMethodTablePointer->visualizeLine(@this, p0, p1, color); }
        }

        [FieldOffset(0)] public VirtualMethodTable* VirtualMethodTablePointer;

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct VirtualMethodTable
        {
            /// <summary>Virtual method pointer for `~PxConstraintVisualizer`</summary>
            public void* __DeletingDestructorPointer;
            /// <summary>Virtual method pointer for `visualizeJointFrames`</summary>
            public delegate* unmanaged[Cdecl]<PxConstraintVisualizer*, PxTransform*, PxTransform*, void> visualizeJointFrames;
            /// <summary>Virtual method pointer for `visualizeLinearLimit`</summary>
            public delegate* unmanaged[Cdecl]<PxConstraintVisualizer*, PxTransform*, PxTransform*, float, NativeBoolean, void> visualizeLinearLimit;
            /// <summary>Virtual method pointer for `visualizeAngularLimit`</summary>
            public delegate* unmanaged[Cdecl]<PxConstraintVisualizer*, PxTransform*, float, float, NativeBoolean, void> visualizeAngularLimit;
            /// <summary>Virtual method pointer for `visualizeLimitCone`</summary>
            public delegate* unmanaged[Cdecl]<PxConstraintVisualizer*, PxTransform*, float, float, NativeBoolean, void> visualizeLimitCone;
            /// <summary>Virtual method pointer for `visualizeDoubleCone`</summary>
            public delegate* unmanaged[Cdecl]<PxConstraintVisualizer*, PxTransform*, float, NativeBoolean, void> visualizeDoubleCone;
            /// <summary>Virtual method pointer for `visualizeLine`</summary>
            public delegate* unmanaged[Cdecl]<PxConstraintVisualizer*, PxVec3*, PxVec3*, uint, void> visualizeLine;
        }
    }
}