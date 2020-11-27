// This file was automatically generated by Biohazrd and should not be modified by hand!
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit, Size = 24)]
public unsafe partial struct PxMaterial
{
    [FieldOffset(0)] public PxBase Base;

    public unsafe void release()
    {
        fixed (PxMaterial* @this = &this)
        { VirtualMethodTablePointer->release(@this); }
    }

    public unsafe uint getReferenceCount()
    {
        fixed (PxMaterial* @this = &this)
        { return VirtualMethodTablePointer->getReferenceCount(@this); }
    }

    public unsafe void acquireReference()
    {
        fixed (PxMaterial* @this = &this)
        { VirtualMethodTablePointer->acquireReference(@this); }
    }

    public unsafe void setDynamicFriction(float coef)
    {
        fixed (PxMaterial* @this = &this)
        { VirtualMethodTablePointer->setDynamicFriction(@this, coef); }
    }

    public unsafe float getDynamicFriction()
    {
        fixed (PxMaterial* @this = &this)
        { return VirtualMethodTablePointer->getDynamicFriction(@this); }
    }

    public unsafe void setStaticFriction(float coef)
    {
        fixed (PxMaterial* @this = &this)
        { VirtualMethodTablePointer->setStaticFriction(@this, coef); }
    }

    public unsafe float getStaticFriction()
    {
        fixed (PxMaterial* @this = &this)
        { return VirtualMethodTablePointer->getStaticFriction(@this); }
    }

    public unsafe void setRestitution(float rest)
    {
        fixed (PxMaterial* @this = &this)
        { VirtualMethodTablePointer->setRestitution(@this, rest); }
    }

    public unsafe float getRestitution()
    {
        fixed (PxMaterial* @this = &this)
        { return VirtualMethodTablePointer->getRestitution(@this); }
    }

    public unsafe void setFlag(PxMaterialFlags flag, bool __UNICODE_003C____UNICODE_003E__UnnamedTranslatedParameter)
    {
        fixed (PxMaterial* @this = &this)
        { VirtualMethodTablePointer->setFlag(@this, flag, __UNICODE_003C____UNICODE_003E__UnnamedTranslatedParameter); }
    }

    public unsafe void setFlags(PxMaterialFlags inFlags)
    {
        fixed (PxMaterial* @this = &this)
        { VirtualMethodTablePointer->setFlags(@this, inFlags); }
    }

    public unsafe PxMaterialFlags getFlags()
    {
        fixed (PxMaterial* @this = &this)
        { return VirtualMethodTablePointer->getFlags(@this); }
    }

    public unsafe void setFrictionCombineMode(PxCombineMode combMode)
    {
        fixed (PxMaterial* @this = &this)
        { VirtualMethodTablePointer->setFrictionCombineMode(@this, combMode); }
    }

    public unsafe PxCombineMode getFrictionCombineMode()
    {
        fixed (PxMaterial* @this = &this)
        { return VirtualMethodTablePointer->getFrictionCombineMode(@this); }
    }

    public unsafe void setRestitutionCombineMode(PxCombineMode combMode)
    {
        fixed (PxMaterial* @this = &this)
        { VirtualMethodTablePointer->setRestitutionCombineMode(@this, combMode); }
    }

    public unsafe PxCombineMode getRestitutionCombineMode()
    {
        fixed (PxMaterial* @this = &this)
        { return VirtualMethodTablePointer->getRestitutionCombineMode(@this); }
    }

    [FieldOffset(16)] public void* userData;

    public unsafe byte* getConcreteTypeName()
    {
        fixed (PxMaterial* @this = &this)
        { return VirtualMethodTablePointer->getConcreteTypeName(@this); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??0PxMaterial@physx@@IEAA@GV?$PxFlags@W4Enum@PxBaseFlag@physx@@G@1@@Z", ExactSpelling = true)]
    private static extern void Constructor_PInvoke(PxMaterial* @this, ushort concreteType, PxBaseFlags baseFlags);

    public unsafe void Constructor(ushort concreteType, PxBaseFlags baseFlags)
    {
        fixed (PxMaterial* @this = &this)
        { Constructor_PInvoke(@this, concreteType, baseFlags); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??0PxMaterial@physx@@IEAA@V?$PxFlags@W4Enum@PxBaseFlag@physx@@G@1@@Z", ExactSpelling = true)]
    private static extern void Constructor_PInvoke(PxMaterial* @this, PxBaseFlags baseFlags);

    public unsafe void Constructor(PxBaseFlags baseFlags)
    {
        fixed (PxMaterial* @this = &this)
        { Constructor_PInvoke(@this, baseFlags); }
    }

    public unsafe void Destructor()
    {
        fixed (PxMaterial* @this = &this)
        { VirtualMethodTablePointer->__DeletingDestructorPointer(@this); }
    }

    public unsafe bool isKindOf(byte* name)
    {
        fixed (PxMaterial* @this = &this)
        { return VirtualMethodTablePointer->isKindOf(@this, name); }
    }


    [FieldOffset(0)] public VirtualMethodTable* VirtualMethodTablePointer;

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct VirtualMethodTable
    {
        /// <summary>Virtual method pointer for `release`</summary>
        public delegate* unmanaged[Cdecl]<PxMaterial*, void> release;
        /// <summary>Virtual method pointer for `getConcreteTypeName`</summary>
        public delegate* unmanaged[Cdecl]<PxMaterial*, byte*> getConcreteTypeName;
        /// <summary>Virtual method pointer for `isReleasable`</summary>
        public delegate* unmanaged[Cdecl]<PxBase*, NativeBoolean> isReleasable;
        /// <summary>Virtual method pointer for `~PxMaterial`</summary>
        public delegate* unmanaged[Cdecl]<PxMaterial*, void> __DeletingDestructorPointer;
        /// <summary>Virtual method pointer for `isKindOf`</summary>
        public delegate* unmanaged[Cdecl]<PxMaterial*, byte*, NativeBoolean> isKindOf;
        /// <summary>Virtual method pointer for `getReferenceCount`</summary>
        public delegate* unmanaged[Cdecl]<PxMaterial*, uint> getReferenceCount;
        /// <summary>Virtual method pointer for `acquireReference`</summary>
        public delegate* unmanaged[Cdecl]<PxMaterial*, void> acquireReference;
        /// <summary>Virtual method pointer for `setDynamicFriction`</summary>
        public delegate* unmanaged[Cdecl]<PxMaterial*, float, void> setDynamicFriction;
        /// <summary>Virtual method pointer for `getDynamicFriction`</summary>
        public delegate* unmanaged[Cdecl]<PxMaterial*, float> getDynamicFriction;
        /// <summary>Virtual method pointer for `setStaticFriction`</summary>
        public delegate* unmanaged[Cdecl]<PxMaterial*, float, void> setStaticFriction;
        /// <summary>Virtual method pointer for `getStaticFriction`</summary>
        public delegate* unmanaged[Cdecl]<PxMaterial*, float> getStaticFriction;
        /// <summary>Virtual method pointer for `setRestitution`</summary>
        public delegate* unmanaged[Cdecl]<PxMaterial*, float, void> setRestitution;
        /// <summary>Virtual method pointer for `getRestitution`</summary>
        public delegate* unmanaged[Cdecl]<PxMaterial*, float> getRestitution;
        /// <summary>Virtual method pointer for `setFlag`</summary>
        public delegate* unmanaged[Cdecl]<PxMaterial*, PxMaterialFlags, NativeBoolean, void> setFlag;
        /// <summary>Virtual method pointer for `setFlags`</summary>
        public delegate* unmanaged[Cdecl]<PxMaterial*, PxMaterialFlags, void> setFlags;
        /// <summary>Virtual method pointer for `getFlags`</summary>
        public delegate* unmanaged[Cdecl]<PxMaterial*, PxMaterialFlags> getFlags;
        /// <summary>Virtual method pointer for `setFrictionCombineMode`</summary>
        public delegate* unmanaged[Cdecl]<PxMaterial*, PxCombineMode, void> setFrictionCombineMode;
        /// <summary>Virtual method pointer for `getFrictionCombineMode`</summary>
        public delegate* unmanaged[Cdecl]<PxMaterial*, PxCombineMode> getFrictionCombineMode;
        /// <summary>Virtual method pointer for `setRestitutionCombineMode`</summary>
        public delegate* unmanaged[Cdecl]<PxMaterial*, PxCombineMode, void> setRestitutionCombineMode;
        /// <summary>Virtual method pointer for `getRestitutionCombineMode`</summary>
        public delegate* unmanaged[Cdecl]<PxMaterial*, PxCombineMode> getRestitutionCombineMode;
    }
}