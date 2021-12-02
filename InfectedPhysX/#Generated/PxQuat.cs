// <auto-generated>
// This file was automatically generated by Biohazrd and should not be modified by hand!
// </auto-generated>
#nullable enable
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Mochi.PhysX
{
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public unsafe partial struct PxQuat
    {
        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper5", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxQuat* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxQuat()
        {
            fixed (PxQuat* @this = &this)
            { Constructor_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper6", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxQuat* @this, PxIDENTITY r);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxQuat(PxIDENTITY r)
        {
            fixed (PxQuat* @this = &this)
            { Constructor_PInvoke(@this, r); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper7", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxQuat* @this, float r);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxQuat(float r)
        {
            fixed (PxQuat* @this = &this)
            { Constructor_PInvoke(@this, r); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper8", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxQuat* @this, float nx, float ny, float nz, float nw);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxQuat(float nx, float ny, float nz, float nw)
        {
            fixed (PxQuat* @this = &this)
            { Constructor_PInvoke(@this, nx, ny, nz, nw); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper9", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxQuat* @this, float angleRadians, PxVec3* unitAxis);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxQuat(float angleRadians, PxVec3* unitAxis)
        {
            fixed (PxQuat* @this = &this)
            { Constructor_PInvoke(@this, angleRadians, unitAxis); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper10", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxQuat* @this, PxQuat* v);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxQuat(PxQuat* v)
        {
            fixed (PxQuat* @this = &this)
            { Constructor_PInvoke(@this, v); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper11", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxQuat* @this, PxMat33* m);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxQuat(PxMat33* m)
        {
            fixed (PxQuat* @this = &this)
            { Constructor_PInvoke(@this, m); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?isIdentity@PxQuat@physx@@QEBA_NXZ", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool isIdentity_PInvoke(PxQuat* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool isIdentity()
        {
            fixed (PxQuat* @this = &this)
            { return isIdentity_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?isFinite@PxQuat@physx@@QEBA_NXZ", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool isFinite_PInvoke(PxQuat* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool isFinite()
        {
            fixed (PxQuat* @this = &this)
            { return isFinite_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?isUnit@PxQuat@physx@@QEBA_NXZ", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool isUnit_PInvoke(PxQuat* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool isUnit()
        {
            fixed (PxQuat* @this = &this)
            { return isUnit_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?isSane@PxQuat@physx@@QEBA_NXZ", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool isSane_PInvoke(PxQuat* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool isSane()
        {
            fixed (PxQuat* @this = &this)
            { return isSane_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??8PxQuat@physx@@QEBA_NAEBV01@@Z", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool operator_EqualEqual_PInvoke(PxQuat* @this, PxQuat* q);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool operator_EqualEqual(PxQuat* q)
        {
            fixed (PxQuat* @this = &this)
            { return operator_EqualEqual_PInvoke(@this, q); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?toRadiansAndUnitAxis@PxQuat@physx@@QEBAXAEAMAEAVPxVec3@2@@Z", ExactSpelling = true)]
        private static extern void toRadiansAndUnitAxis_PInvoke(PxQuat* @this, float* angle, PxVec3* axis);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void toRadiansAndUnitAxis(float* angle, PxVec3* axis)
        {
            fixed (PxQuat* @this = &this)
            { toRadiansAndUnitAxis_PInvoke(@this, angle, axis); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getAngle@PxQuat@physx@@QEBAMXZ", ExactSpelling = true)]
        private static extern float getAngle_PInvoke(PxQuat* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float getAngle()
        {
            fixed (PxQuat* @this = &this)
            { return getAngle_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getAngle@PxQuat@physx@@QEBAMAEBV12@@Z", ExactSpelling = true)]
        private static extern float getAngle_PInvoke(PxQuat* @this, PxQuat* q);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float getAngle(PxQuat* q)
        {
            fixed (PxQuat* @this = &this)
            { return getAngle_PInvoke(@this, q); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?magnitudeSquared@PxQuat@physx@@QEBAMXZ", ExactSpelling = true)]
        private static extern float magnitudeSquared_PInvoke(PxQuat* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float magnitudeSquared()
        {
            fixed (PxQuat* @this = &this)
            { return magnitudeSquared_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?dot@PxQuat@physx@@QEBAMAEBV12@@Z", ExactSpelling = true)]
        private static extern float dot_PInvoke(PxQuat* @this, PxQuat* v);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float dot(PxQuat* v)
        {
            fixed (PxQuat* @this = &this)
            { return dot_PInvoke(@this, v); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getNormalized@PxQuat@physx@@QEBA?AV12@XZ", ExactSpelling = true)]
        private static extern PxQuat* getNormalized_PInvoke(PxQuat* @this, out PxQuat __returnBuffer);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxQuat getNormalized()
        {
            fixed (PxQuat* @this = &this)
            {
                PxQuat __returnBuffer;
                getNormalized_PInvoke(@this, out __returnBuffer);
                return __returnBuffer;
            }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?magnitude@PxQuat@physx@@QEBAMXZ", ExactSpelling = true)]
        private static extern float magnitude_PInvoke(PxQuat* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float magnitude()
        {
            fixed (PxQuat* @this = &this)
            { return magnitude_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?normalize@PxQuat@physx@@QEAAMXZ", ExactSpelling = true)]
        private static extern float normalize_PInvoke(PxQuat* @this);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float normalize()
        {
            fixed (PxQuat* @this = &this)
            { return normalize_PInvoke(@this); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getConjugate@PxQuat@physx@@QEBA?AV12@XZ", ExactSpelling = true)]
        private static extern PxQuat* getConjugate_PInvoke(PxQuat* @this, out PxQuat __returnBuffer);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxQuat getConjugate()
        {
            fixed (PxQuat* @this = &this)
            {
                PxQuat __returnBuffer;
                getConjugate_PInvoke(@this, out __returnBuffer);
                return __returnBuffer;
            }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getImaginaryPart@PxQuat@physx@@QEBA?AVPxVec3@2@XZ", ExactSpelling = true)]
        private static extern PxVec3* getImaginaryPart_PInvoke(PxQuat* @this, out PxVec3 __returnBuffer);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxVec3 getImaginaryPart()
        {
            fixed (PxQuat* @this = &this)
            {
                PxVec3 __returnBuffer;
                getImaginaryPart_PInvoke(@this, out __returnBuffer);
                return __returnBuffer;
            }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getBasisVector0@PxQuat@physx@@QEBA?AVPxVec3@2@XZ", ExactSpelling = true)]
        private static extern PxVec3* getBasisVector0_PInvoke(PxQuat* @this, out PxVec3 __returnBuffer);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxVec3 getBasisVector0()
        {
            fixed (PxQuat* @this = &this)
            {
                PxVec3 __returnBuffer;
                getBasisVector0_PInvoke(@this, out __returnBuffer);
                return __returnBuffer;
            }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getBasisVector1@PxQuat@physx@@QEBA?AVPxVec3@2@XZ", ExactSpelling = true)]
        private static extern PxVec3* getBasisVector1_PInvoke(PxQuat* @this, out PxVec3 __returnBuffer);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxVec3 getBasisVector1()
        {
            fixed (PxQuat* @this = &this)
            {
                PxVec3 __returnBuffer;
                getBasisVector1_PInvoke(@this, out __returnBuffer);
                return __returnBuffer;
            }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?getBasisVector2@PxQuat@physx@@QEBA?AVPxVec3@2@XZ", ExactSpelling = true)]
        private static extern PxVec3* getBasisVector2_PInvoke(PxQuat* @this, out PxVec3 __returnBuffer);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxVec3 getBasisVector2()
        {
            fixed (PxQuat* @this = &this)
            {
                PxVec3 __returnBuffer;
                getBasisVector2_PInvoke(@this, out __returnBuffer);
                return __returnBuffer;
            }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?rotate@PxQuat@physx@@QEBA?BVPxVec3@2@AEBV32@@Z", ExactSpelling = true)]
        private static extern PxVec3* rotate_PInvoke(PxQuat* @this, out PxVec3 __returnBuffer, PxVec3* v);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxVec3 rotate(PxVec3* v)
        {
            fixed (PxQuat* @this = &this)
            {
                PxVec3 __returnBuffer;
                rotate_PInvoke(@this, out __returnBuffer, v);
                return __returnBuffer;
            }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?rotateInv@PxQuat@physx@@QEBA?BVPxVec3@2@AEBV32@@Z", ExactSpelling = true)]
        private static extern PxVec3* rotateInv_PInvoke(PxQuat* @this, out PxVec3 __returnBuffer, PxVec3* v);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxVec3 rotateInv(PxVec3* v)
        {
            fixed (PxQuat* @this = &this)
            {
                PxVec3 __returnBuffer;
                rotateInv_PInvoke(@this, out __returnBuffer, v);
                return __returnBuffer;
            }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??4PxQuat@physx@@QEAAAEAV01@AEBV01@@Z", ExactSpelling = true)]
        private static extern PxQuat* operator_Equal_PInvoke(PxQuat* @this, PxQuat* p);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxQuat* operator_Equal(PxQuat* p)
        {
            fixed (PxQuat* @this = &this)
            { return operator_Equal_PInvoke(@this, p); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??XPxQuat@physx@@QEAAAEAV01@AEBV01@@Z", ExactSpelling = true)]
        private static extern PxQuat* operator_StarEqual_PInvoke(PxQuat* @this, PxQuat* q);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxQuat* operator_StarEqual(PxQuat* q)
        {
            fixed (PxQuat* @this = &this)
            { return operator_StarEqual_PInvoke(@this, q); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??YPxQuat@physx@@QEAAAEAV01@AEBV01@@Z", ExactSpelling = true)]
        private static extern PxQuat* operator_PlusEqual_PInvoke(PxQuat* @this, PxQuat* q);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxQuat* operator_PlusEqual(PxQuat* q)
        {
            fixed (PxQuat* @this = &this)
            { return operator_PlusEqual_PInvoke(@this, q); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??ZPxQuat@physx@@QEAAAEAV01@AEBV01@@Z", ExactSpelling = true)]
        private static extern PxQuat* operator_MinusEqual_PInvoke(PxQuat* @this, PxQuat* q);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxQuat* operator_MinusEqual(PxQuat* q)
        {
            fixed (PxQuat* @this = &this)
            { return operator_MinusEqual_PInvoke(@this, q); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??XPxQuat@physx@@QEAAAEAV01@M@Z", ExactSpelling = true)]
        private static extern PxQuat* operator_StarEqual_PInvoke(PxQuat* @this, float s);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxQuat* operator_StarEqual(float s)
        {
            fixed (PxQuat* @this = &this)
            { return operator_StarEqual_PInvoke(@this, s); }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??DPxQuat@physx@@QEBA?AV01@AEBV01@@Z", ExactSpelling = true)]
        private static extern PxQuat* operator_Star_PInvoke(PxQuat* @this, out PxQuat __returnBuffer, PxQuat* q);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxQuat operator_Star(PxQuat* q)
        {
            fixed (PxQuat* @this = &this)
            {
                PxQuat __returnBuffer;
                operator_Star_PInvoke(@this, out __returnBuffer, q);
                return __returnBuffer;
            }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??HPxQuat@physx@@QEBA?AV01@AEBV01@@Z", ExactSpelling = true)]
        private static extern PxQuat* operator_Plus_PInvoke(PxQuat* @this, out PxQuat __returnBuffer, PxQuat* q);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxQuat operator_Plus(PxQuat* q)
        {
            fixed (PxQuat* @this = &this)
            {
                PxQuat __returnBuffer;
                operator_Plus_PInvoke(@this, out __returnBuffer, q);
                return __returnBuffer;
            }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??GPxQuat@physx@@QEBA?AV01@XZ", ExactSpelling = true)]
        private static extern PxQuat* operator_Minus_PInvoke(PxQuat* @this, out PxQuat __returnBuffer);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxQuat operator_Minus()
        {
            fixed (PxQuat* @this = &this)
            {
                PxQuat __returnBuffer;
                operator_Minus_PInvoke(@this, out __returnBuffer);
                return __returnBuffer;
            }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??GPxQuat@physx@@QEBA?AV01@AEBV01@@Z", ExactSpelling = true)]
        private static extern PxQuat* operator_Minus_PInvoke(PxQuat* @this, out PxQuat __returnBuffer, PxQuat* q);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxQuat operator_Minus(PxQuat* q)
        {
            fixed (PxQuat* @this = &this)
            {
                PxQuat __returnBuffer;
                operator_Minus_PInvoke(@this, out __returnBuffer, q);
                return __returnBuffer;
            }
        }

        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "??DPxQuat@physx@@QEBA?AV01@M@Z", ExactSpelling = true)]
        private static extern PxQuat* operator_Star_PInvoke(PxQuat* @this, out PxQuat __returnBuffer, float r);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxQuat operator_Star(float r)
        {
            fixed (PxQuat* @this = &this)
            {
                PxQuat __returnBuffer;
                operator_Star_PInvoke(@this, out __returnBuffer, r);
                return __returnBuffer;
            }
        }

        [FieldOffset(0)] public float x;

        [FieldOffset(4)] public float y;

        [FieldOffset(8)] public float z;

        [FieldOffset(12)] public float w;
    }
}
