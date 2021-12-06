// <auto-generated>
// This file was automatically generated by Biohazrd and should not be modified by hand!
// </auto-generated>
#nullable enable
using System.Runtime.InteropServices;

namespace Mochi.PhysX
{
    [StructLayout(LayoutKind.Explicit, Size = 48)]
    public unsafe partial struct PxContactPatch
    {
        public enum PxContactPatchFlags
        {
            eHAS_FACE_INDICES = 1,
            eMODIFIABLE = 2,
            eFORCE_NO_RESPONSE = 4,
            eHAS_MODIFIED_MASS_RATIOS = 8,
            eHAS_TARGET_VELOCITY = 16,
            eHAS_MAX_IMPULSE = 32,
            eREGENERATE_PATCHES = 64,
            eCOMPRESSED_MODIFIED_CONTACT = 128
        }

        [FieldOffset(0)] public PxMassModificationProps mMassModification;

        [FieldOffset(16)] public PxVec3 normal;

        [FieldOffset(28)] public float restitution;

        [FieldOffset(32)] public float dynamicFriction;

        [FieldOffset(36)] public float staticFriction;

        [FieldOffset(40)] public byte startContactIndex;

        [FieldOffset(41)] public byte nbContacts;

        [FieldOffset(42)] public byte materialFlags;

        [FieldOffset(43)] public byte internalFlags;

        [FieldOffset(44)] public ushort materialIndex0;

        [FieldOffset(46)] public ushort materialIndex1;
    }
}