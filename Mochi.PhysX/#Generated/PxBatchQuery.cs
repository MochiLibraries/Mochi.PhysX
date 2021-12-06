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
    public unsafe partial struct PxBatchQuery
    {
        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void execute()
        {
            fixed (PxBatchQuery* @this = &this)
            { VirtualMethodTablePointer->execute(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public delegate* unmanaged[Cdecl]<PxFilterData*, PxFilterData*, void*, uint, PxHitFlags*, PxQueryHitType> getPreFilterShader()
        {
            fixed (PxBatchQuery* @this = &this)
            { return VirtualMethodTablePointer->getPreFilterShader(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public delegate* unmanaged[Cdecl]<PxFilterData*, PxFilterData*, void*, uint, PxQueryHit*, PxQueryHitType> getPostFilterShader()
        {
            fixed (PxBatchQuery* @this = &this)
            { return VirtualMethodTablePointer->getPostFilterShader(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void* getFilterShaderData()
        {
            fixed (PxBatchQuery* @this = &this)
            { return VirtualMethodTablePointer->getFilterShaderData(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint getFilterShaderDataSize()
        {
            fixed (PxBatchQuery* @this = &this)
            { return VirtualMethodTablePointer->getFilterShaderDataSize(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void setUserMemory(PxBatchQueryMemory* arg0)
        {
            fixed (PxBatchQuery* @this = &this)
            { VirtualMethodTablePointer->setUserMemory(@this, arg0); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxBatchQueryMemory* getUserMemory()
        {
            fixed (PxBatchQuery* @this = &this)
            { return VirtualMethodTablePointer->getUserMemory(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void release()
        {
            fixed (PxBatchQuery* @this = &this)
            { VirtualMethodTablePointer->release(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void raycast(PxVec3* origin, PxVec3* unitDir, float distance, ushort maxTouchHits, PxHitFlags* hitFlags, PxQueryFilterData* filterData, void* userData = null, PxQueryCache* cache = null)
        {
            fixed (PxBatchQuery* @this = &this)
            { VirtualMethodTablePointer->raycast(@this, origin, unitDir, distance, maxTouchHits, hitFlags, filterData, userData, cache); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void overlap(PxGeometry* geometry, PxTransform* pose, ushort maxTouchHits, PxQueryFilterData* filterData, void* userData = null, PxQueryCache* cache = null)
        {
            fixed (PxBatchQuery* @this = &this)
            { VirtualMethodTablePointer->overlap(@this, geometry, pose, maxTouchHits, filterData, userData, cache); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void sweep(PxGeometry* geometry, PxTransform* pose, PxVec3* unitDir, float distance, ushort maxTouchHits, PxHitFlags* hitFlags, PxQueryFilterData* filterData, void* userData = null, PxQueryCache* cache = null, float inflation = 0f)
        {
            fixed (PxBatchQuery* @this = &this)
            { VirtualMethodTablePointer->sweep(@this, geometry, pose, unitDir, distance, maxTouchHits, hitFlags, filterData, userData, cache, inflation); }
        }

        [FieldOffset(0)] public VirtualMethodTable* VirtualMethodTablePointer;

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct VirtualMethodTable
        {
            /// <summary>Virtual method pointer for `execute`</summary>
            public delegate* unmanaged[Cdecl]<PxBatchQuery*, void> execute;
            /// <summary>Virtual method pointer for `getPreFilterShader`</summary>
            public delegate* unmanaged[Cdecl]<PxBatchQuery*, delegate* unmanaged[Cdecl]<PxFilterData*, PxFilterData*, void*, uint, PxHitFlags*, PxQueryHitType>> getPreFilterShader;
            /// <summary>Virtual method pointer for `getPostFilterShader`</summary>
            public delegate* unmanaged[Cdecl]<PxBatchQuery*, delegate* unmanaged[Cdecl]<PxFilterData*, PxFilterData*, void*, uint, PxQueryHit*, PxQueryHitType>> getPostFilterShader;
            /// <summary>Virtual method pointer for `getFilterShaderData`</summary>
            public delegate* unmanaged[Cdecl]<PxBatchQuery*, void*> getFilterShaderData;
            /// <summary>Virtual method pointer for `getFilterShaderDataSize`</summary>
            public delegate* unmanaged[Cdecl]<PxBatchQuery*, uint> getFilterShaderDataSize;
            /// <summary>Virtual method pointer for `setUserMemory`</summary>
            public delegate* unmanaged[Cdecl]<PxBatchQuery*, PxBatchQueryMemory*, void> setUserMemory;
            /// <summary>Virtual method pointer for `getUserMemory`</summary>
            public delegate* unmanaged[Cdecl]<PxBatchQuery*, PxBatchQueryMemory*> getUserMemory;
            /// <summary>Virtual method pointer for `release`</summary>
            public delegate* unmanaged[Cdecl]<PxBatchQuery*, void> release;
            /// <summary>Virtual method pointer for `raycast`</summary>
            public delegate* unmanaged[Cdecl]<PxBatchQuery*, PxVec3*, PxVec3*, float, ushort, PxHitFlags*, PxQueryFilterData*, void*, PxQueryCache*, void> raycast;
            /// <summary>Virtual method pointer for `overlap`</summary>
            public delegate* unmanaged[Cdecl]<PxBatchQuery*, PxGeometry*, PxTransform*, ushort, PxQueryFilterData*, void*, PxQueryCache*, void> overlap;
            /// <summary>Virtual method pointer for `sweep`</summary>
            public delegate* unmanaged[Cdecl]<PxBatchQuery*, PxGeometry*, PxTransform*, PxVec3*, float, ushort, PxHitFlags*, PxQueryFilterData*, void*, PxQueryCache*, float, void> sweep;
            /// <summary>Virtual method pointer for `~PxBatchQuery`</summary>
            public void* __DeletingDestructorPointer;
        }
    }
}