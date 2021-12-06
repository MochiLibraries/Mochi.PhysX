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
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public unsafe partial struct PxTriangleMesh
    {
        [FieldOffset(0)] public PxBase Base;

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint getNbVertices()
        {
            fixed (PxTriangleMesh* @this = &this)
            { return VirtualMethodTablePointer->getNbVertices(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxVec3* getVertices()
        {
            fixed (PxTriangleMesh* @this = &this)
            { return VirtualMethodTablePointer->getVertices(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxVec3* getVerticesForModification()
        {
            fixed (PxTriangleMesh* @this = &this)
            { return VirtualMethodTablePointer->getVerticesForModification(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxBounds3 refitBVH()
        {
            fixed (PxTriangleMesh* @this = &this)
            {
                PxBounds3 __returnBuffer;
                VirtualMethodTablePointer->refitBVH(@this, &__returnBuffer);
                return __returnBuffer;
            }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint getNbTriangles()
        {
            fixed (PxTriangleMesh* @this = &this)
            { return VirtualMethodTablePointer->getNbTriangles(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void* getTriangles()
        {
            fixed (PxTriangleMesh* @this = &this)
            { return VirtualMethodTablePointer->getTriangles(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxTriangleMeshFlags getTriangleMeshFlags()
        {
            fixed (PxTriangleMesh* @this = &this)
            {
                PxTriangleMeshFlags __returnBuffer;
                VirtualMethodTablePointer->getTriangleMeshFlags(@this, &__returnBuffer);
                return __returnBuffer;
            }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint* getTrianglesRemap()
        {
            fixed (PxTriangleMesh* @this = &this)
            { return VirtualMethodTablePointer->getTrianglesRemap(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void release()
        {
            fixed (PxTriangleMesh* @this = &this)
            { VirtualMethodTablePointer->release(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ushort getTriangleMaterialIndex(uint triangleIndex)
        {
            fixed (PxTriangleMesh* @this = &this)
            { return VirtualMethodTablePointer->getTriangleMaterialIndex(@this, triangleIndex); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxBounds3 getLocalBounds()
        {
            fixed (PxTriangleMesh* @this = &this)
            {
                PxBounds3 __returnBuffer;
                VirtualMethodTablePointer->getLocalBounds(@this, &__returnBuffer);
                return __returnBuffer;
            }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint getReferenceCount()
        {
            fixed (PxTriangleMesh* @this = &this)
            { return VirtualMethodTablePointer->getReferenceCount(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void acquireReference()
        {
            fixed (PxTriangleMesh* @this = &this)
            { VirtualMethodTablePointer->acquireReference(@this); }
        }

        [FieldOffset(0)] public VirtualMethodTable* VirtualMethodTablePointer;

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct VirtualMethodTable
        {
            /// <summary>Virtual method pointer for `release`</summary>
            public delegate* unmanaged[Cdecl]<PxTriangleMesh*, void> release;
            /// <summary>Virtual method pointer for `getConcreteTypeName`</summary>
            public delegate* unmanaged[Cdecl]<PxTriangleMesh*, byte*> getConcreteTypeName;
            /// <summary>Virtual method pointer for `isReleasable`</summary>
            public delegate* unmanaged[Cdecl]<PxTriangleMesh*, NativeBoolean> isReleasable;
            /// <summary>Virtual method pointer for `~PxTriangleMesh`</summary>
            public void* __DeletingDestructorPointer;
            /// <summary>Virtual method pointer for `isKindOf`</summary>
            public void* isKindOf;
            /// <summary>Virtual method pointer for `getNbVertices`</summary>
            public delegate* unmanaged[Cdecl]<PxTriangleMesh*, uint> getNbVertices;
            /// <summary>Virtual method pointer for `getVertices`</summary>
            public delegate* unmanaged[Cdecl]<PxTriangleMesh*, PxVec3*> getVertices;
            /// <summary>Virtual method pointer for `getVerticesForModification`</summary>
            public delegate* unmanaged[Cdecl]<PxTriangleMesh*, PxVec3*> getVerticesForModification;
            /// <summary>Virtual method pointer for `refitBVH`</summary>
            public delegate* unmanaged[Cdecl]<PxTriangleMesh*, PxBounds3*, PxBounds3*> refitBVH;
            /// <summary>Virtual method pointer for `getNbTriangles`</summary>
            public delegate* unmanaged[Cdecl]<PxTriangleMesh*, uint> getNbTriangles;
            /// <summary>Virtual method pointer for `getTriangles`</summary>
            public delegate* unmanaged[Cdecl]<PxTriangleMesh*, void*> getTriangles;
            /// <summary>Virtual method pointer for `getTriangleMeshFlags`</summary>
            public delegate* unmanaged[Cdecl]<PxTriangleMesh*, PxTriangleMeshFlags*, PxTriangleMeshFlags*> getTriangleMeshFlags;
            /// <summary>Virtual method pointer for `getTrianglesRemap`</summary>
            public delegate* unmanaged[Cdecl]<PxTriangleMesh*, uint*> getTrianglesRemap;
            /// <summary>Virtual method pointer for `getTriangleMaterialIndex`</summary>
            public delegate* unmanaged[Cdecl]<PxTriangleMesh*, uint, ushort> getTriangleMaterialIndex;
            /// <summary>Virtual method pointer for `getLocalBounds`</summary>
            public delegate* unmanaged[Cdecl]<PxTriangleMesh*, PxBounds3*, PxBounds3*> getLocalBounds;
            /// <summary>Virtual method pointer for `getReferenceCount`</summary>
            public delegate* unmanaged[Cdecl]<PxTriangleMesh*, uint> getReferenceCount;
            /// <summary>Virtual method pointer for `acquireReference`</summary>
            public delegate* unmanaged[Cdecl]<PxTriangleMesh*, void> acquireReference;
        }
    }
}