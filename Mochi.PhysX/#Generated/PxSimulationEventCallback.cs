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
    public unsafe partial struct PxSimulationEventCallback
    {
        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void onConstraintBreak(PxConstraintInfo* constraints, uint count)
        {
            fixed (PxSimulationEventCallback* @this = &this)
            { VirtualMethodTablePointer->onConstraintBreak(@this, constraints, count); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void onWake(PxActor** actors, uint count)
        {
            fixed (PxSimulationEventCallback* @this = &this)
            { VirtualMethodTablePointer->onWake(@this, actors, count); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void onSleep(PxActor** actors, uint count)
        {
            fixed (PxSimulationEventCallback* @this = &this)
            { VirtualMethodTablePointer->onSleep(@this, actors, count); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void onContact(PxContactPairHeader* pairHeader, PxContactPair* pairs, uint nbPairs)
        {
            fixed (PxSimulationEventCallback* @this = &this)
            { VirtualMethodTablePointer->onContact(@this, pairHeader, pairs, nbPairs); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void onTrigger(PxTriggerPair* pairs, uint count)
        {
            fixed (PxSimulationEventCallback* @this = &this)
            { VirtualMethodTablePointer->onTrigger(@this, pairs, count); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void onAdvance(PxRigidBody** bodyBuffer, PxTransform* poseBuffer, uint count)
        {
            fixed (PxSimulationEventCallback* @this = &this)
            { VirtualMethodTablePointer->onAdvance(@this, bodyBuffer, poseBuffer, count); }
        }

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destructor()
        {
            fixed (PxSimulationEventCallback* @this = &this)
            { VirtualMethodTablePointer->__DeletingDestructorPointer(@this); }
        }

        [FieldOffset(0)] public VirtualMethodTable* VirtualMethodTablePointer;

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct VirtualMethodTable
        {
            /// <summary>Virtual method pointer for `onConstraintBreak`</summary>
            public delegate* unmanaged[Cdecl]<PxSimulationEventCallback*, PxConstraintInfo*, uint, void> onConstraintBreak;
            /// <summary>Virtual method pointer for `onWake`</summary>
            public delegate* unmanaged[Cdecl]<PxSimulationEventCallback*, PxActor**, uint, void> onWake;
            /// <summary>Virtual method pointer for `onSleep`</summary>
            public delegate* unmanaged[Cdecl]<PxSimulationEventCallback*, PxActor**, uint, void> onSleep;
            /// <summary>Virtual method pointer for `onContact`</summary>
            public delegate* unmanaged[Cdecl]<PxSimulationEventCallback*, PxContactPairHeader*, PxContactPair*, uint, void> onContact;
            /// <summary>Virtual method pointer for `onTrigger`</summary>
            public delegate* unmanaged[Cdecl]<PxSimulationEventCallback*, PxTriggerPair*, uint, void> onTrigger;
            /// <summary>Virtual method pointer for `onAdvance`</summary>
            public delegate* unmanaged[Cdecl]<PxSimulationEventCallback*, PxRigidBody**, PxTransform*, uint, void> onAdvance;
            /// <summary>Virtual method pointer for `~PxSimulationEventCallback`</summary>
            public delegate* unmanaged[Cdecl]<PxSimulationEventCallback*, void> __DeletingDestructorPointer;
        }
    }
}