// <auto-generated>
// This file was automatically generated by Biohazrd and should not be modified by hand!
// </auto-generated>
#nullable enable
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Mochi.PhysX
{
    [StructLayout(LayoutKind.Explicit, Size = 24)]
    public unsafe partial struct PxExtendedVec3
    {
        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper144", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxExtendedVec3* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxExtendedVec3()
        {
            fixed (PxExtendedVec3* @this = &this)
            { Constructor_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper145", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxExtendedVec3* @this, double _x, double _y, double _z);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxExtendedVec3(double _x, double _y, double _z)
        {
            fixed (PxExtendedVec3* @this = &this)
            { Constructor_PInvoke(@this, _x, _y, _z); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?isZero@PxExtendedVec3@physx@@QEBA_NXZ", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool isZero_PInvoke(PxExtendedVec3* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool isZero()
        {
            fixed (PxExtendedVec3* @this = &this)
            { return isZero_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?dot@PxExtendedVec3@physx@@QEBANAEBVPxVec3@2@@Z", ExactSpelling = true)]
        private static extern double dot_PInvoke(PxExtendedVec3* @this, PxVec3* v);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double dot(PxVec3* v)
        {
            fixed (PxExtendedVec3* @this = &this)
            { return dot_PInvoke(@this, v); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?distanceSquared@PxExtendedVec3@physx@@QEBANAEBU12@@Z", ExactSpelling = true)]
        private static extern double distanceSquared_PInvoke(PxExtendedVec3* @this, PxExtendedVec3* v);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double distanceSquared(PxExtendedVec3* v)
        {
            fixed (PxExtendedVec3* @this = &this)
            { return distanceSquared_PInvoke(@this, v); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?magnitudeSquared@PxExtendedVec3@physx@@QEBANXZ", ExactSpelling = true)]
        private static extern double magnitudeSquared_PInvoke(PxExtendedVec3* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double magnitudeSquared()
        {
            fixed (PxExtendedVec3* @this = &this)
            { return magnitudeSquared_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?magnitude@PxExtendedVec3@physx@@QEBANXZ", ExactSpelling = true)]
        private static extern double magnitude_PInvoke(PxExtendedVec3* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double magnitude()
        {
            fixed (PxExtendedVec3* @this = &this)
            { return magnitude_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?normalize@PxExtendedVec3@physx@@QEAANXZ", ExactSpelling = true)]
        private static extern double normalize_PInvoke(PxExtendedVec3* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double normalize()
        {
            fixed (PxExtendedVec3* @this = &this)
            { return normalize_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?isFinite@PxExtendedVec3@physx@@QEBA_NXZ", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool isFinite_PInvoke(PxExtendedVec3* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool isFinite()
        {
            fixed (PxExtendedVec3* @this = &this)
            { return isFinite_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?maximum@PxExtendedVec3@physx@@QEAAXAEBU12@@Z", ExactSpelling = true)]
        private static extern void maximum_PInvoke(PxExtendedVec3* @this, PxExtendedVec3* v);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void maximum(PxExtendedVec3* v)
        {
            fixed (PxExtendedVec3* @this = &this)
            { maximum_PInvoke(@this, v); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?minimum@PxExtendedVec3@physx@@QEAAXAEBU12@@Z", ExactSpelling = true)]
        private static extern void minimum_PInvoke(PxExtendedVec3* @this, PxExtendedVec3* v);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void minimum(PxExtendedVec3* v)
        {
            fixed (PxExtendedVec3* @this = &this)
            { minimum_PInvoke(@this, v); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?set@PxExtendedVec3@physx@@QEAAXNNN@Z", ExactSpelling = true)]
        private static extern void set_PInvoke(PxExtendedVec3* @this, double x_, double y_, double z_);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void set(double x_, double y_, double z_)
        {
            fixed (PxExtendedVec3* @this = &this)
            { set_PInvoke(@this, x_, y_, z_); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?setPlusInfinity@PxExtendedVec3@physx@@QEAAXXZ", ExactSpelling = true)]
        private static extern void setPlusInfinity_PInvoke(PxExtendedVec3* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void setPlusInfinity()
        {
            fixed (PxExtendedVec3* @this = &this)
            { setPlusInfinity_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?setMinusInfinity@PxExtendedVec3@physx@@QEAAXXZ", ExactSpelling = true)]
        private static extern void setMinusInfinity_PInvoke(PxExtendedVec3* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void setMinusInfinity()
        {
            fixed (PxExtendedVec3* @this = &this)
            { setMinusInfinity_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?cross@PxExtendedVec3@physx@@QEAAXAEBU12@AEBVPxVec3@2@@Z", ExactSpelling = true)]
        private static extern void cross_PInvoke(PxExtendedVec3* @this, PxExtendedVec3* left, PxVec3* right);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void cross(PxExtendedVec3* left, PxVec3* right)
        {
            fixed (PxExtendedVec3* @this = &this)
            { cross_PInvoke(@this, left, right); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?cross@PxExtendedVec3@physx@@QEAAXAEBU12@0@Z", ExactSpelling = true)]
        private static extern void cross_PInvoke(PxExtendedVec3* @this, PxExtendedVec3* left, PxExtendedVec3* right);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void cross(PxExtendedVec3* left, PxExtendedVec3* right)
        {
            fixed (PxExtendedVec3* @this = &this)
            { cross_PInvoke(@this, left, right); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?cross@PxExtendedVec3@physx@@QEBA?AU12@AEBU12@@Z", ExactSpelling = true)]
        private static extern PxExtendedVec3* cross_Const_PInvoke(PxExtendedVec3* @this, out PxExtendedVec3 __returnBuffer, PxExtendedVec3* v);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxExtendedVec3 cross_Const(PxExtendedVec3* v)
        {
            fixed (PxExtendedVec3* @this = &this)
            {
                PxExtendedVec3 __returnBuffer;
                cross_Const_PInvoke(@this, out __returnBuffer, v);
                return __returnBuffer;
            }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?cross@PxExtendedVec3@physx@@QEAAXAEBVPxVec3@2@AEBU12@@Z", ExactSpelling = true)]
        private static extern void cross_PInvoke(PxExtendedVec3* @this, PxVec3* left, PxExtendedVec3* right);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void cross(PxVec3* left, PxExtendedVec3* right)
        {
            fixed (PxExtendedVec3* @this = &this)
            { cross_PInvoke(@this, left, right); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??GPxExtendedVec3@physx@@QEBA?AU01@XZ", ExactSpelling = true)]
        private static extern PxExtendedVec3* operator_Minus_PInvoke(PxExtendedVec3* @this, out PxExtendedVec3 __returnBuffer);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxExtendedVec3 operator_Minus()
        {
            fixed (PxExtendedVec3* @this = &this)
            {
                PxExtendedVec3 __returnBuffer;
                operator_Minus_PInvoke(@this, out __returnBuffer);
                return __returnBuffer;
            }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??YPxExtendedVec3@physx@@QEAAAEAU01@AEBU01@@Z", ExactSpelling = true)]
        private static extern PxExtendedVec3* operator_PlusEqual_PInvoke(PxExtendedVec3* @this, PxExtendedVec3* v);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxExtendedVec3* operator_PlusEqual(PxExtendedVec3* v)
        {
            fixed (PxExtendedVec3* @this = &this)
            { return operator_PlusEqual_PInvoke(@this, v); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??ZPxExtendedVec3@physx@@QEAAAEAU01@AEBU01@@Z", ExactSpelling = true)]
        private static extern PxExtendedVec3* operator_MinusEqual_PInvoke(PxExtendedVec3* @this, PxExtendedVec3* v);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxExtendedVec3* operator_MinusEqual(PxExtendedVec3* v)
        {
            fixed (PxExtendedVec3* @this = &this)
            { return operator_MinusEqual_PInvoke(@this, v); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??YPxExtendedVec3@physx@@QEAAAEAU01@AEBVPxVec3@1@@Z", ExactSpelling = true)]
        private static extern PxExtendedVec3* operator_PlusEqual_PInvoke(PxExtendedVec3* @this, PxVec3* v);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxExtendedVec3* operator_PlusEqual(PxVec3* v)
        {
            fixed (PxExtendedVec3* @this = &this)
            { return operator_PlusEqual_PInvoke(@this, v); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??ZPxExtendedVec3@physx@@QEAAAEAU01@AEBVPxVec3@1@@Z", ExactSpelling = true)]
        private static extern PxExtendedVec3* operator_MinusEqual_PInvoke(PxExtendedVec3* @this, PxVec3* v);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxExtendedVec3* operator_MinusEqual(PxVec3* v)
        {
            fixed (PxExtendedVec3* @this = &this)
            { return operator_MinusEqual_PInvoke(@this, v); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??XPxExtendedVec3@physx@@QEAAAEAU01@AEBM@Z", ExactSpelling = true)]
        private static extern PxExtendedVec3* operator_StarEqual_PInvoke(PxExtendedVec3* @this, float* s);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxExtendedVec3* operator_StarEqual(float* s)
        {
            fixed (PxExtendedVec3* @this = &this)
            { return operator_StarEqual_PInvoke(@this, s); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??HPxExtendedVec3@physx@@QEBA?AU01@AEBU01@@Z", ExactSpelling = true)]
        private static extern PxExtendedVec3* operator_Plus_PInvoke(PxExtendedVec3* @this, out PxExtendedVec3 __returnBuffer, PxExtendedVec3* v);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxExtendedVec3 operator_Plus(PxExtendedVec3* v)
        {
            fixed (PxExtendedVec3* @this = &this)
            {
                PxExtendedVec3 __returnBuffer;
                operator_Plus_PInvoke(@this, out __returnBuffer, v);
                return __returnBuffer;
            }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??GPxExtendedVec3@physx@@QEBA?AVPxVec3@1@AEBU01@@Z", ExactSpelling = true)]
        private static extern PxVec3* operator_Minus_PInvoke(PxExtendedVec3* @this, out PxVec3 __returnBuffer, PxExtendedVec3* v);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxVec3 operator_Minus(PxExtendedVec3* v)
        {
            fixed (PxExtendedVec3* @this = &this)
            {
                PxVec3 __returnBuffer;
                operator_Minus_PInvoke(@this, out __returnBuffer, v);
                return __returnBuffer;
            }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??APxExtendedVec3@physx@@QEAAAEANH@Z", ExactSpelling = true)]
        private static extern double* operator_Subscript_PInvoke(PxExtendedVec3* @this, int index);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double* operator_Subscript(int index)
        {
            fixed (PxExtendedVec3* @this = &this)
            { return operator_Subscript_PInvoke(@this, index); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??APxExtendedVec3@physx@@QEBANH@Z", ExactSpelling = true)]
        private static extern double operator_Subscript_Const_PInvoke(PxExtendedVec3* @this, int index);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double operator_Subscript_Const(int index)
        {
            fixed (PxExtendedVec3* @this = &this)
            { return operator_Subscript_Const_PInvoke(@this, index); }
        }

        [FieldOffset(0)] public double x;

        [FieldOffset(8)] public double y;

        [FieldOffset(16)] public double z;
    }
}