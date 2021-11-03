using Mochi.PhysX;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace InfectedPhysX.Sample
{
    internal static unsafe class ErrorCallback
    {
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static void PxErrorCallback_ReportError(PxErrorCallback* @this, PxErrorCode errorCode, byte* message, byte* filePath, int lineNumber)
        {
            Console.Error.Write($"PhysX Error {errorCode}: '");
            Console.Error.WriteAnsi(message);
            Console.Error.Write("' from ");
            Console.Error.WriteAnsi(filePath);
            Console.Error.WriteLine($":{lineNumber}");
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static void PxErrorCallback_Destructor(PxErrorCallback* @this)
        { }

        private static PxErrorCallback.VirtualMethodTable* VTable;
        private static PxErrorCallback.VirtualMethodTable[]? PinnedVTable;
        public static PxErrorCallback Create()
        {
            // If the VTable hasn't been initialized yet, initialize it
            if (VTable is null)
            {
                PinnedVTable = GC.AllocateArray<PxErrorCallback.VirtualMethodTable>(length: 1, pinned: true);
                PinnedVTable[0] = new PxErrorCallback.VirtualMethodTable()
                {
                    reportError = &PxErrorCallback_ReportError,
                    __DeletingDestructorPointer = &PxErrorCallback_Destructor
                };

                VTable = (PxErrorCallback.VirtualMethodTable*)Unsafe.AsPointer(ref PinnedVTable[0]);
            }

            return new PxErrorCallback()
            {
                VirtualMethodTablePointer = VTable
            };
        }
    }
}
