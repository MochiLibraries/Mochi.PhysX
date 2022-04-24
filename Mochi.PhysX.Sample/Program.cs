// This is a port of the PhysX Hello World snippet to Mochi.PhysX
// https://github.com/NVIDIAGameWorks/PhysX/blob/909a7c4fe940154be8c1aca19d655137435dd2f5/physx/snippets/snippethelloworld/SnippetHelloWorld.cpp
// Biohazrd-specific quirks we intend to improve in the future are marked with "BIOQUIRK" comments.
using System;
using System.Diagnostics;
using System.Text;
using static Mochi.PhysX.Globals;

namespace Mochi.PhysX.Sample
{
    public static unsafe class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine($"PhysX native runtime build information: '{MochiPhysX.BuildInfo}'...");

            //---------------------------------------------------------------------------------------------------------------------------------------
            Console.WriteLine("Initializing error callback");
            // Switch between these to use PhysX's default error callback or one implemented from C#
            PxErrorCallback errorCallback = new PxDefaultErrorCallback().Base; //BIOQUIRK: Awkward, unsafe base conversion
            //PxErrorCallback errorCallback = ErrorCallback.Create();

            //---------------------------------------------------------------------------------------------------------------------------------------
            Console.WriteLine("Initializing allocator callback");
            // Switch between these to use PhysX's default allocator callback or one implemented from C#
            PxAllocatorCallback allocator = new PxDefaultAllocator().Base; //BIOQUIRK: Awkward, unsafe base conversion
            //PxAllocatorCallback allocator = BasicAllocator.Create();
            //PxAllocatorCallback allocator = LoggingAllocator.Create();

            //---------------------------------------------------------------------------------------------------------------------------------------
            Console.WriteLine("Initializing foundation");
            //BIOQUIRK: PhysX owns both of these references, which means both the allocator and error callback must remain pinned for the lifetime of the foundation.
            // (In our case they're stack allocated and implicitly pinned.)
            // This seems somewhat unobvious since C# references don't normally care. Should we emit this function differently to convey the unsafe-ness here?
            PxFoundation* foundation = PxCreateFoundation(PX_PHYSICS_VERSION, ref allocator, ref errorCallback);

            if (foundation == null)
            {
                Console.Error.WriteLine("Failed to create foundation.");
                return;
            }

            //---------------------------------------------------------------------------------------------------------------------------------------
            Console.WriteLine("Initializing Pvd...");
            //BIOQUIRK: This pattern comes up a lot in PhysX. It does technically match how it is in C++ though, not sure if it's a problem.
            // I experimentally enabled having C++ reference returns translate as C# reference returns, but that creates a weird situation when you need to store them.
            PxPvd* pvd = PxCreatePvd(ref *foundation);

            PxPvdTransport* transport;
            byte[] host = Encoding.ASCII.GetBytes("127.0.0.1");
            fixed (byte* hostP = host)
            { transport = PxDefaultPvdSocketTransportCreate(hostP, 5425, 10); }

            Console.WriteLine("Connecting to Pvd...");
            pvd->connect(ref *transport, PxPvdInstrumentationFlags.eALL);

            //---------------------------------------------------------------------------------------------------------------------------------------
            Console.WriteLine("Initializing physics");
            PxPhysics* physics = PxCreatePhysics(PX_PHYSICS_VERSION, ref *foundation, new PxTolerancesScale(), trackOutstandingAllocations: true, pvd);

            if (physics == null)
            {
                Console.Error.WriteLine("Failed to create physics.");
                return;
            }

            //---------------------------------------------------------------------------------------------------------------------------------------
            Console.WriteLine("Creating dispatcher");
            PxDefaultCpuDispatcher* dispatcher = PxDefaultCpuDispatcherCreate(2, null);

            //---------------------------------------------------------------------------------------------------------------------------------------
            Console.WriteLine("Creating scene");
            PxSceneDesc sceneDescription = new(*physics->getTolerancesScale());
            sceneDescription.gravity = new PxVec3() { x = 0f, y = -9.81f, z = 0f };
            sceneDescription.cpuDispatcher = (PxCpuDispatcher*)dispatcher;
            sceneDescription.filterShader = PxDefaultSimulationFilterShader;
            PxScene* scene = physics->createScene(sceneDescription);

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
            PxRigidStatic* groundPlane = PxCreatePlane(ref *physics, planeDescription, ref *material);
            scene->addActor(ref *(PxActor*)groundPlane, null);

            //---------------------------------------------------------------------------------------------------------------------------------------
            Console.WriteLine("Adding stacks");
            {
                const float halfExtent = 2f;
                PxBoxGeometry stackBoxGeometry = new PxBoxGeometry(halfExtent, halfExtent, halfExtent);
                PxShapeFlags shapeFlags = PxShapeFlags.eVISUALIZATION | PxShapeFlags.eSCENE_QUERY_SHAPE | PxShapeFlags.eSIMULATION_SHAPE;
                //BIOQUIRK: shapeFlags should be able to be defaulted now but it isn't.
                PxShape* shape = physics->createShape(*(PxGeometry*)&stackBoxGeometry, *material, isExclusive: false, shapeFlags);
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
                    PxTransform transform = new(transformPosition);

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
                            PxTransform localTransform = new(position);

                            PxTransform bodyTransform = transform.transform(localTransform);
                            PxRigidDynamic* body = physics->createRigidDynamic(bodyTransform);
                            body->Base.Base.attachShape(ref *shape);
                            PxRigidBodyExt.updateMassAndInertia(ref *(PxRigidBody*)body, 10f);
                            scene->addActor(ref *(PxActor*)body);
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
                PxTransform transform = new(position);
                PxSphereGeometry geometry = new(10f);

                PxVec3 velocity = new PxVec3()
                {
                    x = 0f,
                    y = -50f,
                    z = -100f
                };

                PxTransform identity = new(default(PxIDENTITY)); //BIOQUIRK: This could be a special generated property instead. Also missing default.
                PxRigidDynamic* dynamic = PxCreateDynamic(ref *physics, transform, *(PxGeometry*)&geometry, ref *material, 10f, identity);
                dynamic->Base.setAngularDamping(0.5f);
                dynamic->Base.setLinearVelocity(velocity);
                scene->addActor(ref *(PxActor*)dynamic);
            }

            //---------------------------------------------------------------------------------------------------------------------------------------
            const int noInputFrameCount = 100;
            Console.WriteLine($"Simulating the world{(Console.IsInputRedirected ? $" for {noInputFrameCount} frames." : "... (Press escape to stop.)")}");
            Stopwatch sw = new Stopwatch();
            int frameNum = 0;

            const uint scratchMemoryBlockSize = 16 * 1024;
            const uint scratchMemoryBlockCount = 4;
            uint scratchMemorySize = scratchMemoryBlockSize * scratchMemoryBlockCount;
            void* scratchMemory = allocator.allocate(scratchMemorySize, null, null, 0);

            while (true)
            {
                double msSinceLastTick = sw.Elapsed.TotalMilliseconds;
                string consoleTitle = $"Simulating frame {frameNum} -- {msSinceLastTick:0.00} ms -- {1.0 / (msSinceLastTick / 1000.0):00.0} FPS";
                if (BasicAllocator.AllocationCount > 0) // This is only applicable when a allocator implemented in C# is in use, assume 0 allocations implies the PhysX one is being used
                {
                    consoleTitle += $" -- {BasicAllocator.AllocationCount} allocations";
                    BasicAllocator.AllocationCount = 0;
                }
                Console.Title = consoleTitle;
                frameNum++;

                sw.Restart();

                scene->simulate(1f / 60f, scratchMemBlock: scratchMemory, scratchMemBlockSize: scratchMemorySize);

                uint errors;
                scene->fetchResults(true, &errors);
                if (errors != 0)
                { Console.WriteLine($"fetchResults error: {errors}"); }

                if (Console.IsInputRedirected)
                {
                    if (frameNum > noInputFrameCount)
                    { break; }
                }
                else if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
                { break; }
            }

            //---------------------------------------------------------------------------------------------------------------------------------------
            Console.WriteLine("Shutting down");
            allocator.deallocate(scratchMemory);
            physics->release();
            foundation->release();
        }
    }
}
