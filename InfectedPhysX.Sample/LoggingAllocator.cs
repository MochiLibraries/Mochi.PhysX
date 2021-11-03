using Mochi.PhysX;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace InfectedPhysX.Sample
{
    internal static unsafe class LoggingAllocator
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct AllocationInformation
        {
            public void* ActualAllocation;
            public ulong AllocationSize;
            public byte* TypeName;
            public byte* FilePath;
            public int LineNumber;
        }

        public static volatile uint AllocationCount = 0;

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static void* PxAllocatorCallback_Allocate(PxAllocatorCallback* @this, ulong size, byte* typeName, byte* filePath, int lineNumber)
        {
            Console.Write("Allocating ");
            Console.Out.WriteAnsi(typeName);
            Console.Write($" ({size} bytes) for ");
            Console.Out.WriteAnsi(filePath);
            Console.WriteLine($":{lineNumber}");

            AllocationCount++;

            // Allocate memory
            // Over-provision it by sizeof(AllocationInformation) so we can store our extra info and 16 bytes for alignment
            const ulong alignment = 16ul;
            ulong bytesToAllocate = size + (ulong)sizeof(AllocationInformation) + alignment;
            byte* actualAllocation = (byte*)Marshal.AllocHGlobal((IntPtr)bytesToAllocate);

            // Skip the allocation information
            byte* physXMemory = actualAllocation + sizeof(AllocationInformation);

            // Align
            physXMemory += alignment - (((ulong)physXMemory) % alignment);
            Debug.Assert(((ulong)physXMemory) % alignment == 0, "The memory must be appropriately aligned!");
            Debug.Assert((((ulong)physXMemory) & 15) == 0, "The memory must be appropriately aligned!"); // This is the same assert PhysX uses internally.

            // Record the pointer to the actual allocation
            AllocationInformation* info = (AllocationInformation*)(physXMemory - sizeof(AllocationInformation));
            info->ActualAllocation = actualAllocation;
            info->AllocationSize = size;
            info->TypeName = typeName;
            info->FilePath = filePath;
            info->LineNumber = lineNumber;

            // Return the buffer
            return physXMemory;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static void PxAllocatorCallback_Deallocate(PxAllocatorCallback* @this, void* ptr)
        {
            AllocationInformation* info = (AllocationInformation*)((byte*)ptr - sizeof(AllocationInformation));

            Console.Write("Deallocating ");
            Console.Out.WriteAnsi(info->TypeName);
            Console.Write($" ({info->AllocationSize} bytes) for ");
            Console.Out.WriteAnsi(info->FilePath);
            Console.WriteLine($":{info->LineNumber}");

            Marshal.FreeHGlobal((IntPtr)info->ActualAllocation);
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static void PxAllocatorCallback_Destructor(PxAllocatorCallback* @this)
        { }

        private static PxAllocatorCallback.VirtualMethodTable* VTable;
        private static PxAllocatorCallback.VirtualMethodTable[]? PinnedVTable;
        public static PxAllocatorCallback Create()
        {
            // If the VTable hasn't been initialized yet, initialize it
            if (VTable is null)
            {
                PinnedVTable = GC.AllocateArray<PxAllocatorCallback.VirtualMethodTable>(length: 1, pinned: true);
                PinnedVTable[0] = new PxAllocatorCallback.VirtualMethodTable()
                {
                    allocate = &PxAllocatorCallback_Allocate,
                    deallocate = &PxAllocatorCallback_Deallocate,
                    __DeletingDestructorPointer = &PxAllocatorCallback_Destructor,
                };

                VTable = (PxAllocatorCallback.VirtualMethodTable*)Unsafe.AsPointer(ref PinnedVTable[0]);
            }

            return new PxAllocatorCallback()
            {
                VirtualMethodTablePointer = VTable
            };
        }
    }
}
