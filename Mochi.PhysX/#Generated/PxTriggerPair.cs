// <auto-generated>
// This file was automatically generated by Biohazrd and should not be modified by hand!
// </auto-generated>
#nullable enable
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Mochi.PhysX
{
    [StructLayout(LayoutKind.Explicit, Size = 40)]
    public unsafe partial struct PxTriggerPair
    {
        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper141", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxTriggerPair* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxTriggerPair()
        {
            fixed (PxTriggerPair* @this = &this)
            { Constructor_PInvoke(@this); }
        }

        [FieldOffset(0)] public PxShape* triggerShape;

        [FieldOffset(8)] public PxRigidActor* triggerActor;

        [FieldOffset(16)] public PxShape* otherShape;

        [FieldOffset(24)] public PxRigidActor* otherActor;

        [FieldOffset(32)] public PxPairFlags status;

        [FieldOffset(36)] public PxTriggerPairFlags flags;
    }
}