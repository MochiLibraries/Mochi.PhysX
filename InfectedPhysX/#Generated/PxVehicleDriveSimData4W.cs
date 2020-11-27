// This file was automatically generated by Biohazrd and should not be modified by hand!
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit, Size = 576)]
public unsafe partial struct PxVehicleDriveSimData4W
{
    [FieldOffset(0)] public PxVehicleDriveSimData Base;

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??0PxVehicleDriveSimData4W@physx@@QEAA@XZ", ExactSpelling = true)]
    private static extern void Constructor_PInvoke(PxVehicleDriveSimData4W* @this);

    public unsafe void Constructor()
    {
        fixed (PxVehicleDriveSimData4W* @this = &this)
        { Constructor_PInvoke(@this); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getDiffData@PxVehicleDriveSimData4W@physx@@QEBAAEBVPxVehicleDifferential4WData@2@XZ", ExactSpelling = true)]
    private static extern PxVehicleDifferential4WData* getDiffData_PInvoke(PxVehicleDriveSimData4W* @this);

    public unsafe PxVehicleDifferential4WData* getDiffData()
    {
        fixed (PxVehicleDriveSimData4W* @this = &this)
        { return getDiffData_PInvoke(@this); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getAckermannGeometryData@PxVehicleDriveSimData4W@physx@@QEBAAEBVPxVehicleAckermannGeometryData@2@XZ", ExactSpelling = true)]
    private static extern PxVehicleAckermannGeometryData* getAckermannGeometryData_PInvoke(PxVehicleDriveSimData4W* @this);

    public unsafe PxVehicleAckermannGeometryData* getAckermannGeometryData()
    {
        fixed (PxVehicleDriveSimData4W* @this = &this)
        { return getAckermannGeometryData_PInvoke(@this); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?setDiffData@PxVehicleDriveSimData4W@physx@@QEAAXAEBVPxVehicleDifferential4WData@2@@Z", ExactSpelling = true)]
    private static extern void setDiffData_PInvoke(PxVehicleDriveSimData4W* @this, PxVehicleDifferential4WData* diff);

    public unsafe void setDiffData(PxVehicleDifferential4WData* diff)
    {
        fixed (PxVehicleDriveSimData4W* @this = &this)
        { setDiffData_PInvoke(@this, diff); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?setAckermannGeometryData@PxVehicleDriveSimData4W@physx@@QEAAXAEBVPxVehicleAckermannGeometryData@2@@Z", ExactSpelling = true)]
    private static extern void setAckermannGeometryData_PInvoke(PxVehicleDriveSimData4W* @this, PxVehicleAckermannGeometryData* ackermannData);

    public unsafe void setAckermannGeometryData(PxVehicleAckermannGeometryData* ackermannData)
    {
        fixed (PxVehicleDriveSimData4W* @this = &this)
        { setAckermannGeometryData_PInvoke(@this, ackermannData); }
    }

    [FieldOffset(528)] public PxVehicleDifferential4WData mDiff;

    [FieldOffset(560)] public PxVehicleAckermannGeometryData mAckermannGeometry;

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?isValid@PxVehicleDriveSimData4W@physx@@AEBA_NXZ", ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.I1)]
    private static extern bool isValid_PInvoke(PxVehicleDriveSimData4W* @this);

    public unsafe bool isValid()
    {
        fixed (PxVehicleDriveSimData4W* @this = &this)
        { return isValid_PInvoke(@this); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??0PxVehicleDriveSimData4W@physx@@QEAA@W4PxEMPTY@1@@Z", ExactSpelling = true)]
    private static extern void Constructor_PInvoke(PxVehicleDriveSimData4W* @this, PxEMPTY __UNICODE_003C____UNICODE_003E__UnnamedTranslatedParameter);

    public unsafe void Constructor(PxEMPTY __UNICODE_003C____UNICODE_003E__UnnamedTranslatedParameter)
    {
        fixed (PxVehicleDriveSimData4W* @this = &this)
        { Constructor_PInvoke(@this, __UNICODE_003C____UNICODE_003E__UnnamedTranslatedParameter); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getBinaryMetaData@PxVehicleDriveSimData4W@physx@@SAXAEAVPxOutputStream@2@@Z", ExactSpelling = true)]
    public static extern void getBinaryMetaData(PxOutputStream* stream);
}