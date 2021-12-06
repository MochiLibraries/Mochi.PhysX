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
    public unsafe partial struct PxCudaMemoryManager
    {
        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxCudaBuffer* alloc(PxCudaBufferType* type, ulong size, byte* file = null, int line = 0, byte* allocName = null, PxAllocId allocId = PxAllocId.UNASSIGNED)
        {
            fixed (PxCudaMemoryManager* @this = &this)
            { return VirtualMethodTablePointer->alloc_1(@this, type, size, file, line, allocName, allocId); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong alloc(PxCudaBufferMemorySpace memorySpace, ulong size, byte* file = null, int line = 0, byte* allocName = null, PxAllocId allocId = PxAllocId.UNASSIGNED)
        {
            fixed (PxCudaMemoryManager* @this = &this)
            { return VirtualMethodTablePointer->alloc_0(@this, memorySpace, size, file, line, allocName, allocId); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool free(PxCudaBufferMemorySpace memorySpace, ulong addr)
        {
            fixed (PxCudaMemoryManager* @this = &this)
            { return VirtualMethodTablePointer->free(@this, memorySpace, addr); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool realloc(PxCudaBufferMemorySpace memorySpace, ulong addr, ulong size, byte* file = null, int line = 0, byte* allocName = null, PxAllocId allocId = PxAllocId.UNASSIGNED)
        {
            fixed (PxCudaMemoryManager* @this = &this)
            { return VirtualMethodTablePointer->realloc(@this, memorySpace, addr, size, file, line, allocName, allocId); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void getStats(PxCudaBufferType* type, PxCudaMemoryManagerStats* outStats)
        {
            fixed (PxCudaMemoryManager* @this = &this)
            { VirtualMethodTablePointer->getStats(@this, type, outStats); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool reserve(PxCudaBufferType* type, ulong size)
        {
            fixed (PxCudaMemoryManager* @this = &this)
            { return VirtualMethodTablePointer->reserve(@this, type, size); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool setPageSize(PxCudaBufferType* type, ulong size)
        {
            fixed (PxCudaMemoryManager* @this = &this)
            { return VirtualMethodTablePointer->setPageSize(@this, type, size); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool setMaxMemorySize(PxCudaBufferType* type, ulong size)
        {
            fixed (PxCudaMemoryManager* @this = &this)
            { return VirtualMethodTablePointer->setMaxMemorySize(@this, type, size); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong getBaseSize(PxCudaBufferType* type)
        {
            fixed (PxCudaMemoryManager* @this = &this)
            { return VirtualMethodTablePointer->getBaseSize(@this, type); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong getPageSize(PxCudaBufferType* type)
        {
            fixed (PxCudaMemoryManager* @this = &this)
            { return VirtualMethodTablePointer->getPageSize(@this, type); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong getMaxMemorySize(PxCudaBufferType* type)
        {
            fixed (PxCudaMemoryManager* @this = &this)
            { return VirtualMethodTablePointer->getMaxMemorySize(@this, type); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong getMappedPinnedPtr(ulong hostPtr)
        {
            fixed (PxCudaMemoryManager* @this = &this)
            { return VirtualMethodTablePointer->getMappedPinnedPtr(@this, hostPtr); }
        }

        [FieldOffset(0)] public VirtualMethodTable* VirtualMethodTablePointer;

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct VirtualMethodTable
        {
            /// <summary>Virtual method pointer for `alloc`</summary>
            public delegate* unmanaged[Cdecl]<PxCudaMemoryManager*, PxCudaBufferMemorySpace, ulong, byte*, int, byte*, PxAllocId, ulong> alloc_0;
            /// <summary>Virtual method pointer for `alloc`</summary>
            public delegate* unmanaged[Cdecl]<PxCudaMemoryManager*, PxCudaBufferType*, ulong, byte*, int, byte*, PxAllocId, PxCudaBuffer*> alloc_1;
            /// <summary>Virtual method pointer for `free`</summary>
            public delegate* unmanaged[Cdecl]<PxCudaMemoryManager*, PxCudaBufferMemorySpace, ulong, NativeBoolean> free;
            /// <summary>Virtual method pointer for `realloc`</summary>
            public delegate* unmanaged[Cdecl]<PxCudaMemoryManager*, PxCudaBufferMemorySpace, ulong, ulong, byte*, int, byte*, PxAllocId, NativeBoolean> realloc;
            /// <summary>Virtual method pointer for `getStats`</summary>
            public delegate* unmanaged[Cdecl]<PxCudaMemoryManager*, PxCudaBufferType*, PxCudaMemoryManagerStats*, void> getStats;
            /// <summary>Virtual method pointer for `reserve`</summary>
            public delegate* unmanaged[Cdecl]<PxCudaMemoryManager*, PxCudaBufferType*, ulong, NativeBoolean> reserve;
            /// <summary>Virtual method pointer for `setPageSize`</summary>
            public delegate* unmanaged[Cdecl]<PxCudaMemoryManager*, PxCudaBufferType*, ulong, NativeBoolean> setPageSize;
            /// <summary>Virtual method pointer for `setMaxMemorySize`</summary>
            public delegate* unmanaged[Cdecl]<PxCudaMemoryManager*, PxCudaBufferType*, ulong, NativeBoolean> setMaxMemorySize;
            /// <summary>Virtual method pointer for `getBaseSize`</summary>
            public delegate* unmanaged[Cdecl]<PxCudaMemoryManager*, PxCudaBufferType*, ulong> getBaseSize;
            /// <summary>Virtual method pointer for `getPageSize`</summary>
            public delegate* unmanaged[Cdecl]<PxCudaMemoryManager*, PxCudaBufferType*, ulong> getPageSize;
            /// <summary>Virtual method pointer for `getMaxMemorySize`</summary>
            public delegate* unmanaged[Cdecl]<PxCudaMemoryManager*, PxCudaBufferType*, ulong> getMaxMemorySize;
            /// <summary>Virtual method pointer for `getMappedPinnedPtr`</summary>
            public delegate* unmanaged[Cdecl]<PxCudaMemoryManager*, ulong, ulong> getMappedPinnedPtr;
            /// <summary>Virtual method pointer for `~PxCudaMemoryManager`</summary>
            public void* __DeletingDestructorPointer;
        }
    }
}