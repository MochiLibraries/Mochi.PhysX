// This is a quick and dirty proof-of-concept port of the PhysX Hello World snippet to InfectedPhysX
// https://github.com/InfectedLibraries/PhysX/blob/909a7c4fe940154be8c1aca19d655137435dd2f5/physx/snippets/snippethelloworld/SnippetHelloWorld.cpp
// It's longer than the PhysX snippet due to extra test code and a custom allocator.
// Some quirks due to unimplemented features or bugs in Biohazrd are marked with "BIOQUIRK" comments.

//#define VERBOSE_ALLOCATOR // Enables printing information about allocations as they happen.
#define LIGHTWEIGHT_ALLOCATOR // Disables recording diagnostic information about allocations.
#define USE_SCRATCH_MEMORY // Does not seem to have a large impact on the number of allocations. Probably because PhysX just allocates its own when we don't provide one.
#define COUNT_ALLOCATIONS // Keep track of and display allocation counts per-frame.
#define ENABLE_PVD // Having PVD connected does cause a lot of extra allocations. Although the difference between PVD enabled-and-not-connected and disabled entirely is negligible.
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using System.Runtime.CompilerServices;

#if ENABLE_PVD
using System.Text;
#endif

#if !LIGHTWEIGHT_ALLOCATOR
using System.Runtime.CompilerServices;
#endif

namespace InfectedPhysX.Sample
{
    public static unsafe class Program
    {
        public static void Main(string[] args)
            => DoTest();

        private static void WriteAnsi(TextWriter writer, byte* stringPointer)
        {
            if (stringPointer == null)
            {
                writer.Write("<null>");
                return;
            }

            while (*stringPointer != 0)
            {
                writer.Write((char)*stringPointer);
                stringPointer++;
            }
        }

        private static void WriteAnsi(byte* stringPointer)
            => WriteAnsi(Console.Out, stringPointer);

        private static void WriteAnsiError(byte* stringPointer)
            => WriteAnsi(Console.Error, stringPointer);

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static void PxErrorCallback_ReportError(PxErrorCallback* @this, PxErrorCode errorCode, byte* message, byte* filePath, int lineNumber)
        {
            Console.Error.Write($"PhysX Error {errorCode}: '");
            WriteAnsiError(message);
            Console.Error.Write("' from ");
            WriteAnsiError(filePath);
            Console.Error.WriteLine($":{lineNumber}");
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static void PxErrorCallback_Destructor(PxErrorCallback* @this)
        { }

        [StructLayout(LayoutKind.Sequential)]
        struct AllocationInformation
        {
            public void* ActualAllocation;
#if !LIGHTWEIGHT_ALLOCATOR
            public ulong AllocationSize;
            public byte* TypeName;
            public byte* FilePath;
            public int LineNumber;
#endif
        }

#if COUNT_ALLOCATIONS
        private static volatile uint AllocationCount = 0;
#endif

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static void* PxAllocatorCallback_Allocate(PxAllocatorCallback* @this, ulong size, byte* typeName, byte* filePath, int lineNumber)
        {
#if VERBOSE_ALLOCATOR
            Console.Write("Allocating ");
            WriteAnsi(typeName);
            Console.Write($" ({size} bytes) for ");
            WriteAnsi(filePath);
            Console.WriteLine($":{lineNumber}");
#endif

#if COUNT_ALLOCATIONS
            AllocationCount++;
#endif

            // Allocate memory
            // Over-provision it by sizeof(AllocationInformation) so we can store our extra info and 16 bytes for alignment
            const ulong alignment = 16ul;
            ulong bytesToAllocate = size + (ulong)sizeof(AllocationInformation) + alignment;
            byte* actualAllocation = (byte*)Marshal.AllocHGlobal((IntPtr)bytesToAllocate);

#if !LIGHTWEIGHT_ALLOCATOR
            // Zero the memory for sanity
            Unsafe.InitBlock(actualAllocation, 0, checked((uint)bytesToAllocate));
#endif

            // Skip the allocation information
            byte* physXMemory = actualAllocation + sizeof(AllocationInformation);

            // Align
            physXMemory += alignment - (((ulong)physXMemory) % alignment);
            Debug.Assert(((ulong)physXMemory) % alignment == 0, "The memory must be appropriately aligned!");
            Debug.Assert((((ulong)physXMemory) & 15) == 0, "The memory must be appropriately aligned!"); // This is the same assert PhysX uses internally.

            // Ember our allocation information
            AllocationInformation* info = (AllocationInformation*)(physXMemory - sizeof(AllocationInformation));
            info->ActualAllocation = actualAllocation;
#if !LIGHTWEIGHT_ALLOCATOR
            info->AllocationSize = size;
            info->TypeName = typeName;
            info->FilePath = filePath;
            info->LineNumber = lineNumber;
#endif

            // Return the buffer
            return physXMemory;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static void PxAllocatorCallback_Deallocate(PxAllocatorCallback* @this, void* ptr)
        {
            AllocationInformation* info = (AllocationInformation*)((byte*)ptr - sizeof(AllocationInformation));

#if VERBOSE_ALLOCATOR && !LIGHTWEIGHT_ALLOCATOR
            Console.Write("Deallocating ");
            WriteAnsi(info->TypeName);
            Console.Write($" ({info->AllocationSize} bytes) for ");
            WriteAnsi(info->FilePath);
            Console.WriteLine($":{info->LineNumber}");
#endif

            Marshal.FreeHGlobal((IntPtr)info->ActualAllocation);
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static void PxAllocatorCallback_Destructor(PxAllocatorCallback* @this)
        { }

        private static void DoTest()
        {
            NativeLibrary.SetDllImportResolver(typeof(PxDefaultErrorCallback).Assembly, NativeLibraryResolver);

            //---------------------------------------------------------------------------------------------------------------------------------------
            // Can't use PxDefaultErrorCallback because it's in a static library.
            Console.WriteLine("Initializing error callback");
            PxErrorCallback.VirtualMethodTable errorCallbackVTable = new PxErrorCallback.VirtualMethodTable()
            {
                reportError = &PxErrorCallback_ReportError,
                __DeletingDestructorPointer = &PxErrorCallback_Destructor
            };

            PxErrorCallback errorCallback = new PxErrorCallback()
            {
                VirtualMethodTablePointer = &errorCallbackVTable
            };

            //---------------------------------------------------------------------------------------------------------------------------------------
            //BIOQUIRK: It'd be nice to use the default C++ allocator here to see the performance difference, but we need to be able to initialize a PxDefaultAllocator
            // We can't because it needs the vTable initialized but it has no constructor. - https://github.com/InfectedLibraries/Biohazrd/issues/31
            // On the bright side, this is a nice demonstration of manually extending a C++ type in C#.
            Console.WriteLine("Initializing allocator callback");
            PxAllocatorCallback.VirtualMethodTable allocatorCallbackVTable = new PxAllocatorCallback.VirtualMethodTable()
            {
                allocate = &PxAllocatorCallback_Allocate,
                deallocate = &PxAllocatorCallback_Deallocate,
                __DeletingDestructorPointer = &PxAllocatorCallback_Destructor,
            };

            PxAllocatorCallback allocator = new PxAllocatorCallback()
            {
                VirtualMethodTablePointer = &allocatorCallbackVTable
            };

            //---------------------------------------------------------------------------------------------------------------------------------------
            Console.WriteLine("Initializing foundation");
            const uint PX_PHYSICS_VERSION = (4 << 24) + (1 << 16) + (1 << 8);
            PxFoundation* foundation = PxFoundation.PxCreateFoundation(PX_PHYSICS_VERSION, &allocator, &errorCallback);

            if (foundation == null)
            {
                Console.Error.WriteLine("Failed to create foundation.");
                return;
            }

            //---------------------------------------------------------------------------------------------------------------------------------------
            PxPvd* pvd = null;
#if ENABLE_PVD
            Console.WriteLine("Initializing Pvd...");
            pvd = PxPvd.PxCreatePvd(foundation);

            PxPvdTransport* transport;
            byte[] host = Encoding.ASCII.GetBytes("127.0.0.1");
            fixed (byte* hostP = host)
            { transport = PxPvdTransport.PxDefaultPvdSocketTransportCreate(hostP, 5425, 10); }

            Console.WriteLine("Connecting to Pvd...");
            pvd->connect(transport, PxPvdInstrumentationFlags.eALL);
#endif

            //---------------------------------------------------------------------------------------------------------------------------------------
            Console.WriteLine("Initializing physics");
            PxTolerancesScale scale = default;
            scale.Constructor();
            PxPhysics* physics = PxPhysics.PxCreatePhysics(PX_PHYSICS_VERSION, foundation, &scale, trackOutstandingAllocations: true, pvd);

            if (physics == null)
            {
                Console.Error.WriteLine("Failed to create physics.");
                return;
            }

            //---------------------------------------------------------------------------------------------------------------------------------------
            Console.WriteLine("Creating dispatcher");
            PxDefaultCpuDispatcher* dispatcher = PxDefaultCpuDispatcher.PxDefaultCpuDispatcherCreate(2, null);

            //---------------------------------------------------------------------------------------------------------------------------------------
            Console.WriteLine("Creating scene");
            PxSceneDesc sceneDescription = default;
            sceneDescription.Constructor(physics->getTolerancesScale());
            sceneDescription.gravity = new PxVec3() { x = 0f, y = -9.81f, z = 0f };
            sceneDescription.cpuDispatcher = (PxCpuDispatcher*)dispatcher;
            sceneDescription.filterShader =
                //BIOQUIRK: This function pointer wasn't translated quite right.
                (delegate* unmanaged[Cdecl]<uint, PxFilterData, uint, PxFilterData, int*, void*, uint, int>*)
                //BIOQUIRK: We're basically trying to get a pointer to this function in PhysX.
                // We can't actually use a function pointer here because C# considers this a managed function (since it could be a managed stub.)
                //&PxDefaultSimulationFilterShader.PxDefaultSimulationFilterShader__
                // As such, we manually use NativeLibrary.GetExport here.
                // This code should be improved by https://github.com/InfectedLibraries/Biohazrd/issues/80
                NativeLibrary.GetExport(PhysXDllHandle, "?PxDefaultSimulationFilterShader@physx@@YA?AV?$PxFlags@W4Enum@PxFilterFlag@physx@@G@1@IUPxFilterData@1@I0AEAV?$PxFlags@W4Enum@PxPairFlag@physx@@G@1@PEBXI@Z")
            ;
            PxScene* scene = physics->createScene(&sceneDescription);

            //---------------------------------------------------------------------------------------------------------------------------------------
            Console.WriteLine("Configuring scene Pvd client");
            PxPvdSceneClient* pvdClient = scene->getScenePvdClient();
            if (pvdClient != null)
            {
                pvdClient->setScenePvdFlag(PxPvdSceneFlags.eTRANSMIT_CONSTRAINTS, true);
                pvdClient->setScenePvdFlag(PxPvdSceneFlags.eTRANSMIT_CONTACTS, true);
                pvdClient->setScenePvdFlag(PxPvdSceneFlags.eTRANSMIT_SCENEQUERIES, true);
            }

            //---------------------------------------------------------------------------------------------------------------------------------------
            Console.WriteLine("Creating a basic material");
            PxMaterial* material = physics->createMaterial(0.5f, 0.5f, 0.6f);

            //---------------------------------------------------------------------------------------------------------------------------------------
            Console.WriteLine("Adding a ground plane");
            PxPlane planeDescription = new PxPlane()
            {
                n = new PxVec3()
                {
                    x = 0f,
                    y = 1f,
                    z = 0f
                },
                d = 0f
            };
            PxRigidStatic* groundPlane = PxSimpleFactory.PxCreatePlane(physics, &planeDescription, material);
            scene->addActor((PxActor*)groundPlane, null);

            //---------------------------------------------------------------------------------------------------------------------------------------
            Console.WriteLine("Adding stacks");
            {
                PxBoxGeometry stackBoxGeometry = new PxBoxGeometry();
                const float halfExtent = 2f;
                stackBoxGeometry.Constructor(halfExtent, halfExtent, halfExtent);
                PxShape* shape = physics->createShape((PxGeometry*)&stackBoxGeometry, material, isExclusive: false, PxShapeFlags.eVISUALIZATION | PxShapeFlags.eSCENE_QUERY_SHAPE | PxShapeFlags.eSIMULATION_SHAPE);
                float stackZ = 10f;
                for (int stackNum = 0; stackNum < 5; stackNum++)
                {
                    const int size = 10;

                    PxVec3 transformPosition = new PxVec3()
                    {
                        x = 0f,
                        y = 0f,
                        z = stackZ -= 10f
                    };
                    PxTransform transform = default;
                    transform.Constructor(&transformPosition);

                    bool isSane = transform.isSane();
                    bool isFinite = transform.isFinite();
                    bool qIsSane = transform.q.isSane();

                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size - i; j++)
                        {
                            PxVec3 position = new PxVec3()
                            {
                                x = ((float)(j * 2) - (float)(size - i)) * halfExtent,
                                y = ((float)(i * 2 + 1)) * halfExtent,
                                z = 0f
                            };
                            PxTransform localTransform = default;
                            localTransform.Constructor(&position);

                            PxTransform bodyTransform = transform.transform(&localTransform);
                            PxRigidDynamic* body = physics->createRigidDynamic(&bodyTransform);
                            body->Base.Base.attachShape(shape);
                            PxRigidBodyExt.updateMassAndInertia((PxRigidBody*)body, 10f, null, false);
                            scene->addActor((PxActor*)body, null);
                        }
                    }
                }
                shape->release();
            }

            //---------------------------------------------------------------------------------------------------------------------------------------
            Console.WriteLine("Throwing a ball at the stacks");
            {
                PxVec3 position = new PxVec3()
                {
                    x = 0f,
                    y = 40f,
                    z = 100f,
                };
                PxTransform transform = default;
                transform.Constructor(&position);

                PxSphereGeometry geometry = default;
                geometry.Constructor(10f);

                PxVec3 velocity = new PxVec3()
                {
                    x = 0f,
                    y = -50f,
                    z = -100f
                };

                PxTransform identity = default;
                identity.Constructor(default(PxIDENTITY));
                PxRigidDynamic* dynamic = PxSimpleFactory.PxCreateDynamic(physics, &transform, (PxGeometry*)&geometry, material, 10f, &identity);
                dynamic->Base.setAngularDamping(0.5f);
                dynamic->Base.setLinearVelocity(&velocity, true);
                scene->addActor((PxActor*)dynamic, null);
            }

            //---------------------------------------------------------------------------------------------------------------------------------------
            Console.WriteLine("Simulating the world... (Press escape to stop.)");
            Stopwatch sw = new Stopwatch();
            int frameNum = 0;
            void* scratchMemory = null;
            uint scratchMemorySize = 0;

#if USE_SCRATCH_MEMORY
            {
                const uint scratchMemoryBlockSize = 16 * 1024;
                const uint scratchMemoryBlockCount = 4;
                scratchMemorySize = scratchMemoryBlockSize * scratchMemoryBlockCount;
                scratchMemory = allocator.allocate(scratchMemorySize, null, null, 0);
            }
#endif

            while (true)
            {
                double msSinceLastTick = sw.Elapsed.TotalMilliseconds;
                string title = $"Simulating frame {frameNum} -- {msSinceLastTick:0.00} ms -- {1.0 / (msSinceLastTick / 1000.0):00.0} FPS";
#if COUNT_ALLOCATIONS
                title += $" -- {AllocationCount} allocations";
                AllocationCount = 0;
#endif
                Console.Title = title;
                frameNum++;

                sw.Restart();

                scene->simulate(1f / 60f, null, scratchMemory, scratchMemorySize, true);

                uint errors;
                scene->fetchResults(true, &errors);
                if (errors != 0)
                { Console.WriteLine($"fetchResults error: {errors}"); }

                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
                { break; }
            }

#if USE_SCRATCH_MEMORY
            allocator.deallocate(scratchMemory);
#endif

            //---------------------------------------------------------------------------------------------------------------------------------------
            Console.WriteLine("Shutting down");
            physics->release();
            foundation->release();
        }

        private static IntPtr PhysXDllHandle;
        private static IntPtr NativeLibraryResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            if (libraryName == "TODO.dll")
            { return PhysXDllHandle = NativeLibrary.Load(@"InfectedPhysX.Native_64.dll"); }

            return IntPtr.Zero;
        }
    }
}
