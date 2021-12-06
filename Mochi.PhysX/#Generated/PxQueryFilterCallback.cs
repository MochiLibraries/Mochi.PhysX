// <auto-generated>
// This file was automatically generated by Biohazrd and should not be modified by hand!
// </auto-generated>
#nullable enable
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Mochi.PhysX
{
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public unsafe partial struct PxQueryFilterCallback
    {
        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxQueryHitType preFilter(PxFilterData* filterData, PxShape* shape, PxRigidActor* actor, PxHitFlags* queryFlags)
        {
            fixed (PxQueryFilterCallback* @this = &this)
            { return VirtualMethodTablePointer->preFilter(@this, filterData, shape, actor, queryFlags); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxQueryHitType postFilter(PxFilterData* filterData, PxQueryHit* hit)
        {
            fixed (PxQueryFilterCallback* @this = &this)
            { return VirtualMethodTablePointer->postFilter(@this, filterData, hit); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destructor()
        {
            fixed (PxQueryFilterCallback* @this = &this)
            { VirtualMethodTablePointer->__DeletingDestructorPointer(@this); }
        }

        [FieldOffset(0)] public VirtualMethodTable* VirtualMethodTablePointer;

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct VirtualMethodTable
        {
            /// <summary>Virtual method pointer for `preFilter`</summary>
            public delegate* unmanaged[Cdecl]<PxQueryFilterCallback*, PxFilterData*, PxShape*, PxRigidActor*, PxHitFlags*, PxQueryHitType> preFilter;
            /// <summary>Virtual method pointer for `postFilter`</summary>
            public delegate* unmanaged[Cdecl]<PxQueryFilterCallback*, PxFilterData*, PxQueryHit*, PxQueryHitType> postFilter;
            /// <summary>Virtual method pointer for `~PxQueryFilterCallback`</summary>
            public delegate* unmanaged[Cdecl]<PxQueryFilterCallback*, void> __DeletingDestructorPointer;
        }
    }
}