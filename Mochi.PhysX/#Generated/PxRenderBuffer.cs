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
    public unsafe partial struct PxRenderBuffer
    {
        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destructor()
        {
            fixed (PxRenderBuffer* @this = &this)
            { VirtualMethodTablePointer->__DeletingDestructorPointer(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint getNbPoints()
        {
            fixed (PxRenderBuffer* @this = &this)
            { return VirtualMethodTablePointer->getNbPoints(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxDebugPoint* getPoints()
        {
            fixed (PxRenderBuffer* @this = &this)
            { return VirtualMethodTablePointer->getPoints(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint getNbLines()
        {
            fixed (PxRenderBuffer* @this = &this)
            { return VirtualMethodTablePointer->getNbLines(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxDebugLine* getLines()
        {
            fixed (PxRenderBuffer* @this = &this)
            { return VirtualMethodTablePointer->getLines(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint getNbTriangles()
        {
            fixed (PxRenderBuffer* @this = &this)
            { return VirtualMethodTablePointer->getNbTriangles(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxDebugTriangle* getTriangles()
        {
            fixed (PxRenderBuffer* @this = &this)
            { return VirtualMethodTablePointer->getTriangles(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint getNbTexts()
        {
            fixed (PxRenderBuffer* @this = &this)
            { return VirtualMethodTablePointer->getNbTexts(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxDebugText* getTexts()
        {
            fixed (PxRenderBuffer* @this = &this)
            { return VirtualMethodTablePointer->getTexts(@this); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void append(PxRenderBuffer* other)
        {
            fixed (PxRenderBuffer* @this = &this)
            { VirtualMethodTablePointer->append(@this, other); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void clear()
        {
            fixed (PxRenderBuffer* @this = &this)
            { VirtualMethodTablePointer->clear(@this); }
        }

        [FieldOffset(0)] public VirtualMethodTable* VirtualMethodTablePointer;

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct VirtualMethodTable
        {
            /// <summary>Virtual method pointer for `~PxRenderBuffer`</summary>
            public delegate* unmanaged[Cdecl]<PxRenderBuffer*, void> __DeletingDestructorPointer;
            /// <summary>Virtual method pointer for `getNbPoints`</summary>
            public delegate* unmanaged[Cdecl]<PxRenderBuffer*, uint> getNbPoints;
            /// <summary>Virtual method pointer for `getPoints`</summary>
            public delegate* unmanaged[Cdecl]<PxRenderBuffer*, PxDebugPoint*> getPoints;
            /// <summary>Virtual method pointer for `getNbLines`</summary>
            public delegate* unmanaged[Cdecl]<PxRenderBuffer*, uint> getNbLines;
            /// <summary>Virtual method pointer for `getLines`</summary>
            public delegate* unmanaged[Cdecl]<PxRenderBuffer*, PxDebugLine*> getLines;
            /// <summary>Virtual method pointer for `getNbTriangles`</summary>
            public delegate* unmanaged[Cdecl]<PxRenderBuffer*, uint> getNbTriangles;
            /// <summary>Virtual method pointer for `getTriangles`</summary>
            public delegate* unmanaged[Cdecl]<PxRenderBuffer*, PxDebugTriangle*> getTriangles;
            /// <summary>Virtual method pointer for `getNbTexts`</summary>
            public delegate* unmanaged[Cdecl]<PxRenderBuffer*, uint> getNbTexts;
            /// <summary>Virtual method pointer for `getTexts`</summary>
            public delegate* unmanaged[Cdecl]<PxRenderBuffer*, PxDebugText*> getTexts;
            /// <summary>Virtual method pointer for `append`</summary>
            public delegate* unmanaged[Cdecl]<PxRenderBuffer*, PxRenderBuffer*, void> append;
            /// <summary>Virtual method pointer for `clear`</summary>
            public delegate* unmanaged[Cdecl]<PxRenderBuffer*, void> clear;
        }
    }
}