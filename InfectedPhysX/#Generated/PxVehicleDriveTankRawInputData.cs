// This file was automatically generated by Biohazrd and should not be modified by hand!
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit, Size = 32)]
public unsafe partial struct PxVehicleDriveTankRawInputData
{
    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??0PxVehicleDriveTankRawInputData@physx@@QEAA@W4Enum@PxVehicleDriveTankControlModel@1@@Z", ExactSpelling = true)]
    private static extern void Constructor_PInvoke(PxVehicleDriveTankRawInputData* @this, PxVehicleDriveTankControlModel mode);

    public unsafe void Constructor(PxVehicleDriveTankControlModel mode)
    {
        fixed (PxVehicleDriveTankRawInputData* @this = &this)
        { Constructor_PInvoke(@this, mode); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??_DPxVehicleDriveTankRawInputData@physx@@QEAAXXZ", ExactSpelling = true)]
    private static extern void Destructor_PInvoke(PxVehicleDriveTankRawInputData* @this);

    public unsafe void Destructor()
    {
        fixed (PxVehicleDriveTankRawInputData* @this = &this)
        { Destructor_PInvoke(@this); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getDriveModel@PxVehicleDriveTankRawInputData@physx@@QEBA?AW4Enum@PxVehicleDriveTankControlModel@2@XZ", ExactSpelling = true)]
    private static extern PxVehicleDriveTankControlModel getDriveModel_PInvoke(PxVehicleDriveTankRawInputData* @this);

    public unsafe PxVehicleDriveTankControlModel getDriveModel()
    {
        fixed (PxVehicleDriveTankRawInputData* @this = &this)
        { return getDriveModel_PInvoke(@this); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?setDigitalAccel@PxVehicleDriveTankRawInputData@physx@@QEAAX_N@Z", ExactSpelling = true)]
    private static extern void setDigitalAccel_PInvoke(PxVehicleDriveTankRawInputData* @this, [MarshalAs(UnmanagedType.I1)] bool b);

    public unsafe void setDigitalAccel(bool b)
    {
        fixed (PxVehicleDriveTankRawInputData* @this = &this)
        { setDigitalAccel_PInvoke(@this, b); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?setDigitalLeftThrust@PxVehicleDriveTankRawInputData@physx@@QEAAX_N@Z", ExactSpelling = true)]
    private static extern void setDigitalLeftThrust_PInvoke(PxVehicleDriveTankRawInputData* @this, [MarshalAs(UnmanagedType.I1)] bool b);

    public unsafe void setDigitalLeftThrust(bool b)
    {
        fixed (PxVehicleDriveTankRawInputData* @this = &this)
        { setDigitalLeftThrust_PInvoke(@this, b); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?setDigitalRightThrust@PxVehicleDriveTankRawInputData@physx@@QEAAX_N@Z", ExactSpelling = true)]
    private static extern void setDigitalRightThrust_PInvoke(PxVehicleDriveTankRawInputData* @this, [MarshalAs(UnmanagedType.I1)] bool b);

    public unsafe void setDigitalRightThrust(bool b)
    {
        fixed (PxVehicleDriveTankRawInputData* @this = &this)
        { setDigitalRightThrust_PInvoke(@this, b); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?setDigitalLeftBrake@PxVehicleDriveTankRawInputData@physx@@QEAAX_N@Z", ExactSpelling = true)]
    private static extern void setDigitalLeftBrake_PInvoke(PxVehicleDriveTankRawInputData* @this, [MarshalAs(UnmanagedType.I1)] bool b);

    public unsafe void setDigitalLeftBrake(bool b)
    {
        fixed (PxVehicleDriveTankRawInputData* @this = &this)
        { setDigitalLeftBrake_PInvoke(@this, b); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?setDigitalRightBrake@PxVehicleDriveTankRawInputData@physx@@QEAAX_N@Z", ExactSpelling = true)]
    private static extern void setDigitalRightBrake_PInvoke(PxVehicleDriveTankRawInputData* @this, [MarshalAs(UnmanagedType.I1)] bool b);

    public unsafe void setDigitalRightBrake(bool b)
    {
        fixed (PxVehicleDriveTankRawInputData* @this = &this)
        { setDigitalRightBrake_PInvoke(@this, b); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getDigitalAccel@PxVehicleDriveTankRawInputData@physx@@QEBA_NXZ", ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.I1)]
    private static extern bool getDigitalAccel_PInvoke(PxVehicleDriveTankRawInputData* @this);

    public unsafe bool getDigitalAccel()
    {
        fixed (PxVehicleDriveTankRawInputData* @this = &this)
        { return getDigitalAccel_PInvoke(@this); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getDigitalLeftThrust@PxVehicleDriveTankRawInputData@physx@@QEBA_NXZ", ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.I1)]
    private static extern bool getDigitalLeftThrust_PInvoke(PxVehicleDriveTankRawInputData* @this);

    public unsafe bool getDigitalLeftThrust()
    {
        fixed (PxVehicleDriveTankRawInputData* @this = &this)
        { return getDigitalLeftThrust_PInvoke(@this); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getDigitalRightThrust@PxVehicleDriveTankRawInputData@physx@@QEBA_NXZ", ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.I1)]
    private static extern bool getDigitalRightThrust_PInvoke(PxVehicleDriveTankRawInputData* @this);

    public unsafe bool getDigitalRightThrust()
    {
        fixed (PxVehicleDriveTankRawInputData* @this = &this)
        { return getDigitalRightThrust_PInvoke(@this); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getDigitalLeftBrake@PxVehicleDriveTankRawInputData@physx@@QEBA_NXZ", ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.I1)]
    private static extern bool getDigitalLeftBrake_PInvoke(PxVehicleDriveTankRawInputData* @this);

    public unsafe bool getDigitalLeftBrake()
    {
        fixed (PxVehicleDriveTankRawInputData* @this = &this)
        { return getDigitalLeftBrake_PInvoke(@this); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getDigitalRightBrake@PxVehicleDriveTankRawInputData@physx@@QEBA_NXZ", ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.I1)]
    private static extern bool getDigitalRightBrake_PInvoke(PxVehicleDriveTankRawInputData* @this);

    public unsafe bool getDigitalRightBrake()
    {
        fixed (PxVehicleDriveTankRawInputData* @this = &this)
        { return getDigitalRightBrake_PInvoke(@this); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?setAnalogAccel@PxVehicleDriveTankRawInputData@physx@@QEAAXM@Z", ExactSpelling = true)]
    private static extern void setAnalogAccel_PInvoke(PxVehicleDriveTankRawInputData* @this, float accel);

    public unsafe void setAnalogAccel(float accel)
    {
        fixed (PxVehicleDriveTankRawInputData* @this = &this)
        { setAnalogAccel_PInvoke(@this, accel); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?setAnalogLeftThrust@PxVehicleDriveTankRawInputData@physx@@QEAAXM@Z", ExactSpelling = true)]
    private static extern void setAnalogLeftThrust_PInvoke(PxVehicleDriveTankRawInputData* @this, float leftThrust);

    public unsafe void setAnalogLeftThrust(float leftThrust)
    {
        fixed (PxVehicleDriveTankRawInputData* @this = &this)
        { setAnalogLeftThrust_PInvoke(@this, leftThrust); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?setAnalogRightThrust@PxVehicleDriveTankRawInputData@physx@@QEAAXM@Z", ExactSpelling = true)]
    private static extern void setAnalogRightThrust_PInvoke(PxVehicleDriveTankRawInputData* @this, float rightThrust);

    public unsafe void setAnalogRightThrust(float rightThrust)
    {
        fixed (PxVehicleDriveTankRawInputData* @this = &this)
        { setAnalogRightThrust_PInvoke(@this, rightThrust); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?setAnalogLeftBrake@PxVehicleDriveTankRawInputData@physx@@QEAAXM@Z", ExactSpelling = true)]
    private static extern void setAnalogLeftBrake_PInvoke(PxVehicleDriveTankRawInputData* @this, float leftBrake);

    public unsafe void setAnalogLeftBrake(float leftBrake)
    {
        fixed (PxVehicleDriveTankRawInputData* @this = &this)
        { setAnalogLeftBrake_PInvoke(@this, leftBrake); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?setAnalogRightBrake@PxVehicleDriveTankRawInputData@physx@@QEAAXM@Z", ExactSpelling = true)]
    private static extern void setAnalogRightBrake_PInvoke(PxVehicleDriveTankRawInputData* @this, float rightBrake);

    public unsafe void setAnalogRightBrake(float rightBrake)
    {
        fixed (PxVehicleDriveTankRawInputData* @this = &this)
        { setAnalogRightBrake_PInvoke(@this, rightBrake); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getAnalogAccel@PxVehicleDriveTankRawInputData@physx@@QEBAMXZ", ExactSpelling = true)]
    private static extern float getAnalogAccel_PInvoke(PxVehicleDriveTankRawInputData* @this);

    public unsafe float getAnalogAccel()
    {
        fixed (PxVehicleDriveTankRawInputData* @this = &this)
        { return getAnalogAccel_PInvoke(@this); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getAnalogLeftThrust@PxVehicleDriveTankRawInputData@physx@@QEBAMXZ", ExactSpelling = true)]
    private static extern float getAnalogLeftThrust_PInvoke(PxVehicleDriveTankRawInputData* @this);

    public unsafe float getAnalogLeftThrust()
    {
        fixed (PxVehicleDriveTankRawInputData* @this = &this)
        { return getAnalogLeftThrust_PInvoke(@this); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getAnalogRightThrust@PxVehicleDriveTankRawInputData@physx@@QEBAMXZ", ExactSpelling = true)]
    private static extern float getAnalogRightThrust_PInvoke(PxVehicleDriveTankRawInputData* @this);

    public unsafe float getAnalogRightThrust()
    {
        fixed (PxVehicleDriveTankRawInputData* @this = &this)
        { return getAnalogRightThrust_PInvoke(@this); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getAnalogLeftBrake@PxVehicleDriveTankRawInputData@physx@@QEBAMXZ", ExactSpelling = true)]
    private static extern float getAnalogLeftBrake_PInvoke(PxVehicleDriveTankRawInputData* @this);

    public unsafe float getAnalogLeftBrake()
    {
        fixed (PxVehicleDriveTankRawInputData* @this = &this)
        { return getAnalogLeftBrake_PInvoke(@this); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getAnalogRightBrake@PxVehicleDriveTankRawInputData@physx@@QEBAMXZ", ExactSpelling = true)]
    private static extern float getAnalogRightBrake_PInvoke(PxVehicleDriveTankRawInputData* @this);

    public unsafe float getAnalogRightBrake()
    {
        fixed (PxVehicleDriveTankRawInputData* @this = &this)
        { return getAnalogRightBrake_PInvoke(@this); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?setGearUp@PxVehicleDriveTankRawInputData@physx@@QEAAX_N@Z", ExactSpelling = true)]
    private static extern void setGearUp_PInvoke(PxVehicleDriveTankRawInputData* @this, [MarshalAs(UnmanagedType.I1)] bool gearUp);

    public unsafe void setGearUp(bool gearUp)
    {
        fixed (PxVehicleDriveTankRawInputData* @this = &this)
        { setGearUp_PInvoke(@this, gearUp); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?setGearDown@PxVehicleDriveTankRawInputData@physx@@QEAAX_N@Z", ExactSpelling = true)]
    private static extern void setGearDown_PInvoke(PxVehicleDriveTankRawInputData* @this, [MarshalAs(UnmanagedType.I1)] bool gearDown);

    public unsafe void setGearDown(bool gearDown)
    {
        fixed (PxVehicleDriveTankRawInputData* @this = &this)
        { setGearDown_PInvoke(@this, gearDown); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getGearUp@PxVehicleDriveTankRawInputData@physx@@QEBA_NXZ", ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.I1)]
    private static extern bool getGearUp_PInvoke(PxVehicleDriveTankRawInputData* @this);

    public unsafe bool getGearUp()
    {
        fixed (PxVehicleDriveTankRawInputData* @this = &this)
        { return getGearUp_PInvoke(@this); }
    }

    [DllImport("TODO.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getGearDown@PxVehicleDriveTankRawInputData@physx@@QEBA_NXZ", ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.I1)]
    private static extern bool getGearDown_PInvoke(PxVehicleDriveTankRawInputData* @this);

    public unsafe bool getGearDown()
    {
        fixed (PxVehicleDriveTankRawInputData* @this = &this)
        { return getGearDown_PInvoke(@this); }
    }

    [FieldOffset(0)] public PxVehicleDriveTankControlModel mMode;

    [FieldOffset(4)] public ConstantArray_physx__UNICODE_003A____UNICODE_003A__PxReal_5 mRawAnalogInputs;

    [FieldOffset(24)] public ConstantArray_bool_5 mRawDigitalInputs;

    [FieldOffset(29)] [MarshalAs(UnmanagedType.I1)] public bool mGearUp;

    [FieldOffset(30)] [MarshalAs(UnmanagedType.I1)] public bool mGearDown;
}