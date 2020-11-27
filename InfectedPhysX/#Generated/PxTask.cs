// This file was automatically generated by Biohazrd and should not be modified by hand!
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit, Size = 32)]
public unsafe partial struct PxTask
{
    [FieldOffset(0)] public PxBaseTask Base;

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??0PxTask@physx@@QEAA@XZ", ExactSpelling = true)]
    private static extern void Constructor_PInvoke(PxTask* @this);

    public unsafe void Constructor()
    {
        fixed (PxTask* @this = &this)
        { Constructor_PInvoke(@this); }
    }

    public unsafe void Destructor()
    {
        fixed (PxTask* @this = &this)
        { VirtualMethodTablePointer->__DeletingDestructorPointer(@this); }
    }

    public unsafe void release()
    {
        fixed (PxTask* @this = &this)
        { VirtualMethodTablePointer->release(@this); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?finishBefore@PxTask@physx@@QEAAXI@Z", ExactSpelling = true)]
    private static extern void finishBefore_PInvoke(PxTask* @this, uint taskID);

    public unsafe void finishBefore(uint taskID)
    {
        fixed (PxTask* @this = &this)
        { finishBefore_PInvoke(@this, taskID); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?startAfter@PxTask@physx@@QEAAXI@Z", ExactSpelling = true)]
    private static extern void startAfter_PInvoke(PxTask* @this, uint taskID);

    public unsafe void startAfter(uint taskID)
    {
        fixed (PxTask* @this = &this)
        { startAfter_PInvoke(@this, taskID); }
    }

    public unsafe void addReference()
    {
        fixed (PxTask* @this = &this)
        { VirtualMethodTablePointer->addReference(@this); }
    }

    public unsafe void removeReference()
    {
        fixed (PxTask* @this = &this)
        { VirtualMethodTablePointer->removeReference(@this); }
    }

    public unsafe int getReference()
    {
        fixed (PxTask* @this = &this)
        { return VirtualMethodTablePointer->getReference(@this); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getTaskID@PxTask@physx@@QEBAIXZ", ExactSpelling = true)]
    private static extern uint getTaskID_PInvoke(PxTask* @this);

    public unsafe uint getTaskID()
    {
        fixed (PxTask* @this = &this)
        { return getTaskID_PInvoke(@this); }
    }

    public unsafe void submitted()
    {
        fixed (PxTask* @this = &this)
        { VirtualMethodTablePointer->submitted(@this); }
    }

    [FieldOffset(24)] public uint mTaskID;


    [FieldOffset(0)] public VirtualMethodTable* VirtualMethodTablePointer;

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct VirtualMethodTable
    {
        /// <summary>Virtual method pointer for `~PxTask`</summary>
        public delegate* unmanaged[Cdecl]<PxTask*, void> __DeletingDestructorPointer;
        /// <summary>Virtual method pointer for `run`</summary>
        public delegate* unmanaged[Cdecl]<PxBaseTask*, void> run;
        /// <summary>Virtual method pointer for `getName`</summary>
        public delegate* unmanaged[Cdecl]<PxBaseTask*, byte*> getName;
        /// <summary>Virtual method pointer for `addReference`</summary>
        public delegate* unmanaged[Cdecl]<PxTask*, void> addReference;
        /// <summary>Virtual method pointer for `removeReference`</summary>
        public delegate* unmanaged[Cdecl]<PxTask*, void> removeReference;
        /// <summary>Virtual method pointer for `getReference`</summary>
        public delegate* unmanaged[Cdecl]<PxTask*, int> getReference;
        /// <summary>Virtual method pointer for `release`</summary>
        public delegate* unmanaged[Cdecl]<PxTask*, void> release;
        /// <summary>Virtual method pointer for `submitted`</summary>
        public delegate* unmanaged[Cdecl]<PxTask*, void> submitted;
    }
}