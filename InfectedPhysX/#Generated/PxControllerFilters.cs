// <auto-generated>
// This file was automatically generated by Biohazrd and should not be modified by hand!
// </auto-generated>
#nullable enable
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Mochi.PhysX
{
    [StructLayout(LayoutKind.Explicit, Size = 32)]
    public unsafe partial struct PxControllerFilters
    {
        [DllImport("Mochi.PhysX.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "__InlineHelper148", ExactSpelling = true)]
        private static extern void Constructor_PInvoke(PxControllerFilters* @this, PxFilterData* filterData, PxQueryFilterCallback* cb, PxControllerFilterCallback* cctFilterCb);

        [DebuggerStepThrough, DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PxControllerFilters(PxFilterData* filterData = null, PxQueryFilterCallback* cb = null, PxControllerFilterCallback* cctFilterCb = null)
        {
            fixed (PxControllerFilters* @this = &this)
            { Constructor_PInvoke(@this, filterData, cb, cctFilterCb); }
        }

        [FieldOffset(0)] public PxFilterData* mFilterData;

        [FieldOffset(8)] public PxQueryFilterCallback* mFilterCallback;

        [FieldOffset(16)] public PxQueryFlags mFilterFlags;

        [FieldOffset(24)] public PxControllerFilterCallback* mCCTFilterCallback;
    }
}
